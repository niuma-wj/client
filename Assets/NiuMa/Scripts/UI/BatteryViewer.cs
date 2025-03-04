using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryViewer : MonoBehaviour
{
    private Image _energy = null;
    private float _elapsed = 11.0f;

    private void Awake()
    {
        Transform child = gameObject.transform.Find("Fill");
        if (child != null)
            _energy = child.gameObject.GetComponent<Image>();
    }

    // Use this for initialization
    void Start()
    {}
	
	// Update is called once per frame
	void Update()
    {
        if (_energy == null)
            return;

        if (_elapsed < 10.0f)
        {
            _elapsed += Time.unscaledDeltaTime;
            return;
        }
        _elapsed = 0.0f;
        _energy.fillAmount = SystemInfo.batteryLevel;
    }
}
