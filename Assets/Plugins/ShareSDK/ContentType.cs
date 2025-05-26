using System;

namespace cn.sharesdk.unity3d {
	public class ContentType {

#if UNITY_IPHONE || UNITY_IOS
		public const int MiniProgram = 10;
		public const int Message = 11;
#endif

#if UNITY_ANDROID
		public const int MiniProgram = 11;
		public const int QQ_MINI_PROGRAM = 15;
#endif
		public const int Auto = 0;
		public const int Text = 1;
		public const int Image = 2;
		public const int Webpage = 4;
		public const int Music = 5;
		public const int Video = 6;
		public const int App = 7;
		public const int File = 8;
		public const int Emoji = 9;
		public const int OpenMiniProgram = 12;

	}

	public class FacebookShareType {
		public const int Native = 1;
		public const int ShareSheet = 2;
		public const int Browser = 3;
		public const int Web = 4;
		public const int FeedBrowser = 5;
		public const int FeedWeb = 6;

	}
}