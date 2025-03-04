using System.Collections;
using System.Collections.Generic;
using MessagePack;

namespace NiuMa
{
    // ����������Ϣ
    // �������ӳɹ���ʧ�ܶ����������Ϣ
    [MessagePackObject(keyAsPropertyName: true)]
    public class MsgConnect : MsgBase
    {
        public const string TYPE = "MsgConnect";

        public override string GetMsgType() 
        {
            return TYPE;
        }

        // �Ƿ����ӳɹ�
        public int succeed { get; set; }

        // ������Ϣ
        public string errMsg { get; set; }
    }
}