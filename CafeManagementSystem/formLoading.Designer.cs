namespace CafeManagementSystem
{
    partial class formLoading
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
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelLocation = new System.Windows.Forms.Label();
            this.labelCafeName = new System.Windows.Forms.Label();
            this.panelSlider = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CMSLogo = new System.Windows.Forms.PictureBox();
            this.cafeManagementDBDataSet1 = new CafeManagementSystem.CafeManagementDBDataSet();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CMSLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cafeManagementDBDataSet1)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(242)))), ((int)(((byte)(202)))));
            this.panel2.Controls.Add(this.labelLocation);
            this.panel2.Controls.Add(this.labelCafeName);
            this.panel2.Controls.Add(this.panelSlider);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.CMSLogo);
            this.panel2.Location = new System.Drawing.Point(10, 8);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1010, 616);
            this.panel2.TabIndex = 4;
            // 
            // labelLocation
            // 
            this.labelLocation.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelLocation.AutoSize = true;
            this.labelLocation.Font = new System.Drawing.Font("Cooper Black", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(51)))), ((int)(((byte)(11)))));
            this.labelLocation.Location = new System.Drawing.Point(19, 511);
            this.labelLocation.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(103, 24);
            this.labelLocation.TabIndex = 11;
            this.labelLocation.Text = "Location";
            // 
            // labelCafeName
            // 
            this.labelCafeName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelCafeName.AutoSize = true;
            this.labelCafeName.Font = new System.Drawing.Font("Cooper Black", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCafeName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(51)))), ((int)(((byte)(11)))));
            this.labelCafeName.Location = new System.Drawing.Point(18, 466);
            this.labelCafeName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelCafeName.Name = "labelCafeName";
            this.labelCafeName.Size = new System.Drawing.Size(187, 36);
            this.labelCafeName.TabIndex = 10;
            this.labelCafeName.Text = "XYZ CAFE";
            // 
            // panelSlider
            // 
            this.panelSlider.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(51)))), ((int)(((byte)(11)))));
            this.panelSlider.Location = new System.Drawing.Point(165, 298);
            this.panelSlider.Margin = new System.Windows.Forms.Padding(2);
            this.panelSlider.Name = "panelSlider";
            this.panelSlider.Size = new System.Drawing.Size(10, 15);
            this.panelSlider.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Cooper Black", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(51)))), ((int)(((byte)(11)))));
            this.label1.Location = new System.Drawing.Point(299, 328);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(427, 36);
            this.label1.TabIndex = 7;
            this.label1.Text = "Cafe Management System";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(164)))), ((int)(((byte)(129)))));
            this.panel1.Location = new System.Drawing.Point(165, 298);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(675, 15);
            this.panel1.TabIndex = 8;
            // 
            // CMSLogo
            // 
            this.CMSLogo.Image = global::CafeManagementSystem.Properties.Resources.loading2;
            this.CMSLogo.Location = new System.Drawing.Point(151, -90);
            this.CMSLogo.Margin = new System.Windows.Forms.Padding(2);
            this.CMSLogo.Name = "CMSLogo";
            this.CMSLogo.Size = new System.Drawing.Size(710, 535);
            this.CMSLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.CMSLogo.TabIndex = 3;
            this.CMSLogo.TabStop = false;
            // 
            // cafeManagementDBDataSet1
            // 
            this.cafeManagementDBDataSet1.DataSetName = "CafeManagementDBDataSet";
            this.cafeManagementDBDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // formLoading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(51)))), ((int)(((byte)(11)))));
            this.ClientSize = new System.Drawing.Size(1031, 635);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "formLoading";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Loading";
            this.Load += new System.EventHandler(this.formLoading_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CMSLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cafeManagementDBDataSet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox CMSLogo;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panelSlider;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelLocation;
        private System.Windows.Forms.Label labelCafeName;
        private CafeManagementDBDataSet cafeManagementDBDataSet1;
    }
}

