using System;
using System.Collections.Generic;
using UnityEngine;
using MessagePack;
using XLua;
using LitJson;
using System.IO;

namespace NiuMa
{
    /// <summary>
    /// 离线处理接口
    /// </summary>
    public interface IDisconnectHandler
    {
        public void OnDisconnect();
    }

    /// <summary>
    /// 重连处理接口
    /// </summary>
    public interface IReconnectHandler
    {
        public void OnReconnect();
    }

    /// <summary>
    /// 等待进入场地数据
    /// </summary>
    public class WaitEnterData
    {
        // 服务器ip地址
        public string ip;

        // 服务器端口
        public int port = 0;

        // 场地id
        public string venueId;

        // 游戏类型
        public int gameType;
    }

    [LuaCallCSharp]
    public class NetworkManager : MsgHandler
    {
        private NetworkManager() { }
        private static readonly NetworkManager _instance = new NetworkManager();

        public static NetworkManager Instance
        {
            get { return _instance; }
        }

        // 当前服务器IP
        private string _srvIp = null;

        // 当前服务器端口
        private int _svrPort = 0;

        // 当前是否正在建立连接
        private bool _connecting = false;

        // 断线后尝试重连次数
        private int _reconnects = 0;

        // 是否已经做网络连接失败提示
        private bool _prompted = false;

        // 心跳计数器
        private int _heartbeat = 0;

        // 当前等待进入的场地
        private string _venueId = null;

        // 当前等待进入的游戏类型
        private int _gameType = 0;

        // 当前等待进入的场地数据
        private WaitEnterData _waitEnterData = null;

        private Connection _connection = new Connection();

        // 上一次请求建立连接已过了多久
        private float _connectElapsed = 0.0f;

        // 上一次发送心跳的已过了多久
        private float _heartbeatElapsed = 0.0f;

        // 上一次发送心跳是否已经收到回复
        private bool _receivedHeartbeat = true;

        // 离线处理接口列表
        private List<IDisconnectHandler> _disconnectHandlers = new List<IDisconnectHandler>();

        // 重连处理接口列表
        private List<IReconnectHandler> _reconnectHandlers = new List<IReconnectHandler>();

        // 初始化
        [BlackList]
        public void Initialize()
        {
            MsgManager.Instance.RegisterMsgHandler(this);
        }

        [BlackList]
        public bool RegisterDisconnectHandler(IDisconnectHandler handler)
        {
            if (handler == null)
                return false;
            foreach (MsgHandler h in _disconnectHandlers)
            {
                if (h == handler)
                    return false;
            }
            _disconnectHandlers.Add(handler);
            return true;
        }

        [BlackList]
        public bool UnregisterDisconnectHandler(IDisconnectHandler handler)
        {
            return _disconnectHandlers.Remove(handler);
        }

        [BlackList]
        public bool RegisterReconnectHandler(IReconnectHandler handler)
        {
            if (handler == null)
                return false;
            foreach (MsgHandler h in _reconnectHandlers)
            {
                if (h == handler)
                    return false;
            }
            _reconnectHandlers.Add(handler);
            return true;
        }

        [BlackList]
        public bool UnregisterReconnectHandler(IReconnectHandler handler)
        {
            return _reconnectHandlers.Remove(handler);
        }

        // 进入场地
        [BlackList]
        public void EnterVenue(string ip, int port, string venueId, int gameType)
        {
            if (string.IsNullOrEmpty(ip) || port == 0 ||
                string.IsNullOrEmpty(venueId) || gameType == 0)
                return;
            if (_connection.IsConnected())
            {
                if (ip == _srvIp && port == _svrPort)
                {
                    // 当前已连接到服务器，直接发送进入场地消息
                    MsgEnterVenue enter = new MsgEnterVenue();
                    enter.venueId = venueId;
                    enter.gameType = gameType;
                    GameManager.Instance.SignatureMessage(enter);
                    SendMessage(enter);
                }
                else
                {
                    // 断开当前连接
                    Close();
                    _waitEnterData = new WaitEnterData();
                    _waitEnterData.ip = ip;
                    _waitEnterData.port = port;
                    _waitEnterData.venueId = venueId;
                    _waitEnterData.gameType = gameType;
                }
                return;
            }
            if (_connecting)
                return;
            _srvIp = ip;
            _svrPort = port;
            _venueId = venueId;
            _gameType = gameType;
            Connect();
        }

