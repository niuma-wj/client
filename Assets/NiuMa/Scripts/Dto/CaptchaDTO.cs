using System.Collections;
using System.Collections.Generic;

namespace NiuMa
{
    public class CaptchaDTO
    {
        // �����
        public string code;

        // ��Ϣ
        public string msg;

        // �Ƿ�����֤��
        public bool captchaEnabled;

        // base64��֤��ͼƬ
        public string img;

        //
        public string uuid;
    }
}