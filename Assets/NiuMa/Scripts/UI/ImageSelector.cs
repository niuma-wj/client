using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEngine
{
    public class ImageSelector : MonoBehaviour
    {
        [SerializeField]
        private List<Sprite> _sprites = new List<Sprite>();

        private Image _image = null;

        private void Awake()
        {
            _image = gameObject.GetComponent<Image>();
        }

        // Use this for initialization
        private void Start()
        { }

        // Update is called once per frame
        private void Update()
        { }

        public void SetSprite(int idx, bool nativeSize = true)
        {
            if (idx < 0 || idx >= _sprites.Count)
                return;

            if (_image != null)
            {
                _image.sprite = _sprites[idx];
                if (nativeSize)
                    _image.SetNativeSize();
            }
        }
    }
}