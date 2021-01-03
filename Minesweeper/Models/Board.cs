using System;

namespace Minesweeper.Models
{
    public class Board : NotifyPropertyChangedModel
    {
        private int flagsRemaining;
        public int Rows { get; set; }
        public int Cols { get; set; }
        public int Mines { get; set; }
        public int CellsRevealed { get; private set; }
        public Cell[,] Grid { get; set; }

        // TODO maybe refactor difficulty to Game class
        public Difficulty Difficulty { get; set; }

        public Board(Difficulty difficulty)
        {
            Difficulty = difficulty;

            Init();
        }

        public int FlagsRemaining
        {
            get
            {
                return flagsRemaining;
            }

            private set
            {
                if (value != flagsRemaining)
                {
                    flagsRemaining = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private void Init()
        {
            switch (Difficulty)
            {
                case Difficulty.Easy:
                    Rows = 8;
                    Cols = 10;
                    Mines = 10;
                    FlagsRemaining = 10;
                    break;
                case Difficulty.Medium:
                    Rows = 14;
                    Cols = 18;
                    Mines = 40;
                    FlagsRemaining = 40;
                    break;
                case Difficulty.Hard:
                    Rows = 20;
                    Cols = 24;
                    Mines = 99;
                    FlagsRemaining = 99;
                    break;
            }

            InitBoard();
            PlaceMines();
            CalculateCellNeighbors();
        }

        private void InitBoard()
        {
            Grid = new Cell[Rows, Cols];

            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Cols; col++)
                {
                    Grid[row, col] = new Cell(row, col);
                }
            }
        }

        private void PlaceMines()
        {
            var rand = new Random();
            int minesPlaced = 0;

            while (minesPlaced < Mines)
            {
                int row = rand.Next(0, Rows);
                int col = rand.Next(0, Cols);

                if (!Grid[row, col].HasMine)
                {
                    Grid[row, col].HasMine = true;
                    minesPlaced++;
                }
            }

        }

        private void CalculateCellNeighbors()
        {
            for (int currentRow = 0; currentRow < Rows; currentRow++)
            {
                for (int currentCol = 0; currentCol < Cols; currentCol++)
                {
                    Cell currentCell = Grid[currentRow, currentCol];

                    if (currentCell.HasMine)
                    {
                        currentCell.NumNeighborsWithMines = CellNumber.Mine;
                        continue;
                    }

                    for (int neighboringRow = currentRow - 1; neighboringRow <= currentRow + 1; neighboringRow++)
                    {
                        for (int neighboringCol = currentCol - 1; neighboringCol <= currentCol + 1; neighboringCol++)
                        {
                            if (IsValidRowCol(neighboringRow, neighboringCol)
                                && !(neighboringRow == currentRow && neighboringCol == currentCol)
                                && Grid[neighboringRow, neighboringCol].HasMine)
                            {
                                currentCell.NumNeighborsWithMines++;
                            }
                        }
                    }
                }
            }
        }

        public void RevealCell(Cell cell)
        {
            if (!cell.IsRevealed && !cell.IsFlagged)
            {
                cell.IsRevealed = true;
                CellsRevealed++;

                if (cell.NumNeighborsWithMines == CellNumber.Zero)
                {
                    RevealNeighbors(cell);
                }
            }
        }

        public void ToggleFlag(Cell cell)
        {
            if (!cell.IsRevealed)
            {
                if (cell.IsFlagged)
                {
                    cell.IsFlagged = false;
                    FlagsRemaining++;
                    return;
                }

                if (FlagsRemaining > 0)
                {
                    cell.IsFlagged = true;
                    FlagsRemaining--;
                }
            }
        }

        private void RevealNeighbors(Cell currentCell)
        {
            for (int neighboringRow = currentCell.Row - 1; neighboringRow <= currentCell.Row + 1; neighboringRow++)
            {
                for (int neighboringCol = currentCell.Col - 1; neighboringCol <= currentCell.Col + 1; neighboringCol++)
                {
                    if (IsValidRowCol(neighboringRow, neighboringCol))
                    {
                        Cell neighboringCell = Grid[neighboringRow, neighboringCol];

                        if (neighboringCell != currentCell && !neighboringCell.IsRevealed)
                        {
                            if (neighboringCell.IsFlagged)
                            {
                                ToggleFlag(neighboringCell);
                            }

                            RevealCell(neighboringCell);
                        }
                    }
                }
            }
        }

        public void RevealAllMines()
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Cols; col++)
                {
                    Cell cell = Grid[row, col];

                    if (cell.HasMine)
                    {
                        RevealCell(cell);
                    }
                }
            }
        }

        private bool IsValidRowCol(int row, int col) => row >= 0 && row < Rows && col >= 0 && col < Cols;
    }
}
