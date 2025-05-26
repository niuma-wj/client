using System;
using UnityEngine;
using System.Collections;

namespace cn.sharesdk.unity3d {
	[Serializable]
	public class DevInfoSet {

#if UNITY_IPHONE || UNITY_IOS
		public Copy copy;
		public YixinFavorites yixinFavorites;
		public YixinSeries yixinSeries;
		public WechatSeries wechatSeries;
		public QQSeries qqSeries;
		public KakaoSeries kakaoSeries;
		public SnapChat snapChat;
		public EvernoteInternational evernoteInternational;
		public Apple apple;
		public WatermelonVideo watermelonVideo;
#endif

#if UNITY_ANDROID
		public FourSquare fourSquare;
#endif
		public SinaWeiboDevInfo sinaweibo;
		public TencentWeiboDevInfo tencentweibo;
		public Facebook facebook;
		public Twitter twitter;
		public Email email;
		public ShortMessage shortMessage;
		public Douban douban;
		public Renren renren;
		public GooglePlus googlePlus;
		public KaiXin kaiXin;
		public Pocket pocket;
		public Instagram instagram;
		public LinkedIn linkedIn;
		public Tumblr tumblr;
		public YouDao youDao;
		public Flickr flickr;
		public Evernote evernote;
		public WhatsApp whatsApp;
		public Line line;
		public Dropbox dropbox;
		public VKontakte vkontakte;
		public Pinterest pinterest;
		public Mingdao mingdao;
		public KakaoTalk kakaoTalk;
		public KakaoStory kakaoStory;
		public QQ qq;
		public QZone qzone;
		public WeChat wechat;
		public WeChatMoments wechatMoments;
		public WeChatFavorites wechatFavorites;
		public Yixin yixin;
		public YixinMoments yixinMoments;
		public FacebookMessenger facebookMessenger;
		public AliSocial aliSocial;
		public Instapaper instapaper;
		public AliSocialMoments aliSocialMoments;
		public Dingding dingTalk;
		public Youtube youtube;
		public MeiPai meiPai;
		public CMCC cmcc;
		public Reddit reddit;
		public Telegram telegram;
		public ESurfing eSurfing;
		public FacebookAccount facebookAccount;
		public WeWork wework;
		public Douyin douyin;
		public Oasis oasis;
		public TikTok tiktok;
		public KuaiShou kuaishou;

	}

	[Serializable]
	public class DevInfo {
		public bool Enable = true;

	}

	[Serializable]
	public class SinaWeiboDevInfo : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.SinaWeibo;
		public string app_key = "1556465402";
		public string app_secret = "d40c0a5436156e2604a30851df6434bf";
		public string redirect_uri = "https://www.mob.com/";
		public string app_universalLink = "https://70imc.share2dlink.com/";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.SinaWeibo;
		public string SortId = "4";
		public string AppKey = "568898243";
		public string AppSecret = "38a4f8204cc784f81f9f0daaf31e02e3";
		public string RedirectUrl = "http://www.sharesdk.cn";
		public bool ShareByAppClient = true;
#endif


	}

	[Serializable]
	public class TencentWeiboDevInfo : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.TencentWeibo;
		public string app_key = "801307650";
		public string app_secret = "ae36f4ee3946e1cbb98d6965b0b2ff5c";
		public string redirect_uri = "http://www.sharesdk.cn";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.TencentWeibo;
		public string SortId = "3";
		public string AppKey = "801307650";
		public string AppSecret = "ae36f4ee3946e1cbb98d6965b0b2ff5c";
		public string RedirectUri = "http://www.sharesdk.cn";
#endif


	}

	[Serializable]
	public class QQ : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.QQ;
		public string app_id = "1110451818";
		public string app_key = "OlbzvuSP3Hgj5yNS";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.QQ;
		public string SortId = "2";
		public string AppId = "1110451818";
		public string AppKey = "aed9b0303e3ed1e27bae87c33761161d";
		public bool ShareByAppClient = true;
