using System.Collections.Generic;

namespace QRSpace.Client.Models.Shogi.Komas
{
    public class Hisha : BasePiece
    {
        public override List<(int x, int y)> Directions => IsPromoted?
            new List<(int x, int y)>() {
                (1, -1),    (0, -1),   (-1, -1),
                (1, 0),                 (-1, 0),
                (1, 1),     (0, 1),     (-1, 1),
                (0, 8), (0, -8), (8, 0), (-8, 0) }:
            new List<(int x, int y)>() {
                (0, 8), (0, -8), (8, 0), (-8, 0) };

        public Hisha(bool player, byte id)
        {
            Player = player;
            Id = id;
        }

        public override char GetName()
        {
            return IsPromoted ? '竜' : '飛';
        }
    }
}