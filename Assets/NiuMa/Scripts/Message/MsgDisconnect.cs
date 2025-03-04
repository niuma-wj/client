using System.Collections;
using System.Collections.Generic;
using MessagePack;

namespace NiuMa
{
    // �������ӶϿ���Ϣ
    // ע�⣬�������ر���������ʱ���ᷢ������Ϣ
    [MessagePackObject(keyAsPropertyName: true)]
    public class MsgDisconnect : MsgBase
    {
        public const string TYPE = "MsgDisconnect";

        public override string GetMsgType()
        {
            return TYPE;
        }

        // ������Ϣ
        public string msg { get; set; }
    }
}