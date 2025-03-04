using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using NiuMa;

public class GameEntry : MonoBehaviour
{
    public string _gameName = "";
    public string _gameAlias = "";
    private GameObject _new = null;
    private GameObject _download = null;
    private Slider _progress = null;
    private Text _precent = null;

#if UNITY_EDITOR
#elif (UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS)
    private List<string> _assetBundles = new List<string>();
    private int _current = 0;
    private bool _outdated = false;     // 是否需要更新
    private bool _downloading = false;  // 是否正在下载AssetBundle
#endif

    private void Awake()
    {
        Transform child = gameObject.transform.Find("Button/New");
        if (child != null)
            _new = child.gameObject;
        child = gameObject.transform.Find("Button/Download");
        if (child != null)
            _download = child.gameObject;
        child = gameObject.transform.Find("Button/Progress");
        if (child != null)
        {
            _progress = child.gameObject.GetComponent<Slider>();
            child = child.Find("Text");
            if (child != null)
                _precent = child.gameObject.GetComponent<Text>();
        }
    }

    // Use this for initialization
    private void Start()
    {
#if UNITY_EDITOR
#elif (UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS)
        _outdated = GameManifest.Instance.IsGameOutdated(_gameName);
        if (_outdated)
        {
            if (_new != null)
                _new.SetActive(true);
            if (_download != null)
                _download.SetActive(true);
        }
        else
            GameManager.Instance.AddGameLoaded(_gameName);
#endif
    }

    // Update is called once per frame
    private void Update()
    {}

    public void OnClick()
    {
#if UNITY_EDITOR
        EnterGame();
#elif (UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS)
        if (_downloading)
            return;
        if (_outdated)
        {
            _assetBundles.Clear();
            _current = 0;
            GameManifest.Instance.GetGameOutdatedABs(_gameName, ref _assetBundles);
            if (_assetBundles.Count > 0)
            {
                _downloading = true;

                if (_progress != null)
                    _progress.gameObject.SetActive(true);
                DownloadNextAssetBundle();
            }
            else
                SetUpdated();
        }
        else
            EnterGame();
#endif
    }

#if UNITY_EDITOR
#elif (UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS)
    private void SetUpdated()
    {
        GameManifest.Instance.SetGameUpdated(_gameName);
        _outdated = false;
        if (_new != null)
            _new.SetActive(false);
        if (_download != null)
            _download.SetActive(false);
    }

    private void DownloadNextAssetBundle()
    {
        if (_current < _assetBundles.Count)
        {
            float ratio = (float)_current / (float)_assetBundles.Count;
            if (_progress != null)
                _progress.value = ratio;
            ratio *= 100.0f;
            if (_precent != null)
                _precent.text = string.Format("{0}%", (int)ratio);

            DownloadAssetBundle();
        }
        else
        {
            _downloading = false;
            if (_progress != null)
                _progress.gameObject.SetActive(false);

            SetUpdated();

            // 下载完成，更新本地哈希并保存哈希文件
            foreach (string ab in _assetBundles)
                ResourceManager.Instance.UpdateLocalHash(ab);
            ResourceManager.Instance.SaveLocalHashes();
            string tip = "游戏【" + _gameAlias + "】更新完成！";
            GameManager.Instance.ShowPromptTip(tip, 3.0f);
            GameManager.Instance.AddGameLoaded(_gameName);
        }
    }

    private void DownloadAssetBundle()
    {
        if (_current < _assetBundles.Count && _current >= 0)
        {
            string name = _assetBundles[_current];
            string url = ResourceManager.Instance.AssetBundleHost + name;
            string path = ResourceManager.GetLocalAssetBundlePath();
            path = path + name;
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
            Debug.LogErrorFormat("Update game \"{0}\" failed, download asset bundle \"{1}\" error: {2}", _gameName, _assetBundles[_current], path);
            GameManager.Instance.ShowPromptDialog("更新游戏失败，请检查网络状态。");
        }
    }
#endif

    private void EnterGame()
    {
        Hall hall = GameManager.Instance.Hall;
        if (hall != null)
            hall.OnGameClick(_gameName);
    }
}