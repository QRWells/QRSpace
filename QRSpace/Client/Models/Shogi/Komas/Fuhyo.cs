using System.Collections.Generic;

namespace QRSpace.Client.Models.Shogi.Komas
{
    public class Fuhyo : BasePiece
    {
        public override List<(int x, int y)> Directions =>
                IsPromoted ? KinshoDirection : new List<(int x, int y)>() { (0, -1) };

        public Fuhyo(bool player, byte id)
        {
            Player = player;
            Id = id;
        }

        public override char GetName()
        {
            return IsPromoted ? 'と' : '歩';
        }
    }
}