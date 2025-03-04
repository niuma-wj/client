using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using XLua;

namespace UnityEngine
{
    [CSharpCallLua]
    public interface IMahjongPaver
    {
        void ThrowTile(int id);

        void ClickTile(int id);
    }

    [LuaCallCSharp]
    public class MahjongBrick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        private Image _imgTile = null;
        private MahjongSprites _sprites = null;
        private IMahjongPaver _paver = null;    // 麻将牌铺设器
        private int _pattern = 0;
        private int _number = 0;
        private int _id = 0;
        private bool _started = false;
        private bool _draging = false;  // 当前是否正在拖拽麻将，如果是，则不处理OnPointerClick事件

        private float _dragStartY = 0.0f;

        private void Awake()
        {
            Transform child = gameObject.transform.Find("Tile");
            if (child != null)
                _imgTile = child.GetComponent<Image>();
        }

        // Use this for initialization
        void Start()
        {
            _started = true;

            SetMahjongTile(_pattern, _number, _id);
        }

        // Update is called once per frame
        void Update() {}

        private void SetTileSprite(Sprite sp)
        {
            if (_imgTile != null)
            {
                _imgTile.sprite = sp;
                _imgTile.SetNativeSize();
            }
        }

        public void SetMahjongSprites(GameObject obj)
        {
            if (obj != null)
                _sprites = obj.GetComponent<MahjongSprites>();
        }

        public void SetMahjongTile(int pattern, int number, int id)
        {
            _pattern = pattern;
            _number = number;
            _id = id;
            if (!_started || (_pattern < 1) || (_imgTile == null) || (_sprites == null))
                return;
            SetTileSprite(_sprites.GetSprite(_pattern, _number));
        }

        public void SetMahjongPaver(LuaTable p)
        {
            if (p == null)
                return;
            _paver = p.Cast<IMahjongPaver>();
        }

        [BlackList]
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_paver == null)
                return;

            _draging = true;
            _dragStartY = eventData.position.y;
        }

        [BlackList]
        public void OnDrag(PointerEventData eventData) {}

        [BlackList]
        public void OnEndDrag(PointerEventData eventData)
        {
            if (_paver == null)
                return;
            _draging = false;
            float deltaY = eventData.position.y - _dragStartY;
            // 若拖拽距离超过30个像素，则直接打出该牌，否则只是点击该牌
            if (deltaY > 30.0f)
                _paver.ThrowTile(_id);
            else
                _paver.ClickTile(_id);
        }

        [BlackList]
        public void OnPointerClick(PointerEventData eventData)
        {
            if (_paver == null)
                return;
            if (!_draging)
                _paver.ClickTile(_id);
        }
    }
}