using System.Collections.Generic;

namespace QRSpace.Client.Models.Shogi
{
    public class Cell
    {
        public GameBoard Parent { get; private set; }

        /// <summary>
        /// The position of the cell
        /// </summary>
        public (int x, int y) Pos { get; set; }

        public BasePiece Piece { get; set; }

        public Cell(GameBoard parent, (int x, int y) pos)
        {
            Parent = parent;
            Pos = pos;
        }

        public void Update()
        {
            if (Piece != null && !Piece.Player)
            {
                Piece.LegalMoves = new List<(int x, int y)>();
                switch (Piece.Id)
                {
                    #region Kakugyo

                    case 18://kaku
                        if (Piece.IsPromoted)
                        {
                            if (Pos.y > 0)
                            {
                                var piece = Parent.Cells[Pos.x, Pos.y - 1].Piece;
                                if (piece == null || piece.Player)
                                    Piece.LegalMoves.Add((Pos.x, Pos.y - 1));
                            }
                            if (Pos.y < 8)
                            {
                                var piece = Parent.Cells[Pos.x, Pos.y + 1].Piece;
                                if (piece == null || piece.Player)
                                    Piece.LegalMoves.Add((Pos.x, Pos.y + 1));
                            }
                            if (Pos.x > 0)
                            {
                                var piece = Parent.Cells[Pos.x - 1, Pos.y].Piece;
                                if (piece == null || piece.Player)
                                    Piece.LegalMoves.Add((Pos.x - 1, Pos.y));
                            }
                            if (Pos.x < 8)
                            {
                                var piece = Parent.Cells[Pos.x + 1, Pos.y].Piece;
                                if (piece == null || piece.Player)
                                    Piece.LegalMoves.Add((Pos.x + 1, Pos.y));
                            }
                        }
                        for (int i = 1; i < 9; i++)
                        {
                            if (Pos.x - i < 0 || Pos.y - i < 0)
                                break;
                            if (Parent.Cells[Pos.x - i, Pos.y - i].Piece == null)
                                Piece.LegalMoves.Add((Pos.x - i, Pos.y - i));
                            else
                            {
                                if (Parent.Cells[Pos.x - i, Pos.y - i].Piece.Player)
                                    Piece.LegalMoves.Add((Pos.x - i, Pos.y - i));
                                break;
                            }
                        }
                        for (int i = 1; i < 9; i++)
                        {
                            if (Pos.x + i > 8 || Pos.y + i > 8)
                                break;
                            if (Parent.Cells[Pos.x + i, Pos.y + i].Piece == null)
                                Piece.LegalMoves.Add((Pos.x + i, Pos.y + i));
                            else
                            {
                                if (Parent.Cells[Pos.x + i, Pos.y + i].Piece.Player)
                                    Piece.LegalMoves.Add((Pos.x + i, Pos.y + i));
                                break;
                            }
                        }
                        for (int i = 1; i < 9; i++)
                        {
                            if (Pos.x - i < 0 || Pos.y + i > 8)
                                break;
                            if (Parent.Cells[Pos.x - i, Pos.y + i].Piece == null)
                                Piece.LegalMoves.Add((Pos.x - i, Pos.y + i));
                            else
                            {
                                if (Parent.Cells[Pos.x - i, Pos.y + i].Piece.Player)
                                    Piece.LegalMoves.Add((Pos.x - i, Pos.y + i));
                                break;
                            }
                        }
                        for (int i = 1; i < 9; i++)
                        {
                            if (Pos.x + i > 8 || Pos.y - i < 0)
                                break;
                            if (Parent.Cells[Pos.x + i, Pos.y - i].Piece == null)
                                Piece.LegalMoves.Add((Pos.x + i, Pos.y - i));
                            else
                            {
                                if (Parent.Cells[Pos.x + i, Pos.y - i].Piece.Player)
                                    Piece.LegalMoves.Add((Pos.x + i, Pos.y - i));
                                break;
                            }
                        }
                        break;

                    #endregion Kakugyo

                    #region Hisha

                    case 19:
                        if (Piece.IsPromoted)
                        {
                            if (Pos.x > 0 && Pos.y > 0)
                            {
                                var piece = Parent.Cells[Pos.x - 1, Pos.y - 1].Piece;
                                if (piece == null || piece.Player)
                                    Piece.LegalMoves.Add((Pos.x - 1, Pos.y - 1));
                            }
                            if (Pos.x > 0 && Pos.y < 8)
                            {
                                var piece = Parent.Cells[Pos.x - 1, Pos.y + 1].Piece;
                                if (piece == null || piece.Player)
                                    Piece.LegalMoves.Add((Pos.x - 1, Pos.y + 1));
                            }
                            if (Pos.x < 8 && Pos.y > 0)
                            {
                                var piece = Parent.Cells[Pos.x + 1, Pos.y - 1].Piece;
                                if (piece == null || piece.Player)
                                    Piece.LegalMoves.Add((Pos.x + 1, Pos.y - 1));
                            }
                            if (Pos.x < 8 && Pos.y < 8)
                            {
                                var piece = Parent.Cells[Pos.x + 1, Pos.y + 1].Piece;
                                if (piece == null || piece.Player)
                                    Piece.LegalMoves.Add((Pos.x + 1, Pos.y + 1));
                            }
                        }
                        for (int i = 1; i < 9; i++)
                        {
                            if (Pos.y - i < 0)
                                break;
                            if (Parent.Cells[Pos.x, Pos.y - i].Piece == null)
                                Piece.LegalMoves.Add((Pos.x, Pos.y - i));
                            else
                            {
                                if (Parent.Cells[Pos.x, Pos.y - i].Piece.Player)
                                    Piece.LegalMoves.Add((Pos.x, Pos.y - i));
                                break;
                            }
                        }
                        for (int i = 1; i < 9; i++)
                        {
                            if (Pos.y + i > 8)
                                break;
                            if (Parent.Cells[Pos.x, Pos.y + i].Piece == null)
                                Piece.LegalMoves.Add((Pos.x, Pos.y + i));
                            else
                            {
                                if (Parent.Cells[Pos.x, Pos.y + i].Piece.Player)
                                    Piece.LegalMoves.Add((Pos.x, Pos.y + i));
                                break;
                            }
                        }
                        for (int i = 1; i < 9; i++)
                        {
                            if (Pos.x - i < 0)
                                break;
                            if (Parent.Cells[Pos.x - i, Pos.y].Piece == null)
                                Piece.LegalMoves.Add((Pos.x - i, Pos.y));
                            else
                            {
                                if (Parent.Cells[Pos.x - i, Pos.y].Piece.Player)
                                    Piece.LegalMoves.Add((Pos.x - i, Pos.y));
                                break;
                            }
                        }
                        for (int i = 1; i < 9; i++)
                        {
                            if (Pos.x + i > 8)
                                break;
                            if (Parent.Cells[Pos.x + i, Pos.y].Piece == null)
                                Piece.LegalMoves.Add((Pos.x + i, Pos.y));
                            else
                            {
                                if (Parent.Cells[Pos.x + i, Pos.y].Piece.Player)
                                    Piece.LegalMoves.Add((Pos.x + i, Pos.y));
                                break;
                            }
                        }
                        break;

                    #endregion Hisha

                    case 9://kyo
                    case 10:
                        if (Piece.IsPromoted)
                        {
                            goto default;
                        }
                        else
                        {
                            for (int i = 1; i < 9; i++)
                            {
                                if (Pos.y - i < 0)
                                    break;
                                if (Parent.Cells[Pos.x, Pos.y - i].Piece == null)
                                    Piece.LegalMoves.Add((Pos.x, Pos.y - i));
                                else
                                {
                                    if (Parent.Cells[Pos.x, Pos.y - i].Piece.Player)
                                        Piece.LegalMoves.Add((Pos.x, Pos.y - i));
                                    break;
                                }
                            }
                        }
                        break;

                    default:
                        foreach (var (x, y) in Piece.Directions)
                        {
                            var tempX = Pos.x + x;
                            var tempY = Pos.y + y;
                            if (tempX < 9 && tempX > -1 && tempY < 9 && tempY > -1)
                            {
                                if (Parent.Cells[tempX, tempY].Piece == null || Parent.Cells[tempX, tempY].Piece.Player)
                                {
                                    Piece.LegalMoves.Add((tempX, tempY));
                                }
                            }
                        }
                        break;
                }
            }
        }
    }
}