        public bool IsConnected()
        {
            return _connection.IsConnected();
        }

        // 连接到服务器
        private void Connect()
        {
            if (_connecting)
                return;
            _connection.BeginConnect(_srvIp, _svrPort);
            _connecting = true;
            _connectElapsed = 0.0f;
        }

        [BlackList]
        public void CheckConnection(float deltaTime)
        {
            bool test = true;
            if (_connecting)
            {
                test = false;
                _connectElapsed += deltaTime;
                if (_connectElapsed > 5.0f)
                {
                    // 上一次发起连接已经超过5秒，认为连接失败
                    _connection.Close();
                    _connection.OnConnect(false, "Connect timeout.");
                }
            }
            if (_connecting)
                return;
            if (string.IsNullOrEmpty(GameManager.Instance.VenueId))
                return; // 当前没有进入任何场地，直接退出
            if (_connection.IsConnected())
            {
                _heartbeatElapsed += deltaTime;
                if (_heartbeatElapsed > 5.0f)
                {
                    if (!_receivedHeartbeat)
                    {
                        // 超过5秒没有收到心跳回复，认为网络连接异常，关闭当前网络连接
                        _receivedHeartbeat = true;
                        _connection.Close();
                        _connection.OnDisconnect("Abnormal shutdown.");
                    }
                    else
                    {
                        // 连接正常，定时发送心跳
                        // 每5秒发送一次心跳
                        _receivedHeartbeat = false;
                        _heartbeatElapsed = 0.0f;
                        MsgHeartbeat msg = new MsgHeartbeat();
                        msg.counter = _heartbeat;
                        msg.venueId = GameManager.Instance.VenueId;
                        GameManager.Instance.SignatureMessage(msg);
                        SendMessage(msg);
                    }
                }
            }
            else
            {
                // 重连次数已达上限
                if (_reconnects > 4)
                    return;
                if (test)
                    _connectElapsed += deltaTime;
                if (_connectElapsed > 5.0f)
                {
                    // 再次尝试重连
                    _reconnects++;
                    Connect();
                }
            }
        }

        /// <summary>
        /// 断开网络连接
        /// </summary>
        [BlackList]
        public void Close()
        {
            _connection.Close();
        }

        /// <summary>
        /// 发送消息，由C#调用
        /// </summary>
        /// <param name="msg">消息体</param>
        [BlackList]
        public void SendMessage(MsgBase msg)
        {
            if ((msg == null) || !_connection.IsConnected())
                return;
            try {
                MsgWrapper mw = msg.PackMessage();
                _connection.SendMessage(mw);
            }
            catch (System.Exception ex)
            {
                Debug.LogErrorFormat("Send message(type: {0}) error: {1}", msg.GetMsgType(), ex.Message);
            }
        }

        /// <summary>
        /// 发送消息，由LUA调用
        /// </summary>
        /// <param name="msgType">消息类型</param>
        /// <param name="json">消息体</param>
        /// <param name="signature">是否需要做消息签名</param>
        public void SendMessage(string msgType, string json, bool signature = false)
        {
            if (string.IsNullOrEmpty(msgType) || string.IsNullOrEmpty(json) || !_connection.IsConnected())
                return;
            try
            {
                if (signature)
                {
                    // 需要做消息签名
                    JsonData data = null;
                    try
                    {
                        data = JsonMapper.ToObject(json);
                    }
                    catch (Exception)
                    {
                        data = new JsonData();
                    }
                    GameManager.Instance.SignatureJson(data);
                    json = data.ToJson();
                }
                byte[] buf = MessagePackSerializer.ConvertFromJson(json);
                if (buf == null || buf.Length == 0)
                {
                    Debug.LogError("Serialize json error.");
                    return;
                }
                MsgWrapper mw = new MsgWrapper();
                mw.msgType = msgType;
                mw.msgPack = System.Convert.ToBase64String(buf);
                _connection.SendMessage(mw);
            }
            catch (System.Exception ex)
            {
                Debug.LogErrorFormat("Serialize json error: {0}.", ex.Message);
                return;
            }
        }

