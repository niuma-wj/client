using System.Collections;
using System.Collections.Generic;
using MessagePack;

namespace NiuMa
{
    // 网络连接断开消息
    // 注意，当主动关闭网络连接时不会发生该消息
    [MessagePackObject(keyAsPropertyName: true)]
    public class MsgDisconnect : MsgBase
    {
        public const string TYPE = "MsgDisconnect";

        public override string GetMsgType()
        {
            return TYPE;
        }

        // 离线消息
        public string msg { get; set; }
    }
}