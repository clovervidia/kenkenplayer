namespace KenKenPlayer
{
    partial class KenKenPlayer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.InstructionsLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.CheckButton = new System.Windows.Forms.Button();
            this.NotesTextBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // InstructionsLabel
            // 
            this.InstructionsLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InstructionsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InstructionsLabel.ForeColor = System.Drawing.SystemColors.Window;
            this.InstructionsLabel.Location = new System.Drawing.Point(0, 0);
            this.InstructionsLabel.Name = "InstructionsLabel";
            this.InstructionsLabel.Size = new System.Drawing.Size(800, 450);
            this.InstructionsLabel.TabIndex = 2;
            this.InstructionsLabel.Text = "Drag a .txt file here to begin!";
            this.InstructionsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.CheckButton, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.NotesTextBox, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 30);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // CheckButton
            // 
            this.CheckButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CheckButton.Enabled = false;
            this.CheckButton.Location = new System.Drawing.Point(3, 3);
            this.CheckButton.Name = "CheckButton";
            this.CheckButton.Size = new System.Drawing.Size(75, 24);
            this.CheckButton.TabIndex = 0;
            this.CheckButton.Text = "Check";
            this.CheckButton.UseVisualStyleBackColor = true;
            this.CheckButton.Click += new System.EventHandler(this.CheckButton_Click);
            // 
            // NotesTextBox
            // 
            this.NotesTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NotesTextBox.Location = new System.Drawing.Point(84, 5);
            this.NotesTextBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.NotesTextBox.Name = "NotesTextBox";
            this.NotesTextBox.ReadOnly = true;
            this.NotesTextBox.Size = new System.Drawing.Size(713, 20);
            this.NotesTextBox.TabIndex = 1;
            this.NotesTextBox.TextChanged += new System.EventHandler(this.NotesTextBox_TextChanged);
            this.NotesTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NotesTextBox_KeyDown);
            // 
            // KenKenPlayer
            // 
            this.AcceptButton = this.CheckButton;
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.InstructionsLabel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "KenKenPlayer";
            this.Text = "KenKen Player (no file loaded)";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.KenKenPlayer_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.KenKenPlayer_DragEnter);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label InstructionsLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button CheckButton;
        private System.Windows.Forms.TextBox NotesTextBox;
    }
}

