using System.Collections;
using System.Collections.Generic;
using MessagePack;

namespace NiuMa
{
    /// <summary>
    /// 响应玩家连接消息
    /// 服务器->客户端
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    public class MsgPlayerConnectResp : MsgBase
    {
        public const string TYPE = "MsgPlayerConnectResp";

        public override string GetMsgType()
        {
            return TYPE;
        }

        public int placeholder { get; set; }
    }
}