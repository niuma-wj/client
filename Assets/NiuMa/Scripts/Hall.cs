using System;
using NiuMa;
using UnityEngine;
using UnityEngine.UI;
using XLua;

public class Hall : LuaBehaviour
{
    private Text _labelID = null;
    private GameObject _gameList = null;
    private Action _onCapitalChange = null;
    private Action<string> _gameClickAction = null;

    protected override void AwakeEx(LuaTable module)
    {
        base.AwakeEx(module);
        Transform child = gameObject.transform.Find("Canvas/Player/TextID");
        if (child != null)
            _labelID = child.gameObject.GetComponent<Text>();
        _onCapitalChange = module.Get<Action>("OnCapitalChange");
        _gameClickAction = module.Get<Action<string>>("OnGameClick");
        GameManager.Instance.Hall = this;
    }

    protected override void StartEx()
    {
        base.StartEx();
        if (_labelID != null)
            _labelID.text = string.Format("{0}", GameManager.Instance.PlayerId);

        Transform child = gameObject.transform.Find("Canvas/GameList");
        if (child != null)
        {
            UnityEngine.Object obj = ResourceManager.Instance.LoadResource("Prefabs/GameList", "prefabs/gamelist.ab", "Assets/NiuMa/Resources/", ".prefab");
            GameObject prefab = obj as GameObject;
            if (prefab != null)
                _gameList = GameObject.Instantiate<GameObject>(prefab, child);
        }
    }

    protected override void OnDestroyEx()
    {
        _onCapitalChange = null;
        _gameClickAction = null;
        GameManager.Instance.Hall = null;
    }

    public void OnGameClick(string gameName)
    {
        if (_gameClickAction != null)
            _gameClickAction(gameName);
    }

    public void OnCapitalChange()
    {
        if (_onCapitalChange != null)
            _onCapitalChange();
    }

    public void PlayBackgroundMusic()
    {
        AudioSource source = AudioManager.Instance.BackgroundMusic;
        if (source == null)
            return;
        string resName = "Sounds/Main/bg_hall";
        AudioClip audioClip = ResourceManager.Instance.LoadResource(resName, "sounds/main.ab", "Assets/NiuMa/Resources/", ".mp3") as AudioClip;
        if (audioClip != null)
        {
            source.clip = audioClip;
            source.Play();
        }
    }
}
