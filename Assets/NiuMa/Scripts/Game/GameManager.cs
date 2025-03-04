using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using XLua;
using LitJson;
using cn.sharesdk.unity3d;

namespace NiuMa
{
    [LuaCallCSharp]
    public class GameManager
    {
        // 禁止外部创建
        private GameManager() { }
        private static readonly GameManager _instance = new GameManager();

        public static GameManager Instance
        {
            get { return _instance; }
        }

        private bool _debugMode = false;
        [BlackList]
        public bool DebugMode
        {
            get { return _debugMode; }
        }

#if true // (UNITY_EDITOR || UNITY_STANDALONE)
#elif false // (UNITY_ANDROID || UNITY_IOS)
        private ShareSDK _ssdk = null;
        public ShareSDK SSDK
        {
            get { return _ssdk; }
            set { _ssdk = value; }
        }
#endif
        private GameObject _login = null;
        [BlackList]
        public GameObject Login
        {
            set { _login = value; }
        }

        // App大厅
        private Hall _hall = null;
        public Hall Hall
        {
            get { return _hall; }

            [BlackList]
            set { _hall = value; }
        }
 
        // App大厅父母节点
        private Transform _hallPanel = null;
        [BlackList]
        public Transform HallPanel
        {
            set { _hallPanel = value; }
        }

        // 游戏(游戏中包含游戏大厅及游戏房间)父母节点
        private Transform _gamePanel = null;
        [BlackList]
        public Transform GamePanel
        {
            set { _gamePanel = value; }
        }

        // 游戏大厅父母节点
        private Transform _gameHallParent = null;
        [BlackList]
        public Transform GameHallParent
        {
            get { return _gameHallParent; }
            set { _gameHallParent = value; }
        }

        // 游戏房间父母节点
        private Transform _gameRoomParent = null;
        [BlackList]
        public Transform GameRoomParent
        {
            get { return _gameRoomParent; }
            set { _gameRoomParent = value; }
        }

        private Transform _tipPanel = null;
        [BlackList]
        public Transform TipPanel
        {
            set { _tipPanel = value; }
        }

        private Transform _waitPanel = null;
        [BlackList]
        public Transform WaitPanel
        {
            set { _waitPanel = value; }
        }

        private Transform _promptPanel = null;
        [BlackList]
        public Transform PromptPanel
        {
            set { _promptPanel = value; }
        }

        private GameObject _waitLogin = null;
        [BlackList]
        public GameObject WaitLogin
        {
            set { _waitLogin = value; }
        }

        private GameObject _waiting = null;
        [BlackList]
        public GameObject Waiting
        {
            set { _waiting = value; }
        }

        private GameObject _connecting;
        [BlackList]
        public GameObject Connecting
        {
            set { _connecting = value; }
        }

        // 当前所在场地ID
        private string _venueId = null;
        public string VenueId
        {
            get { return _venueId; }
            set { _venueId = value; }
        }

        // 玩家ID
        private string _playerId = null;
        public string PlayerId
        {
            get { return _playerId; }
            set { _playerId = value; }
        }

        private bool _loginFlag = false;
        [BlackList]
        public bool LoginFlag
        {
            get { return _loginFlag; }
            set { _loginFlag = value; }
        }

        // token
        private string _token = null;
        [BlackList]
        public string Token
        {
            get { return _token; }
            set { _token = value; }
        }

        // 消息密钥
        private string secret = null;
        [BlackList]
        public string Secret
        {
            get { return secret; }
            set { secret = value; }
        }

        // 是否正在请求消息密钥
        private bool _requestingSecret = false;

        // 昵称
        private string _nickName = null;
        public string NickName
        {
            get { return _nickName; }
            set { _nickName = value; }
        }

        // 电话
        private string _phone = null;
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        // 0未知，1为男性，2为女性
        private int _sex = 0;
        public int Sex
        {
            get { return _sex; }
            set { _sex = value; }
        }

