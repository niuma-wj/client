using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragScrollRectSetter : MonoBehaviour
{
    public DragScrollRect _scrollRect = null;
    public float _viewExtentX = 100.0f;
    public float _viewExtentY = 100.0f;

    // Use this for initialization
    void Start()
    {
        if (_scrollRect != null)
            _scrollRect.SetViewExtent(_viewExtentX, _viewExtentY);
    }
	
	// Update is called once per frame
	void Update()
    {}
}
