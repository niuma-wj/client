using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using XLua;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using MessagePack;

namespace NiuMa
{
    [LuaCallCSharp]
    public static class Utility
    {
        // 大写字母表
        private static readonly char[] CapitalLetters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        // 小写字母表
        private static readonly char[] LowercaseLetters = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        // 数字表
        private static readonly char[] Numbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        // 特殊字符
        private static readonly char[] SpecialLetters = { '!', '#', '$', '%', '&', '(', ')', '*', '+', ',', '-', '.', '/' };

        //
        public const int CODE_CAPITAL = 0x01;

        //
        public const int CODE_LOWERCASE = 0x02;

        //
        public const int CODE_NUMBER = 0x04;

        //
        public const int CODE_ALL = (0x01 | 0x02 | 0x04);

        /// <summary>
        /// 生成随机码
        /// </summary>
        /// <param name="length">随机码长度</param>
        /// <param name="codeMask">随机码类型掩码，如：CODE_CAPITAL | CODE_NUMBER</param>
        /// <returns>随机码</returns>
        public static string GenerateRandomCode(int length, int codeMask = CODE_ALL)
        {
            if (length < 1)
                return null;
            int types = 0;
            char[][] arr = new char[3][];
            if ((codeMask & CODE_CAPITAL) != 0)
            {
                arr[types] = CapitalLetters;
                types++;
            }
            if ((codeMask & CODE_LOWERCASE) != 0)
            {
                arr[types] = LowercaseLetters;
                types++;
            }
            if ((codeMask & CODE_NUMBER) != 0)
            {
                arr[types] = Numbers;
                types++;
            }
            if (types == 0)
                return null;
            StringBuilder sb = new StringBuilder();
            int tmp = 0;
            char[] characters = null;
            Random generator = new Random();
            for (int i = 0; i < length; i++)
            {
                tmp = generator.Next(types);
                characters = arr[tmp];
                tmp = generator.Next(characters.Length);
                sb.Append(characters[tmp]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 字符串编码成base64
        /// </summary>
        /// <param name="text">输入字符串</param>
        /// <returns>返回base64</returns>
        public static string EncodeBase64(string text)
        {
            if (text == null || text.Length == 0)
                return string.Empty;
            byte[] buf = System.Text.Encoding.UTF8.GetBytes(text);
            string base64 = Convert.ToBase64String(buf);
            return base64;
        }

        /// <summary>
        /// base64字符串解码
        /// </summary>
        /// <param name="base64">base64字符串</param>
        /// <returns>解码后的字符串</returns>
        public static string DecodeBase64(string base64)
        {
            if (base64 == null || base64.Length == 0)
                return string.Empty;
            try
            {
                byte[] buf = Convert.FromBase64String(base64);
                if (buf == null || buf.Length == 0)
                    return string.Empty;
                string text = System.Text.Encoding.UTF8.GetString(buf);
                return text;
            }
            catch (Exception) {
                return string.Empty;
            }
        }

        /// <summary>
        /// 将文件内容读出并转换成base64编码
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>文件内容base64编码</returns>
        public static string FileToBase64(string filePath)
        {
            if (!File.Exists(filePath))
                return string.Empty;
            try
            {
                byte[] fileBytes = File.ReadAllBytes(filePath);
                if (fileBytes == null || fileBytes.Length == 0)
                    return string.Empty;
                string base64 = Convert.ToBase64String(fileBytes);
                return base64;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 将base64字符串解码并存入文件
        /// </summary>
        /// <param name="base64">base64字符串</param>
        /// <param name="filePath">文件路径</param>
        /// <returns>是否保存成功</returns>
        public static bool FileFromBase64(string base64, string filePath)
        {
            if (string.IsNullOrEmpty(base64))
                return false;
            try
            {
                byte[] fileBytes = Convert.FromBase64String(base64);
                if (fileBytes == null || fileBytes.Length == 0)
                    return false;
                File.WriteAllBytes(filePath, fileBytes);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 从base64中解码出压缩内容，并将压缩内容解压，然后再反序列化
        /// </summary>
        /// <param name="base64">base64字符串</param>
        /// <returns>反序列化后的Json字符串</returns>
        public static string DecompressAndDeserializeBase64(string base64)
        {
            if (string.IsNullOrEmpty(base64))
                return string.Empty;
            try
            {
                byte[] compressedData = Convert.FromBase64String(base64);
                if (compressedData == null || compressedData.Length == 0)
                    return string.Empty;
                MemoryStream compressedStream = new MemoryStream(compressedData);
                InflaterInputStream inputStream = new InflaterInputStream(compressedStream);
                MemoryStream decompressedStream = new MemoryStream();
                inputStream.CopyTo(decompressedStream);
                inputStream.Close();
                compressedStream.Close();
                byte[] uncompressedData = decompressedStream.ToArray();
                decompressedStream.Close();
                ReadOnlyMemory<byte> mem = new ReadOnlyMemory<byte>(uncompressedData);
                string json = MessagePackSerializer.ConvertToJson(mem);
                return json;
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError(ex.Message);
                return string.Empty;
            }
        }

        /// <summary>
        /// 计算字符串的MD5
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="len16">是否返回16位，默认32位</param>
        /// <param name="capital">是否返回大写，默认小写</param>
        /// <returns></returns>
        public static string EncodeMD5(string input, bool len16, bool capital = false)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            using (MD5 md5 = MD5.Create())
            {
                byte[] buf = Encoding.UTF8.GetBytes(input);
                byte[] data = md5.ComputeHash(buf);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    // 将字节转换为十六进制字符串
                    if (capital)
                        sb.Append(data[i].ToString("X2"));
                    else
                        sb.Append(data[i].ToString("x2"));
                }
                string hash = sb.ToString();
                if (len16)
                {
                    // 16位
                    hash = hash.Substring(8, 16);
                }
                return hash;
            }
        }

        public static string Utf8ToUnicode(String utf8)
        {
            if (utf8 == null || utf8.Length == 0)
                return utf8;

            // Convert the string into a byte[].
            byte[] utf8Bytes = Encoding.UTF8.GetBytes(utf8);
            // Perform the conversion from one encoding to the other.
            byte[] defaultBytes = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, utf8Bytes);
            char[] defaultChars = new char[Encoding.Unicode.GetCharCount(defaultBytes, 0, defaultBytes.Length)];
            Encoding.Unicode.GetChars(defaultBytes, 0, defaultBytes.Length, defaultChars, 0);
            String result = new String(defaultChars);

            return result;
        }

        public static string GB2312ToUtf8(string text)
        {
            if (text == null)
                return null;
            if (text.Length == 0)
                return "";

            try
            {
                System.Text.Encoding utf8 = System.Text.Encoding.GetEncoding("utf-8");
                System.Text.Encoding gb2312 = System.Text.Encoding.GetEncoding("gb2312");
                byte[] gb = gb2312.GetBytes(text);
                gb = System.Text.Encoding.Convert(gb2312, utf8, gb);
                return utf8.GetString(gb);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string ClientVersion2String(uint uiClientVersion)
        {
            uint v1 = uiClientVersion & 0x000000ff;
            uint v2 = uiClientVersion & 0x0000ff00;
            uint v3 = uiClientVersion & 0x00ff0000;
            v2 >>= 8;
            v3 >>= 16;

            string str = string.Format("{0}.{1}.{2}", v3, v2, v1);
            return str;
        }

        public static bool IsFileExistByPath(String path)
        {
            if (path == null || path.Length == 0)
                return false;

            FileInfo info = new FileInfo(path);
            if (info == null || info.Exists == false)
                return false;

            return true;
        }

        public static string GetTimeMMddHHmm()
        {
            return DateTime.Now.ToString("MM-dd HH:mm");
        }

        /// <summary>
        /// 左移位
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="bits">位数</param>
        /// <returns>结果</returns>
        public static int ShiftLeft(int value, int bits)
        {
            return (value << bits);
        }

        /// <summary>
        /// 右移位
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="bits">位数</param>
        /// <returns>结果</returns>
        public static int ShiftRight(int value, int bits)
        {
            return (value >> bits);
        }

        /// <summary>
        /// 按位与
        /// </summary>
        /// <param name="value1">数值1</param>
        /// <param name="value2">数值2</param>
        /// <returns>结果</returns>
        public static int BitwiseAnd(int value1, int value2)
        {
            return (value1 & value2);
        }

        /// <summary>
        /// value1按位与value2之后是否等于value2
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static bool BitwiseAndEqual(int value1, int value2)
        {
            int ret = (value1 & value2);
            return (ret == value2);
        }

        /// <summary>
        /// 按位或
        /// </summary>
        /// <param name="value1">数值1</param>
        /// <param name="value2">数值2</param>
        /// <returns>结果</returns>
        public static int BitwiseOr(int value1, int value2)
        {
            return (value1 | value2);
        }

        private static Random _random = null;

        /// <summary>
        /// 返回[0， 1]区间的浮点数
        /// </summary>
        /// <returns></returns>
        public static float RandFloat()
        {
            if (_random == null)
                _random = new Random();
            int ret = _random.Next(0, 1001);
            return (float)ret / 1000.0f;
        }

        /// <summary>
        /// 本地时间转换到Unix时间戳，单位秒
        /// </summary>
        /// <param name="dateTime">本地时间，例如DateTime.Now</param>
        /// <returns>Unix时间戳</returns>
        [BlackList]
        public static long DateTimeToTimestamp(DateTime dateTime)
        {
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = dateTime.ToUniversalTime() - unixEpoch;
            return (long)timeSpan.TotalSeconds;
        }

        /// <summary>
        /// Unix时间戳转换到本地时间
        /// </summary>
        /// <param name="timestamp">Unix时间戳</param>
        /// <returns>本地时间</returns>
        [BlackList]
        public static DateTime TimestampToDateTime(long timestamp)
        {
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime dateTime = unixEpoch.AddSeconds(timestamp);
            return dateTime.ToLocalTime();
        }

        public static void CopyToClipboard(string text)
        {
            Clipboard.CopyToClipboard(text);
        }

        public static bool IsPasswordValid(string password, ref string errMsg)
        {
            if (string.IsNullOrEmpty(password))
            {
                errMsg = "密码为空";
                return false;
            }
            if (password.Length < 6)
            {
                errMsg = "密码长度小于6个字符";
                return false;
            }
            char[] arr = password.ToCharArray();
            int length = arr.Length;
            char lowerA = 'a';
            char lowerZ = 'z';
            char upperA = 'A';
            char upperZ = 'Z';
            char zero = '0';
            char nine = '9';
            HashSet<char> specialLetters = new HashSet<char>();
            bool test1 = false;
            bool test2 = false;
            bool test3 = false;
            bool test4 = false;
            foreach (char c in SpecialLetters)
                specialLetters.Add(c);
            for (int i = 0; i < length; i++)
            {
                if (arr[i] >= lowerA && arr[i] <= lowerZ)
                {
                    test1 = true;
                    continue;
                }
                if (arr[i] >= upperA && arr[i] <= upperZ)
                {
                    test2 = true;
                    continue;
                }
                if (arr[i] >= zero && arr[i] <= nine)
                {
                    test3 = true;
                    continue;
                }
                if (specialLetters.Contains(arr[i]))
                {
                    test4 = true;
                    continue;
                }
                errMsg = "密码包含非法字符";
                return false;
            }
            int types = 0;
            if (test1 || test2)
                types++;
            if (test3)
                types++;
            if (test4)
                types++;
            /*if (types < 2) {
                errMsg = "密码必须包含字母、数字、特殊符号中的两种";
                return false;
            }*/
            return true;
        }
    }
}

