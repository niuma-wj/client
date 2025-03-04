using System.Collections;
using System.Collections.Generic;
using MessagePack;

namespace NiuMa
{
    /// <summary>
    /// ��Ϣ������ǩ������
    /// ������Ҫǩ������Ϣ�������������Ϣ����Ҫǩ�����ر�����Ϸ�߼���ص���Ϣ�����̳��Ը���
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    public abstract class MsgPlayerSignature : MsgBase
    {
        // ���id
        public string playerId { get; set; }

        // unixʱ�������λ�룩��1��������Ч
        public string timestamp { get; set; }

        // �ͻ������ɵ��������1����ʱ���ڲ��ظ�(��ֹ�طŹ���)
        public string nonce { get; set; }

        // md5ǩ��
        public string signature { get; set; }
    }
}