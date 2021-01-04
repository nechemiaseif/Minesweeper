using System;
using System.Drawing;
using System.Windows.Forms;

namespace Minesweeper
{
    partial class View
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.headerGroupBox = new GroupBox();
            this.difficultyComboBox = new ComboBox();
            this.flagLabel = new Label();
            this.timer = new Timer();
            this.timerLabel = new Label();
            this.boardTableLayoutPanel = new TableLayoutPanel();

            this.headerGroupBox.SuspendLayout();
            this.boardTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();

            // 
            // headerGroupBox
            // 
            this.headerGroupBox.Controls.AddRange(new Control[] { this.difficultyComboBox, this.flagLabel, this.timerLabel });
            this.headerGroupBox.Location = new Point(50, 25);
            this.headerGroupBox.Dock = DockStyle.Top;

            // 
            // difficultyComboBox
            // 
            this.difficultyComboBox.Location = new Point(10, 30);
            this.difficultyComboBox.Size = new Size(100, 30);
            this.difficultyComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            foreach (Difficulty difficulty in Enum.GetValues(typeof(Difficulty)))
            {
                this.difficultyComboBox.Items.Add(difficulty);
            }

            this.difficultyComboBox.SelectedItem = Game.Difficulty;
            this.difficultyComboBox.SelectedIndexChanged += new EventHandler(this.DifficultyComboBox_SelectedIndexChanged);


            //
            // flagLabel
            //
            this.flagLabel.DataBindings.Add("Text", Game.Board, "FlagsRemaining");
            this.flagLabel.Location = new Point(140, 30);
            this.flagLabel.AutoSize = true;

            //
            // timer
            //
            this.timer.Interval = 1000;
            this.timer.Tick += new EventHandler(Timer_Tick);

            //
            // timerLabel
            //
            this.timerLabel.DataBindings.Add("Text", Game, "TimeElapsed");
            this.timerLabel.Location = new Point(220, 30);
            this.timerLabel.AutoSize = true;

            //
            // tableLayoutPanel
            //
            this.boardTableLayoutPanel.Name = "board";
            this.boardTableLayoutPanel.Location = new Point(40, 160);
            this.boardTableLayoutPanel.AutoSize = true;
            this.boardTableLayoutPanel.Dock = DockStyle.Fill;

            // tableLayoutPanel - rows
            this.boardTableLayoutPanel.RowCount = Game.Board.Rows;

            for (int row = 0; row < this.boardTableLayoutPanel.RowCount; row++)
            {
                this.boardTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / this.boardTableLayoutPanel.RowCount));
            }

            // tableLayoutPanel - cols
            this.boardTableLayoutPanel.ColumnCount = Game.Board.Cols;

            for (int col = 0; col < this.boardTableLayoutPanel.ColumnCount; col++)
            {
                this.boardTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / this.boardTableLayoutPanel.ColumnCount));
            }

            // tableLayoutPanel - buttons
            for (int row = 0; row < this.boardTableLayoutPanel.RowCount; row++)
            {
                for (int col = 0; col < this.boardTableLayoutPanel.ColumnCount; col++)
                {
                    Button newButton = new Button();

                    newButton.MouseDown += new MouseEventHandler(this.CellButton_Click);
                    newButton.Tag = Game.Board.Grid[row, col];
                    newButton.DataBindings.Add("Text", Game.Board.Grid[row, col], "Text");
                    newButton.DataBindings.Add("BackColor", Game.Board.Grid[row, col], "BackColor");
                    newButton.DataBindings.Add("ForeColor", Game.Board.Grid[row, col], "ForeColor");
                    newButton.Font = new Font(newButton.Font, FontStyle.Bold);
                    newButton.Dock = DockStyle.Fill;
                    newButton.Size = this.GetCellButtonSize();
                    newButton.Margin = new Padding(0);
                    newButton.Padding = new Padding(0);

                    this.boardTableLayoutPanel.Controls.Add(newButton, col, row);
                }
            }

            // 
            // View
            // 
            this.Text = "Minesweeper";
            this.AutoSize = true;
            this.Controls.AddRange(new Control[] { this.boardTableLayoutPanel, this.headerGroupBox });

            this.headerGroupBox.ResumeLayout(false);
            this.headerGroupBox.PerformLayout();
            this.boardTableLayoutPanel.ResumeLayout(false);
            this.boardTableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
        }

        private GroupBox headerGroupBox;
        private ComboBox difficultyComboBox;
        private Label flagLabel, timerLabel;
        private Timer timer;
        private TableLayoutPanel boardTableLayoutPanel;
    }
}

