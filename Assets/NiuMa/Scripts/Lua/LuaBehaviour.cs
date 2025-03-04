using System.Collections.Generic;
using System.Text;
using UnityEngine;
using XLua;

namespace NiuMa
{
    [System.Serializable]
    public class Injection
    {
        public string name;
        public GameObject value;
    }

    [CSharpCallLua]
    public interface ILuaBehaviour
    {
        void Awake();

        void Start();

        void Update();

        void OnMessage(string msgType, string json);

        void OnDisconnect();

        void OnReconnect();

        void OnDestroy();
    }

    public class LuaBehaviour : MonoBehaviour
    {
        // LUA脚本文件名
        public string _luaScript;

        // 注入到LUA脚本的对象
        public Injection[] _injections;

        // LUA脚本执行环境
        private LuaTable _scriptScopeTable;

        // LUA脚本对象实例
        protected ILuaBehaviour _behaviour = null;

        void Awake()
        {
            if (string.IsNullOrEmpty(_luaScript))
                return;

            LuaEnv lua = LuaManager.Instance.Lua;
            // 为每个脚本设置一个独立的脚本域，可一定程度上防止脚本间全局变量、函数冲突
            _scriptScopeTable = lua.NewTable();

            // 设置其元表的 __index, 使其能够访问全局变量
            using (LuaTable meta = lua.NewTable())
            {
                meta.Set("__index", lua.Global);
                _scriptScopeTable.SetMetaTable(meta);
            }
            // 构建脚本
            string str = MakeLuaScript();
            // 执行脚本
            lua.DoString(str, "GetModule", _scriptScopeTable);
            // 获取模块
            LuaTable module = _scriptScopeTable.GetInPath<LuaTable>("Module");
            // 从 Lua 脚本域中获取定义的函数
            if (module != null)
            {
                _behaviour = module.Cast<ILuaBehaviour>();
                // 将所需值注入到 Lua 脚本域中
                module.Set("gameObject", gameObject);
                foreach (var injection in _injections)
                {
                    module.Set(injection.name, injection.value);
                }
                if (_behaviour != null)
                    _behaviour.Awake();
            }
            AwakeEx(module);
        }

        private string MakeLuaScript()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("local module = require \'");
            sb.Append(_luaScript);
            sb.Append('\'');
            sb.Append("\r\n");
            sb.Append("Module = module;\r\n");
            return sb.ToString();
        }

        protected virtual void AwakeEx(LuaTable module) { }

        // Use this for initialization
        void Start()
        {
            if (_behaviour != null)
                _behaviour.Start();
            StartEx();
        }

        protected virtual void StartEx() { }

        // Update is called once per frame
        void Update()
        {
            if (_behaviour != null)
                _behaviour.Update();
            UpdateEx();
        }

        protected virtual void UpdateEx() { }

        void OnDestroy()
        {
            if (_behaviour != null)
                _behaviour.OnDestroy();
            if (_scriptScopeTable != null)
            {
                _scriptScopeTable.Dispose();
                _scriptScopeTable = null;
            }
            _injections = null;

            OnDestroyEx();
        }

        protected virtual void OnDestroyEx() { }
    }
}