using System.Collections;
using System.Collections.Generic;

namespace NiuMa
{
    // 消息工厂接口
    public interface IMsgFactory
    {
        // 创建消息数据
        MsgBase Deserialize(MsgWrapper mw);

        // 返回消息类型
        string GetName();
    }

    // 消息工厂
    public class MsgFactory<T> : IMsgFactory where T : MsgBase
    {
        public MsgFactory(string name)
        {
            _name = name;
        }

        public MsgBase Deserialize(MsgWrapper mw)
        {
            return mw.UnpackMessage<T>();
        }

        public string GetName()
        {
            return _name;
        }

        private string _name;
    }
}