using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PromptDialog : MonoBehaviour
{
    private Text _label = null;
    
    private event UnityAction _okClicked = null;
    private event UnityAction _cancelClicked = null;

    private void Awake()
    {
        Transform child = gameObject.transform.Find("Frame");
        Transform child1 = null;
        if (child != null)
        {
            child1 = child.Find("Description/Text");
            if (child1 != null)
                _label = child1.gameObject.GetComponent<Text>();
        }
    }

    public void OnOkClick()
    {
        GameObject.Destroy(gameObject);

        if (_okClicked != null)
            _okClicked();
    }

    public void OnCancelClick()
    {
        GameObject.Destroy(gameObject);

        if (_cancelClicked != null)
            _cancelClicked();
    }

    public void SetDescription(string desc)
    {
        if (_label != null)
            _label.text = desc;
    }

    public void SetButtonClickedHandler(UnityAction handler, bool ok)
    {
        if (handler == null)
            return;

        if (ok)
        {
            if (_okClicked != null)
                _okClicked += handler;
            else
                _okClicked = handler;
        }
        else
        {
            if (_cancelClicked != null)
                _cancelClicked += handler;
            else
                _cancelClicked = handler;
        }
    }
}
