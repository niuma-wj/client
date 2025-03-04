using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using XLua;

namespace UnityEngine
{
    [LuaCallCSharp]
    public class PanelToucher : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private event UnityAction _pointerDownHandler;
        private event UnityAction _pointerUpHandler;

        // Use this for initialization
        void Start()
        { }

        // Update is called once per frame
        void Update()
        { }

        public void AddPointerHandler(bool down, UnityAction del)
        {
            if (del == null)
                return;
            if (down)
                _pointerDownHandler += del;
            else
                _pointerUpHandler += del;
        }

        public void RemovePointerHandler(bool down, UnityAction del)
        {
            if (del == null)
                return;
            if (down)
                _pointerDownHandler -= del;
            else
                _pointerUpHandler -= del;
        }

        [BlackList]
        public void OnPointerDown(PointerEventData eventData)
        {
            if (_pointerDownHandler != null)
                _pointerDownHandler();
        }

        [BlackList]
        public void OnPointerUp(PointerEventData eventData)
        {
            if (_pointerUpHandler != null)
                _pointerUpHandler();
        }
    }
}

