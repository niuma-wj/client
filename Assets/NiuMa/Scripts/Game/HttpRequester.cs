using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using XLua;

namespace NiuMa
{
    /// <summary>
    /// Http请求器类，因为要发起携程，所以该类继承于MonoBehaviour，在使用的时候注意
    /// 全局只需要一个GameObject实例挂了该脚本
    /// </summary>
    [LuaCallCSharp]
    public class HttpRequester : MonoBehaviour
    {
        private static HttpRequester _instance = null;

        public static HttpRequester Instance
        {
            get { return _instance; }
        }

        private static void setInstance(HttpRequester inst)
        {
            if (_instance == null)
                _instance = inst;
        }

        void Awake()
        {
            HttpRequester.setInstance(this);
        }

        #region Get请求
        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="actionResult"></param>
        public void Get(string url, Dictionary<string, string> headers = null, Action<int, string> actionResult = null)
        {
            if (string.IsNullOrEmpty(url))
                return;
            StartCoroutine(_Get(url, headers, actionResult));
        }

        private IEnumerator _Get(string url, Dictionary<string, string> headers = null, Action<int, string> action = null)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                if ((headers != null) && (headers.Count > 0))
                {
                    foreach (KeyValuePair<string, string> pr in headers)
                    {
                        request.SetRequestHeader(pr.Key, pr.Value);
                    }
                }
                yield return request.SendWebRequest();

                int code = 200;
                string text = "";
                if (request.result == UnityWebRequest.Result.Success)
                {
                    text = request.downloadHandler.text;
                }
                else if (request.result == UnityWebRequest.Result.ProtocolError)
                {
                    code = (int)request.responseCode;
                    text = request.downloadHandler.text;
                }
                else
                {
                    code = -1;
                    text = request.error;
                }
                if (action != null)
                {
                    action(code, text);
                }
            }
        }
        #endregion

        #region POST请求
        public void Post(string url, Dictionary<string, string> headers = null, string body = null, Action<int, string> actionResult = null)
        {
            if (string.IsNullOrEmpty(url))
                return;
            StartCoroutine(_Post(url, headers, body, actionResult));
        }

        private IEnumerator _Post(string url, Dictionary<string, string> headers = null, string body = null, Action<int, string> action = null)
        {
            using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
            {
                if ((headers != null) && (headers.Count > 0))
                {
                    foreach (KeyValuePair<string, string> pr in headers)
                    {
                        request.SetRequestHeader(pr.Key, pr.Value);
                    }
                }
                if (!string.IsNullOrEmpty(body))
                    request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(body));
                request.SetRequestHeader("content-type", "application/json;charset=utf-8");
                request.downloadHandler = new DownloadHandlerBuffer();

                yield return request.SendWebRequest();

                int code = 200;
                string text = "";
                if (request.result == UnityWebRequest.Result.Success)
                {
                    text = request.downloadHandler.text;
                }
                else if (request.result == UnityWebRequest.Result.ProtocolError)
                {
                    code = (int)request.responseCode;
                    text = request.downloadHandler.text;
                }
                else
                {
                    code = -1;
                    text = request.error;
                }
                if (action != null)
                {
                    action(code, text);
                }
            }
        }
        #endregion

        #region 下载图片
        public void GetTexture(string url, Action<Texture2D> actionResult)
        {
            if (string.IsNullOrEmpty(url))
                return;
            StartCoroutine(_GetTexture(url, actionResult));
        }

        private IEnumerator _GetTexture(string url, Action<Texture2D> actionResult)
        {
            using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    // 从下载处理器获取纹理
                    DownloadHandlerTexture handler = (DownloadHandlerTexture)request.downloadHandler;
                    Texture2D tex = null;
                    if (handler != null)
                        tex = handler.texture;
                    if (tex != null)
                        actionResult(tex);
                }
                else
                {
                    Debug.LogErrorFormat("Download image({0}) error: {1}", url, request.error);
                }
            } 
        }
        #endregion

        #region 下载文件
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url">下载地址</param>
        /// <param name="path">存储路径(包含文件名)</param>
        /// <param name="actionResult">下载成功回调</param>
        public void Download(string url, string path, Action<int, string> actionResult = null)
        {
            if (string.IsNullOrEmpty(url))
                return;
            StartCoroutine(_Download(url, path, actionResult));
        }

        private IEnumerator _Download(string url, string path, Action<int, string> actionResult)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();

                int code = 200;
                string errMsg = "";
                if (request.result == UnityWebRequest.Result.Success)
                {
                    try
                    {
                        byte[] data = request.downloadHandler.data;
                        if (data != null && data.Length > 0)
                        {
                            int pos = path.LastIndexOf('/');
                            if (pos > 0)
                            {
                                string dir = path.Substring(0, pos);
                                if (!Directory.Exists(dir))
                                    Directory.CreateDirectory(dir);
                            }
                            if (File.Exists(path))
                                File.Delete(path);
                            File.WriteAllBytes(path, data);
                        }
                    }
                    catch (Exception ex)
                    {
                        code = -1;
                        errMsg = ex.Message;
                    }
                }
                else if (request.result == UnityWebRequest.Result.ProtocolError)
                {
                    code = (int)request.responseCode;
                    //Debug.LogErrorFormat("Download file({0}) error: {1}", url, request.downloadHandler.text);
                    errMsg = request.downloadHandler.text;
                }
                else
                {
                    code = -1;
                    //Debug.LogErrorFormat("Download file({0}) error: {1}", url, request.error);
                    errMsg = request.error;
                }
                if (actionResult != null)
                {
                    if (code == 200)
                        actionResult(code, path);
                    else
                        actionResult(code, errMsg);
                }
            }
        }
        #endregion
    }
}