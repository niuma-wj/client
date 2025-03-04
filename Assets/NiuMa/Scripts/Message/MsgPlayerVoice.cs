using System.Collections;
using System.Collections.Generic;
using MessagePack;


namespace NiuMa
{
    /// <summary>
    /// 语音消息
    /// 客户端消息转换，由C#转发到Lua
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    public class MsgPlayerVoice : MsgBase
    {
        public const string TYPE = "MsgPlayerVoice";

        public override string GetMsgType()
        {
            return TYPE;
        }

        // 发出语音的玩家座位号
        public int seat { get; set; }

        // 发出语音的玩家Id
        public string playerId { get; set; }

        // 语音mp3文件名
        public string fileName { get; set; }
    }
}