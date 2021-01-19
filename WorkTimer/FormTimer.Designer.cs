namespace WorkTimer
{
    partial class FormTimer
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTimer));
            this.btnPause = new System.Windows.Forms.PictureBox();
            this.btnResume = new System.Windows.Forms.PictureBox();
            this.btnNew = new System.Windows.Forms.PictureBox();
            this.labelTime = new System.Windows.Forms.Label();
            this.timerMinute = new System.Windows.Forms.Timer(this.components);
            this.timerBlink = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.btnPause)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnResume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnNew)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPause
            // 
            this.btnPause.Image = ((System.Drawing.Image)(resources.GetObject("btnPause.Image")));
            this.btnPause.Location = new System.Drawing.Point(169, 69);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(8, 8);
            this.btnPause.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.btnPause.TabIndex = 0;
            this.btnPause.TabStop = false;
            // 
            // btnResume
            // 
            this.btnResume.Image = ((System.Drawing.Image)(resources.GetObject("btnResume.Image")));
            this.btnResume.Location = new System.Drawing.Point(183, 69);
            this.btnResume.Name = "btnResume";
            this.btnResume.Size = new System.Drawing.Size(8, 8);
            this.btnResume.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.btnResume.TabIndex = 0;
            this.btnResume.TabStop = false;
            // 
            // btnNew
            // 
            this.btnNew.Image = ((System.Drawing.Image)(resources.GetObject("btnNew.Image")));
            this.btnNew.Location = new System.Drawing.Point(197, 69);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(8, 8);
            this.btnNew.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.btnNew.TabIndex = 0;
            this.btnNew.TabStop = false;
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelTime.ForeColor = System.Drawing.Color.Silver;
            this.labelTime.Location = new System.Drawing.Point(12, 9);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(62, 26);
            this.labelTime.TabIndex = 1;
            this.labelTime.Text = "00:00";
            this.labelTime.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormTimer_MouseDown);
            // 
            // timerMinute
            // 
            this.timerMinute.Enabled = true;
            this.timerMinute.Interval = 1000;
            this.timerMinute.Tick += new System.EventHandler(this.timerMinute_Tick);
            // 
            // timerBlink
            // 
            this.timerBlink.Interval = 500;
            this.timerBlink.Tick += new System.EventHandler(this.timerBlink_Tick);
            // 
            // FormTimer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(264, 139);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnResume);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.labelTime);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormTimer";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "WorkTimer";
            this.Load += new System.EventHandler(this.FormTimer_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormTimer_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.btnPause)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnResume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnNew)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox btnPause;
        private System.Windows.Forms.PictureBox btnResume;
        private System.Windows.Forms.PictureBox btnNew;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.Timer timerMinute;
        private System.Windows.Forms.Timer timerBlink;
    }
}