#endif


	}

	[Serializable]
	public class QZone : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.QZone;
		public string app_id = "1110451818";
		public string app_key = "OlbzvuSP3Hgj5yNS";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.QZone;
		public string SortId = "1";
		public string AppId = "1110451818";
		public string AppKey = "ae36f4ee3946e1cbb98d6965b0b2ff5c";
		public bool ShareByAppClient = true;
#endif


	}

	[Serializable]
	public class WeChat : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.WeChat;
		public string app_id = "wx617c77c82218ea2c";
		public string app_secret = "c7253e5289986cf4c4c74d1ccc185fb1";
		public string app_universalLink = "https://70imc.share2dlink.com/";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.WeChat;
		public string SortId = "5";
		public string AppId = "wx4868b35061f87885";
		public string AppSecret = "64020361b8ec4c99936c0e3999a9f249";
		public string UserName = "gh_afb25ac019c9@app";
		public string Path = "/page/API/pages/share/share";
		public bool BypassApproval = false;
		public bool WithShareTicket = true;
		public string MiniprogramType = "0";
#endif


	}

	[Serializable]
	public class WeChatMoments : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.WeChatMoments;
		public string app_id = "wx617c77c82218ea2c";
		public string app_secret = "c7253e5289986cf4c4c74d1ccc185fb1";
		public string app_universalLink = "https://70imc.share2dlink.com/";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.WeChatMoments;
		public string SortId = "6";
		public string AppId = "wx4868b35061f87885";
		public string AppSecret = "64020361b8ec4c99936c0e3999a9f249";
		public bool BypassApproval = true;
#endif


	}

	[Serializable]
	public class WeChatFavorites : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.WeChatFavorites;
		public string app_id = "wx617c77c82218ea2c";
		public string app_secret = "c7253e5289986cf4c4c74d1ccc185fb1";
		public string app_universalLink = "https://70imc.share2dlink.com/";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.WeChatFavorites;
		public string SortId = "7";
		public string AppId = "wx4868b35061f87885";
		public string AppSecret = "64020361b8ec4c99936c0e3999a9f249";
#endif


	}

	[Serializable]
	public class Facebook : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.Facebook;
		public string api_key = "1412473428822331";
		public string app_secret = "a42f4f3f867dc947b9ed6020c2e93558";
		public string display_name = "ShareSDK";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.Facebook;
		public string SortId = "8";
		public string ConsumerKey = "1412473428822331";
		public string ConsumerSecret = "a42f4f3f867dc947b9ed6020c2e93558";
		public string RedirectUrl = "https://www.baidu.com/";
		public string ShareByAppClient = "true";
#endif


	}

	[Serializable]
	public class Twitter : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.Twitter;
		public string consumer_key = "YUJMMTFOcE93MGI4Zi1aRTlHQnM6MTpjaQ";
		public string consumer_secret = "ppuG1yR5Sb7PXn2IZAu-4nJwbhh1mLq3ieJXOkC5wOJpVxmWli";
		public string redirect_uri = "https://www.mob.com";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.Twitter;
		public string SortId = "9";
		public string ConsumerKey = "LRBM0H75rWrU9gNHvlEAA2aOy";
		public string ConsumerSecret = "gbeWsZvA9ELJSdoBzJ5oLKX0TU09UOwrzdGfo9Tg7DjyGuMe8G";
		public string CallbackUrl = "http://mob.com";
#endif


	}

	[Serializable]
	public class Renren : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.Renren;
		public string app_id = "226427";
		public string app_key = "fc5b8aed373c4c27a05b712acba0f8c3";
		public string secret_key = "f29df781abdd4f49beca5a2194676ca4";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.Renren;
		public string SortId = "10";
		public string AppId = "226427";
		public string ApiKey = "fc5b8aed373c4c27a05b712acba0f8c3";
		public string SecretKey = "f29df781abdd4f49beca5a2194676ca4";
