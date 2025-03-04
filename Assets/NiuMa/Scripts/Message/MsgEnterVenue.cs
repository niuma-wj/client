using System.Collections;
using System.Collections.Generic;
using MessagePack;

namespace NiuMa
{
    /// <summary>
    /// ���볡����Ϣ
    /// �ͻ���->������
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

        // ����id
        public string venueId { get; set; }

        // ��Ϸ����
        public int gameType { get; set; }
    }
}