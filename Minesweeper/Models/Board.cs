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

        public Board(Difficulty difficulty)
        {
            Init(difficulty);
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

        private void Init(Difficulty difficulty)
        {
            switch (difficulty)
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

                Cell cell = Grid[row, col];

                if (cell.CellValue != CellValue.Mine
                    && cell.CellValue != CellValue.NonMinePlaceholder)
                {
                    cell.CellValue = CellValue.Mine;
                    minesPlaced++;
                }
            }

        }

        private void CalculateCellValues()
        {
            for (int currentRow = 0; currentRow < Rows; currentRow++)
            {
                for (int currentCol = 0; currentCol < Cols; currentCol++)
                {
                    Cell currentCell = Grid[currentRow, currentCol];

                    if (currentCell.CellValue == CellValue.NonMinePlaceholder)
                    {
                        currentCell.CellValue++;
                    }

                    for (int neighboringRow = currentRow - 1; neighboringRow <= currentRow + 1; neighboringRow++)
                    {
                        for (int neighboringCol = currentCol - 1; neighboringCol <= currentCol + 1; neighboringCol++)
                        {
                            if (IsValidRowCol(neighboringRow, neighboringCol))
                            {
                                if (!(neighboringRow == currentRow && neighboringCol == currentCol)
                                    && Grid[neighboringRow, neighboringCol].CellValue == CellValue.Mine)
                                {
                                    currentCell.CellValue++;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void RevealFirstCell(Cell cell)
        {
            cell.CellValue = CellValue.Zero;
            cell.IsRevealed = true;
            CellsRevealed++;

            for (int neighboringRow = cell.Row - 1; neighboringRow <= cell.Row + 1; neighboringRow++)
            {
                for (int neighboringCol = cell.Col - 1; neighboringCol <= cell.Col + 1; neighboringCol++)
                {
                    if (IsValidRowCol(neighboringRow, neighboringCol)
                        && !(neighboringRow == cell.Row && neighboringCol == cell.Col))
                    {
                        Grid[neighboringRow, neighboringCol].CellValue = CellValue.NonMinePlaceholder;
                    }
                }
            }

            PlaceMines();
            CalculateCellValues();
            RevealNeighbors(cell);
        }

        public void RevealCell(Cell cell)
        {
            if (!cell.IsRevealed && !cell.IsFlagged)
            {
                cell.IsRevealed = true;
                CellsRevealed++;

                if (cell.CellValue == CellValue.Zero)
                {
                    RevealNeighbors(cell);
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

        public void RevealAllMinesAndMistakenFlags()
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Cols; col++)
                {
                    Cell cell = Grid[row, col];

                    if (cell.CellValue == CellValue.Mine)
                    {
                        RevealCell(cell);
                    }
                    else if (cell.IsFlagged)
                    {
                        cell.Text = "X";
                    }
                }
            }
        }

        private bool IsValidRowCol(int row, int col) => row >= 0 && row < Rows && col >= 0 && col < Cols;
    }
}
