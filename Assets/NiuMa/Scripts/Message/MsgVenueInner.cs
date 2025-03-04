using System.Collections;
using System.Collections.Generic;
using MessagePack;

namespace NiuMa
{
    /// <summary>
    /// 场地内消息基类
    /// 所有场地内消息都继承自该类
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    public abstract class MsgVenueInner : MsgPlayerSignature
    {
        // 场地id
        public string venueId { get; set; }
    }
}