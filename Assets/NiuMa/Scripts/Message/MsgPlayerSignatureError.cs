using System.Collections;
using System.Collections.Generic;
using MessagePack;

namespace NiuMa
{
    /// <summary>
    /// �ͻ�����Ϣǩ��������Ϣ
    /// ������->�ͻ���
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    public class MsgPlayerSignatureError : MsgBase
    {
        public const string TYPE = "MsgPlayerSignatureError";

        public override string GetMsgType()
        {
            return TYPE;
        }

        public bool outdate { get; set; }
    }
}