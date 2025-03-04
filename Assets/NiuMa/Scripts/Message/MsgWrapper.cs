using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePack;

namespace NiuMa
{
    // 网络消息数据封装器
    [MessagePackObject(keyAsPropertyName: true)]
    public class MsgWrapper
    {
        // 消息类型
        public string msgType { get; set; }

        // 消息体
        public string msgPack { get; set; }

        public static MsgWrapper PackMessage<T>(T msg) where T : MsgBase
        {
            try
            {
                byte[] buf = MessagePackSerializer.Serialize(msg);
                if (buf == null || buf.Length == 0)
                    return null;
                string msgPack = System.Convert.ToBase64String(buf);
                MsgWrapper mw = new MsgWrapper();
                mw.msgType = msg.GetMsgType();
                mw.msgPack = msgPack;
                return mw;
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.Message);
                return null;
            }
        }

        public T UnpackMessage<T>() where T : MsgBase
        {
            try
            {
                byte[] buf = System.Convert.FromBase64String(msgPack);
                if (buf == null || buf.Length == 0)
                    return null;
                System.ReadOnlyMemory<byte> mem = new System.ReadOnlyMemory<byte>(buf);
                T msg = MessagePackSerializer.Deserialize<T>(mem);
                return msg;
            }
            catch (System.Exception)
            {
                return null;
            }
        }
    }
}