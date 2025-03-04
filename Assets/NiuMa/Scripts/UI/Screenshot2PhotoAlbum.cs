using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using XLua;

namespace UnityEngine
{
    public class Screenshot2PhotoAlbum : MonoBehaviour
    {
        [BlackList]
        public string _fileName = "gzqp_picture";
        private int _no = 1;

        // Use this for initialization
        void Start()
        {}

        // Update is called once per frame
        void Update()
        {}

        // 与调用ios里面的保存相册接口
#if UNITY_IOS
        [DllImport("__Internal")]
        private static extern void _SavePhoto(string readAddr);
#endif

        public void SaveScreenShot()
        {
#if (UNITY_EDITOR || UNITY_STANDALONE)
#elif (UNITY_ANDROID || UNITY_IOS)
        StartCoroutine(CaptureScreenShot());
#endif
        }

        IEnumerator CaptureScreenShot()
        {
            string fileName = string.Format("{0}_{1:D3}.jpg", _fileName, _no++);
            Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
            yield return new WaitForEndOfFrame();
            tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, true);
            tex.Apply();
            yield return tex;
            try
            {
                byte[] buf = tex.EncodeToJPG();
#if (UNITY_EDITOR || UNITY_STANDALONE)
#elif UNITY_ANDROID
                string path = Application.persistentDataPath.Substring(0, Application.persistentDataPath.IndexOf("Android"));
                path += "/Pictures/gzqp";
                string file = path + "/" + fileName;
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                File.WriteAllBytes(file, buf);
                ScanFile(file);
#elif UNITY_IOS
                string path = Application.persistentDataPath;
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                string file = path + "/" + fileName;
                // 保存文件ios需要在本地保存文件后，然后将保存的图片的路径传给ios
                File.WriteAllBytes(file, buf);
                _SavePhoto(file);
#endif
                NiuMa.GameManager.Instance.ShowPromptTip("保存截图成功", 2.0f);
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        void ScanFile(string path)
        {
            /*AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
            AndroidJavaObject jo = new AndroidJavaObject("com.unity3d.player.scanmedia.MainActivity");
            jo.Call("ScanFile", context, path);

            AndroidJavaClass toast = new AndroidJavaClass("android.widget.Toast");
            currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                toast.CallStatic<AndroidJavaObject>("makeText", context, "截图成功保存到相册", toast.GetStatic<int>("LENGTH_LONG")).Call("show");
            }));*/

            //设置成我们aar库中的签名+类名
            AndroidJavaObject javaObj = new AndroidJavaObject("com.guangwei.saveimage.SaveImageActivity");
            if (javaObj != null)
            {
                // 这里我们可以设置保存成功弹窗内容
                javaObj.Call("scanFile", path, "保存图片成功"); 
            }
        }
    }
}
