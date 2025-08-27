using System;
using UnityEngine;
using UnityEngine.UI;

public class SimpleWatch : MonoBehaviour
{
    [SerializeField]
    private Text _text;

    private float _elapsed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {}

    // Update is called once per frame
    void Update()
    {
        if (_text == null)
            return;
        _elapsed += Time.fixedUnscaledDeltaTime;
        if (_elapsed < 0.1f)
            return;
        _elapsed -= 0.1f;
        DateTime date = DateTime.Now;
        string div = date.Second % 2 == 0 ? ":" : " ";
        _text.text = string.Format("{0:00}{1}{2:00}", date.Hour, div, date.Minute);
    }
}