        /// <summary>
        /// 发送场地内消息
        /// </summary>
        /// <param name="msgType">消息类型</param>
        public void SendInnerMessage(string msgType)
        {
            if (string.IsNullOrEmpty(msgType) || !_connection.IsConnected())
                return;
            if (string.IsNullOrEmpty(GameManager.Instance.VenueId))
                return;
            try
            {
                JsonData data = new JsonData();
                data["venueId"] = GameManager.Instance.VenueId;
                GameManager.Instance.SignatureJson(data);
                string json = data.ToJson();
                byte[] buf = MessagePackSerializer.ConvertFromJson(json);
                if (buf == null || buf.Length == 0)
                {
                    Debug.LogError("Serialize json error.");
                    return;
                }
                MsgWrapper mw = new MsgWrapper();
                mw.msgType = msgType;
                mw.msgPack = System.Convert.ToBase64String(buf);
                _connection.SendMessage(mw);
            }
            catch (System.Exception ex)
            {
                Debug.LogErrorFormat("Serialize json error: {0}.", ex.Message);
                return;
            }
        }

        [BlackList]
        public override bool OnMessage(MsgBase msg)
        {
            if (msg == null)
                return false;

            bool ret = true;
            string msgType = msg.GetMsgType();
            if (msgType.Equals(MsgConnect.TYPE))
                OnConnect(msg);
            else if (msgType.Equals(MsgDisconnect.TYPE))
                OnDisconnect(msg);
            else if (msgType.Equals(MsgEnterVenueResp.TYPE))
                OnEnterVenue(msg);
            else if (msgType.Equals(MsgHeartbeatResp.TYPE))
                OnHeartBeat();
            else if (msgType.Equals(MsgPlayerSignatureError.TYPE))
                OnSignatureError(msg);
            else if (msgType.Equals(MsgVoiceServer.TYPE))
                OnVoiceServer(msg);
            else
                ret = false;
            return ret;
        }

        private void OnConnect(MsgBase msg)
        {
            if (msg == null)
                return;
            MsgConnect inst = msg as MsgConnect;
            if (inst == null)
                return;

            _connecting = false;
            if (inst.succeed != 0)
            {
                // 连接成功
                _reconnects = 0;
                _prompted = false;
                GameManager.Instance.ShowConnecting(false);
                // 向服务器发送玩家连接消息
                MsgPlayerConnect tmp = new MsgPlayerConnect();
                GameManager.Instance.SignatureMessage(tmp);
                SendMessage(tmp);

                if (!string.IsNullOrEmpty(_venueId) && _gameType != 0)
                {
                    // 进入场地
                    Debug.LogFormat("Enter venue(Id: {0})", _venueId);
                    MsgEnterVenue enter = new MsgEnterVenue();
                    enter.venueId = _venueId;
                    enter.gameType = _gameType;
                    GameManager.Instance.SignatureMessage(enter);
                    SendMessage(enter);
                }
                else if (!string.IsNullOrEmpty(GameManager.Instance.VenueId))
                {
                    foreach (IReconnectHandler handler in _reconnectHandlers)
                        handler.OnReconnect();
                }
            }
            else
            {
                // 连接失败
#if (UNITY_EDITOR || UNITY_STANDALONE)
#elif (UNITY_ANDROID || UNITY_IOS)
                if (!string.IsNullOrEmpty(inst.errMsg))
                    Debug.LogError(inst.errMsg);
#endif
                _connection.OnConnectFailed();
                if (_reconnects > 4)
                {
                    if (!_prompted) {
                        _prompted = true;
                        GameManager.Instance.ShowConnecting(false);
                        GameManager.Instance.ShowPromptDialog("网络连接失败，请检查网络设置", GameManager.Instance.DestroyGameRoom);
                    }
                }
                else if (!string.IsNullOrEmpty(_venueId))
                {
                    // 连接服务器失败
                    GameManager.Instance.OnEnterFailed(_venueId, "网络连接失败，请检查网络设置");
                    _venueId = null;
                    _gameType = 0;
                }
            }
        }