#endif


	}

	[Serializable]
	public class KaiXin : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.Kaixin;
		public string api_key = "358443394194887cee81ff5890870c7c";
		public string secret_key = "da32179d859c016169f66d90b6db2a23";
		public string redirect_uri = "http://www.sharesdk.cn";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.Kaixin;
		public string SortId = "11";
		public string AppKey = "358443394194887cee81ff5890870c7c";
		public string AppSecret = "da32179d859c016169f66d90b6db2a23";
		public string RedirectUri = "http://www.sharesdk.cn";
#endif


	}

	[Serializable]
	public class Email : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.Mail;
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.Mail;
		public string SortId = "12";
#endif


	}

	[Serializable]
	public class ShortMessage : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.SMS;
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.SMS;
		public string SortId = "13";
#endif


	}

	[Serializable]
	public class Douban : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.DouBan;
		public string api_key = "02e2cbe5ca06de5908a863b15e149b0b";
		public string secret = "9f1e7b4f71304f2f";
		public string redirect_uri = "http://www.sharesdk.cn";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.DouBan;
		public string SortId = "14";
		public string ApiKey = "02e2cbe5ca06de5908a863b15e149b0b";
		public string Secret = "9f1e7b4f71304f2f";
		public string RedirectUri = "http://www.sharesdk.cn";
#endif


	}

	[Serializable]
	public class YouDao : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.YouDaoNote;
		public string consumer_key = "568898243";
		public string consumer_secret = "d98217b4020e7f1874263795f44838fe";
		public string oauth_callback = "http://www.sharesdk.cn";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.YouDaoNote;
		public string SortId = "15";
		public string ConsumerKey = "dcde25dca105bcc36884ed4534dab940";
		public string ConsumerSecret = "d98217b4020e7f1874263795f44838fe";
		public string RedirectUri = "http://www.sharesdk.cn";
		public string HostType = "product";
#endif


	}

	[Serializable]
	public class Evernote : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.Evernote;
		public string consumer_key = "46131514-6903";
		public string consumer_secret = "08d7a6f3afcc888a";
		public int isSandBox = 1;
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.Evernote;
		public string SortId = "17";
		public string ConsumerKey = "sharesdk-7807";
		public string ConsumerSecret = "d05bf86993836004";
		public bool ShareByAppClient = false;
#endif


	}

	[Serializable]
	public class EvernoteInternational : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.EvernoteInternational;
		public string consumer_key = "46131514-6903";
		public string consumer_secret = "08d7a6f3afcc888a";
		public int RedirectUrl = 0;
#endif


	}

	[Serializable]
	public class LinkedIn : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.LinkedIn;
		public string api_key = "75x5xdhllzno44";
		public string secret_key = "uiS3nlE7XBGmTL3P";
		public string redirect_url = "http://mob.com";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.LinkedIn;
		public string SortId = "18";
		public string ApiKey = "ejo5ibkye3vo";
		public string SecretKey = "cC7B2jpxITqPLZ5M";
		public string RedirectUrl = "http://www.sharesdk.cn";
#endif


	}

	[Serializable]
	public class GooglePlus : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.GooglePlus;
		public string client_id = "232554794995-4m0c7j0f289fkq05ee0abkso4p9d904i.apps.googleusercontent.com";
		public string client_secret = "";
		public string redirect_uri = "com.googleusercontent.apps.232554794995-4m0c7j0f289fkq05ee0abkso4p9d904i:/oauthredirect";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.GooglePlus;
		public string SortId = "19";
		public string ClientID = "236300675100-am5pm8km7md1memjevq8rl9pg5c4s4b8.apps.googleusercontent.com";
		public string RedirectUrl = "http://localhost";
		public bool ShareByAppClient = false;
		public string OfficialVersion = "default";
#endif


	}

	[Serializable]
	public class FourSquare : DevInfo {

#if UNITY_ANDROID
		public const int type = (int)PlatformType.Foursquare;
		public string SortId = "20";
		public string ClientID = "G0ZI20FM30SJAJTX2RIBGD05QV1NE2KVIM2SPXML2XUJNXEU";
		public string ClientSecret = "3XHQNSMMHIFBYOLWEPONNV4DOTCDBQH0AEMVGCBG0MZ32XNU";
		public string RedirectUrl = "http://www.sharesdk.cn";
#endif


	}

	[Serializable]
	public class Pinterest : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.Pinterest;
		public string client_id = "5057854497590653616";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.Pinterest;
		public string SortId = "21";
		public string ClientId = "1432928";
