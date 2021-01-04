using System;

namespace Minesweeper.Models
{
    class Game : NotifyPropertyChangedModel
    {
        private TimeSpan timeElapsed;

        public bool HasBegun { get; set; }
        public Difficulty Difficulty { get; set; }
        public Board Board { get; private set; }

        public Game(Difficulty difficulty)
        {
            Difficulty = difficulty;
            Board = new Board(Difficulty);
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

        public bool HasBeenWon() => Board.CellsRevealed == Board.Cols * Board.Rows - Board.Mines;
    }
}