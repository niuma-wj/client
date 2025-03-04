using System.Collections;
using System.Collections.Generic;
using MessagePack;

namespace NiuMa
{
    // 网络连接消息
    // 网络连接成功或失败都会产生该消息
    [MessagePackObject(keyAsPropertyName: true)]
    public class MsgConnect : MsgBase
    {
        public const string TYPE = "MsgConnect";

        public override string GetMsgType() 
        {
            return TYPE;
        }

        // 是否连接成功
        public int succeed { get; set; }

        // 错误消息
        public string errMsg { get; set; }
    }
}