#endif


	}

	[Serializable]
	public class Flickr : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.Flickr;
		public string api_key = "cbed81d4a1bc7417693ab7865e354717";
		public string api_secret = "4c490343869091f2";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.Flickr;
		public string SortId = "22";
		public string ApiKey = "33d833ee6b6fca49943363282dd313dd";
		public string ApiSecret = "3a2c5b42a8fbb8bb";
		public string RedirectUri = "http://www.sharesdk.cn";
#endif


	}

	[Serializable]
	public class Tumblr : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.Tumblr;
		public string consumer_key = "2QUXqO9fcgGdtGG1FcvML6ZunIQzAEL8xY6hIaxdJnDti2DYwM";
		public string consumer_secret = "3Rt0sPFj7u2g39mEVB3IBpOzKnM3JnTtxX2bao2JKk4VV1gtNo";
		public string callback_url = "http://sharesdk.cn";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.Tumblr;
		public string SortId = "23";
		public string OAuthConsumerKey = "2QUXqO9fcgGdtGG1FcvML6ZunIQzAEL8xY6hIaxdJnDti2DYwM";
		public string SecretKey = "3Rt0sPFj7u2g39mEVB3IBpOzKnM3JnTtxX2bao2JKk4VV1gtNo";
		public string CallbackUrl = "http://www.sharesdk.cn";
#endif


	}

	[Serializable]
	public class Dropbox : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.Dropbox;
		public string app_secret = "w0nmp4os3ngo1ja";
		public string app_key = "us514wslpfojbxc";
		public string oauth_callback = "http://localhost";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.Dropbox;
		public string SortId = "24";
		public string AppKey = "7janx53ilz11gbs";
		public string AppSecret = "c1hpx5fz6tzkm32";
#endif


	}

	[Serializable]
	public class VKontakte : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.VKontakte;
		public string application_id = "5312801";
		public string secret_key = "ZHG2wGymmNUCRLG2r6CY";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.VKontakte;
		public string SortId = "25";
		public string ApplicationId = "3921561";
#endif


	}

	[Serializable]
	public class Instagram : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.Instagram;
		public string client_id = "1145188725813939";
		public string client_secret = "256191f3abb381a9d481f6f9bbaef179";
		public string redirect_uri = "https://www.mob.com/";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.Instagram;
		public string SortId = "26";
		public string ClientId = "ff68e3216b4f4f989121aa1c2962d058";
		public string ClientSecret = "1b2e82f110264869b3505c3fe34e31a1";
		public string RedirectUri = "http://sharesdk.cn";
#endif


	}

	[Serializable]
	public class Yixin : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.YiXinSession;
		public string app_id = "yxfddfe3934340436da964fd20885fe2a4";
		public string app_secret = "574471e102e1e5d2a";
		public string redirect_uri = "http://www.mob.com";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.YiXinSession;
		public string SortId = "27";
		public string AppId = "yx0d9a9f9088ea44d78680f3274da1765f";
		public bool BypassApproval = true;
#endif


	}

	[Serializable]
	public class YixinMoments : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.YiXinTimeline;
		public string app_id = "yxfddfe3934340436da964fd20885fe2a4";
		public string app_secret = "574471e102e1e5d2a";
		public string redirect_uri = "http://www.mob.com";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.YiXinTimeline;
		public string SortId = "28";
		public string AppId = "yx0d9a9f9088ea44d78680f3274da1765f";
		public bool BypassApproval = true;
