using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using NiuMa;
using LitJson;

/// <summary>
/// AssetBundle更新流程：
/// 1、从远程服务器下载包含全部AssetBundle文件及对应哈希值的清单文件（files.txt），
///    其文件内容为xxx.ab|哈希值，如：biji/room.ab|bc72636de606a0c6280e16e2cb06deb8。
///    然后加载本地保存的files.txt，对比远程files.txt和本地files.txt文件中的哈希值，
///    找出并记录全部需要更新的AssetBundle文件。
/// 2、从远程服务器下载全部AssetBundle文件的依赖AssetBundle文件（Dependencies.xml），
///    并从依赖文件中加载AssetBundle文件的依赖关系。
/// 3、从远程服务器下载包含进入主界面所必需的AssetBundle文件的主清单文件（MainManifest.xml），
///    其文件内容如下：
///    <MainManifest>
///      <AssetBundle>common.ab</AssetBundle>
///      <AssetBundle>configs.ab</AssetBundle>
///      ...
///    <MainManifest>
/// 4、检查主清单文件中全部需要更新的AssetBundle文件，并依次下载和替换本地对应的旧版本AssetBundle文件。
/// 5、从远程服务器下载包含各个游戏所必需的全部AssetBundle文件的清单文件（GameManifest.xml），
///    其文件内容如下：
///    <GameManifest>
///      <Game Name="Mahjong">
///        <AssetBundle>mahjong/hall.ab</AssetBundle>
///        <AssetBundle>mahjong/room.ab</AssetBundle>
///        ...
///      </Game>
///      <Game Name="BiJi">
///        <AssetBundle>biji/room.ab</AssetBundle>
///        <AssetBundle>biji/room_chat.ab</AssetBundle>
///        ...
///      </Game>
///    <GameManifest>
/// 6、检查游戏清单文件中各个游戏需要更新的全部AssetBundle文件，并记录哪些游戏需要更新才能进入。
/// 7、在用户点击进入游戏时，如果游戏需要更新，则依次下载和替换本地对应的旧版本AssetBundle文件。
/// </summary>
public class Load : MonoBehaviour
{
    public Client _client = null;

#if UNITY_EDITOR
#elif (UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS)
    private List<string> _mainAssetBundles = new List<string>();
    private List<string> _newAssetBundles = new List<string>();
    private int _current = 0;
#endif
    private Slider _progress = null;
    private Text _tip = null;
    private Text _percent = null;

    private void Awake()
    {
        Transform child = gameObject.transform.Find("Progress");
        if (child != null)
        {
            _progress = child.gameObject.GetComponent<Slider>();
            child = child.Find("Text");
            if (child != null)
                _percent = child.gameObject.GetComponent<Text>();
        }
        child = gameObject.transform.Find("Tip");
        if (child != null)
            _tip = child.gameObject.GetComponent<Text>();
    }

    // Use this for initialization
    private void Start()
    {
        //byte[] bytes = File.ReadAllBytes("D:/Work/Project/NiuMa/Client/AssetBundle/android/Dependencies.xml");
        //string text = System.Text.Encoding.UTF8.GetString(bytes);
        //ResourceManager.Instance.OnDownloadDependencies(text);
        LoadConfig();
    }

    // Update is called once per frame
    private void Update() {}

    /// <summary>
    /// 加载配置文件
    /// </summary>
    private void LoadConfig()
    {
        const string url = "http://192.168.1.2:8080/config.json";
        HttpRequester.Instance.Get(url, null, OnLoadConfig);
    }

    private void OnLoadConfig(int code, string text)
    {
        string errMsg = null;
        if (code == 200)
        {
            JsonData data = null;
            try
            {
                string httpHost = null;
                string assetBundleHost = null;
                data = JsonMapper.ToObject(text);
                if (data.ContainsKey("httpHost"))
                    httpHost = data["httpHost"].ToString();
                if (data.ContainsKey("assetBundleHost"))
                {
                    assetBundleHost = data["assetBundleHost"].ToString();
#if UNITY_STANDALONE_WIN
                    assetBundleHost = assetBundleHost + "windows/";
#elif UNITY_ANDROID
                    assetBundleHost = assetBundleHost + "android/";
#elif UNITY_IOS
                    assetBundleHost = assetBundleHost + "ios/";
#endif
                }
                if (string.IsNullOrEmpty(httpHost) || string.IsNullOrEmpty(assetBundleHost))
                    errMsg = "加载配置错误：缺少必要信息";
                else
                {
                    ResourceManager.Instance.HttpHost = httpHost;
                    ResourceManager.Instance.AssetBundleHost = assetBundleHost;
                    BeginLoad();
                }
            }
            catch (Exception ex)
            {
                errMsg = "加载配置错误：" + ex.Message;
            }
        }
        else
        {
            errMsg = "加载配置错误：" + text;
        }
        if (!string.IsNullOrEmpty(errMsg))
        {
            Debug.LogError(errMsg);
            GameManager.Instance.ShowPromptDialog(errMsg);
        }
    }

    private void BeginLoad()
    {
#if UNITY_EDITOR
        LoadComplete();
#elif (UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS)
        string localPath = ResourceManager.GetLocalAssetBundlePath();
        if (localPath.Length > 0)
        {
            // 首先确保AssetBundle目录存在
            if (!Directory.Exists(localPath))
                Directory.CreateDirectory(localPath);
        }
        if (_progress != null)
            _progress.value = 0.0f;
        if (_tip != null)
            _tip.text = "正在下载更新...";
        if (_percent != null)
            _percent.text = "0%";
        DownloadHashFile();
#endif
    }

#if UNITY_EDITOR
#elif (UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS)
    private void DownloadHashFile()
    {
        string url = ResourceManager.Instance.AssetBundleHost + "files.txt";
        HttpRequester.Instance.Get(url, null, OnDownloadHashFile);
    }

