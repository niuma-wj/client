using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using NiuMa;

public class PromptTip : MonoBehaviour
{
    private Image _frame = null;
    private Text _label = null;
    private string _text = "";
    // 提示的消失时间
    private float _life = 0.0f;
    private bool _waitTween = false;
    private bool _started = false;
    private bool _countdown = false;

    private void Awake()
    {
        _frame = gameObject.GetComponent<Image>();
        Transform child = gameObject.transform.Find("Text");
        if (child != null)
            _label = child.gameObject.GetComponent<Text>();
    }

    // Use this for initialization
    void Start()
    {
        _started = true;
        if (_waitTween)
        {
            if (_label != null)
                _label.text = _text;
            StartMove();
            _waitTween = false;
        }
    }
	
	// Update is called once per frame
	void Update()
    {
        if (!_countdown)
            return;

        _life -= Time.unscaledDeltaTime;
        if (_life < 0.0f)
        {
            _countdown = false;
            StartFadeOut();
        }
    }

    public void DoTip(string text, float life)
    {
        _text = text;
        _life = life;
        _countdown = false;
        if (_started)
        {
            if (_label != null)
                _label.text = text;
            StartMove();
        }
        else
            _waitTween = true;
    }

    private void StartMove()
    {
        RectTransform trans = gameObject.GetComponent<RectTransform>();
        if (trans != null)
        {
            Tweener tw = trans.DOAnchorPos(new Vector2(0.0f, 200.0f), 0.2f);
            if (tw != null)
                tw.OnComplete(this.OnMoveComplete);
        }
    }

    private void OnMoveComplete()
    {
        _countdown = true;
    }

    private void StartFadeOut()
    {
        if (_frame != null)
        {
            Tweener tw = _frame.DOFade(0.0f, 0.5f);
            if (tw != null)
                tw.OnComplete(this.OnFadeOutComplete);
        }
        if (_label != null)
            _label.DOFade(0.0f, 0.5f);
    }

    private void OnFadeOutComplete()
    {
        NiuMa.GameManager.Instance.PromptTipDisappear();
    }
}