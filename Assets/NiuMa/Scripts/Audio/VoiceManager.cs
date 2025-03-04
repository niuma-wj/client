using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using XLua;

namespace NiuMa
{
    // 录音器接口
    public interface RecorderListener
    {
        void OnRecordStart();
        void OnRecordStop();
    }

    /// <summary>
    /// 语音管理者，因为要发起携程，所以该类继承于MonoBehaviour，在使用的时候注意
    /// 全局只需要一个GameObject实例挂了该脚本
    /// </summary>
    [LuaCallCSharp]
    public class VoiceManager : MonoBehaviour
    {
        private static VoiceManager _instance = null;
        public static VoiceManager Instance
        {
            get { return _instance; }
        }

        private static void setInstance(VoiceManager inst)
        {
            if (_instance == null)
                _instance = inst;
        }

        // 是否已经初始化
        private bool _initialized = false;
        public bool Initialized
        {
            get { return _initialized; }
        }

        // 是否正在录音
        private bool _recording = false;
        public bool Recording
        {
            get { return _recording; }
        }

        // 录音监听器接口
        private List<RecorderListener> _recorders = new List<RecorderListener>();

        // 语音监听器
        private List<VoiceListener> _listeners = new List<VoiceListener>();

        // 播放队列
        private Queue<KeyValuePair<string, int> > _playQueue = new Queue<KeyValuePair<string, int> >();

        // 当前是否正在播放
        private bool _playing = false;

        // 是否放弃录音
        private bool _abort = false;

        // 声音源
        private AudioSource _audioSource = null;

        // 录制的音频剪辑
        private AudioClip _recordedClip = null;

        // 加载的音频剪辑
        private AudioClip _loadedClip = null;

        void Awake()
        {
            setInstance(this);
        }

        void Start()
        {
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            if (audioSource != null)
                _audioSource = audioSource;
            Initialize();
        }

        // 初始化语音
        private void Initialize()
        {
            if (_initialized)
                return;
            string[] devices = Microphone.devices;
            if (devices.Length == 0)
            {
                Debug.LogError("语音初始化失败，未找到麦克风设备");
                GameManager.Instance.ShowPromptTip("语音初始化失败，未找到麦克风设备", 3.0f);
                return;
            }
            _initialized = true;
        }

        [BlackList]
        public void AddRecorder(RecorderListener rec)
        {
            if (rec == null)
                return;

            if (!_recorders.Contains(rec))
                _recorders.Add(rec);
        }

        [BlackList]
        public void RemoveRecorder(RecorderListener rec)
        {
            _recorders.Remove(rec);
        }

        [BlackList]
        public void AddListener(VoiceListener lis)
        {
            if (lis == null)
                return;

            if (!_listeners.Contains(lis))
                _listeners.Add(lis);
        }

        [BlackList]
        public void RemoveListener(VoiceListener lis)
        {
            _listeners.Remove(lis);
        }

        public void StartRecord()
        {
            if (!_initialized || _recording)
                return;

            _abort = false;
            _recording = true;
            _recordedClip = Microphone.Start(null, false, 60, 11025);
            AudioManager.Instance.StartRecord();
            foreach (RecorderListener rec in _recorders)
                rec.OnRecordStart();
        }

        public void StopRecord(bool abort)
        {
            if (!_recording)
                return;

            _abort = abort;
            _recording = false;
            int position = Microphone.GetPosition(null);
            Microphone.End(null);
            AudioManager.Instance.StopRecord();
            foreach (RecorderListener rec in _recorders)
                rec.OnRecordStop();
            if (_recordedClip != null)
            {
                string fileName = DateTime.Now.ToString("MMddHHmm");
                fileName = "my_record_" + fileName + ".mp3";
                string path = Path.Combine(Application.persistentDataPath, fileName);
                AudioClipConverter.SaveMp3(_recordedClip, position, path);
                foreach (VoiceListener lis in _listeners)
                    lis.OnRecordCompleted(fileName);
                _recordedClip = null;
            }
        }

        public void PushVoice(string fileName, int seat)
        {
            _playQueue.Enqueue(new KeyValuePair<string, int>(fileName, seat));
            PlayVoice();
        }

        [BlackList]
        private void PlayVoice()
        {
            if (_playing)
                return;

            while (_playQueue.Count > 0)
            {
                KeyValuePair<string, int> pr = _playQueue.Dequeue();
                // 先将mp3文件转成wav文件
                string mp3File = Path.Combine(Application.persistentDataPath, pr.Key);
                string wavFile = mp3File.Replace(".mp3", ".wav");
                if (!AudioClipConverter.ConvertMp3ToWav(mp3File, wavFile))
                {
                    Debug.LogFormat("Convert mp3 file to wav file failed, mp3 file: {0}", pr.Key);
                    continue;
                }
                // 删除mp3文件
                File.Delete(mp3File);
                // 从wav文件加载AudioClip
                StartCoroutine(LoadAudio(wavFile, pr.Key, pr.Value));
                break;
            }
        }

        private void OnPlayStart(string fileName, int seat)
        {
            if (_loadedClip == null)
            {
                PlayVoice();
                return;
            }
            _playing = true;
            _audioSource.clip = _loadedClip;
            _audioSource.Play();
            StartCoroutine(AudioPlayFinished(_loadedClip.length, fileName, seat));
            Debug.Log(string.Format("start play voice file: {0}, seat: {1}", fileName, seat));
            foreach (VoiceListener lis in _listeners)
                lis.OnPlayStart(fileName, seat);
        }

        private IEnumerator AudioPlayFinished(float time, string fileName, int seat)
        {
            yield return new WaitForSeconds(time);
            OnPlayStop(fileName, seat);
        }

        private void OnPlayStop(string fileName, int seat)
        {
            _playing = false;
            _loadedClip = null;
            Debug.Log(string.Format("stop play voice file: {0}, seat: {1}", fileName, seat));
            foreach (VoiceListener lis in _listeners)
                lis.OnPlayStop(fileName, seat);

            PlayVoice();
        }

        IEnumerator LoadAudio(string wavFile, string fileName, int seat)
        {
#if UNITY_EDITOR
#elif UNITY_ANDROID
            wavFile = "file://" + wavFile;
#endif
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(wavFile, AudioType.WAV))
            {
                yield return www.SendWebRequest();
                if (www.result == UnityWebRequest.Result.Success)
                {
                    _loadedClip = DownloadHandlerAudioClip.GetContent(www);
                    OnPlayStart(fileName, seat);
                }
                else
                {
                    Debug.LogErrorFormat("Load audio {0} error: {1}", wavFile, www.error);
                }
            }
            // 删除wav文件
            File.Delete(wavFile);
        }
    }
}