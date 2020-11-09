using System.Collections.Generic;

namespace QRSpace.Client.Models.Shogi.Komas
{
    public class Keima : BasePiece
    {
        public override List<(int x, int y)> Directions => IsPromoted ? KinshoDirection :
            new List<(int x, int y)>() { (1, -2), (-1, -2) };

        public Keima(bool player, byte id)
        {
            Player = player;
            Id = id;
        }

        public override char GetName()
        {
            return IsPromoted ? '圭' : '桂';
        }
    }
}