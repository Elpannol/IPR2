namespace IPR2Client.Forms
{
    partial class NewTest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewTest));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.weerstandLabel = new System.Windows.Forms.Label();
            this.hartslagLabel = new System.Windows.Forms.Label();
            this.rondesLabel = new System.Windows.Forms.Label();
            this.tijdLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(334, 30);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(176, 140);
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 24);
            this.label1.TabIndex = 14;
            this.label1.Text = "Weerstand:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(15, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 24);
            this.label2.TabIndex = 15;
            this.label2.Text = "Hartslag:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(15, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 24);
            this.label3.TabIndex = 16;
            this.label3.Text = "Rondes/Min:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(15, 135);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 24);
            this.label4.TabIndex = 17;
            this.label4.Text = "Tijd:";
            // 
            // weerstandLabel
            // 
            this.weerstandLabel.AutoSize = true;
            this.weerstandLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.weerstandLabel.Location = new System.Drawing.Point(145, 30);
            this.weerstandLabel.Name = "weerstandLabel";
            this.weerstandLabel.Size = new System.Drawing.Size(80, 24);
            this.weerstandLabel.TabIndex = 18;
            this.weerstandLabel.Text = "0000000";
            // 
            // hartslagLabel
            // 
            this.hartslagLabel.AutoSize = true;
            this.hartslagLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hartslagLabel.Location = new System.Drawing.Point(145, 65);
            this.hartslagLabel.Name = "hartslagLabel";
            this.hartslagLabel.Size = new System.Drawing.Size(80, 24);
            this.hartslagLabel.TabIndex = 19;
            this.hartslagLabel.Text = "0000000";
            // 
            // rondesLabel
            // 
            this.rondesLabel.AutoSize = true;
            this.rondesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rondesLabel.Location = new System.Drawing.Point(145, 100);
            this.rondesLabel.Name = "rondesLabel";
            this.rondesLabel.Size = new System.Drawing.Size(80, 24);
            this.rondesLabel.TabIndex = 20;
            this.rondesLabel.Text = "0000000";
            // 
            // tijdLabel
            // 
            this.tijdLabel.AutoSize = true;
            this.tijdLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tijdLabel.Location = new System.Drawing.Point(145, 135);
            this.tijdLabel.Name = "tijdLabel";
            this.tijdLabel.Size = new System.Drawing.Size(80, 24);
            this.tijdLabel.TabIndex = 21;
            this.tijdLabel.Text = "0000000";
            // 
            // NewTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 262);
            this.Controls.Add(this.tijdLabel);
            this.Controls.Add(this.rondesLabel);
            this.Controls.Add(this.hartslagLabel);
            this.Controls.Add(this.weerstandLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NewTest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CG Fit";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label weerstandLabel;
        private System.Windows.Forms.Label hartslagLabel;
        private System.Windows.Forms.Label rondesLabel;
        private System.Windows.Forms.Label tijdLabel;
    }
}