        // 头像url
        private string _avatar = null;
        public string Avatar
        {
            get { return _avatar; }
            set { _avatar = value; }
        }

        // 金币数量
        private long _gold = 0L;
        public long Gold
        {
            get { return _gold; }
            set { _gold = value; }
        }

        // 银行存款
        private long _deposit = 0L;
        public long Deposit
        {
            get { return _deposit; }
            set { _deposit = value; }
        }

        // 钻石数量
        private long _diamond = 0L;
        public long Diamond
        {
            get { return _diamond; }
            set { _diamond = value; }
        }

        // 是否为代理
        private bool _isAgency = false;
        public bool IsAgency
        {
            get { return _isAgency; }
            set { _isAgency = value; }
        }

        // 代理玩家id
        private string _agencyId = null;
        public string AgencyId
        {
            get { return _agencyId; }
            set { _agencyId = value; }
        }

        // 玩家当前所在的地址
        private string _address = null;
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        // 头像纹理
        private Texture2D _headTexture = null;
        public Texture2D HeadTexture
        {
            get { return _headTexture; }
            set { _headTexture = value; }
        }

        // 是否获得定位信息
        private bool _location = false;
        public bool Location
        {
            get { return _location; }
            set { _location = value; }
        }

        // 纬度
        private double _latitude = 0.0;
        public double Latitude
        {
            get { return _latitude; }
            set { _latitude = value; }
        }

        // 经度
        private double _longitude = 0.0;
        public double Longitude
        {
            get { return _longitude; }
            set { _longitude = value; }
        }

        // 海拔
        private double _altitude = 0.0;
        public double Altitude
        {
            get { return _altitude; }
            set { _altitude = value; }
        }

        // 正在进入的场地id
        private string _enterVenueId = null;

        // 进入成功回调
        private Action _enterCallback = null;

        // 上一次发送Http心跳的时间
        private DateTime _timeLastHeartbeat = new DateTime();

        // 保存url对应的头像纹理，以免多次加载
        private Dictionary<String, Texture2D> _headTextureMap = new Dictionary<String, Texture2D>();

        // 游戏大厅(注意这里要与整个App的大厅区别开来)
        private GameObject _gameHall = null;

        // 游戏房间
        private GameObject _gameRoom = null;

        // 当前已加载的游戏
        private List<string> _gameLoaded = new List<string>();

        // 一分钟的生成的Nonce序列
        private List<KeyValuePair<long, string> > _nonceSequence = new List<KeyValuePair<long, string> >();

        //
        private HashSet<string> _nonces = new HashSet<string>();

        public Texture2D GetTextureByUrl(String url)
        {
            if (url == null || url.Length == 0)
                return null;

            Texture2D tex = null;
            _headTextureMap.TryGetValue(url, out tex);
            return tex;
        }

        public void AddTextureUrl(String url, Texture2D tex)
        {
            if (url == null || url.Length == 0)
                return;
            if (tex == null)
                return;
            if (_headTextureMap.ContainsKey(url))
                return;

            _headTextureMap.Add(url, tex);
        }

        [BlackList]
        public void ShowWaitLogin(bool s)
        {
            if (_waitLogin == null)
                return;

            if (_waitPanel != null)
                _waitPanel.gameObject.SetActive(s);

            _waitLogin.SetActive(s);
        }

        public void ShowWaiting(bool s)
        {
            if (_waiting == null)
                return;

            if (_waitPanel != null)
                _waitPanel.gameObject.SetActive(s);

            _waiting.SetActive(s);
        }

        // 显示或隐藏正在连接
        [BlackList]
        public void ShowConnecting(bool s)
        {
            if (_connecting != null)
                _connecting.SetActive(s);
        }

