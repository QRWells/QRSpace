using System.Collections.Generic;

namespace QRSpace.Client.Models.Shogi.Komas
{
    public class Gyokusho : BasePiece
    {
        public override List<(int x, int y)> Directions =>
            new List<(int x, int y)>() { (1, -1), (0, -1), (-1, -1), (1, 0), (-1, 0), (1, 1), (0, 1), (-1, 1) };

        public Gyokusho(bool player, byte id)
        {
            IsPromotable = false;
            Player = player;
            Id = id;
        }

        public override char GetName()
        {
            return Player ? '玉' : '王';
        }
    }
}