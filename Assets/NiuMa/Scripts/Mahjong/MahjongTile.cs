// MahjongTile.cs

namespace NiuMa
{
    public class MahjongTile
    {
        public enum Pattern
        {
            Invalid,    // 无效
            Tong,       // 筒子
            Tiao,       // 条子
            Wan,        // 万子
            Dong,       // 东风
            Nan,        // 南风
            Xi,         // 西风
            Bei,        // 北风
            Zhong,      // 红中
            Fa,         // 发财
            Bai,        // 白板
            Chun,       // 春
            Xia,        // 夏
            Qiu,        // 秋
            Winter,     // 冬
            Mei,        // 梅
            Lan,        // 兰
            Ju,         // 菊
            Zhu			// 竹
        }

        public enum Number
        {
            Invalid,    // 无效
            Yi,         // 1
            Er,         // 2
            San,        // 3
            Si,         // 4
            Wu,         // 5
            Liu,        // 6
            Qi,         // 7
            Ba,         // 8
            Jiu			// 9
        }

        public class Tile
        {
            public Tile() { }
            public Tile(Pattern p, Number n)
            {
                _pattern = p;
                _number = n;
            }

            public Pattern _pattern = Pattern.Invalid;
            public Number _number = Number.Invalid;
        }

        public MahjongTile(int pattern, int number)
        {
            _tile = new Tile((Pattern)pattern, (Number)number);
        }

        public MahjongTile(Tile t, int id)
        {
            _tile = t;
            _id = id;
        }

        public Tile GetTile()
        {
            return _tile;
        }

        public void SetTile(Tile t)
        {
            _tile = t;
        }

        public int GetId()
        {
            return _id;
        }

        public void SetId(int id)
        {
            _id = id;
        }

        private Tile _tile = null;
        private int _id = 0;
    }
}
