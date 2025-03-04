using System;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using MessagePack;

namespace NiuMa
{
    public class Connection
    {
        // 套接字
        private Socket _socket = null;
        public Socket Socket
        {
            get { return _socket; }
        }

        // 是否已经成功连接
        bool _connected = false;

        // 是否主动关闭
        bool _activeClose = false;

        // 线程锁
        object _lock = new object();

        // 接收缓存
        private byte[] _bufferReceive = null;

        // 合并缓存
        private byte[] _bufferCompact = null;

        // 交换缓存
        private byte[] _bufferSwap = null;

        // 接收缓存长度，固定为16KB
        public const int RECEIVE_LENGTH = 8192;

        // 解包数据缓存长度，固定为4MB
        public const int BUFFER_LENGTH = 4194304;

        // 打包缓存中已占用字节数
        private int _bufferCount = 0;

        // 网络发送节点
        private class SendNode
        {
            public byte[] _buf = null;
            public int _len = 0;
        }
        private bool _sending = false;
        private Queue<SendNode> _sendingQueue = new Queue<SendNode>();

        public bool IsConnected()
        {
            lock (_lock)
            {
                return _connected;
            }
        }

        public void SetConnected(bool s)
        {
            lock (_lock)
            {
                _connected = s;
            }
        }

        public bool IsActiveClose()
        {
            lock (_lock)
            {
                return _activeClose;
            }
        }

        private void SetActiveClose(bool s)
        {
            lock (_lock)
            {
                _activeClose = s;
                if (s)
                {
                    _connected = false;
                    _sending = false;
                    _sendingQueue.Clear();
                }
            }
        }

        private void SetDisconnect()
        {
            lock (_lock)
            {
                _connected = false;
                _sending = false;
                _sendingQueue.Clear();
            }
        }

        public void Close()
        {
            if (IsActiveClose())
                return;
            if (_socket != null)
            {
                try
                {
                    SetActiveClose(true);
                    _socket.Shutdown(SocketShutdown.Both);
                    _socket.Close();
                }
                catch
                {}
            }
        }

        public void BeginConnect(string ip, int port)
        {
            SetActiveClose(false);
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // 设置非阻塞
            _socket.Blocking = false;
            // 设置接收超时时间（单位：毫秒）
            _socket.ReceiveTimeout = 3000;
            // 设置发送超时时间（单位：毫秒）
            _socket.SendTimeout = 3000;
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS)
            _socket.SetSocketOption(System.Net.Sockets.SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, RECEIVE_LENGTH);
            _socket.SetSocketOption(System.Net.Sockets.SocketOptionLevel.Socket, SocketOptionName.NoDelay, true);
#elif UNITY_ANDROID
            _socket.ReceiveBufferSize = RECEIVE_LENGTH;
            _socket.NoDelay = true;
#endif
            Debug.Log("Begin Connect");
            _socket.BeginConnect(new IPEndPoint(IPAddress.Parse(ip), port), ConnectCallBack, this);
        }

        private static void ConnectCallBack(IAsyncResult ar)
        {
            UnityMainThreadDispatcher.Enqueue(() =>
            {
                // 在IL2CPP环境Unity日志必须在主线程打印才能正常显示
                Debug.Log("Connect callback");
            });
            bool success = false;
            string errMsg = null;
            Connection con = null;
            try
            {
                con = ar.AsyncState as Connection;
                if (con != null)
                    con.Socket.EndConnect(ar);
                if (ar.IsCompleted)
                {
                    UnityMainThreadDispatcher.Enqueue(() =>
                    {
                        // 在IL2CPP环境Unity日志必须在主线程打印才能正常显示
                        Debug.Log("Connect to server success");
                    });
                    success = true;
                }
            }
            catch (System.Exception ex)
            {
                errMsg = "Connect to server error: " + ex.Message;
                UnityMainThreadDispatcher.Enqueue(() =>
                {
                    // 在IL2CPP环境Unity日志必须在主线程打印才能正常显示
                    Debug.LogError(errMsg);
                });
            }
            if (con != null)
                con.OnConnect(success, errMsg);
        }

        public void OnConnect(bool success, string errMsg)
        {
            if (success)
            {
                if (_bufferReceive == null)
                    _bufferReceive = new byte[8192];
                SetConnected(true);
                _socket.BeginReceive(_bufferReceive, 0, 8192, SocketFlags.None, new AsyncCallback(ReceiveCallBack), this);
            }
            MsgConnect msg = new MsgConnect();
            msg.succeed = success ? 1 : 0;
            msg.errMsg = errMsg;
            MsgWrapper mw = MsgWrapper.PackMessage(msg);
            MsgManager.Instance.PushMessage(mw);
        }

        public void OnDisconnect(string err)
        {
            if (_socket == null)
                return;
            _socket = null;
            if (!IsActiveClose())
                SetDisconnect();
            MsgDisconnect msg = new MsgDisconnect();
            msg.msg = err;
            MsgWrapper mw = MsgWrapper.PackMessage(msg);
            MsgManager.Instance.PushMessage(mw);
        }

        public void OnConnectFailed()
        {
            _socket = null;
        }