        private void OnDisconnect(MsgBase msg)
        {
#if UNITY_EDITOR
            MsgDisconnect inst = msg as MsgDisconnect;
            if ((inst != null) && !string.IsNullOrEmpty(inst.msg))
                Debug.Log(inst.msg);
#endif
            if (!string.IsNullOrEmpty(GameManager.Instance.VenueId))
                GameManager.Instance.ShowConnecting(true);
            if (!_connection.IsActiveClose())
            {
                foreach (IDisconnectHandler handler in _disconnectHandlers)
                    handler.OnDisconnect();
            }
            if (_waitEnterData != null)
            {
                EnterVenue(_waitEnterData.ip, _waitEnterData.port, _waitEnterData.venueId, _waitEnterData.gameType);
                _waitEnterData = null;
            }
        }

        private void OnEnterVenue(MsgBase msg)
        {
            _venueId = null;
            _gameType = 0;
            MsgEnterVenueResp inst = msg as MsgEnterVenueResp;
            if (inst.code == 0)
            {
                // 进入成功
                _receivedHeartbeat = true;
                _heartbeatElapsed = 0.0f;
                GameManager.Instance.OnEnterVenue(inst.venueId);
                Debug.LogFormat("Enter venue(id: {0}) success", inst.venueId);
            }
            else
            {
                // 进入场地失败，断开网络连接
                Close();
                string errMsg = string.Format("进入游戏失败: {0}", inst.errMsg);
                GameManager.Instance.OnEnterFailed(inst.venueId, errMsg);
            }
        }

        private void OnHeartBeat()
        {
            _heartbeat++;
            if (_heartbeat > 99999)
                _heartbeat = 0;
            _receivedHeartbeat = true;
        }

        private void OnSignatureError(MsgBase msg)
        {
            MsgPlayerSignatureError inst = msg as MsgPlayerSignatureError;
            if (inst == null)
                return;
            if (inst.outdate)
                return;
            GameManager.Instance.OnSignatureError();
        }

        private void OnVoiceServer(MsgBase msg)
        {
            MsgVoiceServer inst = msg as MsgVoiceServer;
            if (inst == null)
                return;
            string fileName = DateTime.Now.ToString("MMddHHmm");
            fileName = inst.playerId + "_" + fileName + ".mp3";
            string path = Path.Combine(Application.persistentDataPath, fileName);
            if (!Utility.FileFromBase64(inst.base64, path))
                return;
            MsgPlayerVoice msg1 = new MsgPlayerVoice();
            msg1.seat = inst.seat;
            msg1.playerId = inst.playerId;
            msg1.fileName = fileName;
            MsgWrapper mw = MsgWrapper.PackMessage(msg1);
            MsgManager.Instance.PushMessage(mw);
        }

        /// <summary>
        /// 发送语音消息
        /// </summary>
        /// <param name="fileName">文件名</param>
        public void SendVoiceMessage(string fileName)
        {
            string path = Path.Combine(Application.persistentDataPath, fileName);
            string base64 = Utility.FileToBase64(path);
            if (string.IsNullOrEmpty(base64))
                return;
            MsgVoiceClient msg = new MsgVoiceClient();
            GameManager.Instance.SignatureMessage(msg);
            msg.venueId = GameManager.Instance.VenueId;
            msg.base64 = base64;
            SendMessage(msg);
        }
    }
}