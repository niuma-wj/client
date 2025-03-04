using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace NiuMa
{
    public static class CommonUtil
    {
        public static Texture2D Base64StringToTexture(string base64Str)
        {
            try
            {
                // 将base64头部信息去掉
                base64Str = base64Str.Replace("data:image/png;base64,", "").Replace("data:image/jgp;base64,", "")
                    .Replace("data:image/jpg;base64,", "").Replace("data:image/jpeg;base64,", "");
                byte[] bytes = System.Convert.FromBase64String(base64Str);
                Texture2D texture = new Texture2D(10, 10);
                texture.LoadImage(bytes);
                return texture;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public static void SaveTextToFile(string fileName, string text)
        {
            string path = Path.Combine(Application.persistentDataPath, fileName);
            File.WriteAllText(path, text);
        }

        public static string ReadTextFromFile(string fileName)
        {
            string path = Path.Combine(Application.persistentDataPath, fileName);
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            return null;
        }
    }
}