using System;

namespace Minesweeper.Models
{
    class Game : NotifyPropertyChangedModel
    {
        private TimeSpan timeElapsed;

        public bool HasBegun { get; set; }

        public Board Board { get; private set; }

        public Game()
        {
            Board = new Board(Difficulty.Easy);
            TimeElapsed = new TimeSpan();
        }

        public TimeSpan TimeElapsed
        {
            get
            {
                return timeElapsed;
            }

            set
            {
                if (value != timeElapsed)
                {
                    timeElapsed = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool WasBeaten() => Board.CellsRevealed == Board.Cols * Board.Rows;
    }
}