        // 弹出提示对话框
        public void ShowPromptDialog(String strPrompt, UnityAction ok = null, UnityAction cancel = null)
        {
            if (_promptPanel == null)
                return;
            if (strPrompt == null || strPrompt.Length == 0)
                return;
            GameObject prefab = Resources.Load("Prefabs/PromptDialog") as GameObject;
            if (prefab == null)
                return;
            GameObject inst = GameObject.Instantiate(prefab, _promptPanel);
            PromptDialog pd = inst.GetComponent<PromptDialog>();
            if (pd != null)
            {
                pd.SetDescription(strPrompt);
                if (ok != null)
                    pd.SetButtonClickedHandler(ok, true);
                if (cancel != null)
                    pd.SetButtonClickedHandler(cancel, false);
            }
        }

        private GameObject _promptTip = null;

        public void ShowPromptTip(string text, float life)
        {
            if (text == null || text.Length == 0 || life <= 0.0f)
                return;

            if (_promptTip != null)
            {
                GameObject.Destroy(_promptTip);
                _promptTip = null;
            }
            GameObject prefab = Resources.Load("Prefabs/PromptTip") as GameObject;
            if (prefab == null)
                return;
            _promptTip = GameObject.Instantiate(prefab, _tipPanel);
            if (_promptTip != null)
            {
                PromptTip pt = _promptTip.GetComponent<PromptTip>();
                if (pt != null)
                    pt.DoTip(text, life);
            }
        }

        [BlackList]
        public void PromptTipDisappear()
        {
            if (_promptTip != null)
            {
                GameObject.Destroy(_promptTip);
                _promptTip = null;
            }
        }

        [BlackList]
        // 返回登录界面
        public void BackLogin()
        {
            DestroyGameRoom();
            DestroyGameHall();
            if (_hall != null)
            {
                GameObject.Destroy(_hall.gameObject);
                _hall = null;
            }
            if (_login != null)
                _login.SetActive(true);
        }

        // 登出
        public void Logout()
        {
            BackLogin();
            if (_login != null)
            {
                Login lg = _login.GetComponent<Login>();
                if (lg != null)
                    lg.Logout();
            }
            AuthPost("/player/logout", null, null);
            PlayerId = null;
            Token = null;
            Secret = null;
            NickName = null;
            Phone = null;
            Sex = 0;
            Avatar = null;
            Gold = 0L;
            Deposit = 0L;
            Diamond = 0L;
            IsAgency = false;
            Address = null;
            HeadTexture = null;
            Latitude = 0.0f;
            Longitude = 0.0f;
            Altitude = 0.0f;
        }

        // 签到于服务器
        [BlackList]
        public void LoginSucceed()
        {
            Debug.LogFormat("Login success, token: {0}", Token);
            LoginFlag = true;
            UpdateLocation();
            // 加载大厅
            if (_hall != null)
            {
                if (_hallPanel != null)
                    _hallPanel.gameObject.SetActive(true);
                _hall.gameObject.SetActive(true);
            }
            else
            {
                if (_hallPanel == null)
                    return;
                UnityEngine.Object obj = ResourceManager.Instance.LoadResource("Prefabs/Hall", "prefabs/hall.ab", "Assets/NiuMa/Resources/", ".prefab");
                GameObject prefab = obj as GameObject;
                if (prefab != null)
                {
                    _hallPanel.gameObject.SetActive(true);
                    GameObject.Instantiate<GameObject>(prefab, _hallPanel);
                }
            }
            // CreateDumbGame();
            // XoGmwf8z8u
            // EnterDumbGame("Zjv75Y6aj5");
        }

        private void CreateDumbGame()
        {
            AuthPost("/player/game/create/dumb", null, OnCreateDumbGame);
        }

