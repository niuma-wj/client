using System.Collections;
using System.Collections.Generic;
using MessagePack;

namespace NiuMa
{
    /// <summary>
    /// ������Ϣ
    /// ������->�ͻ���
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    public class MsgVoiceServer : MsgBase
    {
        public const string TYPE = "MsgVoiceServer";

        public override string GetMsgType()
        {
            return TYPE;
        }

        // ���������������λ��
        public int seat { get; set; }

        // �������������Id
        public string playerId { get; set; }

        // ������mp3�ļ������base64
        public string base64 { get; set; }
    }
}