using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using NiuMa;

public class DialogSetting : MonoBehaviour
{
    // 在编辑器中绑定UI控件，方便代码重用
    public Slider _sliderVolumeMusic = null;
    public Slider _sliderVolumeSound = null;
    public Toggle _checkMuteMusic = null;
    public Toggle _checkMuteSound = null;
    private bool _refreshing = false;

    // Use this for initialization
    protected virtual void Start()
    {
        if (_sliderVolumeMusic != null)
            _sliderVolumeMusic.onValueChanged.AddListener(new UnityAction<float>(this.OnVolumeMusicChanged));
        if (_sliderVolumeSound != null)
            _sliderVolumeSound.onValueChanged.AddListener(new UnityAction<float>(this.OnVolumeSoundChanged));
        if (_checkMuteMusic != null)
            _checkMuteMusic.onValueChanged.AddListener(new UnityAction<bool>(this.OnMuteMusicChanged));
        if (_checkMuteSound != null)
            _checkMuteSound.onValueChanged.AddListener(new UnityAction<bool>(this.OnMuteSoundChanged));

        Refresh();
    }

    // Update is called once per frame
    protected virtual void Update()
    {}

    public void Refresh()
    {
        _refreshing = true;
        if (_sliderVolumeMusic != null)
            _sliderVolumeMusic.value = AudioManager.Instance.VolumeMusic;
        if (_sliderVolumeSound != null)
            _sliderVolumeSound.value = AudioManager.Instance.VolumeSound;
        if (_checkMuteMusic != null)
            _checkMuteMusic.isOn = !(AudioManager.Instance.MuteMusic);
        if (_checkMuteSound != null)
            _checkMuteSound.isOn = !(AudioManager.Instance.MuteSound);
        _refreshing = false;
    }

    private void OnVolumeMusicChanged(float v)
    {
        if (_refreshing)
            return;

        AudioManager.Instance.SetVolumeMusic(v);
    }

    private void OnVolumeSoundChanged(float v)
    {
        if (_refreshing)
            return;
        AudioManager.Instance.SetVolumeSound(v);
    }

    private void OnMuteMusicChanged(bool s)
    {
        if (_refreshing)
            return;
        AudioManager.Instance.SetMuteMusic(!s);
    }

    private void OnMuteSoundChanged(bool s)
    {
        if (_refreshing)
            return;
        AudioManager.Instance.SetMuteSound(!s);
    }
}