        private void OnCreateDumbGame(int code, string text)
        {
            Debug.LogFormat("Create dumb game response status: {0}, body: {1}", code, text);
            JsonData data = null;
            try
            {
                data = JsonMapper.ToObject(text);
            }
            catch (Exception)
            {
                data = new JsonData();
            }
            if (code == 200)
            {
                string address = null;
                string venueId = null;
                if (data.ContainsKey("address"))
                    address = data["address"].ToString();
                if (data.ContainsKey("venueId"))
                    venueId = data["venueId"].ToString();
                Debug.LogFormat("Create dumb game success, venueId: {0}, server address: {1}", venueId, address);
                EnterVenue(address, venueId, 1, null);
            }
            else
            {
                string errMsg = null;
                if (data.ContainsKey("msg"))
                    errMsg = data["msg"].ToString();
                if (string.IsNullOrEmpty(errMsg))
                    errMsg = text;
                errMsg = string.Format("创建游戏失败: {0}", errMsg);
                ShowPromptDialog(errMsg);
            }
        }

        private void EnterDumbGame(string venueId)
        {
            JsonData data = new JsonData();
            data["venueId"] = venueId;
            data["gameType"] = 1;
            string body = data.ToJson();
            AuthPost("/player/game/enter", body, OnEnterDumbGame);
        }

        private void OnEnterDumbGame(int code, string text)
        {
            Debug.LogFormat("Enter dumb game response status: {0}, body: {1}", code, text);
            JsonData data = null;
            try
            {
                data = JsonMapper.ToObject(text);
            }
            catch (Exception)
            {
                data = new JsonData();
            }
            if (code == 200)
            {
                string address = null;
                string venueId = null;
                if (data.ContainsKey("address"))
                    address = data["address"].ToString();
                if (data.ContainsKey("venueId"))
                    venueId = data["venueId"].ToString();
                Debug.LogFormat("Enter dumb game success, venueId: {0}, server address: {1}", venueId, address);
                EnterVenue(address, venueId, 1, null);
            }
            else
            {
                string errMsg = null;
                if (data.ContainsKey("msg"))
                    errMsg = data["msg"].ToString();
                if (string.IsNullOrEmpty(errMsg))
                    errMsg = text;
                errMsg = string.Format("进入游戏失败: {0}", errMsg);
                ShowPromptDialog(errMsg);
            }
        }

        /// <summary>
        /// 进入场地
        /// </summary>
        /// <param name="address">服务器地址</param>
        /// <param name="venueId">场地id</param>
        /// <param name="gameType">游戏类型</param>
        /// <param name="callback">进入成功回调</param>
        public void EnterVenue(string address, string venueId, int gameType, Action callback)
        {
            if (!string.IsNullOrEmpty(_enterVenueId))
            {
                // 当前正在进入场地，直接返回
                return;
            }
            if (string.IsNullOrEmpty(address) || string.IsNullOrEmpty(venueId))
                return;
            int pos = address.IndexOf(':');
            if (pos == -1)
                return;
            VenueId = null;
            _enterVenueId = venueId;
            _enterCallback = callback;
            string ip = address.Substring(0, pos);
            string tmp = address.Substring(pos + 1);
            int port = int.Parse(tmp);
            NetworkManager.Instance.EnterVenue(ip, port, venueId, gameType);
        }

        /// <summary>
        /// 进入场地成功，执行回调
        /// </summary>
        /// <param name="venueId">场地id</param>
        [BlackList]
        public void OnEnterVenue(string venueId)
        {
            if (!venueId.Equals(_enterVenueId))
                return;
            VenueId = venueId;
            _enterVenueId = null;
            if (_enterCallback != null)
                _enterCallback();
        }

        /// <summary>
        /// 通知进入场地失败
        /// </summary>
        /// <param name="venueId">场地id</param>
        /// <param name="errMsg">错误消息</param>
        [BlackList]
        public void OnEnterFailed(string venueId, string errMsg)
        {
            if (!venueId.Equals(_enterVenueId))
                return;
            _enterVenueId = null;
            ShowPromptDialog(errMsg);
        }

        public void GetCapital()
        {
            AuthGet("/player/capital/get", OnGetCapital);
        }

