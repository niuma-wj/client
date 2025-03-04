using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine;

namespace NiuMa
{
    /// <summary>
    /// 游戏清单，用于子游戏的版本管理及动态更新
    /// </summary>
    public class GameManifest
    {
        // 禁止外部创建
        private GameManifest() {}
        private static readonly GameManifest _instance = new GameManifest();

        public static GameManifest Instance
        {
            get { return _instance; }
        }

#if UNITY_EDITOR
#elif (UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS)
        // 游戏表，Key为游戏名称，Value为该游戏所依赖的全部AssetBundle
        private Dictionary<string, HashSet<string>> _gameAssetBundles = new Dictionary<string, HashSet<string>>();

        // 需要更新的游戏
        private HashSet<string> _outdatedGames = new HashSet<string>();

        public void LoadManifest(string text)
        {
            LoadManifest(text, ref _gameAssetBundles);
        }

        private static void LoadManifest(string text, ref Dictionary<string, HashSet<string>> games)
        {
            if (string.IsNullOrEmpty(text) || games == null)
                return;

            games.Clear();
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(text);
                XmlElement root = xml.DocumentElement;
                if (root == null)
                    return;
                LoadGameAssetBundles(root.ChildNodes, ref games);
            }
            catch (Exception e)
            {
                // 加载清单失败
                Debug.LogError("Load game manifest error:" + e.Message);
            }
        }

        private static void LoadGameAssetBundles(XmlNodeList childNodes, ref Dictionary<string, HashSet<string>> games)
        {
            if (childNodes == null || games == null)
                return;

            string gameName = "";
            HashSet<string> temp = null;
            for (int i = 0; i < childNodes.Count; i++)
            {
                XmlElement elt = childNodes[i] as XmlElement;
                if (elt == null)
                    continue;
                if (!elt.Name.Equals("Game"))
                    continue;
                gameName = elt.GetAttribute("Name");
                if (games.ContainsKey(gameName))
                {
                    Debug.LogError("GameManifest:Game '" + gameName + "' is duplicate!");
                    continue;
                }
                temp = new HashSet<string>();
                int count = elt.ChildNodes.Count;
                for (int j = 0; j < count; j++)
                {
                    XmlElement elt1 = elt.ChildNodes[j] as XmlElement;
                    if (elt1 == null)
                        continue;
                    if (!elt1.Name.Equals("AssetBundle"))
                        continue;
                    temp.Add(elt1.InnerText);
                }
                games.Add(gameName, temp);
            }
        }

        // 找出需要更新的游戏
        public void SeekGameOutOfDate()
        {
            _outdatedGames.Clear();
            foreach (KeyValuePair<string, HashSet<string>> pr in _gameAssetBundles)
            {
                foreach (string ab in pr.Value)
                {
                    if (ResourceManager.Instance.IsAssetBundleOutOfDate(ab))
                    {
                        _outdatedGames.Add(pr.Key);
                        break;
                    }
                }
            }
        }

        // 判定游戏是否已经过时
        public bool IsGameOutdated(string game)
        {
            return _outdatedGames.Contains(game);
        }

        // 查找游戏需要更新的全部AssetBundle
        public void GetGameOutdatedABs(string game, ref List<string> assetBundles)
        {
            if (string.IsNullOrEmpty(game) || assetBundles == null)
                return;
            assetBundles.Clear();
            if (!_outdatedGames.Contains(game))
                return;
            HashSet<string> abs = null;
            if (!_gameAssetBundles.TryGetValue(game, out abs))
                return;
            foreach (string ab in abs)
            {
                if (ResourceManager.Instance.IsAssetBundleOutOfDate(ab))
                    assetBundles.Add(ab);
            }
        }

        // 查找游戏需要的全部AssetBundle
        public void GetGameAssetBundles(string game, ref List<string> assetBundles)
        {
            if (string.IsNullOrEmpty(game) || assetBundles == null)
                return;
            assetBundles.Clear();
            HashSet<string> abs = null;
            if (!_gameAssetBundles.TryGetValue(game, out abs))
                return;
            foreach (string ab in abs)
                assetBundles.Add(ab);
        }

        // 设置游戏已经更新
        public void SetGameUpdated(string game)
        {
            if (!_outdatedGames.Contains(game))
                return;

            _outdatedGames.Remove(game);
        }
#endif
    }
}