#endif


	}

	[Serializable]
	public class Mingdao : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.MingDao;
		public string app_key = "97230F25CA5C";
		public string app_secret = "A5DC29AF7C5A5851F28E903AE9EAC0";
		public string redirect_uri = "http://mob.com";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.MingDao;
		public string SortId = "29";
		public string AppKey = "EEEE9578D1D431D3215D8C21BF5357E3";
		public string AppSecret = "5EDE59F37B3EFA8F65EEFB9976A4E933";
		public string RedirectUri = "http://sharesdk.cn";
#endif


	}

	[Serializable]
	public class Line : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.Line;
		public string channel_id = "1639219273";
		public string app_universalLink = "https://70imc.share2dlink.com/line-auth";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.Line;
		public string SortId = "30";
		public string ChannelID = "1477692153";
		public string ChannelSecret = "f30c036370f2e04ade71c52eef73a9af";
#endif


	}

	[Serializable]
	public class KakaoTalk : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.KakaoTalk;
		public string app_key = "9c17eb03317e0e627ec95a400f5785fb";
		public string rest_api_key = "802e551a5048c3172fc1dedaaf40fcf1";
		public string redirect_uri = "http://www.mob.com/oauth";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.KakaoTalk;
		public string SortId = "31";
		public string AppKey = "48d3f524e4a636b08d81b3ceb50f1003";
#endif


	}

	[Serializable]
	public class KakaoStory : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.KakaoStory;
		public string app_key = "9c17eb03317e0e627ec95a400f5785fb";
		public string rest_api_key = "802e551a5048c3172fc1dedaaf40fcf1";
		public string redirect_uri = "http://www.mob.com/oauth";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.KakaoStory;
		public string SortId = "32";
		public string AppKey = "48d3f524e4a636b08d81b3ceb50f1003";
#endif


	}

	[Serializable]
	public class WhatsApp : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.WhatsApp;
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.WhatsApp;
		public string SortId = "33";
#endif


	}

	[Serializable]
	public class Bluetooth : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.Bluetooth;
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.Bluetooth;
		public string SortId = "34";
#endif


	}

	[Serializable]
	public class Pocket : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.Pocket;
		public string consumer_key = "11496-de7c8c5eb25b2c9fcdc2b627";
		public string redirect_uri = "pocketapp1234";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.Pocket;
		public string SortId = "35";
		public string ConsumerKey = "32741-389c565043c49947ba7edf05";
#endif


	}

	[Serializable]
	public class Instapaper : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.Instapaper;
		public string consumer_key = "4rDJORmcOcSAZL1YpqGHRI605xUvrLbOhkJ07yO0wWrYrc61FA";
		public string consumer_secret = "GNr1GespOQbrm8nvd7rlUsyRQsIo3boIbMguAl9gfpdL0aKZWe";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.Instapaper;
		public string SortId = "36";
		public string ConsumerKey = "4rDJORmcOcSAZL1YpqGHRI605xUvrLbOhkJ07yO0wWrYrc61FA";
		public string ConsumerSecret = "GNr1GespOQbrm8nvd7rlUsyRQsIo3boIbMguAl9gfpdL0aKZWe";
#endif


	}

	[Serializable]
	public class FacebookMessenger : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.FacebookMessenger;
		public string api_key = "1412473428822331";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.FacebookMessenger;
		public string SortId = "37";
		public string AppId = "107704292745179";
#endif


	}

	[Serializable]
	public class Copy : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.Copy;
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.Copy;
#endif


	}

	[Serializable]
	public class YixinSeries : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.YixinPlatform;
		public string app_id = "yxfddfe3934340436da964fd20885fe2a4";
		public string app_secret = "574471e102e1e5d2a";
		public string redirect_uri = "http://www.mob.com";
#endif


	}

	[Serializable]
	public class YixinFavorites : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.YiXinFav;
		public string app_id = "yxfddfe3934340436da964fd20885fe2a4";
		public string app_secret = "574471e102e1e5d2a";
		public string redirect_uri = "http://www.mob.com";
#endif


	}

	[Serializable]
	public class AliSocial : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.AliSocial;
		public string app_id = "2017062107540437";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.AliSocial;
		public string SortId = "50";
		public string AppId = "2015072400185895";
