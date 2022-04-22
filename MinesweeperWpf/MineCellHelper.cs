using System;
using System.Collections.Generic;

namespace MinesweeperWpf
{
    public enum RelativeDirection
    {
        TopLeft,
        Top,
        TopRight,
        Left,
        Right,
        BottomLeft,
        Bottom,
        BottomRight
    }

    public static class MineCellHelper
    {
        public static void UpdateMinesAround(IList<MineCellViewModel> cells, MineCellViewModel cell, int columns)
        {
            int sum = 0;

            var directions = Enum.GetValues<RelativeDirection>();

            foreach (var direction in directions)
            {
                var relative = GetRelativeCell(cells, cell, columns, direction);

                if (relative?.HasMine == true)
                {
                    sum++;
                }
            }

            cell.MinesAround = sum;
        }

        public static void TryOpenEmptyFields(IList<MineCellViewModel> cells, MineCellViewModel cell, int columns)
        {
            if (cell.IsEmpty)
            {
                var directions = Enum.GetValues<RelativeDirection>();

                List<MineCellViewModel> openCollection = new List<MineCellViewModel>();

                foreach (var direction in directions)
                {
                    var relative = GetRelativeCell(cells, cell, columns, direction);

                    if (relative != null && !relative.IsOpened)
                    {
                        relative.IsOpened = true;

                        if (relative.IsEmpty)
                        {
                            openCollection.Add(relative);
                        }
                    }
                }

                foreach (var open in openCollection)
                {
                    TryOpenEmptyFields(cells, open, columns);
                }
            }
        }

        public static bool FieldIsOpen(IList<MineCellViewModel> cells)
        {
            foreach (var cell in cells)
            {
                if (!cell.IsEmpty)
                {
                    if (cell.HasMine && !cell.IsMarked)
                    {
                        return false;
                    }
                    else if (!cell.HasMine && cell.IsMarked)
                    {
                        return false;
                    }
                    else if (!cell.HasMine && !cell.IsOpened)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static MineCellViewModel GetRelativeCell(IList<MineCellViewModel> cells, MineCellViewModel cell, int columns, RelativeDirection direction)
        {
            int index = cells.IndexOf(cell);
            bool firstRow = index < columns;
            bool left = index % columns == 0;
            bool right = (index + 1) % columns == 0;
            bool lastRow = index + columns >= cells.Count;

            int relativeIndex;

            switch (direction)
            {
                case RelativeDirection.TopLeft:
                    {
                        if (firstRow || left)
                        {
                            relativeIndex = -1;
                        }
                        else
                        {
                            relativeIndex = index - columns - 1;
                        }

                        break;
                    }
                case RelativeDirection.Top:
                    {
                        if (firstRow)
                        {
                            relativeIndex = -1;
                        }
                        else
                        {
                            relativeIndex = index - columns;
                        }

                        break;
                    }
                case RelativeDirection.TopRight:
                    {
                        if (firstRow || right)
                        {
                            relativeIndex = -1;
                        }
                        else
                        {
                            relativeIndex = index - columns + 1;
                        }

                        break;
                    }
                case RelativeDirection.Left:
                    {
                        if (left)
                        {
                            relativeIndex = -1;
                        }
                        else
                        {
                            relativeIndex = index - 1;
                        }

                        break;
                    }
                case RelativeDirection.Right:
                    {
                        if (right)
                        {
                            relativeIndex = -1;
                        }
                        else
                        {
                            relativeIndex = index + 1;
                        }

                        break;
                    }
                case RelativeDirection.BottomLeft:
                    {
                        if (lastRow || left)
                        {
                            relativeIndex = -1;
                        }
                        else
                        {
                            relativeIndex = index + columns - 1;
                        }

                        break;
                    }
                case RelativeDirection.Bottom:
                    {
                        if (lastRow)
                        {
                            relativeIndex = -1;
                        }
                        else
                        {
                            relativeIndex = index + columns;
                        }

                        break;
                    }
                case RelativeDirection.BottomRight:
                    {
                        if (lastRow || right)
                        {
                            relativeIndex = -1;
                        }
                        else
                        {
                            relativeIndex = index + columns + 1;
                        }
                        
                        break;
                    }
                default:
                    throw new InvalidOperationException();
            }

            if (relativeIndex >= 0 && relativeIndex < cells.Count)
            {
                return cells[relativeIndex];
            }
            else
            {
                return null;
            }
        }
    }
}
