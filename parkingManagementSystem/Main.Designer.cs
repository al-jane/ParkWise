namespace parkingManagementSystem
{
    partial class Main
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            panelMain = new Panel();
            sidebarPanel = new Panel();
            pnlSeparator = new Panel();
            label2 = new Label();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            reservationbtn = new RoundedButton();
            trackerbtn = new RoundedButton();
            historybtn = new RoundedButton();
            billingbtn = new RoundedButton();
            entryexitbtn = new RoundedButton();
            parkingbtn = new RoundedButton();
            dashboardbtn = new RoundedButton();
            sidebarPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panelMain
            // 
            panelMain.BackColor = Color.FromArgb(247, 250, 252);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(0, 0);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(1248, 642);
            panelMain.TabIndex = 1;
            // 
            // sidebarPanel
            // 
            sidebarPanel.BackColor = Color.FromArgb(40, 42, 58);
            sidebarPanel.Controls.Add(pnlSeparator);
            sidebarPanel.Controls.Add(label2);
            sidebarPanel.Controls.Add(label1);
            sidebarPanel.Controls.Add(pictureBox1);
            sidebarPanel.Controls.Add(reservationbtn);
            sidebarPanel.Controls.Add(trackerbtn);
            sidebarPanel.Controls.Add(historybtn);
            sidebarPanel.Controls.Add(billingbtn);
            sidebarPanel.Controls.Add(entryexitbtn);
            sidebarPanel.Controls.Add(parkingbtn);
            sidebarPanel.Controls.Add(dashboardbtn);
            sidebarPanel.Dock = DockStyle.Left;
            sidebarPanel.Location = new Point(0, 0);
            sidebarPanel.Name = "sidebarPanel";
            sidebarPanel.Size = new Size(240, 642);
            sidebarPanel.TabIndex = 1;
            // 
            // pnlSeparator
            // 
            pnlSeparator.BackColor = Color.FromArgb(55, 57, 75);
            pnlSeparator.Location = new Point(30, 155);
            pnlSeparator.Name = "pnlSeparator";
            pnlSeparator.Size = new Size(180, 1);
            pnlSeparator.TabIndex = 12;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 8F);
            label2.ForeColor = Color.FromArgb(160, 165, 180);
            label2.Location = new Point(55, 115);
            label2.Name = "label2";
            label2.Size = new Size(141, 13);
            label2.TabIndex = 10;
            label2.Text = "Parking Management System";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(78, 85);
            label1.Name = "label1";
            label1.Size = new Size(100, 30);
            label1.TabIndex = 9;
            label1.Text = "ParkWise";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(90, 20);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(61, 56);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 8;
            pictureBox1.TabStop = false;
            // 
            // dashboardbtn
            // 
            dashboardbtn.BackColor = Color.Transparent;
            dashboardbtn.BorderRadius = 12;
            dashboardbtn.FlatAppearance.BorderSize = 0;
            dashboardbtn.FlatStyle = FlatStyle.Flat;
            dashboardbtn.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            dashboardbtn.ForeColor = Color.White;
            dashboardbtn.Image = (Image)resources.GetObject("dashboardbtn.Image");
            dashboardbtn.ImageAlign = ContentAlignment.MiddleLeft;
            dashboardbtn.Location = new Point(15, 180);
            dashboardbtn.Name = "dashboardbtn";
            dashboardbtn.Padding = new Padding(20, 0, 0, 0);
            dashboardbtn.Size = new Size(210, 45);
            dashboardbtn.TabIndex = 0;
            dashboardbtn.Text = "     Dashboard";
            dashboardbtn.TextAlign = ContentAlignment.MiddleLeft;
            dashboardbtn.TextImageRelation = TextImageRelation.ImageBeforeText;
            dashboardbtn.UseVisualStyleBackColor = false;
            dashboardbtn.Cursor = Cursors.Hand;
            dashboardbtn.Click += dashboardbtn_Click;
            // 
            // parkingbtn
            // 
            parkingbtn.BackColor = Color.Transparent;
            parkingbtn.BorderRadius = 12;
            parkingbtn.FlatAppearance.BorderSize = 0;
            parkingbtn.FlatStyle = FlatStyle.Flat;
            parkingbtn.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            parkingbtn.ForeColor = Color.White;
            parkingbtn.Image = (Image)resources.GetObject("parkingbtn.Image");
            parkingbtn.ImageAlign = ContentAlignment.MiddleLeft;
            parkingbtn.Location = new Point(15, 232);
            parkingbtn.Name = "parkingbtn";
            parkingbtn.Padding = new Padding(20, 0, 0, 0);
            parkingbtn.Size = new Size(210, 45);
            parkingbtn.TabIndex = 1;
            parkingbtn.Text = "     Parking Grid";
            parkingbtn.TextAlign = ContentAlignment.MiddleLeft;
            parkingbtn.TextImageRelation = TextImageRelation.ImageBeforeText;
            parkingbtn.UseVisualStyleBackColor = false;
            parkingbtn.Cursor = Cursors.Hand;
            parkingbtn.Click += parkingbtn_Click;
            // 
            // entryexitbtn
            // 
            entryexitbtn.BackColor = Color.Transparent;
            entryexitbtn.BorderRadius = 12;
            entryexitbtn.FlatAppearance.BorderSize = 0;
            entryexitbtn.FlatStyle = FlatStyle.Flat;
            entryexitbtn.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            entryexitbtn.ForeColor = Color.White;
            entryexitbtn.Image = (Image)resources.GetObject("entryexitbtn.Image");
            entryexitbtn.ImageAlign = ContentAlignment.MiddleLeft;
            entryexitbtn.Location = new Point(15, 284);
            entryexitbtn.Name = "entryexitbtn";
            entryexitbtn.Padding = new Padding(20, 0, 0, 0);
            entryexitbtn.Size = new Size(210, 45);
            entryexitbtn.TabIndex = 2;
            entryexitbtn.Text = "     Entry and Exit";
            entryexitbtn.TextAlign = ContentAlignment.MiddleLeft;
            entryexitbtn.TextImageRelation = TextImageRelation.ImageBeforeText;
            entryexitbtn.UseVisualStyleBackColor = false;
            entryexitbtn.Cursor = Cursors.Hand;
            entryexitbtn.Click += entryexitbtn_Click;
            // 
            // billingbtn
            // 
            billingbtn.BackColor = Color.Transparent;
            billingbtn.BorderRadius = 12;
            billingbtn.FlatAppearance.BorderSize = 0;
            billingbtn.FlatStyle = FlatStyle.Flat;
            billingbtn.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            billingbtn.ForeColor = Color.White;
            billingbtn.Image = (Image)resources.GetObject("billingbtn.Image");
            billingbtn.ImageAlign = ContentAlignment.MiddleLeft;
            billingbtn.Location = new Point(15, 336);
            billingbtn.Name = "billingbtn";
            billingbtn.Padding = new Padding(20, 0, 0, 0);
            billingbtn.Size = new Size(210, 45);
            billingbtn.TabIndex = 3;
            billingbtn.Text = "     Billing";
            billingbtn.TextAlign = ContentAlignment.MiddleLeft;
            billingbtn.TextImageRelation = TextImageRelation.ImageBeforeText;
            billingbtn.UseVisualStyleBackColor = false;
            billingbtn.Cursor = Cursors.Hand;
            billingbtn.Click += billingbtn_Click;
            // 
            // historybtn
            // 
            historybtn.BackColor = Color.Transparent;
            historybtn.BorderRadius = 12;
            historybtn.FlatAppearance.BorderSize = 0;
            historybtn.FlatStyle = FlatStyle.Flat;
            historybtn.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            historybtn.ForeColor = Color.White;
            historybtn.Image = (Image)resources.GetObject("historybtn.Image");
            historybtn.ImageAlign = ContentAlignment.MiddleLeft;
            historybtn.Location = new Point(15, 388);
            historybtn.Name = "historybtn";
            historybtn.Padding = new Padding(20, 0, 0, 0);
            historybtn.Size = new Size(210, 45);
            historybtn.TabIndex = 4;
            historybtn.Text = "     History Logs";
            historybtn.TextAlign = ContentAlignment.MiddleLeft;
            historybtn.TextImageRelation = TextImageRelation.ImageBeforeText;
            historybtn.UseVisualStyleBackColor = false;
            historybtn.Cursor = Cursors.Hand;
            historybtn.Click += historybtn_Click;
            // 
            // trackerbtn
            // 
            trackerbtn.BackColor = Color.Transparent;
            trackerbtn.BorderRadius = 12;
            trackerbtn.FlatAppearance.BorderSize = 0;
            trackerbtn.FlatStyle = FlatStyle.Flat;
            trackerbtn.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            trackerbtn.ForeColor = Color.White;
            trackerbtn.Image = (Image)resources.GetObject("trackerbtn.Image");
            trackerbtn.ImageAlign = ContentAlignment.MiddleLeft;
            trackerbtn.Location = new Point(15, 440);
            trackerbtn.Name = "trackerbtn";
            trackerbtn.Padding = new Padding(20, 0, 0, 0);
            trackerbtn.Size = new Size(210, 45);
            trackerbtn.TabIndex = 5;
            trackerbtn.Text = "     Vehicle Tracker";
            trackerbtn.TextAlign = ContentAlignment.MiddleLeft;
            trackerbtn.TextImageRelation = TextImageRelation.ImageBeforeText;
            trackerbtn.UseVisualStyleBackColor = false;
            trackerbtn.Cursor = Cursors.Hand;
            trackerbtn.Click += trackerbtn_Click;
            // 
            // reservationbtn
            // 
            reservationbtn.BackColor = Color.Transparent;
            reservationbtn.BorderRadius = 12;
            reservationbtn.FlatAppearance.BorderSize = 0;
            reservationbtn.FlatStyle = FlatStyle.Flat;
            reservationbtn.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            reservationbtn.ForeColor = Color.White;
            reservationbtn.Image = (Image)resources.GetObject("reservationbtn.Image");
            reservationbtn.ImageAlign = ContentAlignment.MiddleLeft;
            reservationbtn.Location = new Point(15, 492);
            reservationbtn.Name = "reservationbtn";
            reservationbtn.Padding = new Padding(20, 0, 0, 0);
            reservationbtn.Size = new Size(210, 45);
            reservationbtn.TabIndex = 6;
            reservationbtn.Text = "     Reservations";
            reservationbtn.TextAlign = ContentAlignment.MiddleLeft;
            reservationbtn.TextImageRelation = TextImageRelation.ImageBeforeText;
            reservationbtn.UseVisualStyleBackColor = false;
            reservationbtn.Cursor = Cursors.Hand;
            reservationbtn.Click += reservationbtn_Click;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1248, 642);
            Controls.Add(panelMain);
            Controls.Add(sidebarPanel);
            Name = "Main";
            Text = "ParkWise - Parking Management System";
            StartPosition = FormStartPosition.CenterScreen;
            sidebarPanel.ResumeLayout(false);
            sidebarPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelMain;
        private Panel sidebarPanel;
        private Panel pnlSeparator;
        private RoundedButton reservationbtn;
        private RoundedButton trackerbtn;
        private RoundedButton historybtn;
        private RoundedButton billingbtn;
        private RoundedButton entryexitbtn;
        private RoundedButton parkingbtn;
        private RoundedButton dashboardbtn;
        private PictureBox pictureBox1;
        private Label label1;
        private Label label2;
    }
}