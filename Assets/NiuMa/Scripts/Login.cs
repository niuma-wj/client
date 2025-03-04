using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NiuMa;
using cn.sharesdk.unity3d;

public class Login : MonoBehaviour
{
    [SerializeField]
    private GameObject _loginPC;

    [SerializeField]
    private GameObject _loginWechat;

    [SerializeField]
    private GameObject _panelLogin;

    [SerializeField]
    private GameObject _panelRegister;

    [SerializeField]
    private InputField _textName = null;

    [SerializeField]
    private InputField _textPassword = null;

    [SerializeField]
    private InputField _textCode = null;

    [SerializeField]
    private RawImage _imgCode = null;

    [SerializeField]
    private InputField _regUsername = null;

    [SerializeField]
    private InputField _regNickname = null;

    [SerializeField]
    private InputField _regPassword = null;

    [SerializeField]
    private InputField _regPasswordConfirm = null;

    [SerializeField]
    private InputField _regCode = null;

    [SerializeField]
    private RawImage _regImgCode = null;

    // 是否已经初始化登录
    private bool _initLogin = false;
#if true // (UNITY_EDITOR || UNITY_STANDALONE)

    // 当前是否显示注册页面
    private bool _showRegister = false;

    // 是否正在获取图形验证码
    private bool _gettingCaptcha = false;

    // 是否正在登录
    private bool _logining = false;

    // 是否正在注册
    private bool _registering = false;

    // 性别
    private int _sex = 1;

    // 验证码uuid
    private string _uuid;
#elif false // (UNITY_ANDROID || UNITY_IOS)
    private ShareSDK _ssdk;
    private int _reqID = 0;
#endif

    void Awake() {}

    // Use this for initialization
    void Start()
    {
        GameManager.Instance.Login = gameObject;

        string token = null;
#if (UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS)
        token = PlayerPrefs.GetString("token");
#endif
        if (!string.IsNullOrEmpty(token))
        {
            // 解密token
            try
            {
                token = AesUtil.Decrypt1(token);
            }
            catch(Exception)
            {
                token = null;
            }
        }
        if (string.IsNullOrEmpty(token))
        {
            InitLogin();
        }
        else
        {
            GameManager.Instance.Token = token;
            GetPlayerInfo();
        }
    }

    // Update is called once per frame
    void Update()
    {}

    private void InitLogin()
    {
        if (_initLogin)
            return;
        _initLogin = true;
        bool showPC = false;
        bool showWechat = false;
#if true // (UNITY_EDITOR || UNITY_STANDALONE)
        showPC = true;
        // 获取图形验证码
        GetCaptchaCode();
#elif false // (UNITY_ANDROID || UNITY_IOS)
        showWechat = true;
        _ssdk = gameObject.GetComponent<ShareSDK>();
        if (_ssdk != null)
        {
            _ssdk.authHandler += this.OnAuthorizeResult;
            GameManager.Instance.SSDK = _ssdk;
        }
#endif
        if (_loginPC != null)
            _loginPC.SetActive(showPC);
        if (_loginWechat != null)
            _loginWechat.SetActive(showWechat);
    }

    public void OnCaptchaClick()
    {
#if true // (UNITY_EDITOR || UNITY_STANDALONE)
        GetCaptchaCode();
#endif
    }

    public void OnWechatLoginClick()
    {
#if true // (UNITY_EDITOR || UNITY_STANDALONE)
#elif false // (UNITY_ANDROID || UNITY_IOS)
        // 微信登陆
        if (_ssdk != null)
        {
            if (_ssdk.IsAuthorized(PlatformType.WeChat))
            {
                Hashtable info = _ssdk.GetAuthInfo(PlatformType.WeChat);
                if (info != null)
                {
                    String text = MiniJSON.jsonEncode(info);
                    //NetworkManager.Instance.Login(true, text, _agencyID);
                }
            }
            else
            {
                _reqID = _ssdk.Authorize(PlatformType.WeChat);
            }
        }
        GameManager.Instance.ShowWaitLogin(true);
#endif
    }


#if true // (UNITY_EDITOR || UNITY_STANDALONE)
    private void GetCaptchaCode()
    {
        if (_gettingCaptcha)
            return;

        _gettingCaptcha = true;
        Debug.Log("Get captcha code");
        string url = ResourceManager.Instance.HttpHost + "/player/captcha-image";
        HttpRequester.Instance.Get(url, null, OnGetCaptchaCode);
    }

