using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace NiuMa
{
    [LuaCallCSharp]
    public class AudioManager
    {
        // 禁止外部创建
        private AudioManager() { }
        private static readonly AudioManager _instance = new AudioManager();

        public static AudioManager Instance
        {
            get { return _instance; }
        }

        // 背景音乐声源（唯一一个）
        private AudioSource _bgMusic = null;
        public AudioSource BackgroundMusic
        {
            get { return _bgMusic; }
        }

        // 音效控制器列表
        private List<SoundEffect> _sounds = new List<SoundEffect>();

        // 音乐音量
        private float _volumeMusic = 0.5f;
        public float VolumeMusic
        {
            get { return _volumeMusic; }
        }

        // 音效音量
        private float _volumeSound = 0.5f;
        public float VolumeSound
        {
            get { return _volumeSound; }
        }

        // 音乐是否静音
        private bool _muteMusic = false;
        public bool MuteMusic
        {
            get { return _muteMusic; }
        }

        // 音效是否静音
        private bool _muteSound = false;
        public bool MuteSound
        {
            get { return _muteSound; }
        }

        // 初始化音量
        [BlackList]
        public void Initialize()
        {
            if (PlayerPrefs.HasKey("VolumeMusic"))
                _volumeMusic = PlayerPrefs.GetFloat("VolumeMusic");
            if (PlayerPrefs.HasKey("VolumeSound"))
                _volumeSound = PlayerPrefs.GetFloat("VolumeSound");
            if (PlayerPrefs.HasKey("MuteMusic"))
                _muteMusic = (PlayerPrefs.GetInt("MuteMusic") != 0);
            if (PlayerPrefs.HasKey("MuteSound"))
                _muteMusic = (PlayerPrefs.GetInt("MuteSound") != 0);

            if (_bgMusic != null)
            {
                _bgMusic.volume = _volumeMusic;
                _bgMusic.mute = _muteMusic;
            }
        }

        // 设置背景音乐声源
        [BlackList]
        public void SetBackgroundMusic(AudioSource au)
        {
            _bgMusic = au;
        }

        // 添加音效控制器
        [BlackList]
        public void AddSoundEffect(SoundEffect se)
        {
            if (se == null)
                return;
            if (_sounds.Contains(se))
                return;
            _sounds.Add(se);
        }

        // 移除音效控制器
        [BlackList]
        public void RemoveSoundEffect(SoundEffect se)
        {
            _sounds.Remove(se);
        }

        // 设置音乐音量
        public void SetVolumeMusic(float v)
        {
            _volumeMusic = v;
            PlayerPrefs.SetFloat("VolumeMusic", v);

            if (_bgMusic != null)
                _bgMusic.volume = v;
        }

        // 设置音效音量
        public void SetVolumeSound(float v)
        {
            _volumeSound = v;
            PlayerPrefs.SetFloat("VolumeSound", v);

            foreach (SoundEffect se in _sounds)
                se.SetVolume(v);
        }

        // 设置音乐静音
        public void SetMuteMusic(bool s)
        {
            _muteMusic = s;
            PlayerPrefs.SetFloat("MuteMusic", s ? 1 : 0);

            if (_bgMusic != null)
                _bgMusic.mute = s;
        }

        // 设置音效静音
        public void SetMuteSound(bool s)
        {
            _muteSound = s;
            PlayerPrefs.SetFloat("MuteSound", s ? 1 : 0);

            foreach (SoundEffect se in _sounds)
                se.SetMute(s);
        }

        // 开始录音，背景音乐及所有音效设置为静音
        [BlackList]
        public void StartRecord()
        {
            SetRecordMute(true);
        }

        // 停止录音，背景音乐及所有音效恢复发音
        [BlackList]
        public void StopRecord()
        {
            SetRecordMute(false);
        }

        // 如果原先设置了静音则录音开始时无需再设置，停止录音后也不能恢复发音
        private void SetRecordMute(bool s)
        {
            if (!_muteMusic)
            {
                if (_bgMusic != null)
                    _bgMusic.mute = s;
            }
            if (!_muteSound)
            {
                foreach (SoundEffect se in _sounds)
                    se.SetMute(s);
            }
        }
    }
}