        private void OnGetCapital(int code, string text)
        {
            JsonData data = null;
            try
            {
                data = JsonMapper.ToObject(text);
            }
            catch (Exception)
            {
                data = new JsonData();
            }
            if (code == 200)
            {
                if (data.ContainsKey("gold"))
                    Gold = long.Parse(data["gold"].ToString());
                if (data.ContainsKey("deposit"))
                    Deposit = long.Parse(data["deposit"].ToString());
                if (data.ContainsKey("diamond"))
                    Diamond = long.Parse(data["diamond"].ToString());
                OnCapitalChange();
            }
        }

        /// <summary>
        /// 创建游戏大厅或房间
        /// </summary>
        /// <param name="name">大厅或房间名称</param>
        /// <param name="prefab">大厅或房间Prefab</param>
        /// <param name="hallOrRoom">大厅true，房间false</param>
        /// <returns></returns>
        public GameObject CreateHallRoom(string name, GameObject prefab, bool hallOrRoom)
        {
            if (string.IsNullOrEmpty(name) || prefab == null)
                return null;
            if (hallOrRoom)
            {
                if (_gameHall != null)
                {
                    Debug.LogError("Game hall already existed with name:\"" + _gameHall.name + "\", you should destroy the old hall before create a new one.");
                    return null;
                }
                _gameHall = GameObject.Instantiate(prefab, _gameHallParent);
                _gameHall.name = name;
            }
            else
            {
                if (_gameRoom != null)
                {
                    Debug.LogError("Game room already existed with name:\"" + _gameRoom.name + "\", you should destroy the old room before create a new one.");
                    return null;
                }
                _gameRoom = GameObject.Instantiate(prefab, _gameRoomParent);
                _gameRoom.name = name;
            }
            return hallOrRoom ? _gameHall : _gameRoom;
        }

        public bool HasGameRoom()
        {
            return (_gameRoom != null);
        }

        public GameObject GetGameHall()
        {
            return _gameHall;
        }

        public GameObject GetGameRoom()
        {
            return _gameRoom;
        }

        public void DestroyGameHall()
        {
            if (_gameHall == null)
                return;
            GameObject.Destroy(_gameHall);
            _gameHall = null;
        }

        public void DestroyGameRoom()
        {
            VenueId = null;
            _enterVenueId = null;
            NetworkManager.Instance.Close();
            ShowConnecting(false);
            if (_gameRoom == null)
                return;
            GameObject.Destroy(_gameRoom);
            _gameRoom = null;
            Resources.UnloadUnusedAssets();
            if (_hall != null)
                _hall.PlayBackgroundMusic();
        }

        public void OnCapitalChange()
        {
            if (Hall != null)
                Hall.OnCapitalChange();
        }

        [BlackList]
        public void AddGameLoaded(string name)
        {
            if (name == null || name.Length == 0)
                return;

            if (!_gameLoaded.Contains(name))
                _gameLoaded.Add(name);
        }

        public bool IsGameLoaded(string name)
        {
#if (UNITY_EDITOR || UNITY_STANDALONE)
            return true;
#elif (UNITY_ANDROID || UNITY_IOS)
            if (string.IsNullOrEmpty(name))
                return false;

            return _gameLoaded.Contains(name);
#endif
        }

        private string GenerateNonce(long timestamp)
        {
            long delta = 0L;
            int count = _nonceSequence.Count;
            while (count > 0)
            {
                KeyValuePair<long, string> pair = _nonceSequence[0];
                delta = timestamp - pair.Key;
                if (delta < 60L)
                    break;
                _nonces.Remove(pair.Value);
                _nonceSequence.RemoveAt(0);
                count--;
            }
            string nonce = null;
            while (true)
            {
                nonce = Utility.GenerateRandomCode(10);
                if (_nonces.Contains(nonce))
                    continue;
                _nonces.Add(nonce);
                _nonceSequence.Add(new KeyValuePair<long, string>(timestamp, nonce));
                break;
            }
            return nonce;
        }

