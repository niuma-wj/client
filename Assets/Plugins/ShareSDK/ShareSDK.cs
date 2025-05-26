using System;
using System.IO;
using System.Text;
using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace cn.sharesdk.unity3d {
	public delegate void sendWXRequestToken(string uid, string token);
	public delegate void sendWXRefreshToken(string token);

	public class ShareSDK : MonoBehaviour
	{
		//版本号，每次发布新版本都需要更新
		public static string version = "3.0.4";

#if UNITY_IPHONE || UNITY_IOS
		public string appKey = "3276d3e413040";
		public string appSecret = "4280a3a6df667cfce37528dec03fd9c3";
		public string mobNetLater = "2";
		public string mobTwitterVer = "2";

		public List<string> customAssociatedDomains = new List<string>();
		public GetWXRequestTokenHanlerEvent wxRequestHandler;
		public GetWXRefreshTokenHanlerEvent wxRefreshTokenHandler;
		public GetShareCommandHanlerEvent shareCommandHandler;
#endif
		private int reqID;
		public DevInfoSet devInfo;
		public ShareSDKImpl sdk;
		public EventHandler authHandler;
		public EventHandler shareHandler;
		public EventHandler showUserHandler;
		public EventHandler getFriendsHandler;
		public EventHandler followFriendHandler;
		public EventHandler clientValidForAndroidHandler;
		void Awake() {
			Type type = devInfo.GetType();
			Hashtable platformConfigs = new Hashtable();
			FieldInfo[] devInfoFields = type.GetFields();
			foreach (FieldInfo devInfoField in devInfoFields) {
				DevInfo info = (DevInfo) devInfoField.GetValue(devInfo);
				int platformId = (int)info.GetType().GetField("type").GetValue(info);
				FieldInfo[] fields = info.GetType().GetFields();
				Hashtable table = new Hashtable();
				foreach (FieldInfo field in fields) {
					if ("type".EndsWith(field.Name)) {
						continue;
					} else if ("Enable".EndsWith(field.Name) || "ShareByAppClient".EndsWith(field.Name) || "BypassApproval".EndsWith(field.Name) || "WithShareTicket".EndsWith(field.Name)) {
						table.Add(field.Name, Convert.ToString(field.GetValue(info)).ToLower());
					} else {
						table.Add(field.Name, Convert.ToString(field.GetValue(info)));
					}
				}
				platformConfigs.Add(platformId, table);
			}

#if UNITY_ANDROID
			sdk = new AndroidImpl(gameObject);
			sdk.PrepareLoopShare();
			sdk.setChannelId();
#elif UNITY_IPHONE
			sdk = new iOSImpl(gameObject);
#endif

			sdk.SetPlatformConfig(platformConfigs);
		}

		public delegate void EventHandler(int reqID, ResponseState state, PlatformType type, Hashtable data);

		private void _Callback(string data) {
			if (data == null) {
				return;
			}

			Hashtable res = (Hashtable)MiniJSON.jsonDecode(data);
			if (res == null || res.Count <= 0) {
				return;
			}

			int status = Convert.ToInt32(res["status"]);
			int reqID = Convert.ToInt32(res["reqID"]);
			PlatformType platform = (PlatformType)Convert.ToInt32(res["platform"]);
			int action = Convert.ToInt32(res["action"]);

			switch(status) {
				case 1: {
					Console.WriteLine(data);
					Hashtable resp = (Hashtable)res["res"];
					OnComplete(reqID, platform, action, resp);
					break;
				}
				case 2: {
					Console.WriteLine(data);
					Hashtable throwable = (Hashtable)res["res"];
					OnError(reqID, platform, action, throwable);
					break;
				}
				case 3: {
					OnCancel(reqID, platform, action);
					break;
				}
			}
		}

		public void OnError(int reqID, PlatformType platform, int action, Hashtable throwable) {
			switch (action) {
				case 1: {
					if (authHandler != null) {
						authHandler(reqID, ResponseState.Fail, platform, throwable);
					}
					break;
				}
				case 2: {
					if (getFriendsHandler != null) {
						getFriendsHandler(reqID, ResponseState.Fail, platform, throwable);
					}
					break;
				}
				case 6: {
					if (followFriendHandler != null) {
						followFriendHandler(reqID, ResponseState.Fail, platform, throwable);
					}
					break;
				}
				case 9: {
					if (shareHandler != null) {
						shareHandler(reqID, ResponseState.Fail, platform, throwable);
					}
					break;
				}
				case 8: {
					if (showUserHandler != null) {
						showUserHandler(reqID, ResponseState.Fail, platform, throwable);
					}
					break;
				}
			}
		}

		public void OnComplete(int reqID, PlatformType platform, int action, Hashtable res) {
			switch (action) {
				case 1: {
					if (authHandler != null) {
						authHandler(reqID, ResponseState.Success, platform, res);
					}
					break;
				}
				case 2: {
					if (getFriendsHandler != null) {
						getFriendsHandler(reqID, ResponseState.Success, platform, res);
					}
					break;
				}
				case 6: {
					if (followFriendHandler != null) {
						followFriendHandler(reqID, ResponseState.Success, platform, res);
					}
					break;
				}
				case 9: {
					if (shareHandler != null) {
						shareHandler(reqID, ResponseState.Success, platform, res);
					}
					break;
				}
				case 8: {
					if (showUserHandler != null) {
						showUserHandler(reqID, ResponseState.Success, platform, res);
					}
					break;
				}
				case 12: {
                    if (clientValidForAndroidHandler != null) {
                        clientValidForAndroidHandler(reqID,ResponseState.Success,platform, res);
                    }
                    break;
                }
#if UNITY_IPHONE
				case 11: {
					shareCommandHandler(res);
					break;
				}
				case 10: {
					int isRefresh = Convert.ToInt32(res["isRefreshToken"]);
					if (isRefresh == 1) {
						String uid = Convert.ToString(res["uid"]);
						wxRefreshTokenHandler(uid, sendWXRefreshTokenMethod);
					} else {
						String authCode = Convert.ToString(res["authCode"]);
						wxRequestHandler(authCode, sendWXRequestTokenMehtod);
					}
					break;
				}
#endif
			}
		}

		public void OnCancel(int reqID, PlatformType platform, int action) {
			switch (action) {
				case 1: {
					if (authHandler != null) {
						authHandler(reqID, ResponseState.Cancel, platform, null);
					}
					break;
				}
				case 2: {
					if (getFriendsHandler != null) {
						getFriendsHandler(reqID, ResponseState.Cancel, platform, null);
					}
					break;
				}
				case 6: {
					if (followFriendHandler != null) {
						followFriendHandler(reqID, ResponseState.Cancel, platform, null);
					}
					break;
				}
				case 9: {
					if (shareHandler != null) {
						shareHandler(reqID, ResponseState.Cancel, platform, null);
					}
					break;
				}
				case 8: {
					if (showUserHandler != null) {
						showUserHandler(reqID, ResponseState.Cancel, platform, null);
					}
					break;
				}
			}
		}

		public void InitSDK(string appKey) {
			//shareSDKUtils.InitSDK (appKey);
		}

		public void InitSDK(string appKey, string appSecret) {
			//shareSDKUtils.InitSDK (appKey,appSecret);
		}

		public void SetPlatformConfig(Hashtable configInfo) {
			sdk.SetPlatformConfig(configInfo);
		}

		public int Authorize(PlatformType platform) {
			reqID ++;
			sdk.Authorize(reqID, platform);
			return reqID;
		}

		public void CancelAuthorize(PlatformType platform) {
			sdk.CancelAuthorize(platform);
		}

		public bool IsAuthorized(PlatformType platform) {
			return sdk.IsAuthorized(platform);
		}

		public bool IsClientValid(PlatformType platform) {
			return sdk.IsClientValid(platform);
		}

		public int GetUserInfo(PlatformType platform) {
			reqID ++;
			sdk.GetUserInfo(reqID, platform);
			return reqID;
		}

		public int ShareContent(PlatformType platform, ShareContent content) {
			reqID ++;
			sdk.ShareContent(reqID, platform, content);
			return reqID;
		}

		public int ShareContent(PlatformType[] platforms, ShareContent content) {
			reqID ++;
			sdk.ShareContent(reqID, platforms, content);
			return reqID;
		}

		public int ShareContentWithActivity(PlatformType platform, ShareContent content) {
			reqID++;
#if UNITY_IPHONE
			sdk.ShareContentWithActivity(reqID, platform, content);
#endif
			return reqID;
		}

		public int ShowPlatformList(PlatformType[] platforms, ShareContent content, int x, int y) {
			reqID++;
			sdk.ShowPlatformList(reqID, platforms, content, x, y);
			return reqID;
		}

		public int ShowShareContentEditor(PlatformType platform, ShareContent content) {
			reqID++;
			sdk.ShowShareContentEditor(reqID, platform, content);
			return reqID;
		}

		public int ShareWithContentName(PlatformType platform, string contentName, Hashtable customFields) {
			reqID++;
			sdk.ShareWithContentName(reqID, platform, contentName, customFields);
			return reqID;
		}

		public int ShowPlatformListWithContentName(string contentName, Hashtable customFields, PlatformType[] platforms, int x, int y) {
			reqID++;
			sdk.ShowPlatformListWithContentName(reqID, contentName, customFields, platforms, x, y);
			return reqID;
		}

		public int ShowShareContentEditorWithContentName(PlatformType platform, string contentName, Hashtable customFields) {
			reqID++;
			sdk.ShowShareContentEditorWithContentName(reqID, platform, contentName, customFields);
			return reqID;
		}

		public int GetFriendList(PlatformType platform, int count, int page) {
			reqID++;
			sdk.GetFriendList(reqID, platform, count, page);
			return reqID;
		}

		public int AddFriend(PlatformType platform, string account) {
			reqID++;
			sdk.AddFriend(reqID, platform, account);
			return reqID;
		}

		public Hashtable GetAuthInfo(PlatformType platform) {
			return sdk.GetAuthInfo(platform);
		}

		public void DisableSSO(bool open) {
			sdk.DisableSSO(open);
		}

		public void openMiniProgram(string userName, string path, int miniProgramType) {
			sdk.openMiniProgram(userName, path, miniProgramType);
		}

		public int isClientValidForAndroid(PlatformType platform) {
         	reqID++;

#if UNITY_ANDROID
         	sdk.isClientValidForAndroid(reqID,platform);
#endif
         	
         	return reqID;
       	}
#if UNITY_IPHONE || UNITY_IOS
		public delegate void GetWXRequestTokenHanlerEvent(string authCode, sendWXRequestToken send);

		public delegate void GetWXRefreshTokenHanlerEvent(string authCode, sendWXRefreshToken send);

		public delegate void GetShareCommandHanlerEvent(Hashtable data);

		public void ShareWithCommand(Hashtable customFields) {
			sdk.shareSDKWithCommand(customFields);
		}
#endif

		public void getWXRequestToken() {
			sdk.getWXRequestToken();
		}

		public void getWXRefreshToken() {
			sdk.getWXRefreshToken();
		}

		public void sendWXRequestTokenMehtod(string uid, string token) {
			sdk.sendWXRequestToken(uid, token);
		}

		public void sendWXRefreshTokenMethod(string token) {
			sdk.sendWXRefreshToken(token);
		}
	}
}