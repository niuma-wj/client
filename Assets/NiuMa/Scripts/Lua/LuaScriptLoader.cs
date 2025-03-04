using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NiuMa
{
    public static class LuaScriptLoader
    {
        public static byte[] ReadFile(ref string fileName)
        {
            UnityEngine.Object obj = null;
            fileName = fileName.Replace('.', '/');
            if (!fileName.EndsWith(".lua"))
                fileName = fileName + ".lua";
#if UNITY_EDITOR
            string resName = "LuaScripts/" + fileName;
            obj = Resources.Load(resName);
#elif (UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS)
            obj = ResourceManager.Instance.LoadResource(fileName, "luascripts.ab");
#endif
            TextAsset text = obj as TextAsset;
            if (text != null)
                return text.bytes;
            return null;
        }
    }
}
