using System.Collections.Generic;

namespace QRSpace.Client.Models.Shogi.Komas
{
    public class Ginsho : BasePiece
    {
        public override List<(int x, int y)> Directions => IsPromoted ? KinshoDirection
                    : new List<(int x, int y)>() { (1, -1), (0, -1), (-1, -1), (1, 1), (-1, 1) };

        public Ginsho(bool player, byte id)
        {
            Player = player;
            Id = id;
        }

        public override char GetName()
        {
            return IsPromoted ? '全' : '銀';
        }
    }
}