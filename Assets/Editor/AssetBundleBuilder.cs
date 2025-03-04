using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml;
using System;
using System.Text;

public class AssetBundleBuilder
{
    public static readonly string RESOURCES_PATH = Application.dataPath + "/" + "Resources";

    private class BuildItem
    {
        public string _name = "";       // 包名称
        public string _relative = "";   // 资源名称的相对路径
        public string _path = "";       // 资源相对路径
    }

    [MenuItem("Assets/AssetBundleBuilder/Android Resources")]
    public static void BuildAndroidResources()
    {
        BuildResources(BuildTarget.Android);
    }

    [MenuItem("Assets/AssetBundleBuilder/Android LuaScripts")]
    public static void BuildAndroidLuaScripts()
    {
        BuildLuaScripts(BuildTarget.Android);
    }

    [MenuItem("Assets/AssetBundleBuilder/IOS Resources")]
    public static void BuildIOSResources()
    {
        BuildResources(BuildTarget.iOS);
    }

    [MenuItem("Assets/AssetBundleBuilder/IOS LuaScripts")]
    public static void BuildIOSLuaScripts()
    {
        BuildLuaScripts(BuildTarget.iOS);
    }

    [MenuItem("Assets/AssetBundleBuilder/Windows Resources")]
    public static void BuildWindowsResources()
    {
        BuildResources(BuildTarget.StandaloneWindows64);
    }

    [MenuItem("Assets/AssetBundleBuilder/Windows LuaScripts")]
    public static void BuildWindowsLuaScripts()
    {
        BuildLuaScripts(BuildTarget.StandaloneWindows64);
    }

    private static void BuildResources(BuildTarget targetPlatform)
    {
        if (targetPlatform != BuildTarget.Android &&
            targetPlatform != BuildTarget.iOS &&
            targetPlatform != BuildTarget.StandaloneWindows64)
        {
            Debug.LogError("打包AssetBundle错误，目标平台不支持");
            return;
        }
        string outPath = Application.dataPath + "/../AssetBundle/";
        if (targetPlatform == BuildTarget.Android)
            outPath += "android";
        else if (targetPlatform == BuildTarget.iOS)
            outPath += "ios";
        else if (targetPlatform == BuildTarget.StandaloneWindows64)
            outPath += "windows";
        if (!Directory.Exists(outPath))
            Directory.CreateDirectory(outPath);
        AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(outPath, BuildAssetBundleOptions.None, targetPlatform);
        if (manifest == null)
            return;
        Dictionary<string, string> assetHashes = new Dictionary<string, string>();
        string file = outPath + "/" + "files.txt";
        if (File.Exists(file))
        {
            LoadAssetHashes(file, ref assetHashes);
            File.Delete(file);
        }
        SaveAssetHashes(file, manifest, ref assetHashes);
        // 保存所有AssetBundle的依赖关系
        file = outPath + "/" + "Dependencies.xml";
        SaveDependencies(file, manifest);

        AssetDatabase.Refresh();
    }

    private static void BuildLuaScripts(BuildTarget targetPlatform)
    {
        if (targetPlatform != BuildTarget.Android &&
            targetPlatform != BuildTarget.iOS &&
            targetPlatform != BuildTarget.StandaloneWindows64)
        {
            Debug.LogError("打包AssetBundle错误，目标平台不支持");
            return;
        }
        EditorUtility.DisplayProgressBar("打包AssetBundle", "正在读取配置文件...", 0.0f);

        // 加载配置
        List<BuildItem> buildItems = new List<BuildItem>();
        List<AssetBundleBuild> buildMap = new List<AssetBundleBuild>();
        if (!LoadBuildMap(ref buildItems))
        {
            EditorUtility.ClearProgressBar();
            return;
        }
        EditorUtility.DisplayProgressBar("打包AssetBundle", "正在添加打包项...", 0.1f);
        foreach (BuildItem bi in buildItems)
            AddAssetBundleBuild(bi, ref buildMap);
        if (buildMap.Count == 0)
        {
            EditorUtility.ClearProgressBar();
            return;
        }
        EditorUtility.DisplayProgressBar("打包AssetBundle", "正在打包...", 0.2f);
        string outPath = Application.dataPath + "/../AssetBundle/";
        if (targetPlatform == BuildTarget.Android)
            outPath += "Android";
        else if(targetPlatform == BuildTarget.iOS)
            outPath += "iOS";
        else if (targetPlatform == BuildTarget.StandaloneWindows64)
            outPath += "windows";
        if (!Directory.Exists(outPath))
            Directory.CreateDirectory(outPath);
        AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(outPath, buildMap.ToArray(), BuildAssetBundleOptions.None, targetPlatform);
        if (manifest == null)
        {
            EditorUtility.ClearProgressBar();
            return;
        }
        EditorUtility.DisplayProgressBar("打包AssetBundle", "打包完成，正在统计包Hash...", 0.9f);
        Dictionary<string, string> assetHashes = new Dictionary<string, string>();
        string file = outPath + "/" + "files.txt";
        if (File.Exists(file))
        {
            LoadAssetHashes(file, ref assetHashes);
            File.Delete(file);
        }
        SaveAssetHashes(file, manifest, ref assetHashes);

        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
    }

