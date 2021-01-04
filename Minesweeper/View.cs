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
            View newGameView = new View((Difficulty)difficultyComboBox.SelectedItem);

            Hide();
            newGameView.ShowDialog();
            Close();
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
                timer.Enabled = true;
                timer.Start();
                Game.HasBegun = true;
            }

            if (!cell.IsRevealed)
            {

                switch (e.Button)
                {
                    case MouseButtons.Left:
                        Game.Board.RevealCell(cell);
                        if (cell.HasMine)
                        {
                            LoseGame();
                        }
                        if (Game.WasBeaten())
                        {
                            WinGame();
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


        private void LoseGame()
        {
            Game.Board.RevealAllMines();
            PromptRestart();
        }

        private void WinGame()
        {
            MessageBox.Show("Woohoo, you won!");
            PromptRestart();
        }

        private void PromptRestart()
        {
            if (MessageBox.Show(Game.WasBeaten() ? "Play again?" : "Try again?", "Game Over", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Restart();
            }

            Application.Exit();
        }

        private void Restart()
        {
            Process.Start(Process.GetCurrentProcess().MainModule.FileName);
        }
    }
}
