using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    public class DragScrollRect : ScrollRect
    {
        private float _viewExtentX = 100.0f;
        private float _viewExtentY = 100.0f;
        private bool _draging = false;
        private bool _drag2End = false;

        private event UnityAction Scroll2EndHanlder;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();

            this.onValueChanged.AddListener(new UnityAction<Vector2>(this.OnScrollValueChanged));
        }

        public void SetViewExtent(float vx, float vy)
        {
            _viewExtentX = vx;
            _viewExtentY = vy;
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);

            _draging = true;
            _drag2End = false;
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);

            _draging = false;
        }

        public void OnScrollValueChanged(Vector2 pos)
        {
            if (!_draging || _drag2End || content == null)
                return;

            float x = content.anchoredPosition.x;
            float y = content.anchoredPosition.y;
            //Debug.Log(string.Format("_startX:{0:0.##}, x:{1:0.##}, viewX:{2:0.##}, sizeX:{3:0.##}", _startX, x, _viewExtentX, content.sizeDelta.x));
            //Debug.Log(string.Format("_startY:{0:0.##}, y:{1:0.##}, viewY:{2:0.##}, sizeY:{3:0.##}", _startY, y, _viewExtentY, content.sizeDelta.y));
            if ((x + _viewExtentX) > content.sizeDelta.x || (y + _viewExtentY) > content.sizeDelta.y)
            {
                //Debug.Log("testaaaaaaaAAAAbb");
                _drag2End = true;
                if (Scroll2EndHanlder != null)
                    Scroll2EndHanlder();
            }
        }

        public void AddScroll2EndHanlder(UnityAction del)
        {
            Scroll2EndHanlder += del;
        }
    }
}

