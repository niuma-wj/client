using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using XLua;

// 矩形选择器，支持点选和框选，对于点选的情况如果两个矩形重叠，
// 排在列表后面的优先被选中
namespace UnityEngine
{
    [LuaCallCSharp]
    public class RectangleSelector : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerClickHandler
    {
        [SerializeField]
        private List<RectTransform> _rectangles = new List<RectTransform>();

        [SerializeField]
        private int _maxSelects = -1;

        private bool _draging = false;

        private event Action<int> RectangleSelectedHandler;

        private Vector2 _dragStart;

        // Use this for initialization
        void Start()
        {}

        // Update is called once per frame
        void Update()
        {}

        public void AddRectangle(RectTransform rt)
        {
            if (rt == null)
                return;

            if (!_rectangles.Contains(rt))
                _rectangles.Add(rt);
        }

        public void RemoveRectangle(RectTransform rt)
        {
            if (rt == null)
                return;

            if (_rectangles.Contains(rt))
                _rectangles.Remove(rt);
        }

        public void RemoveAllRectangles()
        {
            _rectangles.Clear();
        }

        public void AddSelectedHandler(Action<int> del)
        {
            RectangleSelectedHandler += del;
        }

        public void RemoveSelectedHandler(Action<int> del)
        {
            RectangleSelectedHandler -= del;
        }

        [BlackList]
        public void OnBeginDrag(PointerEventData eventData)
        {
            _draging = true;
            _dragStart = eventData.position;
        }

        [BlackList]
        public void OnDrag(PointerEventData eventData)
        {}

        [BlackList]
        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_draging)
                return;
            int cnt = _rectangles.Count;
            if (cnt == 0)
                return;
            Vector2 startPoint;
            Vector2 endPoint;
            float minX = 0.0f;
            float maxX = 0.0f;
            float minY = 0.0f;
            float maxY = 0.0f;
            Rect rc1;
            Rect rc2;
            int nums = 0;
            for (int i = cnt - 1; i > -1; i--)
            {
                if (_maxSelects >= 0 && nums >= _maxSelects)
                    break;
                if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectangles[i], _dragStart, Camera.main, out startPoint) ||
                    !RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectangles[i], eventData.position, Camera.main, out endPoint))
                    continue;
                if (startPoint.x < endPoint.x)
                {
                    minX = startPoint.x;
                    maxX = endPoint.x;
                }
                else
                {
                    maxX = startPoint.x;
                    minX = endPoint.x;
                }
                if (startPoint.y < endPoint.y)
                {
                    minY = startPoint.y;
                    maxY = endPoint.y;
                }
                else
                {
                    maxY = startPoint.y;
                    minY = endPoint.y;
                }
                rc1 = Rect.MinMaxRect(minX, minY, maxX, maxY);
                rc2 = _rectangles[i].rect;
                if (rc1.Overlaps(rc2))
                {
                    if (RectangleSelectedHandler != null)
                        RectangleSelectedHandler(i);
                    nums++;
                    //Debug.Log(string.Format("Frame select rectangle {0}", i));
                }
            }
        }

        [BlackList]
        public void OnPointerDown(PointerEventData eventData)
        {
            _draging = false;
        }

        [BlackList]
        public void OnPointerClick(PointerEventData eventData)
        {
            if (_draging || _maxSelects == 0)
                return;
            int cnt = _rectangles.Count;
            if (cnt == 0)
                return;

            for (int i = cnt - 1; i > -1; i--)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(_rectangles[i], eventData.position, Camera.main))
                {
                    if (RectangleSelectedHandler != null)
                        RectangleSelectedHandler(i);
                    break;
                    //Debug.Log(string.Format("Click select rectangle {0}", i));
                }
            }
        }
    }
}