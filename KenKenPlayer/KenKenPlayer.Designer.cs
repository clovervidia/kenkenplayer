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
            this.SaveStateButton = new System.Windows.Forms.Button();
            this.LoadStateButton = new System.Windows.Forms.Button();
            this.PauseButton = new System.Windows.Forms.Button();
            this.CheckButton = new System.Windows.Forms.Button();
            this.NotesTextBox = new System.Windows.Forms.RichTextBox();
            this.ResetButton = new System.Windows.Forms.Button();
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
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.SaveStateButton, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.LoadStateButton, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.PauseButton, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.CheckButton, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.NotesTextBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.ResetButton, 5, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 30);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // SaveStateButton
            // 
            this.SaveStateButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SaveStateButton.Enabled = false;
            this.SaveStateButton.Location = new System.Drawing.Point(479, 3);
            this.SaveStateButton.Name = "SaveStateButton";
            this.SaveStateButton.Size = new System.Drawing.Size(75, 24);
            this.SaveStateButton.TabIndex = 3;
            this.SaveStateButton.Text = "&Save";
            this.SaveStateButton.UseVisualStyleBackColor = true;
            this.SaveStateButton.Click += new System.EventHandler(this.SaveStateButton_Click);
            // 
            // LoadStateButton
            // 
            this.LoadStateButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LoadStateButton.Enabled = false;
            this.LoadStateButton.Location = new System.Drawing.Point(560, 3);
            this.LoadStateButton.Name = "LoadStateButton";
            this.LoadStateButton.Size = new System.Drawing.Size(75, 24);
            this.LoadStateButton.TabIndex = 4;
            this.LoadStateButton.Text = "&Load";
            this.LoadStateButton.UseVisualStyleBackColor = true;
            this.LoadStateButton.Click += new System.EventHandler(this.LoadStateButton_Click);
            // 
            // PauseButton
            // 
            this.PauseButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PauseButton.Enabled = false;
            this.PauseButton.Location = new System.Drawing.Point(641, 3);
            this.PauseButton.Name = "PauseButton";
            this.PauseButton.Size = new System.Drawing.Size(75, 24);
            this.PauseButton.TabIndex = 5;
            this.PauseButton.Text = "&Pause";
            this.PauseButton.UseVisualStyleBackColor = true;
            this.PauseButton.Click += new System.EventHandler(this.PauseButton_Click);
            // 
            // CheckButton
            // 
            this.CheckButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CheckButton.Enabled = false;
            this.CheckButton.Location = new System.Drawing.Point(3, 3);
            this.CheckButton.Name = "CheckButton";
            this.CheckButton.Size = new System.Drawing.Size(75, 24);
            this.CheckButton.TabIndex = 0;
            this.CheckButton.Text = "&Check";
            this.CheckButton.UseVisualStyleBackColor = true;
            this.CheckButton.Click += new System.EventHandler(this.CheckButton_Click);
            // 
            // NotesTextBox
            // 
            this.NotesTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NotesTextBox.Enabled = false;
            this.NotesTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NotesTextBox.Location = new System.Drawing.Point(84, 3);
            this.NotesTextBox.Multiline = false;
            this.NotesTextBox.Name = "NotesTextBox";
            this.NotesTextBox.Size = new System.Drawing.Size(389, 24);
            this.NotesTextBox.TabIndex = 1;
            this.NotesTextBox.Text = "";
            this.NotesTextBox.TextChanged += new System.EventHandler(this.NotesTextBox_TextChanged);
            this.NotesTextBox.Enter += new System.EventHandler(this.NotesTextBox_Enter);
            this.NotesTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NotesTextBox_KeyDown);
            // 
            // ResetButton
            // 
            this.ResetButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResetButton.Enabled = false;
            this.ResetButton.Location = new System.Drawing.Point(722, 3);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(75, 24);
            this.ResetButton.TabIndex = 6;
            this.ResetButton.Text = "&Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
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
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label InstructionsLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button CheckButton;
        private System.Windows.Forms.RichTextBox NotesTextBox;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.Button SaveStateButton;
        private System.Windows.Forms.Button LoadStateButton;
        private System.Windows.Forms.Button PauseButton;
    }
}