        /**
         * 消息签名
         */
        public void SignatureMessage(MsgPlayerSignature msg)
        {
            if (msg == null)
                return;
            long timestamp = Utility.DateTimeToTimestamp(DateTime.Now);
            msg.playerId = PlayerId;
            msg.timestamp = timestamp.ToString();
            msg.nonce = GenerateNonce(timestamp);
            string text = PlayerId + '&' + msg.timestamp + '&' + msg.nonce + '&' + Secret;
            msg.signature = Utility.EncodeMD5(text, false, false);
        }

        /**
         * 消息签名，返回json
         */
        public string SignatureJson()
        {
            JsonData data = new JsonData();
            SignatureJson(data);
            string json = data.ToJson();
            return json;
        }

        [BlackList]
        public void SignatureJson(JsonData data)
        {
            long timestamp = Utility.DateTimeToTimestamp(DateTime.Now);
            string ts = timestamp.ToString();
            string nonce = GenerateNonce(timestamp);
            string text = PlayerId + '&' + ts + '&' + nonce + '&' + Secret;
            string signature = Utility.EncodeMD5(text, false, false);
            data["playerId"] = PlayerId;
            data["timestamp"] = ts;
            data["nonce"] = nonce;
            data["signature"] = signature;
        }

        public void AuthGet(string path, Action<int, string> actionResult = null)
        {
            string url = ResourceManager.Instance.HttpHost + path;
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("PLAYER-AUTHORIZATION", Token);
            HttpRequester.Instance.Get(url, headers, (code, text) => {
                if (actionResult != null)
                {
                    try
                    {
                        actionResult(code, text);
                    }
                    catch (Exception ex) {
                        Debug.LogError(ex.Message);
                    }
                }
                if (!path.Equals("/player/info") && code == 401)
                {
                    ShowPromptDialog("登录已失效，请重新登录", BackLogin, BackLogin);
                }
            });
        }

        public void AuthPost(string path, string body, Action<int, string> actionResult = null)
        {
            string url = ResourceManager.Instance.HttpHost + path;
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("PLAYER-AUTHORIZATION", Token);
            HttpRequester.Instance.Post(url, headers, body, (code, text) => {
                if (actionResult != null)
                {
                    try
                    {
                        actionResult(code, text);
                    }
                    catch (Exception ex) {
                        Debug.LogError(ex.Message);
                    }
                }
                if (code == 401)
                {
                    ShowPromptDialog("登录已失效，请重新登录", BackLogin, BackLogin);
                }
            });
        }

        [BlackList]
        public void Update()
        {
            if (string.IsNullOrEmpty(PlayerId))
                return;
            TimeSpan ts = DateTime.Now - _timeLastHeartbeat;
            if (ts.TotalSeconds > 30.0) {
                // 30秒发送一次心跳
                _timeLastHeartbeat = DateTime.Now;
                AuthGet("/player/heartbeat", null);
            }
        }

        [BlackList]
        public void OnSignatureError()
        {
            if (_requestingSecret)
                return;
            // 消息签名错误，尝试更新消息密钥
            _requestingSecret = true;
            AuthGet("/player/message/secret", OnGetMessageSecret);
        }

        private void OnGetMessageSecret(int code, string text)
        {
            _requestingSecret = false;
            if (code != 200)
                return;
            try
            {
                JsonData data = JsonMapper.ToObject(text);
                Secret = data["secret"].ToString();
            }
            catch (Exception)
            {}
        }

        [BlackList]
        public void OnGetLocation()
        {
            Location = true;
            UpdateLocation();
        }

        private void UpdateLocation()
        {
            if (!Location || !LoginFlag)
                return;
            // 向后台更新玩家定位，以便获得玩家所在地址
            JsonData data = new JsonData();
            data["latitude"] = Latitude;
            data["longitude"] = Longitude;
            data["altitude"] = Altitude;
            string body = data.ToString();
            AuthPost("/player/location", body, null);
        }
    }
}