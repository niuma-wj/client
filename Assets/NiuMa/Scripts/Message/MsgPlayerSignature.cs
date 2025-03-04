using System.Collections;
using System.Collections.Generic;
using MessagePack;

namespace NiuMa
{
    /// <summary>
    /// 消息体的玩家签名数据
    /// 所有需要签名的消息（绝大多数的消息都需要签名，特别是游戏逻辑相关的消息）都继承自该类
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    public abstract class MsgPlayerSignature : MsgBase
    {
        // 玩家id
        public string playerId { get; set; }

        // unix时间戳（单位秒），1分钟内有效
        public string timestamp { get; set; }

        // 客户端生成的随机串，1分钟时间内不重复(防止重放攻击)
        public string nonce { get; set; }

        // md5签名
        public string signature { get; set; }
    }
}