    /// <summary>
    /// 获取图形验证码
    /// </summary>
    /// <param name="code">HTTP响应码</param>
    /// <param name="text">若发生错则为错误消息，否则为返回数据</param>
    private void OnGetCaptchaCode(int code, string text)
    {
        _gettingCaptcha = false;
        if (code != 200)
        {
            Debug.LogErrorFormat("Get captcha code error: {0}", text);
            return;
        }
        try
        {
            CaptchaDTO dto = JsonUtility.FromJson<CaptchaDTO>(text);
            _uuid = dto.uuid;
            if (_showRegister)
            {
                if (_regImgCode != null)
                    _regImgCode.texture = CommonUtil.Base64StringToTexture(dto.img);
            }
            else if (_imgCode != null)
                _imgCode.texture = CommonUtil.Base64StringToTexture(dto.img);
        }
        catch (System.Exception ex)
        {
            Debug.LogErrorFormat("Parse captcha code error: {0}", ex.Message);
        }
    }

    /// <summary>
    /// 玩家账户密码登录
    /// </summary>
    public void OnLoginPCClick()
    {
        if (_logining)
            return;
        string name = null;
        string password = null;
        string code = null;
        if (_textName != null)
            name = _textName.text;
        if (_textPassword != null)
            password = _textPassword.text;
        if (_textCode != null)
            code = _textCode.text;
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(code))
            return;

        _logining = true;
        GameManager.Instance.ShowWaitLogin(true);
        name = AesUtil.Encrypt(name);           // 账户名
        password = AesUtil.Encrypt(password);   // 登录密码
        LoginDTO dto = new LoginDTO();
        dto.name = name;
        dto.password = password;
        dto.code = code;
        dto.uuid = _uuid;
        string json = JsonUtility.ToJson(dto);
        string url = ResourceManager.Instance.HttpHost + "/player/login";
        HttpRequester.Instance.Post(url, null, json, OnLoginPlayerPwd);
    }

    private void OnLoginPlayerPwd(int code, string text)
    {
        _logining = false;
        GameManager.Instance.ShowWaitLogin(false);
        if (code != 200)
        {
            string msg = string.Format("登陆失败: {0}", text);
            Debug.LogError(msg);
            GetCaptchaCode();
            GameManager.Instance.ShowPromptDialog(msg);
            return;
        }
        try
        {
            LoginRespDTO dto = JsonUtility.FromJson<LoginRespDTO>(text);
            GameManager.Instance.Token = dto.token;
#if (UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS)
            // 加密token，增强本地存储token的安全性
            string token = AesUtil.Encrypt1(dto.token);
            PlayerPrefs.SetString("token", token);
#endif
            GetPlayerInfo();
        }
        catch (System.Exception ex)
        {
            Debug.LogErrorFormat("Login error: {0}", ex.Message);
        }
    }

    public void OnRegisterClick()
    {
        SwitchPanel(true);
    }

    public void OnRegisterOkClick()
    {
        if (_registering)
            return;
        string username = _regUsername.text;
        if (!IsUsernameValid(username))
        {
            GameManager.Instance.ShowPromptTip("请输入合法用户名，用户名只能包含英文字母、数字、\"_\"及\"-\"，且长度至少为6个字符", 3.0f);
            return;
        }
        string password1 = _regPassword.text;
        string password2 = _regPasswordConfirm.text;
        if (string.IsNullOrEmpty(password1))
        {
            GameManager.Instance.ShowPromptTip("请输入密码", 2.0f);
            return;
        }
        if (string.IsNullOrEmpty(password2))
        {
            GameManager.Instance.ShowPromptTip("请输入确认密码", 2.0f);
            return;
        }
        if (!password1.Equals(password2))
        {
            GameManager.Instance.ShowPromptTip("两次输入密码不相同", 2.0f);
            return;
        }
        string errMsg = null;
        if (!Utility.IsPasswordValid(password1, ref errMsg))
        {
            GameManager.Instance.ShowPromptTip(errMsg, 2.0f);
            return;
        }
        string nickname = _regNickname.text;
        nickname = nickname.Trim();
        if (string.IsNullOrEmpty(nickname))
        {
            GameManager.Instance.ShowPromptTip("请输入昵称", 2.0f);
            return;
        }
        _registering = true;
        username = AesUtil.Encrypt(username);
        password1 = AesUtil.Encrypt(password1);
        RegisterDTO dto = new RegisterDTO();
        dto.name = username;
        dto.nickname = nickname;
        dto.password = password1;
        dto.sex = _sex;
        dto.code = _regCode.text;
        dto.uuid = _uuid;
        string json = JsonUtility.ToJson(dto);
        string url = ResourceManager.Instance.HttpHost + "/player/register";
        HttpRequester.Instance.Post(url, null, json, OnRegisterResponse);
    }

    private bool IsUsernameValid(string username)
    {
        if (string.IsNullOrEmpty(username))
            return false;
        if (username.Length < 6)
            return false;
        char[] arr = username.ToCharArray();
        int length = arr.Length;
        char lowerA = 'a';
        char lowerZ = 'z';
        char upperA = 'A';
        char upperZ = 'Z';
        char zero = '0';
        char nine = '9';
        for (int i = 0; i < length; i++)
        {
            if (arr[i] >= lowerA && arr[i] <= lowerZ)
                continue;
            if (arr[i] >= upperA && arr[i] <= upperZ)
                continue;
            if (arr[i] >= zero && arr[i] <= nine)
                continue;
            if (arr[i] == '_' || arr[i] == '-')
                continue;
            return false;
        }
        return true;
    }

    public void OnRegisterBackClick()
    {
        SwitchPanel(false);
    }

    private void SwitchPanel(bool flag)
    {
        if (_panelLogin != null)
            _panelLogin.SetActive(!flag);
        if (_panelRegister != null)
            _panelRegister.SetActive(flag);
        _showRegister = flag;
        GetCaptchaCode();
    }

    private void OnRegisterResponse(int code, string text)
    {
        _registering = false;
        if (code != 200)
        {
            string msg = string.Format("注册失败: {0}", text);
            GameManager.Instance.ShowPromptDialog(msg);
            return;
        }
        SwitchPanel(false);
        _textName.text = _regUsername.text;
        GameManager.Instance.ShowPromptTip("注册成功", 2.0f);
    }

    public void OnToggleMale(bool val)
    {
        if (!val)
            return;
        _sex = 1;
    }

    public void OnToggleFemale(bool val)
    {
        if (!val)
            return;
        _sex = 2;
    }

