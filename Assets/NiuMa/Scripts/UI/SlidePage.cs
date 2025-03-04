using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class SlidePage : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public Sprite[] _dotSprites = new Sprite[2];
    public List<Image> _pageDots = new List<Image>();

    public List<float> _pageArray = new List<float>();

    private ScrollRect _scrollRect = null;

    // 滑动速度
    private float _smooting = 10.0f;

    // 滑动的起始坐标
    private float _targetHorizontal = 0;

    // 是否拖拽结束
    private bool _isDraging = false;

    // 是否要做插值
    private bool _isLerping = false;

    private int _currentPage = 0;

    private float _flipElpased = 0.0f;

    private void Awake()
    {
        _scrollRect = gameObject.GetComponent<ScrollRect>();
    }

    // Use this for initialization
    void Start()
    {
        if (_pageDots.Count > 0)
        {
            if (_pageDots[0] != null)
                _pageDots[0].sprite = _dotSprites[1];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_scrollRect == null)
            return;

        // 如果不判断。当在拖拽的时候要也会执行插值，所以会出现闪烁的效果
        // 这里只要在拖动结束的时候。在进行插值
        if (!_isDraging)
        {
            if (_isLerping)
            {
                float temp = _scrollRect.horizontalNormalizedPosition;
                temp = Mathf.Lerp(temp, _targetHorizontal, Time.deltaTime * _smooting);
                _scrollRect.horizontalNormalizedPosition = temp;
                if (Math.Abs(temp - _targetHorizontal) < 1.0e-3f)
                    _isLerping = false;
            }
            else if (_pageDots.Count > 0)
            {
                _flipElpased += Time.deltaTime;

                if (_flipElpased > 5.0f)
                {
                    _flipElpased -= 5.0f;
                    _currentPage++;
                    _currentPage %= _pageDots.Count;
                    _targetHorizontal = _pageArray[_currentPage];
                    _isLerping = true;

                    SetPageDots();
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _isDraging = true;

        _flipElpased = 0.0f;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDraging = false;
        if (_scrollRect == null)
            return;
        if (_pageArray == null || _pageArray.Count == 0)
            return;

        //    //拖动停止滑动的坐标 
        //    Vector2 f = rect.normalizedPosition;
        //    //水平  开始值是0  结尾值是1  [0,1]
        //    float h = rect.horizontalNormalizedPosition;
        //    //垂直
        //    float v = rect.verticalNormalizedPosition;
        float posX = _scrollRect.horizontalNormalizedPosition;
        int index = 0;
        // 假设离第一位最近
        float offset = Mathf.Abs(_pageArray[index] - posX);
        for (int i = 1; i < _pageArray.Count; i++)
        {
            float temp = Mathf.Abs(_pageArray[i] - posX);
            if (temp < offset)
            {
                index = i;
                // 保存当前的偏移量
                // 如果到最后一页。反翻页。所以要保存该值，如果不保存。你试试效果就知道
                offset = temp;
            }
        }
        _currentPage = index;
        _targetHorizontal = _pageArray[index];
        _isLerping = true;

        SetPageDots();
    }
    
    private void SetPageDots()
    {
        for (int i = 0; i < _pageDots.Count; i++)
        {
            if (_pageDots[i] == null)
                continue;

            if (i == _currentPage)
                _pageDots[i].sprite = _dotSprites[1];
            else
                _pageDots[i].sprite = _dotSprites[0];
        }
    }
}