#endif


	}

	[Serializable]
	public class AliSocialMoments : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.AliSocialMoments;
		public string app_id = "2017062107540437";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.AliSocialMoments;
		public string SortId = "51";
		public string AppId = "2015072400185895";
#endif


	}

	[Serializable]
	public class Dingding : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.Dingding;
		public string shareApp_id = "dingoabcwtuab76wy0kyzo";
		public string authApp_id = "dingoax9s2mdekb7a6748n";
		public string authApp_Secret = "dxx9KwP4BYN975umF6Mi2QW3jL7O3k3qHCSvcCbes5Y5R7mFF1ocd19p4NdzOKD4";
		public string authApp_RedirectUri = "https://www.mob.com/sharesdk/dingding";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.Dingding;
#endif


	}

	[Serializable]
	public class WechatSeries : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.WechatPlatform;
		public string app_id = "wx617c77c82218ea2c";
		public string app_secret = "c7253e5289986cf4c4c74d1ccc185fb1";
		public string app_universalLink = "https://70imc.share2dlink.com/";
#endif


	}

	[Serializable]
	public class QQSeries : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.QQPlatform;
		public string app_id = "1110451818";
		public string app_key = "OlbzvuSP3Hgj5yNS";
#endif


	}

	[Serializable]
	public class KakaoSeries : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.KakaoPlatform;
		public string app_key = "9c17eb03317e0e627ec95a400f5785fb";
		public string rest_api_key = "802e551a5048c3172fc1dedaaf40fcf1";
		public string redirect_uri = "http://www.mob.com/oauth";
#endif


	}

	[Serializable]
	public class Youtube : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.Youtube;
		public string client_secret = "AIzaSyDO66lmq1iQnPZT5ePqy265QylzaX2WCg8";
		public string client_id = "232554794995-4m0c7j0f289fkq05ee0abkso4p9d904i.apps.googleusercontent.com";
		public string redirect_uri = "http://localhost";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.Youtube;
		public string SortId = "53";
		public string ClientID = "370141748022-bicrnsjfiije93bvdt63dh3728m4shas.apps.googleusercontent.com";
		public string AppSecret = "AIzaSyAO06g-0TDpHcsXXO918a7QE3Zdct2bB5E";
		public string RedirectUrl = "http://localhost";
		public string ShareByAppClient = "true";
#endif


	}

	[Serializable]
	public class MeiPai : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.MeiPai;
		public string app_key = "1089867596";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.MeiPai;
		public string SortId = "54";
		public string ClientID = "1089867596";
		public string ShareByAppClient = "true";
#endif


	}

	[Serializable]
	public class CMCC : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.CMCC;
		public string app_key = "610FC6F60177B9558C59B45C6FE47B9C";
		public string app_id = "300011936870";
		public int displayUI = 1;
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.CMCC;
		public string SortId = "55";
		public string AppKey = "2D464D8BFCE73A44B4F9DF95A2FDBE1C";
		public string AppId = "300011860247";
#endif


	}

	[Serializable]
	public class Reddit : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.Reddit;
		public string app_key = "ObzXn50T7Cg0Xw";
		public string redirect_uri = "https://www.mob.com/reddit_callback";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.Reddit;
		public string SortId = "56";
		public string AppKey = "MExDxPuTCtFiRw";
		public string Id = "56";
		public string RedirectUrl = "http://www.sharesdk.cn";
#endif


	}

	[Serializable]
	public class Telegram : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.Telegram;
		public string bot_token = "792340449:AAE9EZeQaXU9wq_r6X7Jalg8jITUEKYr9vw";
		public string bot_domain = "https://www.mob.com";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.Telegram;
		public string SortId = "47";
		public string AppKey = "782826033";
		public string RedirectUrl = "http://www.mob.com";
#endif


	}

	[Serializable]
	public class ESurfing : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.ESurfing;
		public string app_key = "8252014408";
		public string app_secret = "bkqJOALOPc2i6V6R5mEjqLyuzrxF8rWD";
		public string app_name = "天天日记";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.ESurfing;
		public string SortId = "57";
		public string AppKey = "8148612606";
		public string AppSecret = "mCltrhUqwshFa86egDTs0491ibaAulKA";
		public string RedirectUrl = "http://www.sharesdk.cn";
