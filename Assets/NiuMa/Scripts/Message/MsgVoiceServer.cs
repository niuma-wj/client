using System.Collections;
using System.Collections.Generic;
using MessagePack;

namespace NiuMa
{
    /// <summary>
    /// 语音消息
    /// 服务器->客户端
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    public class MsgVoiceServer : MsgBase
    {
        public const string TYPE = "MsgVoiceServer";

        public override string GetMsgType()
        {
            return TYPE;
        }

        // 发出语音的玩家座位号
        public int seat { get; set; }

        // 发出语音的玩家Id
        public string playerId { get; set; }

        // 将语音mp3文件打包成base64
        public string base64 { get; set; }
    }
}