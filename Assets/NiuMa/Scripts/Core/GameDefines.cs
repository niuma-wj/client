using System;

namespace NiuMa
{
    // 服务器类型
    public enum ServerType
    {
        LoginServer,    // 登录服
        CenterServer,	// 中心服
        HallServer,     // 大厅服
        TableServer     // 牌桌服
    }

    // 登录结果
    public enum LoginResult
    {
        Unknown,            // 未知错误
        Succeed,            // 登录成功
        InvalidAccount,     // 账号错误
        InvalidPassword,    // 密码错误
        InvalidToken,       // 无效或过期Token
        Frozen,             // 账号被冻结
        Repeat,				// 重复登录
        AgencyNotBound,     // 未绑定代理
        AgencyNotExist,		// 指定的代理ID不存在或不是代理
        Connect             // 连接失败
    }

    // 签到结果
    public enum SignResult
    {
        Unknown,            // 未知错误
        Succeed,            // 登录成功
        TryAgain,           // 再次尝试
        InvalidLoginKey,    // 登录Key无效
        Repeat,				// 重复签到
        Connect             // 连接失败
    }

    // 创建牌桌结果
    public enum CreateTableResult
    {
        Unknown,        // 未知错误
        Succeed,        // 创建成功
        Insufficient,   // 金币(房卡)不足
        AboveLimit,     // 当前创建的房间数量已达上限
        Conflict,       // 当前正在创建(或加入)其他牌桌，创建后无法加入
        Locked			// 用户当前被其他牌桌锁住，创建后无法加入
    }

    // 加入牌桌结果
    public enum JoinTableResult
    {
        Unknown,        // 未知错误
        Succeed,        // 创建成功
        NotFound,       // 找不到指定房间
        Insufficient,   // 金币(房卡)不足
        Conflict,       // 当前正在创建(或加入)其他牌桌，无法加入
        Locked,         // 用户当前被其他牌桌锁住，无法加入
        Full,           // 牌桌人已满
        Occupied		// 指定座位已经被占(仅对指定座位的情况有效)
    }
}