using System.Collections;
using System.Collections.Generic;
using MessagePack;

namespace NiuMa
{
    /// <summary>
    /// 语音消息
    /// 客户端->服务器
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    public class MsgVoiceClient : MsgVenueInner
    {
        public const string TYPE = "MsgVoiceClient";

        public override string GetMsgType()
        {
            return TYPE;
        }

        public override MsgWrapper PackMessage()
        {
            return MsgWrapper.PackMessage(this);
        }

        // 将语音mp3文件打包成base64
        public string base64 { get; set; }
    }
}