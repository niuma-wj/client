using System.Collections;
using System.Collections.Generic;
using XLua;

namespace UnityEngine
{
    [LuaCallCSharp]
    public class BezierPath : MonoBehaviour
    {
        [SerializeField]
        private Vector2 _point1;
        [SerializeField]
        private Vector2 _point2;
        [SerializeField]
        private Vector2 _point3;
        private float _elapsed = 0.0f;
        [SerializeField]
        private float _duration = 0.0f;
        private bool _moving = false;
        private RectTransform _rcTrans = null;
        private System.Action<int> _move2EndCallback = null;
        private int _parameter = 0;

        // Use this for initialization
        private void Start()
        {
            _rcTrans = gameObject.GetComponent<RectTransform>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (!_moving || (_rcTrans == null))
                return;

            if (_elapsed < _duration)
            {
                float t = _elapsed / _duration;
                Vector2 pos = _point1 * (1.0f - t) * (1.0f - t) + _point2 * 2.0f * t * (1.0f - t) + _point3 * t * t;
                _rcTrans.anchoredPosition = pos;
                _elapsed += Time.unscaledDeltaTime;
            }
            else
            {
                _rcTrans.anchoredPosition = _point3;
                _moving = false;

                if (_move2EndCallback != null)
                    _move2EndCallback(_parameter);
            }
        }

        public void DoPath()
        {
            _elapsed = 0.0f;
            if (_duration > 0.0f)
                _moving = true;
        }

        public void SetPath(Vector2 p1, Vector2 p2, Vector2 p3, float dur)
        {
            _point1 = p1;
            _point2 = p2;
            _point3 = p3;
            _elapsed = 0.0f;
            _duration = dur;
            if (_duration > 0.0f)
                _moving = true;
        }

        public void SetMove2EndCallback(System.Action<int> del, int param = 0)
        {
            _move2EndCallback = del;
            _parameter = param;
        }
    }
}