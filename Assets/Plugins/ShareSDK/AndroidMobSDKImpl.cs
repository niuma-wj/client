using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace cn.sharesdk.unity3d {
#if UNITY_ANDROID
	public class AndroidMobSDKImpl : MobSDKImpl {
		private AndroidJavaObject sdk;
		public AndroidMobSDKImpl(GameObject go) {
			try {
				sdk = new AndroidJavaObject("cn.sharesdk.unity3d.MobSDKUtils", go.name, "_PolicyGrantResultCallback");
			} catch(Exception e) {
				Console.WriteLine("{0} Exception caught.", e);
			}
		}

		public override string getPrivacyPolicy(bool url, string language) {
			// if (sdk != null) {
			// 	return sdk.Call<string>("getPrivacyPolicy", url);
			// }
			return "No get privacypolicy content";
		}

		public override string getDeviceCurrentLanguage() {
			return null;
		}

		public override Boolean submitPolicyGrantResult(bool granted) {
			if (sdk != null) {
				return sdk.Call<Boolean>("submitPolicyGrantResult", granted);
			}
			return false;
		}

		public override void setAllowDialog(bool allowDialog) {
			sdk.Call("setAllowDialog", allowDialog);
		}

		public override void setPolicyUi(string backgroundColorRes, string positiveBtnColorRes, string negativeBtnColorRes) {
			sdk.Call("setPolicyUi", backgroundColorRes, positiveBtnColorRes, negativeBtnColorRes);
		}
	}
#endif
}