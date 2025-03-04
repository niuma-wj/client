using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace NiuMa
{
    public class LuaMsgHandler : MsgHandler
    {
        public LuaMsgHandler(LuaMessageBehaviour owner)
        {
            _owner = owner;
        }

        private LuaMessageBehaviour _owner;

        public override bool OnMessage(MsgBase msg)
        {
            return _owner.OnMessage(msg);
        }

        public override bool OnMessage(string msgType, string json)
        {
            return _owner.OnMessage(msgType, json);
        }
    }

    public class LuaDisconnectHandler : IDisconnectHandler
    {
        public LuaDisconnectHandler(LuaMessageBehaviour owner)
        {
            _owner = owner;
        }

        private LuaMessageBehaviour _owner;

        public void OnDisconnect()
        {
            _owner.OnDisconnect();
        }
    }

    public class LuaReconnectHandler : IReconnectHandler
    {
        public LuaReconnectHandler(LuaMessageBehaviour owner)
        {
            _owner = owner;
        }

        private LuaMessageBehaviour _owner;

        public void OnReconnect()
        {
            _owner.OnReconnect();
        }
    }

    public class LuaMessageBehaviour : LuaBehaviour
    {
        // 消息处理器
        private LuaMsgHandler _handler = null;

        // 离线处理器
        private LuaDisconnectHandler _disconnectHandler = null;

        // 重连处理器
        private LuaReconnectHandler _reconnectHandler = null;

        // 本消息处理器接受的消息类型列表
        private HashSet<string> _msgTypes = new HashSet<string>();

        protected override void AwakeEx(LuaTable module)
        {
            if (module == null)
                return;
            if (module.ContainsKey("OnMessage"))
            {
                string messages = null;
                module.Get("messages", out messages);
                if (!string.IsNullOrEmpty(messages))
                {
                    string[] arr = messages.Split(',');
                    int count = 0;
                    if (arr != null)
                        count = arr.Length;
                    for (int i = 0; i < count; i++)
                    {
                        if (string.IsNullOrEmpty(arr[i]))
                            continue;
                        _msgTypes.Add(arr[i].Trim());
                    }
                }
            }
            if (_msgTypes.Count > 0)
            {
                _handler = new LuaMsgHandler(this);
                MsgManager.Instance.RegisterMsgHandler(_handler);
            }
            if (module.ContainsKey("OnDisconnect"))
            {
                _disconnectHandler = new LuaDisconnectHandler(this);
                NetworkManager.Instance.RegisterDisconnectHandler(_disconnectHandler);
            }
            if (module.ContainsKey("OnReconnect"))
            {
                _reconnectHandler = new LuaReconnectHandler(this);
                NetworkManager.Instance.RegisterReconnectHandler(_reconnectHandler);
            }
        }

        protected override void OnDestroyEx()
        {
            if (_handler != null)
                MsgManager.Instance.UnregisterMsgHandler(_handler);
            if (_disconnectHandler != null)
                NetworkManager.Instance.UnregisterDisconnectHandler(_disconnectHandler);
            if (_reconnectHandler != null)
                NetworkManager.Instance.UnregisterReconnectHandler(_reconnectHandler);
        }

        public virtual bool OnMessage(MsgBase msg)
        {
            return false;
        }

        public virtual bool OnMessage(string msgType, string json)
        {
            if (!_msgTypes.Contains(msgType))
                return false;
            if (_behaviour != null)
                _behaviour.OnMessage(msgType, json);
            return true;
        }

        public virtual void OnDisconnect()
        {
            if (_behaviour != null)
                _behaviour.OnDisconnect();
        }

        public virtual void OnReconnect()
        {
            if (_behaviour != null)
                _behaviour.OnReconnect();
        }
    }
}