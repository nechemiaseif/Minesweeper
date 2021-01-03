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
            this.flagLabel = new Label();
            this.timer = new Timer();
            this.timerLabel = new Label();
            this.tableLayoutPanel = new TableLayoutPanel();
            this.SuspendLayout();

            // 
            // headerGroupBox
            // 
            this.headerGroupBox.Controls.AddRange(new Control[] { this.flagLabel, this.timerLabel });
            this.headerGroupBox.Location = new Point(50, 25);
            this.headerGroupBox.Size = new System.Drawing.Size(800, 100);

            //
            // flagLabel
            //
            this.flagLabel.DataBindings.Add("Text", Game.Board, "FlagsRemaining");
            this.flagLabel.Location = new Point(60, 30);
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
            this.timerLabel.Location = new Point(120, 30);
            this.timerLabel.AutoSize = true;

            //
            // tableLayoutPanel
            //
            this.tableLayoutPanel.Name = "board";
            this.tableLayoutPanel.Location = new Point(40, 160);
            this.tableLayoutPanel.Size = new Size(500, 500);
            this.tableLayoutPanel.AutoSize = true;
            this.tableLayoutPanel.TabIndex = 0;

            // tableLayoutPanel - rows
            this.tableLayoutPanel.RowCount = Game.Board.Rows;

            for (int row = 0; row < this.tableLayoutPanel.RowCount; row++)
            {
                this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / this.tableLayoutPanel.RowCount));
            }

            // tableLayoutPanel - cols
            this.tableLayoutPanel.ColumnCount = Game.Board.Cols;

            for (int col = 0; col < this.tableLayoutPanel.ColumnCount; col++)
            {
                this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / this.tableLayoutPanel.ColumnCount));
            }

            // tableLayoutPanel - buttons
            for (int row = 0; row < this.tableLayoutPanel.RowCount; row++)
            {
                for (int col = 0; col < this.tableLayoutPanel.ColumnCount; col++)
                {
                    Button newButton = new Button();

                    newButton.MouseDown += new MouseEventHandler(this.CellButton_Click);
                    newButton.Tag = Game.Board.Grid[row, col];
                    newButton.DataBindings.Add("Text", Game.Board.Grid[row, col], "Text");
                    newButton.DataBindings.Add("BackColor", Game.Board.Grid[row, col], "BackColor");
                    newButton.DataBindings.Add("ForeColor", Game.Board.Grid[row, col], "ForeColor");
                    newButton.Font = new Font(newButton.Font, FontStyle.Bold);
                    newButton.Dock = DockStyle.Fill;

                    this.tableLayoutPanel.Controls.Add(newButton, col, row);
                }
            }

            // 
            // View
            // 
            this.Text = "Minesweeper";
            this.Controls.AddRange(new Control[] { this.tableLayoutPanel, this.headerGroupBox });
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 700);
            this.ResumeLayout(false);
        }

        private GroupBox headerGroupBox;
        private Label flagLabel, timerLabel;
        private Timer timer;
        private TableLayoutPanel tableLayoutPanel;
    }
}

