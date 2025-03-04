using System.Collections;
using System.Collections.Generic;
using MessagePack;

namespace NiuMa
{
    /// <summary>
    /// 进入响应消息
    /// 服务器->客户端
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
        /// 场地id
        /// </summary>
        public string venueId { get; set; }

        /// <summary>
        /// 结果代码(0-成功，1-未授权，2-游戏类型错误，3-场地加载失败，4-场地状态错误，
        /// 5-进入失败(例如已满人)，6-其他错误)
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string errMsg { get; set; }
    }
}