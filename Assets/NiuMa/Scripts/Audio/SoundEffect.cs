using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NiuMa;

public class SoundEffect : MonoBehaviour
{
    public List<AudioSource> _sounds = new List<AudioSource>();

	// Use this for initialization
	protected virtual void Start()
    {
        AudioManager.Instance.AddSoundEffect(this);

        SetVolume(AudioManager.Instance.VolumeSound);
        SetMute(AudioManager.Instance.MuteSound);
    }

    protected virtual void OnDestroy()
    {
        AudioManager.Instance.RemoveSoundEffect(this);
    }

    // Update is called once per frame
    protected virtual void Update()
    {}

    public void SetVolume(float v)
    {
        foreach (AudioSource a in _sounds)
            a.volume = v;
    }

    public void SetMute(bool s)
    {
        foreach (AudioSource a in _sounds)
            a.mute = s;
    }
}
