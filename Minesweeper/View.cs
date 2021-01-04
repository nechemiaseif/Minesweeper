using Minesweeper.Models;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class View : Form
    {
        private readonly Game Game;

        public View(Difficulty difficulty = Difficulty.Easy)
        {
            Game = new Game(difficulty);

            InitializeComponent();
        }

        private void DifficultyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox difficultyComboBox = (ComboBox)sender;

            StartNewGame((Difficulty)difficultyComboBox.SelectedItem);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Game.TimeElapsed += TimeSpan.FromSeconds(1);
        }

        private void CellButton_Click(object sender, MouseEventArgs e)
        {
            Button button = (Button)sender;
            Cell cell = (Cell)button.Tag;

            if (!Game.HasBegun && e.Button == MouseButtons.Left)
            {
                BeginGame();
            }

            if (!cell.IsRevealed)
            {
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        if(Game.Board.CellsRevealed == 0)
                        {
                            Game.Board.RevealFirstCell(cell);
                        }
                        else
                        {
                            Game.Board.RevealCell(cell);
                        }

                        if (cell.CellValue == CellValue.Mine)
                        {
                            Game.Board.RevealAllMinesAndMistakenFlags();
                            EndGame();
                        }
                        else if (Game.HasBeenWon())
                        {
                            EndGame();
                        }
                        break;
                    case MouseButtons.Right:
                        Game.Board.ToggleFlag(cell);
                        break;
                    default:
                        return;
                }
            }
        }

        private Size GetCellButtonSize() => Game.Difficulty switch
        {
            Difficulty.Easy => new Size(50, 50),
            Difficulty.Medium => new Size(40, 40),
            _ => new Size(35, 35),
        };


        private void BeginGame()
        {
            timer.Start();
            Game.HasBegun = true;
        }

        private void EndGame()
        {
            timer.Stop();
            PromptRestart();
        }

        private void PromptRestart()
        {
            if (MessageBox.Show(Game.HasBeenWon() ? "You win! Play again?" : "Try again?", "Game Over", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                StartNewGame(Game.Difficulty);
            }

            Application.Exit();
        }

        private void StartNewGame(Difficulty difficulty)
        {
            View newGameView = new View(difficulty);

            Hide();
            newGameView.ShowDialog();
            Close();
        }
    }
}