    private void OnDownloadHashFile(int code, string text)
    {
        if (code == 200)
        {
            ResourceManager.Instance.OnDownloadRemoteHashes(text);
            // 下载依赖文件
            DownloadDependencies();
        }
        else
        {
            string errMsg = "下载哈希文件错误: " + text;
            Debug.LogError(errMsg);
            GameManager.Instance.ShowPromptDialog(errMsg);
        }
    }

    private void DownloadDependencies()
    {
        string url = ResourceManager.Instance.AssetBundleHost + "Dependencies.xml";
        HttpRequester.Instance.Get(url, null, OnDownloadDependencies);
    }

    private void OnDownloadDependencies(int code, string text)
    {
        if (code == 200)
        {
            ResourceManager.Instance.OnDownloadDependencies(text);
            // 下载主清单
            DownloadMainManifest();
        }
        else
        {
            string errMsg = "下载依赖文件错误: " + text;
            Debug.LogError(errMsg);
            GameManager.Instance.ShowPromptDialog(errMsg);
        }
    }

    private void DownloadMainManifest()
    {
        string url = ResourceManager.Instance.AssetBundleHost + "MainManifest.xml";
        HttpRequester.Instance.Get(url, null, OnLoadMainManifest); 
    }

    private void OnLoadMainManifest(int code, string text)
    {
        if (code == 200)
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(text);

                XmlElement root = xml.DocumentElement;
                if (root == null)
                {
                    string errMsg = "加载主清单失败";
                    Debug.LogError(errMsg);
                    GameManager.Instance.ShowPromptDialog(errMsg);
                    return;
                }
                _mainAssetBundles.Clear();
                XmlNodeList childNodes = root.ChildNodes;
                for (int i = 0; i < childNodes.Count; i++)
                {
                    XmlElement elt = childNodes[i] as XmlElement;
                    if (elt == null)
                        continue;
                    if (elt.Name.Equals("AssetBundle"))
                        _mainAssetBundles.Add(elt.InnerText);
                }
            }
            catch (System.Exception e)
            {
                string errMsg = "加载主清单错误：" + e.Message;
                Debug.LogError(errMsg);
                GameManager.Instance.ShowPromptDialog(errMsg);
                return;
            }
            _newAssetBundles.Clear();
            foreach (string ab in _mainAssetBundles)
            {
                if (ResourceManager.Instance.IsAssetBundleOutOfDate(ab))
                    _newAssetBundles.Add(ab);
            }
            // 开始下载新的ab
            DownloadNextAssetBundle();
        }
        else
        {
            string errMsg = "下载主清单错误：" + text;
            Debug.LogError(errMsg);
            GameManager.Instance.ShowPromptDialog(errMsg);
        }
    }

    private void DownloadNextAssetBundle()
    {
        if (_current < _newAssetBundles.Count)
        {
            float ratio = (float)_current / (float)_newAssetBundles.Count;
            ratio *= 0.65f;
            ratio += 0.05f;
            if (_progress != null)
                _progress.value = ratio;
            if (_percent != null)
            {
                ratio *= 100.0f;
                _percent.text = string.Format("{0:0.#}%", ratio);
            }
            if (_tip != null)
                _tip.text = "正在下载文件:" + _newAssetBundles[_current];
            DownloadAssetBundle();
        }
        else
        {
            // 下载完成，更新本地哈希并保存哈希文件
            foreach (string ab in _newAssetBundles)
                ResourceManager.Instance.UpdateLocalHash(ab);
            ResourceManager.Instance.SaveLocalHashes();

            DownloadGameManifest();
        }
    }

    private void DownloadAssetBundle()
    {
        if (_current < _newAssetBundles.Count && _current >= 0)
        {
            string url = ResourceManager.Instance.AssetBundleHost + _newAssetBundles[_current];
            string path = ResourceManager.GetLocalAssetBundlePath();
            path = path + _newAssetBundles[_current];
            HttpRequester.Instance.Download(url, path, OnAssetBundleDownloaded);
        }
    }

    private void OnAssetBundleDownloaded(int code, string path)
    {
        if (code == 200)
        {
            _current++;
            DownloadNextAssetBundle();
        }
        else
        {
            Debug.LogErrorFormat("Download asset bundle \"{0}\" error: {1}", _newAssetBundles[_current], path);
            GameManager.Instance.ShowPromptDialog("下载更新包失败，请检查网络状态。");
        }
    }

    private void DownloadGameManifest()
    {
        string url = ResourceManager.Instance.AssetBundleHost + "GameManifest.xml";
        HttpRequester.Instance.Get(url, null, OnGameManifestDownloaded);
    }

    private void OnGameManifestDownloaded(int code, string text)
    {
        if (code == 200)
        {
            GameManifest.Instance.LoadManifest(text);
            GameManifest.Instance.SeekGameOutOfDate();

            // 加载完成
            LoadComplete();
        }
        else
        {
            string errMsg = "下载游戏清单错误：" + text;
            Debug.LogError(errMsg);
            GameManager.Instance.ShowPromptDialog(errMsg);
        }
    }
#endif

    private void LoadComplete()
    {
        if (_progress != null)
            _progress.value = 1.0f;
        if (_tip != null)
            _tip.text = "加载完成";
        if (_percent != null)
            _percent.text = "100%";

        if (_client != null)
            _client.OnLoadCompleted();
    }
}