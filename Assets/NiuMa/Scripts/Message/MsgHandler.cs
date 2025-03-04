using System.Collections;
using System.Collections.Generic;

namespace NiuMa
{
    /**
     * ��Ϣ������
     */
    public class MsgHandler
    {
        /**
         * ������Ϣ
         * �÷���һ��������C#�д�����Ϣ
         * @param msg ��Ϣ
         * @return ��Ϣ�Ƿ񱻴���
         */
        public virtual bool OnMessage(MsgBase msg)
        {
            return false;
        }

        /**
         * ������Ϣ
         * �÷���һ��������LUA�д�����Ϣ��C#��ʹ��MessagePackSerializer����Ϣ�巴���л�Ϊjson��Ȼ�󴫵ݵ�LUA��ȥ��������
         * @param msgType ��Ϣ����
         * @param json ��Ϣ��json
         * @return ��Ϣ�Ƿ񱻴���
         */
        public virtual bool OnMessage(string msgType, string json)
        {
            return false;
        }
    }
}