    private static bool LoadBuildMap(ref List<BuildItem> buildItems)
    {
        if (buildItems == null)
            return false;

        try
        {
            string path = Application.dataPath + "/../AssetBundle/AssetBundle.xml";
            XmlDocument xml = new XmlDocument();
            xml.Load(path);
            XmlElement root = xml.DocumentElement;
            if (root == null)
                return false;
            XmlNodeList childNodes = root.ChildNodes;
            BuildItem bi = null;
            string name = "";
            string relative = "";
            bool test = false;
            for (int i = 0; i < childNodes.Count; i++)
            {
                XmlElement elt = childNodes[i] as XmlElement;
                if (elt == null)
                    continue;
                if (!elt.Name.Equals("AssetBundle"))
                    continue;
                name = elt.GetAttribute("Name");
                relative = elt.GetAttribute("Relative");
                path = elt.InnerText;
                if (name == null || name.Length == 0 ||
                    path == null || path.Length == 0)
                    continue;
                if (relative == null)
                    relative = "";
                relative = relative.Replace('\\', '/');
                path = path.Replace('\\', '/');
                if (relative.Length > 0 && relative[0] == '/')
                    relative = relative.Remove(0, 1);
                if (path[0] == '/')
                    path = path.Remove(0, 1);
                if ((relative.Length > 0) && !path.StartsWith(relative))
                {
                    Debug.LogError("AssetBundle:\"" + name + "\"配置有误，名称相对路径与资源相对路径冲突");
                    continue;
                }
                test = false;
                for (int j = 0; j < buildItems.Count; j++)
                {
                    if (buildItems[j]._name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                    {
                        // 重名的包
                        Debug.LogError("AssetBundle:\"" + name + "\"配置有误，重名的包");
                        test = true;
                        break;
                    }
                    if (buildItems[j]._path.StartsWith(path, StringComparison.CurrentCultureIgnoreCase))
                    {
                        // 过滤重复的包
                        Debug.LogError("AssetBundle:\"" + name + "\"配置有误，资源路径为其他包的子路径");
                        test = true;
                        break;
                    }
                }
                if (test)
                    continue;
                bi = new BuildItem();
                bi._name = name;
                bi._relative = relative;
                bi._path = path;
                buildItems.Add(bi);
            }
        }
        catch (Exception e)
        {
            // 打开配置文件失败
            Debug.LogError(e.Message);
            return false;
        }
        return true;
    }

    private static void AddAssetBundleBuild(BuildItem bi, ref List<AssetBundleBuild> buildMap)
    {
        if (bi == null || buildMap == null)
            return;

        // 资源文件
        List<string> assetFiles = new List<string>();
        // Prefabs文件
        List<string> prefabFiles = new List<string>();
        // 资源名称
        List<string> assetNames = new List<string>();
        string path = Application.dataPath + "/" + bi._path;
        string temp = "Assets/" + bi._relative;
        int pos = 0;
        int length = temp.Length;
        if (temp[length - 1] != '/')
            length += 1;
        SearchAssetFiles(path, ref assetFiles, ref prefabFiles);
        assetFiles.AddRange(prefabFiles);
        if (assetFiles.Count == 0)
            return;
        foreach (string f in assetFiles)
        {
            // 去掉名称相对路径
            temp = f.Remove(0, length);
            // 去掉后缀名，以便跟调用Resources.Load的名称相同
            pos = temp.LastIndexOf('.');
            if (pos != -1)
                temp = temp.Substring(0, pos);  
            assetNames.Add(temp);
        }
        AssetBundleBuild abb = new AssetBundleBuild();
        abb.assetBundleName = bi._name;
        abb.assetNames = assetFiles.ToArray();
        abb.addressableNames = assetNames.ToArray();
        buildMap.Add(abb);
    }

    private static void SearchAssetFiles(string path, ref List<string> assetFiles, ref List<string> prefabFiles)
    {
        if (path == null || path.Length == 0)
            return;
        if (!Directory.Exists(path))
        {
            Debug.LogError("打包AssetBundle错误，路径\"" + path + "\"不存在!");
            return;
        }
        string temp = Application.dataPath;
        int length = temp.Length - 6;   // "Assets"
        string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
        foreach (string f in files)
        {
            if (f.EndsWith(".meta") || f.EndsWith(".proto"))
                continue;

            temp = f.Remove(0, length);
            temp = temp.Replace('\\', '/');
            if (temp.EndsWith(".prefab"))
                prefabFiles.Add(temp);
            else
                assetFiles.Add(temp);
        }
    }

    private static void LoadAssetHashes(string file, ref Dictionary<string, string> assetHashes)
    {
        if (file == null || file.Length == 0)
            return;
        if (assetHashes == null)
            return;
        try
        {
            StreamReader sr = new StreamReader(file, Encoding.Default);
            string line = null;
            string name = null;
            string hash = null;
            int pos = 0;
            while ((line = sr.ReadLine()) != null)
            {
                pos = line.IndexOf('|');
                if (pos == -1)
                    continue;
                name = line.Substring(0, pos);
                hash = line.Substring(pos + 1, (line.Length - (pos + 1)));
                assetHashes.Add(name, hash);
            }
            sr.Close();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    private static void SaveAssetHashes(string file, AssetBundleManifest manifest, ref Dictionary<string, string> assetHashes)
    {
        if (file == null || file.Length == 0)
            return;
        if (manifest == null || assetHashes == null)
            return;
        FileStream fs = new FileStream(file, FileMode.CreateNew);
        StreamWriter sw = new StreamWriter(fs, Encoding.Default);
        string[] assetBundles = manifest.GetAllAssetBundles();
        string key = "";
        string hash = "";
        foreach (string ab in assetBundles)
        {
            hash = manifest.GetAssetBundleHash(ab).ToString();
            key = ab.ToLower();
            assetHashes[key] = hash;
        }
        string[] abs = new string[assetHashes.Keys.Count];
        assetHashes.Keys.CopyTo(abs, 0);
        Array.Sort(abs);
        foreach (string ab in abs)
        {
            if (!assetHashes.TryGetValue(ab, out hash))
                continue;
            sw.WriteLine(ab + "|" + hash);
        }
        sw.Close();
        fs.Close();
    }

    private static void SaveDependencies(string file, AssetBundleManifest manifest)
    {
        string[] assetBundles = manifest.GetAllAssetBundles();
        if (assetBundles == null || assetBundles.Length == 0)
        {
            if (File.Exists(file))
                File.Delete(file);
            return;
        }
        // 创建XmlDocument对象
        XmlDocument doc = new XmlDocument();
        // 加载XML声明
        XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "utf-8", null);
        doc.AppendChild(xmlDeclaration);
        // 创建一个根元素
        XmlElement rootElement = doc.CreateElement("", "Dependencies", "");
        doc.AppendChild(rootElement);

        // 创建并添加子元素
        XmlElement childElement1 = null;
        XmlElement childElement2 = null;
        foreach (string ab in assetBundles)
        {
            string[] dependencies = manifest.GetAllDependencies(ab);
            if (dependencies == null || dependencies.Length == 0)
                continue;
            childElement1 = doc.CreateElement("", "AssetBundle", "");
            childElement1.SetAttribute("Name", ab);
            foreach (string dep in dependencies)
            {
                childElement2 = doc.CreateElement("", "Dependency", "");
                childElement2.InnerText = dep;
                childElement1.AppendChild(childElement2);
            }
            rootElement.AppendChild(childElement1);
        }
        // 保存XML文件
        doc.Save(file);
    }
}