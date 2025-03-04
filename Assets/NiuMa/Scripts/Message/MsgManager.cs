using System;
using System.Collections.Generic;
using UnityEngine;
using MessagePack;

namespace NiuMa
{
    public class MsgManager
    {
        private MsgManager()
        {
            RegisterMsgFactory(new MsgFactory<MsgConnect>("MsgConnect"));
            RegisterMsgFactory(new MsgFactory<MsgDisconnect>("MsgDisconnect"));
            RegisterMsgFactory(new MsgFactory<MsgEnterVenueResp>("MsgEnterVenueResp"));
            RegisterMsgFactory(new MsgFactory<MsgHeartbeatResp>("MsgHeartbeatResp"));
            RegisterMsgFactory(new MsgFactory<MsgPlayerSignatureError>("MsgPlayerSignatureError"));
            RegisterMsgFactory(new MsgFactory<MsgVoiceServer>("MsgVoiceServer"));
        }

        private static readonly MsgManager _instance = new MsgManager();

        public static MsgManager Instance
        {
            get { return _instance; }
        }

        // 消息类工厂
        private Dictionary<String, IMsgFactory> _factories = new Dictionary<string, IMsgFactory>();

        // 消息队列
        private List<MsgWrapper> _msgQueue = new List<MsgWrapper>();

        // 消息队列线程锁
        private object _lockMsgQueue = new object();

        // 消息处理器列表
        private List<MsgHandler> _handlers = new List<MsgHandler>();

        public void RegisterMsgFactory(IMsgFactory fac)
        {
            _factories.Add(fac.GetName(), fac);
        }

        // 注册消息处理器
        public bool RegisterMsgHandler(MsgHandler handler)
        {
            if (handler == null)
                return false;
            foreach (MsgHandler h in _handlers)
            {
                if (h == handler)
                    return false;
            }
            _handlers.Add(handler);
            return true;
        }

        // 取消注册消息处理器
        public bool UnregisterMsgHandler(MsgHandler handler)
        {
            return _handlers.Remove(handler);
        }

        private MsgBase Deserialize(MsgWrapper mw)
        {
            IMsgFactory fac = null;
            if (_factories.TryGetValue(mw.msgType, out fac))
                return fac.Deserialize(mw);
            return null;
        }

        public void PushMessage(MsgWrapper mw)
        {
            lock (_lockMsgQueue)
            {
                _msgQueue.Add(mw);
            }
        }

        private MsgWrapper PopMessage()
        {
            MsgWrapper mw = null;
            lock (_lockMsgQueue)
            {
                if (_msgQueue.Count > 0)
                {
                    mw = _msgQueue[0];
                    _msgQueue.RemoveAt(0);
                }
            }
            return mw;
        }

        public void HandleMessages()
        {
            MsgWrapper mw = PopMessage();
            while (mw != null)
            {
                OnMessage(mw);
                mw = PopMessage();
            }
        }

        private bool OnMessage(MsgWrapper mw)
        {
            bool ret = false;
            MsgBase msg = Deserialize(mw);
            if (msg == null)
            {
                try
                {
                    byte[] buf = System.Convert.FromBase64String(mw.msgPack);
                    if (buf == null || buf.Length == 0)
                        return false;
                    System.ReadOnlyMemory<byte> mem = new System.ReadOnlyMemory<byte>(buf);
                    string json = MessagePackSerializer.ConvertToJson(mem);
                    foreach (MsgHandler handler in _handlers)
                    {
                        if (handler.OnMessage(mw.msgType, json))
                        {
                            ret = true;
                            break;
                        }
                    }
                    if (!ret)
                        Debug.LogFormat("Message with type: \"{0}\" has no handler.", mw.msgType);
                }
                catch (System.Exception ex)
                {
                    Debug.LogErrorFormat("Handle message(type: {0}) error: {1}", mw.msgType, ex.Message);
                }
            }
            else
            {
                foreach (MsgHandler handler in _handlers)
                {
                    if (handler.OnMessage(msg))
                    {
                        ret = true;
                        break;
                    }
                }
                if (!ret)
                    Debug.LogFormat("Message with type: \"{0}\" has no handler.", mw.msgType);
            }
            return ret;
        }
    }
}