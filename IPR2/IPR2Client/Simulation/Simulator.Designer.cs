namespace IPR2Client.Forms
{
    partial class Simulator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Simulator));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.hartslagMin = new System.Windows.Forms.Button();
            this.rondesMin = new System.Windows.Forms.Button();
            this.hartslagPlus = new System.Windows.Forms.Button();
            this.rondesPlus = new System.Windows.Forms.Button();
            this.weerstand = new System.Windows.Forms.Label();
            this.hartslag = new System.Windows.Forms.Label();
            this.rondes = new System.Windows.Forms.Label();
            this.tijd = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 8);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 18);
            this.label1.TabIndex = 15;
            this.label1.Text = "Weerstand:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(9, 37);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 18);
            this.label2.TabIndex = 16;
            this.label2.Text = "Hartslag:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(9, 65);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 18);
            this.label3.TabIndex = 17;
            this.label3.Text = "Rondes/Min:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(9, 93);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 18);
            this.label4.TabIndex = 18;
            this.label4.Text = "Tijd:";
            // 
            // hartslagMin
            // 
            this.hartslagMin.Location = new System.Drawing.Point(139, 40);
            this.hartslagMin.Margin = new System.Windows.Forms.Padding(2);
            this.hartslagMin.Name = "hartslagMin";
            this.hartslagMin.Size = new System.Drawing.Size(15, 16);
            this.hartslagMin.TabIndex = 21;
            this.hartslagMin.Text = "-";
            this.hartslagMin.UseVisualStyleBackColor = true;
            this.hartslagMin.Click += new System.EventHandler(this.hartslagMin_Click);
            // 
            // rondesMin
            // 
            this.rondesMin.Location = new System.Drawing.Point(139, 68);
            this.rondesMin.Margin = new System.Windows.Forms.Padding(2);
            this.rondesMin.Name = "rondesMin";
            this.rondesMin.Size = new System.Drawing.Size(15, 16);
            this.rondesMin.TabIndex = 22;
            this.rondesMin.Text = "-";
            this.rondesMin.UseVisualStyleBackColor = true;
            this.rondesMin.Click += new System.EventHandler(this.rondesMin_Click);
            // 
            // hartslagPlus
            // 
            this.hartslagPlus.Location = new System.Drawing.Point(265, 40);
            this.hartslagPlus.Margin = new System.Windows.Forms.Padding(2);
            this.hartslagPlus.Name = "hartslagPlus";
            this.hartslagPlus.Size = new System.Drawing.Size(15, 16);
            this.hartslagPlus.TabIndex = 23;
            this.hartslagPlus.Text = "+";
            this.hartslagPlus.UseVisualStyleBackColor = true;
            this.hartslagPlus.Click += new System.EventHandler(this.hartslagPlus_Click);
            // 
            // rondesPlus
            // 
            this.rondesPlus.Location = new System.Drawing.Point(265, 68);
            this.rondesPlus.Margin = new System.Windows.Forms.Padding(2);
            this.rondesPlus.Name = "rondesPlus";
            this.rondesPlus.Size = new System.Drawing.Size(15, 16);
            this.rondesPlus.TabIndex = 24;
            this.rondesPlus.Text = "+";
            this.rondesPlus.UseVisualStyleBackColor = true;
            this.rondesPlus.Click += new System.EventHandler(this.rondesPlus_Click);
            // 
            // weerstand
            // 
            this.weerstand.AutoSize = true;
            this.weerstand.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.weerstand.Location = new System.Drawing.Point(195, 8);
            this.weerstand.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.weerstand.Name = "weerstand";
            this.weerstand.Size = new System.Drawing.Size(40, 18);
            this.weerstand.TabIndex = 25;
            this.weerstand.Text = "0000";
            // 
            // hartslag
            // 
            this.hartslag.AutoSize = true;
            this.hartslag.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hartslag.Location = new System.Drawing.Point(195, 37);
            this.hartslag.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.hartslag.Name = "hartslag";
            this.hartslag.Size = new System.Drawing.Size(40, 18);
            this.hartslag.TabIndex = 26;
            this.hartslag.Text = "0000";
            // 
            // rondes
            // 
            this.rondes.AutoSize = true;
            this.rondes.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rondes.Location = new System.Drawing.Point(195, 65);
            this.rondes.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.rondes.Name = "rondes";
            this.rondes.Size = new System.Drawing.Size(40, 18);
            this.rondes.TabIndex = 27;
            this.rondes.Text = "0000";
            // 
            // tijd
            // 
            this.tijd.AutoSize = true;
            this.tijd.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tijd.Location = new System.Drawing.Point(183, 93);
            this.tijd.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.tijd.Name = "tijd";
            this.tijd.Size = new System.Drawing.Size(44, 18);
            this.tijd.TabIndex = 28;
            this.tijd.Text = "00:00";
            // 
            // Simulator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(308, 124);
            this.Controls.Add(this.tijd);
            this.Controls.Add(this.rondes);
            this.Controls.Add(this.hartslag);
            this.Controls.Add(this.weerstand);
            this.Controls.Add(this.rondesPlus);
            this.Controls.Add(this.hartslagPlus);
            this.Controls.Add(this.rondesMin);
            this.Controls.Add(this.hartslagMin);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Simulator";
            this.Text = "Simulator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button hartslagMin;
        private System.Windows.Forms.Button rondesMin;
        private System.Windows.Forms.Button hartslagPlus;
        private System.Windows.Forms.Button rondesPlus;
        private System.Windows.Forms.Label weerstand;
        private System.Windows.Forms.Label hartslag;
        private System.Windows.Forms.Label rondes;
        private System.Windows.Forms.Label tijd;
    }
}