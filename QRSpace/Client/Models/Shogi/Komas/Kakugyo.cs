using System.Collections.Generic;

namespace QRSpace.Client.Models.Shogi.Komas
{
    public class Kakugyo : BasePiece
    {
        public override List<(int x, int y)> Directions => IsPromoted ?
            new List<(int x, int y)>() {
                (1, -1),    (0, -1),   (-1, -1),
                (1, 0),                 (-1, 0),
                (1, 1),     (0, 1),     (-1, 1),
                (8, 8), (8, -8), (-8, 8), (-8, -8) } :
            new List<(int x, int y)>() {
                (8, 8), (8, -8), (-8, 8), (-8, -8) };

        public Kakugyo(bool player, byte id)
        {
            Player = player;
            Id = id;
        }

        public override char GetName()
        {
            return IsPromoted ? '馬' : '角';
        }
    }
}