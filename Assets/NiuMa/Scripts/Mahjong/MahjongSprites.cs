using System.Collections;
using System.Collections.Generic;
using NiuMa;

namespace UnityEngine
{
    public class MahjongSprites : MonoBehaviour
    {
        public List<Sprite> _sprites = new List<Sprite>();

        // Use this for initialization
        void Start()
        { }

        // Update is called once per frame
        void Update()
        { }

        public Sprite GetSprite(int pattern, int number)
        {
            int index = 0;
            if (pattern < (int)MahjongTile.Pattern.Tong || pattern > (int)MahjongTile.Pattern.Zhu)
                return null;
            if (pattern < (int)MahjongTile.Pattern.Dong)
            {
                if (number < (int)MahjongTile.Number.Yi || number > (int)MahjongTile.Number.Jiu)
                    return null;
                index = (pattern - 1) * 9 + (number - 1);
            }
            else
                index = 27 + (pattern - 4);
            if (index >= _sprites.Count)
                return null;
            return _sprites[index];
        }
    }
}