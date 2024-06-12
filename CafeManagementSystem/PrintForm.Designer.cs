namespace CafeManagementSystem
{
    partial class PrintForm
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
            this.AutoRefreshTimer = new System.Windows.Forms.Timer(this.components);
            this.btnExit = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panelOrderContainer = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.MenuTreeView = new System.Windows.Forms.TreeView();
            this.panel23 = new System.Windows.Forms.Panel();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.panel24 = new System.Windows.Forms.Panel();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.panel25 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.btnExit)).BeginInit();
            this.panel2.SuspendLayout();
            this.panelOrderContainer.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel23.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.panel24.SuspendLayout();
            this.SuspendLayout();
            // 
            // AutoRefreshTimer
            // 
            this.AutoRefreshTimer.Interval = 10000;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(188)))), ((int)(((byte)(140)))));
            this.btnExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExit.Image = global::CafeManagementSystem.Properties.Resources.CloseButton2;
            this.btnExit.Location = new System.Drawing.Point(1044, 5);
            this.btnExit.Margin = new System.Windows.Forms.Padding(2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(26, 25);
            this.btnExit.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnExit.TabIndex = 114;
            this.btnExit.TabStop = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(188)))), ((int)(((byte)(140)))));
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.btnExit);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1081, 35);
            this.panel2.TabIndex = 115;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label1.Font = new System.Drawing.Font("Cooper Black", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(51)))), ((int)(((byte)(11)))));
            this.label1.Location = new System.Drawing.Point(484, 2);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 31);
            this.label1.TabIndex = 115;
            this.label1.Text = "Print";
            // 
            // panelOrderContainer
            // 
            this.panelOrderContainer.AutoScrollMargin = new System.Drawing.Size(25, 25);
            this.panelOrderContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(51)))), ((int)(((byte)(11)))));
            this.panelOrderContainer.Controls.Add(this.panel1);
            this.panelOrderContainer.Controls.Add(this.panel23);
            this.panelOrderContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOrderContainer.Location = new System.Drawing.Point(0, 35);
            this.panelOrderContainer.Name = "panelOrderContainer";
            this.panelOrderContainer.Size = new System.Drawing.Size(1081, 679);
            this.panelOrderContainer.TabIndex = 116;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.MenuTreeView);
            this.panel1.Location = new System.Drawing.Point(273, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(521, 612);
            this.panel1.TabIndex = 139;
            // 
            // MenuTreeView
            // 
            this.MenuTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MenuTreeView.Location = new System.Drawing.Point(0, 0);
            this.MenuTreeView.Name = "MenuTreeView";
            this.MenuTreeView.Size = new System.Drawing.Size(521, 612);
            this.MenuTreeView.TabIndex = 0;
            // 
            // panel23
            // 
            this.panel23.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel23.AutoScroll = true;
            this.panel23.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(242)))), ((int)(((byte)(202)))));
            this.panel23.Controls.Add(this.dataGridView2);
            this.panel23.Controls.Add(this.panel24);
            this.panel23.Location = new System.Drawing.Point(72, 788);
            this.panel23.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.panel23.Name = "panel23";
            this.panel23.Size = new System.Drawing.Size(922, 476);
            this.panel23.TabIndex = 138;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(27, 92);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowHeadersVisible = false;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.Size = new System.Drawing.Size(834, 368);
            this.dataGridView2.TabIndex = 131;
            // 
            // panel24
            // 
            this.panel24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(188)))), ((int)(((byte)(140)))));
            this.panel24.Controls.Add(this.dateTimePicker1);
            this.panel24.Controls.Add(this.label7);
            this.panel24.Controls.Add(this.panel25);
            this.panel24.Controls.Add(this.label6);
            this.panel24.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel24.Location = new System.Drawing.Point(0, 0);
            this.panel24.Name = "panel24";
            this.panel24.Size = new System.Drawing.Size(922, 69);
            this.panel24.TabIndex = 130;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker1.Location = new System.Drawing.Point(613, 19);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(249, 31);
            this.dateTimePicker1.TabIndex = 111;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Cooper Black", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(51)))), ((int)(((byte)(11)))));
            this.label7.Location = new System.Drawing.Point(497, 21);
            this.label7.Margin = new System.Windows.Forms.Padding(90, 20, 0, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 27);
            this.label7.TabIndex = 110;
            this.label7.Text = "By Date";
            // 
            // panel25
            // 
            this.panel25.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(51)))), ((int)(((byte)(11)))));
            this.panel25.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel25.Location = new System.Drawing.Point(0, 66);
            this.panel25.Name = "panel25";
            this.panel25.Size = new System.Drawing.Size(922, 3);
            this.panel25.TabIndex = 109;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Cooper Black", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(51)))), ((int)(((byte)(11)))));
            this.label6.Location = new System.Drawing.Point(15, 19);
            this.label6.Margin = new System.Windows.Forms.Padding(90, 20, 0, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(138, 27);
            this.label6.TabIndex = 97;
            this.label6.Text = "Order List";
            // 
            // PrintForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1081, 714);
            this.Controls.Add(this.panelOrderContainer);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PrintForm";
            this.Text = "PrintForm";
            this.Load += new System.EventHandler(this.PrintForm_LOAD);
            ((System.ComponentModel.ISupportInitialize)(this.btnExit)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panelOrderContainer.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel23.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.panel24.ResumeLayout(false);
            this.panel24.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer AutoRefreshTimer;
        private System.Windows.Forms.PictureBox btnExit;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panelOrderContainer;
        private System.Windows.Forms.Panel panel23;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Panel panel24;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel25;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TreeView MenuTreeView;
    }
}