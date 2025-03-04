using System.Collections;
using System.Collections.Generic;
using MessagePack;

namespace NiuMa
{
    /// <summary>
    /// ������Ӧ��Ϣ
    /// ������->�ͻ���
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