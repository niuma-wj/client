using System.Collections;
using System.Collections.Generic;
using MessagePack;

namespace NiuMa
{
    /// <summary>
    /// ������Ӧ��Ϣ
    /// ������->�ͻ���
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    public class MsgEnterVenueResp : MsgBase
    {
        public const string TYPE = "MsgEnterVenueResp";

        public override string GetMsgType()
        {
            return TYPE;
        }

        /// <summary>
        /// ����id
        /// </summary>
        public string venueId { get; set; }

        /// <summary>
        /// �������(0-�ɹ���1-δ��Ȩ��2-��Ϸ���ʹ���3-���ؼ���ʧ�ܣ�4-����״̬����
        /// 5-����ʧ��(����������)��6-��������)
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        public string errMsg { get; set; }
    }
}