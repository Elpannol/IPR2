namespace IPR2Client.Forms
{
    partial class Doctor
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Doctor));
            this.trainingListBox = new System.Windows.Forms.ListBox();
            this.userListBox = new System.Windows.Forms.ListBox();
            this.loginLabel = new System.Windows.Forms.Label();
            this.measurmentChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.measurmentChart)).BeginInit();
            this.SuspendLayout();
            // 
            // trainingListBox
            // 
            this.trainingListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trainingListBox.FormattingEnabled = true;
            this.trainingListBox.ItemHeight = 18;
            this.trainingListBox.Location = new System.Drawing.Point(146, 10);
            this.trainingListBox.Margin = new System.Windows.Forms.Padding(2);
            this.trainingListBox.Name = "trainingListBox";
            this.trainingListBox.Size = new System.Drawing.Size(133, 166);
            this.trainingListBox.TabIndex = 2;
            this.trainingListBox.SelectedIndexChanged += new System.EventHandler(this.trainingListBox_SelectedIndexChanged);
            // 
            // userListBox
            // 
            this.userListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userListBox.FormattingEnabled = true;
            this.userListBox.ItemHeight = 18;
            this.userListBox.Location = new System.Drawing.Point(9, 10);
            this.userListBox.Margin = new System.Windows.Forms.Padding(2);
            this.userListBox.Name = "userListBox";
            this.userListBox.Size = new System.Drawing.Size(133, 166);
            this.userListBox.TabIndex = 3;
            this.userListBox.SelectedIndexChanged += new System.EventHandler(this.userListBox_SelectedIndexChanged);
            // 
            // loginLabel
            // 
            this.loginLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.loginLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loginLabel.Location = new System.Drawing.Point(83, 178);
            this.loginLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.loginLabel.Name = "loginLabel";
            this.loginLabel.Size = new System.Drawing.Size(196, 20);
            this.loginLabel.TabIndex = 17;
            this.loginLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.loginLabel.Click += new System.EventHandler(this.loginLabel_Click);
            // 
            // measurmentChart
            // 
            chartArea1.CursorY.Interval = 15D;
            chartArea1.Name = "ChartArea1";
            this.measurmentChart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.measurmentChart.Legends.Add(legend1);
            this.measurmentChart.Location = new System.Drawing.Point(292, 10);
            this.measurmentChart.Name = "measurmentChart";
            series1.BorderWidth = 3;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Heartrate";
            series2.BorderWidth = 3;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.Name = "Rondes";
            this.measurmentChart.Series.Add(series1);
            this.measurmentChart.Series.Add(series2);
            this.measurmentChart.Size = new System.Drawing.Size(1107, 300);
            this.measurmentChart.TabIndex = 18;
            this.measurmentChart.Text = "chart1";
            this.measurmentChart.Click += new System.EventHandler(this.measurmentChart_Click);
            // 
            // Doctor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1411, 333);
            this.Controls.Add(this.measurmentChart);
            this.Controls.Add(this.loginLabel);
            this.Controls.Add(this.userListBox);
            this.Controls.Add(this.trainingListBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Doctor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CG Fit";
            ((System.ComponentModel.ISupportInitialize)(this.measurmentChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox trainingListBox;
        private System.Windows.Forms.ListBox userListBox;
        private System.Windows.Forms.Label loginLabel;
        private System.Windows.Forms.DataVisualization.Charting.Chart measurmentChart;
    }
}