#endif


	}

	[Serializable]
	public class FacebookAccount : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.FacebookAccount;
		public string app_id = "1412473428822331";
		public string client_token = "c30c08723aa8c48fbd5e01d1c3103891";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.FacebookAccount;
		public string SortId = "58";
		public string AppKey = "579465512480462";
		public string AppSecret = "8a6383652dd9f23fb0994f03d350d0ca";
		public string RedirectUrl = "http://www.sharesdk.cn";
#endif


	}

	[Serializable]
	public class Douyin : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.Douyin;
		public string app_key = "awycvl19mldccyso";
		public string app_secret = "8793a4dfdc3636cbda0924a3cfbc8424";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.Douyin;
		public string SortId = "59";
		public string AppKey = "aw9ivykfjvi4hpwo";
		public string AppSecret = "42b4caa6bda60bd49f05f06d0a4956e1";
#endif


	}

	[Serializable]
	public class TikTok : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.TikTok;
		public string app_key = "aw3vqar8qg1oy91q";
		public string app_secret = "18cf1714c53e9f9c64aec484ca4f2e29";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.TikTok;
#endif


	}

	[Serializable]
	public class WeWork : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.WeWork;
		public string app_Key = "wwa21eaecf93f0e3ba";
		public string app_secret = "dW7e27P7Hc8NiYdRxnbTeOLgfI1ugR72e";
		public string agent_id = "1000012";
		public string schema = "wwautha21eaecf93f0e3ba000012";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.WeWork;
		public string SortId = "60";
		public string AppKey = "wwa21eaecf93f0e3ba";
		public string AppSecret = "dW7e27P7Hc8NiYdRxnbTeOLgfI1ugR72e";
		public string AgentId = "1000012";
		public string Schema = "wwautha21eaecf93f0e3ba000012";
#endif


	}

	[Serializable]
	public class Oasis : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.Oasis;
		public string app_key = "568898243";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.Oasis;
		public string SortId = "64";
		public string AppKey = "568898243";
		public string AppSecret = "38a4f8204cc784f81f9f0daaf31e02e3";
		public string RedirectUrl = "http://www.sharesdk.cn";
#endif


	}

	[Serializable]
	public class KuaiShou : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.KuaiShou;
		public string app_id = "ks705657770555308030";
		public string app_secret = "RQ17enXUOioeoDMrwk3j2Q";
		public string app_universalLink = "https://70imc.share2dlink.com/";
#endif

#if UNITY_ANDROID
		public const int type = (int)PlatformType.KuaiShou;
#endif


	}

	[Serializable]
	public class SnapChat : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public int type = (int)PlatformType.SnapChat;
		public string client_id = "dc8e6068-0578-41b8-8392-4da009519725";
		public string RedirectUrl = "http://www.sharesdk.cn";
		public string redirect_uri = "ssdkmoba0b0c0d0://mob";
#endif


	}

	[Serializable]
	public class Apple : DevInfo {

#if UNITY_IPHONE || UNITY_IOS
		public const int type = (int)PlatformType.Apple;
#endif


	}

	[Serializable]
	public class WatermelonVideo : DevInfo {

#if UNITY_IPHONE
		public const int type = (int)PlatformType.WatermelonVideo;
#endif


	}

	[Serializable]
	public class RestoreSceneConfigure {

#if UNITY_IPHONE || UNITY_IOS
		public string capabilititesAssociatedDomain = "applinks:ahmn.t4m.cn";
		public string capabilititesEntitlementsPathInXcode = "Unity-iPhone/Base.entitlements";
#endif
		public bool Enable = false;

	}

	public class RestoreSceneInfo {
		public string path;
		public Hashtable customParams;
		public RestoreSceneInfo(string scenePath, Hashtable sceneCustomParams) {

		}


	}
}