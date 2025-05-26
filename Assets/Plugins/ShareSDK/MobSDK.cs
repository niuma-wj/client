using System;
using System.IO;
using System.Text;
using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace cn.sharesdk.unity3d {
	public class MobSDK : MonoBehaviour
	{

#if UNITY_IPHONE || UNITY_IOS
		public getPolicyHandle getPolicy;
#endif
		public MobSDKImpl sdk;
		public OnSubmitPolicyGrantResultCallback onSubmitPolicyGrantResultCallback;
		public delegate void OnSubmitPolicyGrantResultCallback(bool success);

		void Awake() {
			#if UNITY_IPHONE
				sdk = new iOSMobSDKImpl(gameObject);
			#elif UNITY_ANDROID
				sdk = new AndroidMobSDKImpl(gameObject);
			#endif
		}

		private void _PolicyGrantResultCallback(bool success) {
			onSubmitPolicyGrantResultCallback(success);
		}

		/// <summary>
		/// 提交用户授权结果给MobSDK
		/// <summary>
		public Boolean submitPolicyGrantResult(bool granted) {
			return sdk.submitPolicyGrantResult(granted);
		}

		/// <summary>
		/// 是否允许展示二次确认框
		/// <summary>
		public void setAllowDialog(bool allowDialog) {
			sdk.setAllowDialog(allowDialog);
		}

		/// <summary>
		/// 设置二次确认框样式
		/// <summary>
		public void setPolicyUi(string backgroundColorRes, string positiveBtnColorRes, string negativeBtnColorRes) {
			sdk.setPolicyUi(backgroundColorRes, positiveBtnColorRes, negativeBtnColorRes);
		}
#if UNITY_IPHONE || UNITY_IOS
		public delegate void getPolicyHandle(string content);

		public void getPrivacyPolicy(bool url, string language) {
			sdk.getPrivacyPolicy(url, language);
		}

		public string getDeviceCurrentLanguage() {
			return sdk.getDeviceCurrentLanguage();
		}

		private void _Callback(string data) {
			if (data == null) {
				return;
			}

			Hashtable res = (Hashtable)MiniJSON.jsonDecode(data);
			if (res == null || res.Count <= 0) {
				return;
			}

			int status = Convert.ToInt32(res["status"]);
			int action = Convert.ToInt32(res["action"]);

			switch(status) {
				case 1: {
					Console.WriteLine(data);
					Hashtable resp = (Hashtable) res["res"];
					if (action == 1) {
						if (getPolicy != null) {
							getPolicy((string)resp["url"]);
						}
					}
					break;
				}
				case 2: {
					break;
				}
				case 3: {
					break;
				}
			}
		}
#endif

#if UNITY_ANDROID
		public string getPrivacyPolicy(bool url, string language) {
			return sdk.getPrivacyPolicy(url, language);
		}
#endif

	}
}