using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using DG.Tweening;
using NiuMa;

public class Client : MonoBehaviour
{
    private GameObject _load = null;
    private GameObject _login = null;

#if (UNITY_EDITOR || UNITY_STANDALONE)
#else
    // 是否正在获取定位信息
    private bool _locating = false;

    // 上次获取定位已过了多久(每5分钟更新一次)
    private float _locateElapsed = 300.0f;
#endif

#if UNITY_EDITOR
#else
    private string _logText = "";
    private GUIStyle _guiStyle = new GUIStyle();
#endif

    void Awake()
    {
        Transform child = gameObject.transform.Find("Load");
        if (child != null)
            _load = child.gameObject;
        child = gameObject.transform.Find("Login");
        if (child != null)
            _login = child.gameObject;

        GameManager.Instance.HallPanel = gameObject.transform.Find("HallPanel");
        child = gameObject.transform.Find("GamePanel");
        if (child != null)
        {
            GameManager.Instance.GamePanel = child;
            GameManager.Instance.GameHallParent = child.Find("Hall");
            GameManager.Instance.GameRoomParent = child.Find("Game");
        }
        GameManager.Instance.TipPanel = gameObject.transform.Find("TipPanel");
        GameManager.Instance.PromptPanel = gameObject.transform.Find("PromptPanel");

        child = gameObject.transform.Find("WaitPanel");
        Transform child1 = null;
        if (child != null)
        {
            GameManager.Instance.WaitPanel = child;
            child1 = child.Find("WaitLogin");
            if (child1 != null)
                GameManager.Instance.WaitLogin = child1.gameObject;
            child1 = child.Find("Waiting");
            if (child1 != null)
                GameManager.Instance.Waiting = child1.gameObject;
        }
        child = gameObject.transform.Find("Connect");
        if (child != null)
            GameManager.Instance.Connecting = child.gameObject;

        GameObject cam = GameObject.Find("Main Camera");
        if (cam != null)
        {
            AudioSource bgMusic = cam.GetComponent<AudioSource>();
            if (bgMusic != null)
                AudioManager.Instance.SetBackgroundMusic(bgMusic);
        }
#if UNITY_EDITOR
#else
        if (GameManager.Instance.DebugMode)
            Application.logMessageReceived += ShowTips;
        _guiStyle.normal.textColor = new Color(0.86f, 0.72f, 0.18f);
        _guiStyle.alignment = TextAnchor.LowerLeft;
#endif
    }

    // Use this for initialization
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        // 限制帧率，GameObject
        Application.targetFrameRate = 30;

        DOTween.Init();

        AudioManager.Instance.Initialize();
        NetworkManager.Instance.Initialize();
    }
	
	// Update is called once per frame
	void Update()
    {
        UnityMainThreadDispatcher.Update();

        LuaManager.Instance.Update(Time.time);

        MsgManager.Instance.HandleMessages();

        NetworkManager.Instance.CheckConnection(Time.unscaledDeltaTime);

        GameManager.Instance.Update();

#if (UNITY_EDITOR || UNITY_STANDALONE)
#else
        LocationCoroutine();
        ResourceManager.Instance.Update();
#endif
    }

    private void OnDestroy()
    {
#if UNITY_EDITOR
#else
        if (GameManager.Instance.DebugMode)
            Application.logMessageReceived -= ShowTips;
#endif
    }

#if UNITY_EDITOR
#else
    void OnGUI()
    {
        if (!GameManager.Instance.DebugMode)
            return;

        if ((_logText != null) && (_logText.Length > 0))
        {
            Rect rect = new Rect(60, 20, Screen.width - 60, Screen.height - 20);
            GUI.Label(rect, _logText, _guiStyle);
        } 
    }

    void ShowTips(string msg, string stackTrace, LogType type)
    {
        _logText += msg;
        _logText += "\n";

        if (_logText.Length > 2000)
            _logText = _logText.Remove(0, _logText.Length - 2000);
    }
#endif

    public void OnLoadCompleted()
    {
        if (_load != null)
            _load.SetActive(false);
        if (_login != null)
        {
            _login.SetActive(true);
            Login lg = _login.GetComponent<Login>();
            if (lg != null)
                lg.PlayBackgroundMusic();
        }
    }

#if (UNITY_EDITOR || UNITY_STANDALONE)
#else
    public void LocationCoroutine()
    {
        if (_locating)
            return;
        _locateElapsed += Time.unscaledDeltaTime;
        if (_locateElapsed < 300.0f)
            return;

        _locating = false;
        _locateElapsed = 0.0f;

        StartCoroutine("GetLocation");
    }

    private IEnumerator GetLocation()
    {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("GPS is disabled!");
            yield break;
        }
        _locating = true;
        // Start service before querying location
        Input.location.Start();
        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }
        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            Debug.Log("Location timed out!");
        }
        else if (Input.location.status == LocationServiceStatus.Failed)
        {
            // Connection has failed
            Debug.Log("Unable to determine device location");
        }
        else if (Input.location.status == LocationServiceStatus.Running)
        {
            // Access granted and location value could be retrieved
            GameManager.Instance.Latitude = Input.location.lastData.latitude;
            GameManager.Instance.Longitude = Input.location.lastData.longitude;
            GameManager.Instance.Altitude = Input.location.lastData.altitude;
            GameManager.Instance.OnGetLocation();
        }
        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
        _locating = false;
    }
#endif
}