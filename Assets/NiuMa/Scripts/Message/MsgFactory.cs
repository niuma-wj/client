using System.Collections;
using System.Collections.Generic;

namespace NiuMa
{
    // ��Ϣ�����ӿ�
    public interface IMsgFactory
    {
        // ������Ϣ����
        MsgBase Deserialize(MsgWrapper mw);

        // ������Ϣ����
        string GetName();
    }

    // ��Ϣ����
    public class MsgFactory<T> : IMsgFactory where T : MsgBase
    {
        public MsgFactory(string name)
        {
            _name = name;
        }

        public MsgBase Deserialize(MsgWrapper mw)
        {
            return mw.UnpackMessage<T>();
        }

        public string GetName()
        {
            return _name;
        }

        private string _name;
    }
}