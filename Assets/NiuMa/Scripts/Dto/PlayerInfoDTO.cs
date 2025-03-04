using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NiuMa
{
    public class PlayerInfoDTO
    {
        // 玩家id
        public string playerId;

        // 消息密钥
        public string secret;

        // 登录账户名
        public string name;

        // 昵称
        public string nickname;

        // 电话
        public string phone;

        // 性别
        public int sex;

        // 头像
        public string avatar;

        // 金币数量
        public long gold;

        // 金币存款
        public long deposit;

        // 钻石数量
        public long diamond;

        // 是否为代理
        public int isAgency;

        // 上级代理玩家id
        public string agencyId;
    }
}