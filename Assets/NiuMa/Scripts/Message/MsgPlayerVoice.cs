using System.Collections;
using System.Collections.Generic;
using MessagePack;


namespace NiuMa
{
    /// <summary>
    /// ������Ϣ
    /// �ͻ�����Ϣת������C#ת����Lua
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    public class MsgPlayerVoice : MsgBase
    {
        public const string TYPE = "MsgPlayerVoice";

        public override string GetMsgType()
        {
            return TYPE;
        }

        // ���������������λ��
        public int seat { get; set; }

        // �������������Id
        public string playerId { get; set; }

        // ����mp3�ļ���
        public string fileName { get; set; }
    }
}