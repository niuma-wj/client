using System;
using UnityEngine;
using System.Collections;

namespace cn.sharesdk.unity3d {
	public class ShareSDKRestoreScene : MonoBehaviour
	{
		public RestoreSceneConfigure restoreSceneConfig;
		public static bool isInit;
		public static ShareSDKRestoreScene _instance;
		public static ShareSDKRestoreSceneImpl restoreSceneUtils;
		public static event RestoreSceneHandler onRestoreScene;
		public static event AnalysisCommandHandler OnAnalysisCommand;
		public delegate void RestoreSceneHandler(RestoreSceneInfo scene);

		public delegate void AnalysisCommandHandler(Hashtable parameters);

		void Awake() {
			if (!isInit) {
				isInit = true;
			}

			if (_instance != null) {
				Destroy(_instance.gameObject);
			}
			_instance = this;
			DontDestroyOnLoad(this.gameObject);
		}

		public static void setRestoreSceneListener(cn.sharesdk.unity3d.ShareSDKRestoreScene.RestoreSceneHandler sceneHandler) {
			onRestoreScene += sceneHandler;
		}

		public static void setCommandListener(cn.sharesdk.unity3d.ShareSDKRestoreScene.AnalysisCommandHandler commandHandler) {
			OnAnalysisCommand += commandHandler;
		}

		public void _RestoreCallBack(string data) {
			Hashtable res = (Hashtable)MiniJSON.jsonDecode(data);
			if (res == null || res.Count <= 0) {
				return;
			}
			string path = res ["path"].ToString();
			Hashtable customParams = (Hashtable)res ["params"];
			RestoreSceneInfo scene = new RestoreSceneInfo (path, customParams);

			onRestoreScene(scene);
		}

		public void _AnalysisCommandCallBack(string data) {
			Hashtable res = (Hashtable)MiniJSON.jsonDecode(data);
			if (res == null || res.Count <= 0) {
				return;
			}
			OnAnalysisCommand(res);
		}
	}
}