using System.Collections;
using System.Collections.Generic;
using MessagePack;

namespace NiuMa
{
    /// <summary>
    /// 客户端消息签名错误消息
    /// 服务器->客户端
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    public class MsgPlayerSignatureError : MsgBase
    {
        public const string TYPE = "MsgPlayerSignatureError";

        public override string GetMsgType()
        {
            return TYPE;
        }

        public bool outdate { get; set; }
    }
}