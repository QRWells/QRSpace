using System.Collections.Generic;

namespace QRSpace.Client.Models.Shogi.Komas
{
    public class Kyosha : BasePiece
    {
        public override List<(int x, int y)> Directions => IsPromoted ? KinshoDirection :
            new List<(int x, int y)>() { (0, -8) };

        public Kyosha(bool player, byte id)
        {
            Player = player;
            Id = id;
        }

        public override char GetName()
        {
            return IsPromoted ? '杏' : '香';
        }
    }
}