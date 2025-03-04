using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class WebTextViewer : MonoBehaviour
{
    public string _url = "";
    private Text _text = null;

    private void Awake()
    {
        _text = gameObject.GetComponent<Text>();
    }

    // Use this for initialization
    void Start()
    {
        if (_url.Length == 0 || _text == null)
            return;
        StartCoroutine(DownloadWebText());
    }
	
	// Update is called once per frame
	void Update()
    {}

    private IEnumerator DownloadWebText()
    {
        if (_url != null && _url.Length > 0)
        {
            UnityWebRequest request = UnityWebRequest.Get(_url);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                if (_text != null)
                    _text.text = request.downloadHandler.text;
            }
            else
                Debug.LogErrorFormat("Download text error: {0}", request.error);
        }
    }
}
