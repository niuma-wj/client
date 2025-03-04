using System.Collections;
using System.Collections.Generic;
using XLua;

namespace UnityEngine
{
    [LuaCallCSharp]
    public class SpriteList : MonoBehaviour
    {
        [SerializeField]
        private List<Sprite> _sprites = new List<Sprite>();

        // Use this for initialization
        void Start()
        {}

        // Update is called once per frame
        void Update()
        {}

        public Sprite GetSprite(int idx)
        {
            if (idx < 0 || idx > _sprites.Count)
                return null;

            return _sprites[idx];
        }
    }
}