using System.Collections;
using System.Collections.Generic;
using MessagePack;

namespace NiuMa
{
    /// <summary>
    /// ������������Ϣ
    /// ��ұ�����볡�غ���ܷ�����������������������
    /// �ͻ���->������
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    public class MsgHeartbeat : MsgVenueInner
    {
        public const string TYPE = "MsgHeartbeat";

        public override string GetMsgType()
        {
            return TYPE;
        }

        public override MsgWrapper PackMessage()
        {
            return MsgWrapper.PackMessage(this);
        }

        public int counter { get; set; }
    }
}