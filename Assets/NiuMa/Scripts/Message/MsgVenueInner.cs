using System.Collections;
using System.Collections.Generic;
using MessagePack;

namespace NiuMa
{
    /// <summary>
    /// ��������Ϣ����
    /// ���г�������Ϣ���̳��Ը���
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    public abstract class MsgVenueInner : MsgPlayerSignature
    {
        // ����id
        public string venueId { get; set; }
    }
}