        private void UnpackMessage(int readCount)
        {
            if (_bufferCompact == null)
                _bufferCompact = new byte[BUFFER_LENGTH];
            // 接收到数据包
            int totalCount = _bufferCount + readCount;
            if (totalCount > BUFFER_LENGTH)
            {
                // 缓冲区溢出
#if UNITY_EDITOR
                UnityMainThreadDispatcher.Enqueue(() =>
                {
                    // 在IL2CPP环境Unity日志必须在主线程打印才能正常显示
                    Debug.LogError("网络接收缓冲区溢出");
                });
#endif
                Array.Clear(_bufferCompact, 0, BUFFER_LENGTH);
                _bufferCount = 0;
                return;
            }
            Buffer.BlockCopy(_bufferReceive, 0, _bufferCompact, _bufferCount, readCount);
            _bufferCount = totalCount;
            totalCount = 0;
            int tmpCount = 0;
            while (_bufferCount > 0)
            {
                try
                {
                    ReadOnlyMemory<byte> mem = new ReadOnlyMemory<byte>(_bufferCompact, totalCount, _bufferCount);
                    MsgWrapper mw = MessagePackSerializer.Deserialize<MsgWrapper>(mem, out tmpCount);
                    if (mw == null)
                        break;
                    totalCount += tmpCount;
                    _bufferCount -= tmpCount;
                    MsgManager.Instance.PushMessage(mw);
                }
                catch (Exception)
                {
                    break;
                }
            }
            if (totalCount > 0 && _bufferCount > 0)
            {
                // 把数据移动到缓存开头
                if (_bufferSwap == null)
                    _bufferSwap = new byte[BUFFER_LENGTH];
                Buffer.BlockCopy(_bufferCompact, totalCount, _bufferSwap, 0, _bufferCount);
                // 将缓存引用交换
                byte[] buf = _bufferSwap;
                _bufferSwap = _bufferCompact;
                _bufferCompact = buf;
            }
        }

        private void OnReceive(int readCount)
        {
            UnpackMessage(readCount);
            _socket.BeginReceive(_bufferReceive, 0, 8192, SocketFlags.None, new AsyncCallback(ReceiveCallBack), this);
        }

        private static void ReceiveCallBack(IAsyncResult ar)
        {
            int readCount = 0;
            String err = "";
            Connection con = null;
            try
            {
                Socket socket = null;
                con = ar.AsyncState as Connection;
                if (con != null)
                    socket = con.Socket;
                if ((socket != null) && socket.Connected)
                    readCount = socket.EndReceive(ar);
            }
            catch (System.Exception ex)
            {
                err = ex.Message;
                if (string.IsNullOrEmpty(err))
                    err = "Receive network packet error.";
                UnityMainThreadDispatcher.Enqueue(() =>
                {
                    // 在IL2CPP环境Unity日志必须在主线程打印才能正常显示
                    Debug.LogError(err);
                });
            }
            if (con != null)
            {
                if (readCount == 0)
                    con.OnDisconnect(err);
                else
                    con.OnReceive(readCount);
            }
        }

        private bool IsSending()
        {
            lock (_lock)
            {
                return _sending;
            }
        }

        private void SetSending(bool s)
        {
            lock (_lock)
            {
                _sending = s;
            }
        }

        private void PushSendingNode(SendNode node)
        {
            if (node == null || node._len <= 0)
                return;

            lock (_lock)
            {
                _sendingQueue.Enqueue(node);
            }
        }

        private SendNode PopSendingNode()
        {
            lock (_lock)
            {
                if (_sendingQueue.Count <= 0)
                    return null;

                return _sendingQueue.Dequeue();
            }
        }

        public void SendMessage(MsgWrapper mw)
        {
            if (!IsConnected())
                return;
            byte[] buf = null;
            try
            {
                buf = MessagePackSerializer.Serialize<MsgWrapper>(mw);
                if (buf == null || buf.Length == 0)
                {
                    Debug.LogError("Serialize MsgWrapper error.");
                    return;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogErrorFormat("Serialize MsgWrapper error: {0}.", ex.Message);
                return;
            }
            SendNode node = new SendNode();
            node._buf = buf;
            node._len = buf.Length;
            PushSendingNode(node);

            if (!IsSending())
                SendHeadNode();
        }

        private void SendHeadNode()
        {
            SendNode node = PopSendingNode();
            if (node == null)
                SetSending(false);
            else
                _socket.BeginSend(node._buf, 0, node._len, SocketFlags.None, new AsyncCallback(SendCallback), this);
        }

        public static void SendCallback(IAsyncResult ar)
        {
            int sendCount = 0;
            String err = "";
            Connection con = null;
            try
            {
                con = ar.AsyncState as Connection;
                if (con != null)
                    sendCount = con.Socket.EndSend(ar);
            }
            catch (System.Exception ex)
            {
                err = ex.Message;
                if (string.IsNullOrEmpty(err))
                    err = "Send package failed.";
                UnityMainThreadDispatcher.Enqueue(() =>
                {
                    // 在IL2CPP环境Unity日志必须在主线程打印才能正常显示
                    Debug.LogError(err);
                });
            }
            if (con != null)
            {
                if (sendCount == 0)
                    con.OnDisconnect(err);
                else
                    con.SendHeadNode();
            }
        }
    }
}