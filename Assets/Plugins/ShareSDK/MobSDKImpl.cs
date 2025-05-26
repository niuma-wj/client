using System;

namespace cn.sharesdk.unity3d {
	public abstract class MobSDKImpl
	{
		/// <summary>
		/// 获取MobSDK隐私协议内容, url为true时返回MobTech隐私协议链接，false返回协议的内容
		/// <summary>
		public abstract string getPrivacyPolicy(bool url, string language);

		/// <summary>
		/// 获取设备语言
		/// <summary>
		public abstract string getDeviceCurrentLanguage();

		/// <summary>
		/// 提交用户授权结果给MobSDK
		/// <summary>
		public abstract Boolean submitPolicyGrantResult(bool granted);

		/// <summary>
		/// 是否允许展示二次确认框
		/// <summary>
		public abstract void setAllowDialog(bool allowDialog);

		/// <summary>
		/// 设置二次确认框样式
		/// <summary>
		public abstract void setPolicyUi(string backgroundColorRes, string positiveBtnColorRes, string negativeBtnColorRes);

	}
}