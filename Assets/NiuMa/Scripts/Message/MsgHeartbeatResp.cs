using System.Collections;
using System.Collections.Generic;
using MessagePack;

namespace NiuMa
{
    /// <summary>
    /// 心跳响应消息
    /// 服务器->客户端
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    public class MsgHeartbeatResp : MsgBase
    {
        public const string TYPE = "MsgHeartbeatResp";

        public override string GetMsgType()
        {
            return TYPE;
        }

        public int counter { get; set; }
    }
}