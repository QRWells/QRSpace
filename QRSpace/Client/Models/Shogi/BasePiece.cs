using System.Collections.Generic;

namespace QRSpace.Client.Models.Shogi
{
    /// <summary>
    /// The base of the komas
    /// </summary>
    public abstract class BasePiece
    {
        /// <summary>
        /// Global ID which should be unique.
        /// </summary>
        public byte Id { get; set; }

        /// <summary>
        /// Which player possess the piece, false : first player, true : second player.
        /// </summary>
        public bool Player { get; set; }

        /// <summary>
        /// Determine if this piece can promote.
        /// </summary>
        public bool IsPromotable { get; set; } = true;

        /// <summary>
        /// Determine if this piece has promoted.
        /// </summary>
        public bool IsPromoted { get; set; }

        /// <summary>
        /// If captured, the position will be unavailable untill it is dropped.
        /// </summary>
        public bool IsCaptured { get; set; }

        /// <summary>
        /// Legal moves of this piece.
        /// </summary>
        public List<(int x, int y)> LegalMoves { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected static List<(int x, int y)> KinshoDirection = 
            new() { (1, -1), (0, -1), (-1, -1), (1, 0), (-1, 0), (0, 1) };

        /// <summary>
        /// The movable directions
        /// </summary>
        public abstract List<(int x, int y)> Directions { get; }

        /// <summary>
        /// Get the name of the piece
        /// </summary>
        /// <returns></returns>
        public abstract char GetName();

        /// <summary>
        /// Promote the piece
        /// </summary>
        public void Promote()
        {
            if (IsPromotable) IsPromoted = true;
        }

        /// <summary>
        /// Called when the piece is captured.
        /// </summary>
        public void Capture()
        {
            Player = !Player;
            IsPromoted = false;
            IsCaptured = true;
        }
    }
}