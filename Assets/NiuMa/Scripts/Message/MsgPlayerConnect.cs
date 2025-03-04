using System.Collections;
using System.Collections.Generic;
using MessagePack;

namespace NiuMa
{
    /// <summary>
    /// ���������Ϣ����ҿͻ������ӵ���Ϸ������֮�󣨻��������֮�󣩣������������͸���Ϣ
    /// ֪ͨ������������루���������룩
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    public class MsgPlayerConnect : MsgPlayerSignature
    {
        public const string TYPE = "MsgPlayerConnect";

        public override string GetMsgType()
        {
            return TYPE;
        }

        public override MsgWrapper PackMessage()
        {
            return MsgWrapper.PackMessage(this);
        }

        // ռλ����ûɶ��
        public int placeholder { get; set; }
    }
}