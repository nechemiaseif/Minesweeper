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
        public CellValue CellValue { get; set; }

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

            set
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

            return CellValue switch
            {
                CellValue.Zero => "",
                CellValue.Mine => "*",
                _ => CellValue.ToString("D"),
            };
        }

        public Color GetCellForeColor()
        {

            if (IsFlagged)
            {
                return Color.Red;
            }

            return CellValue switch
            {
                CellValue.One => Color.Blue,
                CellValue.Two => Color.Green,
                CellValue.Three => Color.Red,
                CellValue.Four => Color.Purple,
                _ => Color.Black,
            };
        }

        public Color GetCellBackColor()
        {
            bool isInEvenPosition = (Row + Col) % 2 == 0;

            if (IsRevealed)
            {
                return isInEvenPosition ? Color.Wheat : Color.Tan;
            }

            return isInEvenPosition ? Color.GreenYellow : Color.YellowGreen;
        }
    }
}