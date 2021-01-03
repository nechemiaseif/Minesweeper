using Minesweeper.Models;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class View : Form
    {
        private readonly Game Game;

        public View()
        {
            Game = new Game();

            InitializeComponent();
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

        private void Timer_Tick(object sender, EventArgs e)
        {
            Game.TimeElapsed += TimeSpan.FromSeconds(1);
        }

        public void LoseGame()
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
                Process.Start(Process.GetCurrentProcess().MainModule.FileName);
            }

            Application.Exit();
        }
    }
}
