using System.Collections;
using System.Collections.Generic;
using MessagePack;

namespace NiuMa
{
    /// <summary>
    /// 玩家连接消息，玩家客户端连接到游戏服务器之后（或断线重连之后），首先立即发送该消息
    /// 通知服务器玩家连入（或重新连入）
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    public class MsgPlayerConnect : MsgPlayerSignature
    {
        public const string TYPE = "MsgPlayerConnect";

        public override string GetMsgType()
        {
            return TYPE;
        }

        public override MsgWrapper PackMessage()
        {
            return MsgWrapper.PackMessage(this);
        }

        // 占位符，没啥用
        public int placeholder { get; set; }
    }
}