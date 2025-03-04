using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using NiuMa;

public class WebImageViewer : MonoBehaviour
{
    public string _url = "";
    private RawImage _img = null;

    private void Awake()
    {
        _img = gameObject.GetComponent<RawImage>();
    }

    // Use this for initialization
    void Start()
    {
        if (_url.Length == 0 || _img == null)
            return;
        HttpRequester.Instance.GetTexture(_url, (tex) =>
        {
            if (tex != null)
                _img.texture = tex;
        });
    }
}
