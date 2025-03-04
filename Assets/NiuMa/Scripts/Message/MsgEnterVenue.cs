using System.Collections;
using System.Collections.Generic;
using MessagePack;

namespace NiuMa
{
    /// <summary>
    /// 进入场地消息
    /// 客户端->服务器
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    public class MsgEnterVenue : MsgPlayerSignature
    {
        public const string TYPE = "MsgEnterVenue";

        public override string GetMsgType()
        {
            return TYPE;
        }

        public override MsgWrapper PackMessage()
        {
            return MsgWrapper.PackMessage(this);
        }

        // 场地id
        public string venueId { get; set; }

        // 游戏类型
        public int gameType { get; set; }
    }
}