using System;
using LitJson;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace cn.sharesdk.unity3d {
	public class ShareContent : MonoBehaviour
	{
		Hashtable shareParams = new Hashtable();
		Hashtable customizeShareParams = new Hashtable();
		public void SetHidePlatforms(String[] hidePlatformList) {
			shareParams["hidePlatformList"] = String.Join (",", hidePlatformList);
		}

		public void SetTitle(string title) {
			shareParams["title"] = title;
		}

		public void SetText(string text) {
			shareParams["text"] = text;
		}

		public void SetUrl(string url) {
			shareParams["url"] = url;
		}

		public void SetImagePath(string imagePath) {
			#if UNITY_ANDROID
				shareParams["imagePath"] = imagePath;
			#elif UNITY_IPHONE
				shareParams["imageUrl"] = imagePath;
			#endif
		}

		public void SetImageUrl(string imageUrl) {
			shareParams["imageUrl"] = imageUrl;
		}

		public void SetImageArray(string[] imageArray) {
			shareParams["imageArray"] = String.Join(",", imageArray);
		}

		public void SetShareType(int shareType) {
			#if UNITY_ANDROID
				if (shareType == 0) {
					shareType = 1;
				} else if(shareType == 10) {
					shareType = 11;
				}
			#endif
			shareParams["shareType"] = shareType;
		}

		public void SetTitleUrl(string titleUrl) {
			shareParams["titleUrl"] = titleUrl;
		}

		public void SetComment(string comment) {
			shareParams["comment"] = comment;
		}

		public void SetSite(String site) {
			shareParams["site"] = site;
		}

		public void SetSiteUrl(string siteUrl) {
			shareParams["siteUrl"] = siteUrl;
		}

		public void SetAddress(string address) {
			shareParams["address"] = address;
		}

		public void SetFilePath(string filePath) {
			shareParams["filePath"] = filePath;
		}

		public void SetMusicUrl(string musicUrl) {
			shareParams["musicUrl"] = musicUrl;
		}

		public void SetLatitude(string latitude) {
			shareParams["latitude"] = latitude;
		}

		public void SetLongitude(string longitude) {
			shareParams["longitude"] = longitude;
		}

		public void SetSource(string source) {
			#if UNITY_ANDROID
				shareParams["url"] = source;
			#elif UNITY_IPHONE
				shareParams["source"] = source;
			#endif
		}

		public void SetAuthor(string author) {
			#if UNITY_ANDROID
				shareParams["address"] = author;
			#elif UNITY_IPHONE
				shareParams["author"] = author;
			#endif
		}

		public void SetSnapAttachmentUrl(string attachmentUrl) {
			shareParams["attachmentUrl"] = attachmentUrl;
		}

		public void setSnapStickerAnimated(int stickerAnimated) {
			#if UNITY_IPHONE
				shareParams["stickerAnimated"] = stickerAnimated;
			#endif
		}

		public void setSnapStickerRotation(float rotation) {
			#if UNITY_IPHONE
				shareParams["stickerRotation"] = rotation;
			#endif
		}

		public void setSnapStickerImage(string image) {
			#if UNITY_IPHONE
				shareParams["stickerImage"] = image;
			#endif
		}

		public void SetLinkURL(string linkUrl) {
			shareParams["linkUrl"] = linkUrl;
		}

		public void SetOpenID(string openID) {
			shareParams["openID"] = openID;
		}

		public void SetReceiverOpenID(string receiverOpenID) {
			shareParams["receiverOpenID"] = receiverOpenID;
		}

		public void SetLocalIdentifier(string localIdentifier) {
			shareParams["localIdentifier"] = localIdentifier;
		}

		public void SetTagsArray(string[] tagsArray) {
			shareParams["tagsArray"] = tagsArray;
		}

		public void SetExtraInfo(string extraInfo) {
			shareParams["extraInfo"] = extraInfo;
		}

		public void SetSafetyLevel(int safetyLevel) {
			shareParams["safetyLevel"] = safetyLevel;
		}

		public void SetContentType(int contentType) {
			shareParams["contentType"] = contentType;
		}

		public void SetHidden(bool hidden) {
			shareParams["hidden"] = hidden;
		}

		public void SetIsPublic(bool isPublic) {
			shareParams["isPublic"] = isPublic;
		}

		public void SetIsFriend(bool isFriend) {
			shareParams["isFriend"] = isFriend;
		}

		public void SetIsFamily(bool isFamily) {
			shareParams["isFamily"] = isFamily;
		}

		public void SetFriendsOnly(bool friendsOnly) {
			#if UNITY_ANDROID
				shareParams["isFriend"] = friendsOnly;
			#elif UNITY_IPHONE
				shareParams["friendsOnly"] = friendsOnly;
			#endif
		}

		public void SetGroupID(string groupID) {
			shareParams["groupID"] = groupID;
		}

		public void SetAudioPath(string audioPath) {
			#if UNITY_ANDROID
				shareParams["filePath"] = audioPath;
			#elif UNITY_IPHONE
				shareParams["audioPath"] = audioPath;
			#endif
		}

		public void SetVideoPath(string videoPath) {
			#if UNITY_ANDROID
				shareParams["filePath"] = videoPath;
			#elif UNITY_IPHONE
				shareParams["videoPath"] = videoPath;
			#endif
		}

		public void SetNotebook(string notebook) {
			shareParams["notebook"] = notebook;
		}

		public void SetTags(string tags) {
			shareParams["tags"] = tags;
		}

		public void SetPrivateStatus(int status) {
			shareParams["privateStatus"] = status;
		}

		public void SetObjectID(string objectId) {
			shareParams["objectId"] = objectId;
		}

		public void SetAlbumID(string albumId) {
			shareParams["AlbumID"] = albumId;
		}

		public void SetEmotionPath(string emotionPath) {
			shareParams["emotionPath"] = emotionPath;
		}

		public void SetExtInfoPath(string extInfoPath) {
			shareParams["extInfoPath"] = extInfoPath;
		}

		public void SetSourceFileExtension(string sourceFileExtension) {
			shareParams["sourceFileExtension"] = sourceFileExtension;
		}

		public void SetAssetLocalIds(string assetLocalIds) {
			shareParams["assetLocalIds"] = assetLocalIds;
		}

		public void SetAssetLocalIdsArray(string[] assetLocalIdsArray) {
			shareParams["asset_localIds"] = assetLocalIdsArray;
		}

		public void SetDouyinHashtag(string douyin_hashtag) {
			shareParams["douyin_hashtag"] = douyin_hashtag;
		}

		public void SetTiktokHashtag(string tiktok_hashtag) {
			shareParams["tiktok_hashtag"] = tiktok_hashtag;
		}

		public void SetTiktok_extraInfo(Dictionary<string,string> tiktok_extraInfo) {
			shareParams["tiktok_extraInfo"] = tiktok_extraInfo;
		}

		public void SetDouyin_shareActionMode(int shareActionMode) {
			shareParams["shareActionMode"] = shareActionMode;
		}

		public void SetDouyin_extraInfo(Dictionary<string, string> douyin_extraInfo) {
			shareParams["douyin_extraInfo"] = douyin_extraInfo;
		}

		public void SetSourceFilePath(string sourceFilePath) {
			shareParams["sourceFilePath"] = sourceFilePath;
		}

		public void SetThumbImageUrl(string thumbImageUrl) {
			shareParams["thumbImageUrl"] = thumbImageUrl;
		}

		public void SetUrlDescription(string urlDescription) {
			shareParams["urlDescription"] = urlDescription;
		}

		public void SetBoard(string SetBoard) {
			shareParams["SetBoard"] = SetBoard;
		}

		public void SetMenuX(float menuX) {
			shareParams["menuX"] = menuX;
		}

		public void SetMenuY(float menuY) {
			shareParams["menuY"] = menuY;
		}

		public void SetVisibility(string visibility) {
			shareParams["visibility"] = visibility;
		}

		public void SetBlogName(string blogName) {
			shareParams["blogName"] = blogName;
		}

		public void SetMediaDataPath(string mediaDataPath) {
			shareParams["mediaDataPath"] = mediaDataPath;
		}

		public void SetRecipients(string recipients) {
			shareParams["recipients"] = recipients;
		}

		public void SetCCRecipients(string ccRecipients) {
			shareParams["ccRecipients"] = ccRecipients;
		}

		public void SetBCCRecipients(string bccRecipients) {
			shareParams["bccRecipients"] = bccRecipients;
		}

		public void SetAttachmentPath(string attachmentPath) {
			shareParams["attachmentPath"] = attachmentPath;
		}

		public void SetDesc(string desc) {
			shareParams["desc"] = desc;
		}

		public void SetIsPrivateFromSource(bool isPrivateFromSource) {
			shareParams["isPrivateFromSource"] = isPrivateFromSource;
		}

		public void SetResolveFinalUrl(bool resolveFinalUrl) {
			shareParams["resolveFinalUrl"] = resolveFinalUrl;
		}

		public void SetFolderId(int folderId) {
			shareParams["folderId"] = folderId;
		}

		public void SetTweetID(string tweetID) {
			shareParams["tweetID"] = tweetID;
		}

		public void SetToUserID(string toUserID) {
			shareParams["toUserID"] = toUserID;
		}

		public void SetPermission(string permission) {
			shareParams["permission"] = permission;
		}

		public void SetEnableShare(bool enableShare) {
			shareParams["enableShare"] = enableShare;
		}

		public void SetImageWidth(float imageWidth) {
			shareParams["imageWidth"] = imageWidth;
		}

		public void SetImageHeight(float imageHeight) {
			shareParams["imageHeight"] = imageHeight;
		}

		public void SetAppButtonTitle(string appButtonTitle) {
			shareParams["appButtonTitle"] = appButtonTitle;
		}

		public void SetAndroidExecParam(Hashtable androidExecParam) {
			shareParams["androidExecParam"] = androidExecParam;
		}

		public void SetAndroidMarkParam(string androidMarkParam) {
			shareParams["androidMarkParam"] = androidMarkParam;
		}

		public void SetIphoneExecParam(Hashtable iphoneExecParam) {
			shareParams["iphoneExecParam"] = iphoneExecParam;
		}

		public void SetIphoneMarkParam(string iphoneMarkParam) {
			shareParams["iphoneMarkParam"] = iphoneMarkParam;
		}

		public void SetIpadExecParam(Hashtable ipadExecParam) {
			shareParams["ipadExecParam"] = ipadExecParam;
		}

		public void SetIpadMarkParam(string ipadMarkParam) {
			shareParams["ipadMarkParam"] = ipadMarkParam;
		}

		public void SetTemplateArgs(Hashtable templateArgs) {
			shareParams["templateArgs"] = templateArgs;
		}

		public void SetTemplateId(string templateId) {
			shareParams["templateId"] = templateId;
		}

		public void SetFacebookHashtag(string hashtag) {
			#if UNITY_ANDROID
				shareParams["HASHTAG"] = hashtag;
			#elif UNITY_IPHONE
				shareParams["hashtag"] = hashtag;
			#endif
		}

		public void SetFacebookAssetsArray(string[] imageAsset, string videoAsset) {
			#if UNITY_IPHONE
				if (imageAsset != null) {
					shareParams["facebook_imageasset"] = String.Join(",",imageAsset);
				}
				if (videoAsset != null) {
					shareParams["facebook_videoasset"] = videoAsset;
				}
			#endif
		}

		public void SetFacebookQuote(string quote) {
			#if UNITY_ANDROID
				shareParams["QUOTE"] = quote;
			#elif UNITY_IPHONE
				shareParams["quote"] = quote;
			#endif
		}

		public void setFacebookShareType(int type) {
			shareParams["facebook_shareType"] = type;
		}

		public void setFacebookShareTypes(int[] type) {
			shareParams["facebook_shareTypes"] = type;
		}

		public void SetMessengerGif(string gif) {
			shareParams["gif"] = gif;
		}

		public void SetEnableClientShare(bool enable) {
			shareParams["clientShare"] = enable;
		}

		public void SetEnableSinaWeiboAPIShare(bool enable) {
			shareParams["apiShare"] = enable;
		}

		public void SetEnableAdvancedInterfaceShare(bool enalble) {
			shareParams["advancedShare"] = enalble;
		}

		public void SetSinaShareEnableShareToStory(bool enalble) {
			shareParams["isShareToStory"] = enalble;
		}

		public void SetMiniProgramUserName(string userName) {
			shareParams["wxUserName"] = userName;
		}

		public void SetMiniProgramAppID(string appID) {
			#if UNITY_ANDROID
				shareParams["mini_program_appid"] = appID;
			#elif UNITY_IPHONE
				shareParams["qqMiniProgramAppID"] = appID;
			#endif
		}

		public void SetMiniProgramPath(string path) {
			shareParams["wxPath"] = path;
			#if UNITY_ANDROID
				shareParams["mini_program_path"] = path;
			#elif UNITY_IPHONE
				shareParams["qqMiniProgramPath"] = path;
			#endif
		}

		public void SetMiniProgramWithShareTicket(bool enalble) {
			shareParams ["wxWithShareTicket"] = enalble;
		}

		public void SetMiniProgramType(int type) {
			shareParams ["wxMiniProgramType"] = type;
			shareParams["qqMiniprogramType"] = type;
		}

		public void SetQQMiniProgramType(string type) {
			shareParams["mini_program_type"] = type;
		}

		public void SetMiniProgramHdThumbImage(string hdThumbImage) {
			shareParams ["wxMiniProgramHdThumbImage"] = hdThumbImage;
			#if UNITY_IPHONE
				shareParams["qqMiniProgramHdThumbImage"] = hdThumbImage;
			#endif
		}

		public void SetMiniProgramWebpageUrl(string webpageUrl) {
			shareParams["qqMiniProgramWebpageUrl"] = webpageUrl;
		}

		public void SetSubreddit(string subreddit) {
			shareParams["sr"] = subreddit;
		}

		public void SetSinaLinkCard(bool enable) {
			shareParams["sina_linkCard"] = enable;
		}

		public void SetSinaCardTitle(string title) {
			shareParams["sina_cardTitle"] = title;
		}

		public void SetSinaCardSummary(string summary) {
			#if UNITY_ANDROID
				shareParams["lc_summary"] = summary;
			#elif UNITY_IPHONE
				shareParams["sina_cardSummary"] = summary;
			#endif
		}

		public void SetSinaCardImageAndroid(JsonData jsonobject) {
			shareParams["lc_image"] = jsonobject;
		}

		public void SetSinaCardTypeAndroid(string type) {
			shareParams["lc_object_type"] = type;
		}

		public void SetSinaCardDisplayNameAndroid(string displayname) {
			shareParams["lc_display_name"] = displayname;
		}

		public void SetSinaCardCreateAtAndroid(string createtime) {
			shareParams["lc_create_at"] = createtime;
		}

		public void SetSinaCardURLAndroid(string url) {
			shareParams["lc_url"] = url;
		}

		public void SetDisableNewTask(bool disableNewTask) {
			shareParams["disableNewTask"] =  disableNewTask;
		}

		/// <summary>
		/// 抖音视频9.0及其以上版本系统需要传的activity
		/// <summary>
		#if UNITY_ANDROID
		public void SetActivity(AndroidJavaObject activity) {
			shareParams["activity"] = activity;
		}
		#endif

		/// <summary>
		/// 不同平台分享不同内容
		/// <summary>
		public void SetShareContentCustomize(PlatformType platform, ShareContent content) {
			customizeShareParams [(int)platform] = content.GetShareParamsStr();
		}

		public string GetShareParamsStr() {
			if (customizeShareParams.Count > 0) {
				shareParams["customizeShareParams"] = customizeShareParams;
			}
			String jsonStr = MiniJSON.jsonEncode (shareParams);
			return jsonStr;
		}

		public Hashtable GetShareParams() {
			if (customizeShareParams.Count > 0) {
				shareParams["customizeShareParams"] = customizeShareParams;
			}
			String jsonStr = MiniJSON.jsonEncode (shareParams);
			return shareParams;
		}
	}
}