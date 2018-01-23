namespace IPR2Client.Forms
{
    partial class Results
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Results));
            this.newTestButton = new System.Windows.Forms.Button();
            this.trainingListBox = new System.Windows.Forms.ListBox();
            this.timeTrackBar = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.weerstandLabel = new System.Windows.Forms.Label();
            this.hartslagLabel = new System.Windows.Forms.Label();
            this.rondesLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tijdLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.loginLabel = new System.Windows.Forms.Label();
            this.comportBox = new System.Windows.Forms.ComboBox();
            this.warningLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.timeTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // newTestButton
            // 
            this.newTestButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newTestButton.Location = new System.Drawing.Point(9, 10);
            this.newTestButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.newTestButton.Name = "newTestButton";
            this.newTestButton.Size = new System.Drawing.Size(132, 46);
            this.newTestButton.TabIndex = 0;
            this.newTestButton.Text = "Start een test";
            this.newTestButton.UseVisualStyleBackColor = true;
            this.newTestButton.Click += new System.EventHandler(this.newTestButton_Click);
            // 
            // trainingListBox
            // 
            this.trainingListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trainingListBox.FormattingEnabled = true;
            this.trainingListBox.ItemHeight = 18;
            this.trainingListBox.Location = new System.Drawing.Point(9, 60);
            this.trainingListBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.trainingListBox.Name = "trainingListBox";
            this.trainingListBox.Size = new System.Drawing.Size(133, 112);
            this.trainingListBox.TabIndex = 1;
            this.trainingListBox.SelectedIndexChanged += new System.EventHandler(this.trainingListBox_SelectedIndexChanged);
            // 
            // timeTrackBar
            // 
            this.timeTrackBar.Location = new System.Drawing.Point(145, 158);
            this.timeTrackBar.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.timeTrackBar.Name = "timeTrackBar";
            this.timeTrackBar.Size = new System.Drawing.Size(374, 45);
            this.timeTrackBar.TabIndex = 2;
            this.timeTrackBar.Scroll += new System.EventHandler(this.timeTrackBar_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(169, 24);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 18);
            this.label1.TabIndex = 3;
            this.label1.Text = "Weerstand:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(169, 53);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 18);
            this.label2.TabIndex = 4;
            this.label2.Text = "Hartslag:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(169, 81);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 18);
            this.label3.TabIndex = 5;
            this.label3.Text = "Rondes/Min:";
            // 
            // weerstandLabel
            // 
            this.weerstandLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.weerstandLabel.Location = new System.Drawing.Point(266, 24);
            this.weerstandLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.weerstandLabel.Name = "weerstandLabel";
            this.weerstandLabel.Size = new System.Drawing.Size(60, 20);
            this.weerstandLabel.TabIndex = 6;
            // 
            // hartslagLabel
            // 
            this.hartslagLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hartslagLabel.Location = new System.Drawing.Point(266, 53);
            this.hartslagLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.hartslagLabel.Name = "hartslagLabel";
            this.hartslagLabel.Size = new System.Drawing.Size(60, 20);
            this.hartslagLabel.TabIndex = 7;
            // 
            // rondesLabel
            // 
            this.rondesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rondesLabel.Location = new System.Drawing.Point(266, 81);
            this.rondesLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.rondesLabel.Name = "rondesLabel";
            this.rondesLabel.Size = new System.Drawing.Size(60, 20);
            this.rondesLabel.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(169, 110);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 18);
            this.label4.TabIndex = 9;
            this.label4.Text = "Tijd:";
            // 
            // tijdLabel
            // 
            this.tijdLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tijdLabel.Location = new System.Drawing.Point(266, 110);
            this.tijdLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.tijdLabel.Name = "tijdLabel";
            this.tijdLabel.Size = new System.Drawing.Size(60, 20);
            this.tijdLabel.TabIndex = 10;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(386, 45);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(132, 108);
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // loginLabel
            // 
            this.loginLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.loginLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loginLabel.Location = new System.Drawing.Point(323, 7);
            this.loginLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.loginLabel.Name = "loginLabel";
            this.loginLabel.Size = new System.Drawing.Size(196, 20);
            this.loginLabel.TabIndex = 12;
            this.loginLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comportBox
            // 
            this.comportBox.FormattingEnabled = true;
            this.comportBox.Location = new System.Drawing.Point(9, 185);
            this.comportBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comportBox.Name = "comportBox";
            this.comportBox.Size = new System.Drawing.Size(132, 21);
            this.comportBox.TabIndex = 13;
            // 
            // warningLabel
            // 
            this.warningLabel.AutoSize = true;
            this.warningLabel.ForeColor = System.Drawing.Color.Red;
            this.warningLabel.Location = new System.Drawing.Point(147, 192);
            this.warningLabel.Name = "warningLabel";
            this.warningLabel.Size = new System.Drawing.Size(47, 13);
            this.warningLabel.TabIndex = 14;
            this.warningLabel.Text = "Warning";
            this.warningLabel.Visible = false;
            // 
            // Results
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 213);
            this.Controls.Add(this.warningLabel);
            this.Controls.Add(this.comportBox);
            this.Controls.Add(this.loginLabel);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.tijdLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.rondesLabel);
            this.Controls.Add(this.hartslagLabel);
            this.Controls.Add(this.weerstandLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.timeTrackBar);
            this.Controls.Add(this.trainingListBox);
            this.Controls.Add(this.newTestButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Results";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CG Fit";
            ((System.ComponentModel.ISupportInitialize)(this.timeTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button newTestButton;
        private System.Windows.Forms.ListBox trainingListBox;
        private System.Windows.Forms.TrackBar timeTrackBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label weerstandLabel;
        private System.Windows.Forms.Label hartslagLabel;
        private System.Windows.Forms.Label rondesLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label tijdLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label loginLabel;
        private System.Windows.Forms.ComboBox comportBox;
        private System.Windows.Forms.Label warningLabel;
    }
}