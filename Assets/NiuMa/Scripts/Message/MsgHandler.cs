using System.Collections;
using System.Collections.Generic;

namespace NiuMa
{
    /**
     * 消息处理器
     */
    public class MsgHandler
    {
        /**
         * 处理消息
         * 该方法一般用于在C#中处理消息
         * @param msg 消息
         * @return 消息是否被处理
         */
        public virtual bool OnMessage(MsgBase msg)
        {
            return false;
        }

        /**
         * 处理消息
         * 该方法一般用于在LUA中处理消息，C#中使用MessagePackSerializer将消息体反序列化为json，然后传递到LUA中去解析处理
         * @param msgType 消息类型
         * @param json 消息体json
         * @return 消息是否被处理
         */
        public virtual bool OnMessage(string msgType, string json)
        {
            return false;
        }
    }
}