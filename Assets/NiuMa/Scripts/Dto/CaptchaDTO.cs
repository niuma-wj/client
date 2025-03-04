using System.Collections;
using System.Collections.Generic;

namespace NiuMa
{
    public class CaptchaDTO
    {
        // 结果码
        public string code;

        // 消息
        public string msg;

        // 是否开启验证码
        public bool captchaEnabled;

        // base64验证码图片
        public string img;

        //
        public string uuid;
    }
}