using System.Drawing;

namespace Minesweeper.Models
{
    public class Cell : NotifyPropertyChangedModel
    {
        private bool isRevealed, isFlagged;
        private string text;
        private Color foreColor, backColor;

        public int Row { get; set; }
        public int Col { get; set; }
        public bool HasMine { get; set; }
        public CellNumber NumNeighborsWithMines { get; set; }

        public Cell(int row, int col)
        {
            Row = row;
            Col = col;
            BackColor = GetCellBackColor();
        }

        public bool IsRevealed
        {
            get
            {
                return isRevealed;
            }

            set
            {
                if (value != isRevealed)
                {
                    isRevealed = value;
                    Text = GetCellText();
                    ForeColor = GetCellForeColor();
                    BackColor = GetCellBackColor();
                }
            }
        }

        public bool IsFlagged
        {
            get
            {
                return isFlagged;
            }

            set
            {
                if (value != isFlagged)
                {
                    isFlagged = value;
                    Text = GetCellText();
                    ForeColor = GetCellForeColor();
                }
            }
        }

        public string Text
        {
            get
            {
                return text;
            }

            private set
            {
                if (value != text)
                {
                    text = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Color ForeColor
        {
            get
            {
                return foreColor;
            }

            private set
            {
                if (value != foreColor)
                {
                    foreColor = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Color BackColor
        {
            get
            {
                return backColor;
            }

            private set
            {
                if (value != backColor)
                {
                    backColor = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string GetCellText()
        {
            if (!IsRevealed)
            {
                return IsFlagged ? "!" : "";
            }

            if (HasMine)
            {
                return "*";
            }

            if (NumNeighborsWithMines == CellNumber.Zero)
            {
                return "";
            }

            return NumNeighborsWithMines.ToString("D");
        }

        public Color GetCellForeColor()
        {

            if (IsFlagged)
            {
                return Color.Red;
            }

            return NumNeighborsWithMines switch
            {
                CellNumber.One => Color.Blue,
                CellNumber.Two => Color.Green,
                CellNumber.Three => Color.Red,
                CellNumber.Four => Color.Purple,
                _ => Color.Black,
            };
        }

        public Color GetCellBackColor() => IsRevealed ? Color.Tan : Color.YellowGreen;
    }
}
