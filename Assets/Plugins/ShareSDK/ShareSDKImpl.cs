using System;
using System.Collections;

namespace cn.sharesdk.unity3d {
	public abstract class ShareSDKRestoreSceneImpl
	{
		public virtual void setRestoreSceneListener() {}

	}

	public abstract class ShareSDKImpl
	{
		/// <summary>
		/// Init the ShareSDK.
		/// <summary>
		public abstract void InitSDK(string appKey);

		public abstract void InitSDK(string appKey, string secret);

		/// <summary>
		/// add listener for loopshare
		/// <summary>
		public abstract void PrepareLoopShare();

		/// <summary>
		/// set channel Id
		/// <summary>
		public abstract void setChannelId();

		/// <summary>
		/// Sets the platform config.
		/// <summary>
		public abstract void SetPlatformConfig(Hashtable configs);

		/// <summary>
		/// Authorize the specified platform.
		/// <summary>
		public abstract void Authorize(int reqId, PlatformType platform);

		/// <summary>
		/// Removes the account of the specified platform.
		/// <summary>
		public abstract void CancelAuthorize(PlatformType platform);

		/// <summary>
		/// Determine weather the account of the specified platform is valid.
		/// <summary>
		public abstract bool IsAuthorized(PlatformType platform);

		/// <summary>
		/// Determine weather the APP-Client of platform is valid.
		/// <summary>
		public abstract bool IsClientValid(PlatformType platform);

		/// <summary>
		/// Request the user info of the specified platform.
		/// <summary>
		public abstract void GetUserInfo(int reqId, PlatformType platform);

		/// <summary>
		/// Share the content to the specified platform with api.
		/// <summary>
		public abstract void ShareContent(int reqId, PlatformType platform, ShareContent content);

		/// <summary>
		/// Share the content to the specified platform with api.
		/// <summary>
		public abstract void ShareContent(int reqId, PlatformType[] platforms, ShareContent content);

		/// <summary>
		/// Show the platform list to share.
		/// <summary>
		public abstract void ShowPlatformList(int reqId, PlatformType[] platforms, ShareContent content, int x, int y);

		/// <summary>
		/// OGUI share to the specified platform. 
		/// <summary>
		public abstract void ShowShareContentEditor(int reqId, PlatformType platform, ShareContent content);

		/// <summary>
		/// share according to the name of node<Content> in ShareContent.xml(in ShareSDKConfigFile.bunle,you can find it in xcode - ShareSDK folider) [only valid in iOS temporarily)]
		/// <summary>
		public abstract void ShareWithContentName(int reqId, PlatformType platform, string contentName, Hashtable customFields);

		/// <summary>
		/// show share platform list according to the name of node<Content> in ShareContent.xml file(in ShareSDKConfigFile.bunle,you can find it in xcode - ShareSDK folider) [only valid in iOS temporarily)] 
		/// <summary>
		public abstract void ShowPlatformListWithContentName(int reqId, string contentName, Hashtable customFields, PlatformType[] platforms, int x, int y);

		/// <summary>
		/// show share content editor according to the name of node<Content> in ShareContent.xml file(in ShareSDKConfigFile.bunle,you can find it in xcode - ShareSDK folider) [only valid in iOS temporarily)]
		/// <summary>
		public abstract void ShowShareContentEditorWithContentName(int reqId, PlatformType platform, string contentName, Hashtable customFields);

		/// <summary>
		/// Gets the friend list.
		/// <summary>
		public abstract void GetFriendList(int reqID, PlatformType platform, int count, int page);

		/// <summary>
		/// Follows the friend.
		/// <summary>
		public abstract void AddFriend(int reqID, PlatformType platform, string account);

		/// <summary>
		/// Gets the auth info.
		/// <summary>
		public abstract Hashtable GetAuthInfo(PlatformType platform);

		/// <summary>
		/// the setting of SSO
		/// <summary>
		public abstract void DisableSSO(bool disable);

		/// <summary>
		/// Open Wechat miniProgram
		/// <summary>
		public abstract bool openMiniProgram(string userName, string path, int miniProgramType);

		public abstract void getWXRequestToken();

		public abstract void getWXRefreshToken();

		public abstract void sendWXRefreshToken(string token);

		public abstract void sendWXRequestToken(string uid, string token);

#if UNITY_ANDROID
		public abstract void isClientValidForAndroid(int reqID,PlatformType platform);
#endif

#if UNITY_IPHONE || UNITY_IOS
		/// <summary>
		/// 获取MobSDK隐私协议内容, url为true时返回MobTech隐私协议链接，false返回协议的内容
		/// <summary>
		public abstract void shareSDKWithCommand(Hashtable content);

		/// <summary>
		/// Share the content to the specified platform with api.
		/// <summary>
		public abstract void ShareContentWithActivity(int reqID, PlatformType platform, ShareContent content);


#endif

#if UNITY_ANDROID
		/// <summary>
		/// 获取MobSDK隐私协议内容, url为true时返回MobTech隐私协议链接，false返回协议的内容
		/// <summary>
		public abstract void setDisappearShareToast(bool url);


#endif
	}
}