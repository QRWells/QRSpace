using System.Collections.Generic;

namespace QRSpace.Client.Models.Shogi.Komas
{
    public class Kinsho : BasePiece
    {
        public override List<(int x, int y)> Directions => KinshoDirection;
        public Kinsho(bool player, byte id)
        {
            IsPromotable = false;
            Player = player;
            Id = id;
        }

        public override char GetName()
        {
            return '金';
        }
    }
}