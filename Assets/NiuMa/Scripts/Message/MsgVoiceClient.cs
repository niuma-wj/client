using System.Collections;
using System.Collections.Generic;
using MessagePack;

namespace NiuMa
{
    /// <summary>
    /// ������Ϣ
    /// �ͻ���->������
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    public class MsgVoiceClient : MsgVenueInner
    {
        public const string TYPE = "MsgVoiceClient";

        public override string GetMsgType()
        {
            return TYPE;
        }

        public override MsgWrapper PackMessage()
        {
            return MsgWrapper.PackMessage(this);
        }

        // ������mp3�ļ������base64
        public string base64 { get; set; }
    }
}