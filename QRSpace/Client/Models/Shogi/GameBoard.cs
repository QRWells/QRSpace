using System.Collections.Generic;
using QRSpace.Client.Models.Shogi.Komas;

namespace QRSpace.Client.Models.Shogi
{
    public class GameBoard
    {
        /// <summary>
        /// All the cells on the board.
        /// </summary>
        public Cell[,] Cells;

        /// <summary>
        /// All the pieces which are captured.
        /// </summary>
        public List<BasePiece> Captured;

        /// <summary>
        /// Constructor
        /// </summary>
        public GameBoard()
        {
            Init();
        }

        /// <summary>
        /// Initialize the gameboard
        /// </summary>
        public void Init()
        {
            Cells = new Cell[9, 9];
            Captured = new List<BasePiece>();
            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    Cells[i, j] = new Cell(this, (i, j));
                }
            }

            #region Add pieces

            //Add pieces to the board
            for (byte i = 0; i < 9; i++)
            {
                Drop((i, 2), new Fuhyo(true, (byte)(8 - i + 64)));//
                Drop((i, 6), new Fuhyo(false, i));//
            }

            Drop((0, 0), new Kyosha(true, 9 + 64));
            Drop((8, 0), new Kyosha(true, 10 + 64));
            Drop((0, 8), new Kyosha(false, 10));
            Drop((8, 8), new Kyosha(false, 9));

            Drop((1, 0), new Keima(true, 11 + 64));
            Drop((7, 0), new Keima(true, 12 + 64));
            Drop((1, 8), new Keima(false, 12));
            Drop((7, 8), new Keima(false, 11));

            Drop((2, 0), new Ginsho(true, 13 + 64));
            Drop((6, 0), new Ginsho(true, 14 + 64));
            Drop((2, 8), new Ginsho(false, 14));
            Drop((6, 8), new Ginsho(false, 13));

            Drop((3, 0), new Kinsho(true, 15 + 64));
            Drop((5, 0), new Kinsho(true, 16 + 64));
            Drop((3, 8), new Kinsho(false, 16));
            Drop((5, 8), new Kinsho(false, 15));

            Drop((4, 0), new Gyokusho(true, 17 + 64));
            Drop((4, 8), new Gyokusho(false, 17));

            Drop((1, 1), new Kakugyo(true, 18 + 64));
            Drop((7, 7), new Kakugyo(false, 18));

            Drop((7, 1), new Hisha(true, 19 + 64));
            Drop((1, 7), new Hisha(false, 19));

            #endregion Add pieces

            Update();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="piece"></param>
        public void Drop((int x, int y) pos, BasePiece piece)
        {
            var (x, y) = pos;
            Cells[x, y].Piece = piece;
            if (Captured.Contains(piece))
            {
                Captured.Remove(piece);
            }
            Update();
        }

        /// <summary>
        ///
        /// </summary>
        public bool Update()
        {
            foreach (var item in Cells)
            {
                item.Update();
            }
            return true;
        }

        /// <summary>
        ///
        /// </summary>
        public void GameOver()
        {
        }

        /// <summary>
        ///
        /// </summary>
        public void MoveTo((int x, int y) from, (int x, int y) to)
        {
            if (Cells[to.x, to.y].Piece != null)
            {
                Capture(to);
            }

            var (x, y) = from;
            Cells[to.x, to.y].Piece = Cells[x, y].Piece;
            Cells[x, y].Piece = null;
            Update();
        }

        /// <summary>
        ///
        /// </summary>
        public void Capture((int x, int y) pos)
        {
            var temp = Cells[pos.x, pos.y].Piece;
            temp.Player = !temp.Player;
            Captured.Add(temp);
            Update();
        }
    }
}