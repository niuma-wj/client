using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickShowHide : MonoBehaviour
{
	// Use this for initialization
	void Start()
    {}
	
	// Update is called once per frame
	void Update()
    {}

    public void ShowMyself()
    {
        gameObject.SetActive(true);
    }

    public void HideMyself()
    {
        gameObject.SetActive(false);
    }
}
