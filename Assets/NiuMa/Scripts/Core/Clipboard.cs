using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class Clipboard
{
/*#if (UNITY_EDITOR || UNITY_STANDALONE)
#elif UNITY_IOS
    // 对ios剪切板的调用
    [DllImport("__Internal")]
    public static extern void _copyTextToClipboard(string text);
#endif*/
    // 剪切文本
    public static void CopyToClipboard(string input)
    {
        GUIUtility.systemCopyBuffer = input;
/*#if (UNITY_EDITOR || UNITY_STANDALONE)
#elif UNITY_ANDROID
        // 对Android的调用
        AndroidJavaObject androidObject = new AndroidJavaObject("ClipboardTools");
        AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");

        if (activity == null)
            return;
        // 复制到剪贴板
        androidObject.Call("copyTextToClipboard", activity, input);

        // 从剪贴板中获取文本
        // String text = androidObject.Call<String>("getTextFromClipboard");
#elif UNITY_IPHONE
        // 调用clipboard.h中的接口  
        _copyTextToClipboard(input);  
        // Debug.LogError("CopyToClipboard_______" + input);
#endif*/
    }
}
