using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using NiuMa;

public class VoiceRecorder : MonoBehaviour, RecorderListener, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    // 录音秒表
    public GameObject _stopwatch = null;
    public Text _textSec = null;
    public Image _progress = null;
    // 录音已经过了多久
    private float _recordTime = 0.0f;
    // 按钮已经按下了多久，若按钮按下的持续时长少于500毫秒(例如快速不停点击按钮)，
    // 则不能立即停止录音否则程序可能会崩溃，这里强制录音最短1秒中才能结束
    private float _elapsed = 0.0f;
    private bool _force500 = false;
    // 录音按钮是否被按下
    private bool _down = false;
    private bool _abort = false;

    // Use this for initialization
    private void Start()
    {
        VoiceManager.Instance.AddRecorder(this);
    }

    // Update is called once per frame
    private void Update()
    {
        if (_down)
            _elapsed += Time.unscaledDeltaTime;
        if (_force500)
        {
            _elapsed += Time.unscaledDeltaTime;
            if (_elapsed > 0.5f)
            {
                _force500 = false;
                VoiceManager.Instance.StopRecord(_abort);
            }
        }
        if (VoiceManager.Instance.Recording)
        {
            _recordTime += Time.unscaledDeltaTime;
            if (_textSec != null)
            {
                int sec = Mathf.FloorToInt(_recordTime);
                _textSec.text = sec.ToString();
            }
            if (_progress != null)
                _progress.fillAmount = _recordTime / 60.0f;
        }
    }

    private void OnDestroy()
    {
        VoiceManager.Instance.RemoveRecorder(this);
    }

    public void OnRecordStart()
    {
        _recordTime = 0.0f;
        if (_stopwatch != null)
            _stopwatch.SetActive(true);
    }

    public void OnRecordStop()
    {
        if (_stopwatch != null)
            _stopwatch.SetActive(false);
    }

    // 当按钮被按下后系统自动调用此方法
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_force500)
        {
            GameManager.Instance.ShowPromptTip("请等待当前录音结束，不要快速点击录音按钮!", 2.0f);
            return;
        }
        Debug.Log("OnPointerDown");
        if (_down)
        {
            Debug.Log("录音按钮状态错误!");
            return;
        }
        _down = true;
        _abort = false;
        _elapsed = 0.0f;
        VoiceManager.Instance.StartRecord();
    }

    // 当按钮抬起的时候自动调用此方法
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp");
        if (!_down)
            return;
        _down = false;
        if (_abort)
            return;
        if (_elapsed < 0.5f)
        {
            // 按下按钮的持续时间少于500毫秒，强制500毫秒
            _force500 = true;
            return;
        }
        VoiceManager.Instance.StopRecord(false);
    }

    // 当鼠标从按钮上离开的时候自动调用此方法
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_down)
            return;

        _abort = true;
        if (_elapsed < 0.5f)
        {
            // 按下按钮的持续时间少于500毫秒，强制500毫秒
            _force500 = true;
            return;
        }
        VoiceManager.Instance.StopRecord(true);
    }
}