#elif false // (UNITY_ANDROID || UNITY_IOS)
    private void OnAuthorizeResult(int reqID, ResponseState state, PlatformType type, Hashtable data)
    {
        if (_reqID != reqID)
            return;
        bool test = true;
        if (state == ResponseState.Success)
        {
            try
            {
                if (data != null)
                {
                    String text = MiniJSON.jsonEncode(data);
                    //NetworkManager.Instance.Login(true, text, _agencyID);

                    test = false;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }
        else
        {
            Debug.LogError("Authorize failed!");
        }
        if (test)
            GameManager.Instance.ShowWaitLogin(false);
    }
#endif

    /// <summary>
    /// 获取玩家数据
    /// </summary>
    private void GetPlayerInfo()
    {
        GameManager.Instance.AuthGet("/player/info", OnGetPlayerInfo);
    }

    private void OnGetPlayerInfo(int code, string text)
    {
        if (code != 200)
        {
            string msg = string.Format("Get play data error: {0}", text);
            Debug.LogError(msg);
            GameManager.Instance.ShowPromptDialog(msg);
            GameManager.Instance.Token = null;
            InitLogin();
            return;
        }
        try
        {
            PlayerInfoDTO dto = JsonUtility.FromJson<PlayerInfoDTO>(text);
            GameManager.Instance.PlayerId = dto.playerId;
            GameManager.Instance.Secret = dto.secret;
            GameManager.Instance.NickName = dto.nickname;
            GameManager.Instance.Phone = dto.phone;
            GameManager.Instance.Sex = dto.sex;
            GameManager.Instance.Avatar = dto.avatar;
            GameManager.Instance.Gold = dto.gold;
            GameManager.Instance.Deposit = dto.deposit;
            GameManager.Instance.Diamond = dto.diamond;
            GameManager.Instance.IsAgency = (dto.isAgency != 0);
            GameManager.Instance.AgencyId = dto.agencyId;
            GameManager.Instance.LoginSucceed();
        }
        catch (System.Exception ex)
        {
            Debug.LogErrorFormat("Get play information error: {0}", ex.Message);
        }
    }

    public void Logout()
    {
        PlayerPrefs.DeleteKey("token");
#if true // (UNITY_EDITOR || UNITY_STANDALONE)
        // 刷新图形验证码
        _textCode.text = "";
        GetCaptchaCode();
#elif false // (UNITY_ANDROID || UNITY_IOS)
        if (_ssdk != null)
            _ssdk.CancelAuthorize(PlatformType.WeChat);
#endif
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        AudioSource bg = AudioManager.Instance.BackgroundMusic;
        if (bg == null)
            return;
        string resName = "Sounds/Main/bg_login";
#if (UNITY_EDITOR || UNITY_STANDALONE)
        UnityEngine.Object obj = Resources.Load(resName);
#elif (UNITY_ANDROID || UNITY_IOS)
        UnityEngine.Object obj = ResourceManager.Instance.LoadResource(resName, "sounds/main.ab", "Assets/NiuMa/Resources/", ".mp3");
#endif
        AudioClip clip = obj as AudioClip;
        if (clip != null)
        {
            bg.clip = clip;
            bg.Play();
        }
    }
}