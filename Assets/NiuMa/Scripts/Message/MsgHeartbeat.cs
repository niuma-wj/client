using System.Collections;
using System.Collections.Generic;
using MessagePack;

namespace NiuMa
{
    /// <summary>
    /// 场地内心跳消息
    /// 玩家必须进入场地后才能发送心跳，否则心跳无意义
    /// 客户端->服务器
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    public class MsgHeartbeat : MsgVenueInner
    {
        public const string TYPE = "MsgHeartbeat";

        public override string GetMsgType()
        {
            return TYPE;
        }

        public override MsgWrapper PackMessage()
        {
            return MsgWrapper.PackMessage(this);
        }

        public int counter { get; set; }
    }
}