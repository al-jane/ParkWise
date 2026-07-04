namespace parkingManagementSystem
{
    partial class EntryExit
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

        #region Component Designer generated code

        private void InitializeComponent()
        {
            mainContainer = new TableLayoutPanel();
            headerPanel = new Panel();
            lblTitle = new Label();
            lblSubtitle = new Label();
            tabPanel = new Panel();
            vehicleEntryBtn = new RoundedButton();
            vehicleExitBtn = new RoundedButton();
            contentPanel = new Panel();
            entryPanel = new Panel();
            exitPanel = new Panel();

            SuspendLayout();

            // mainContainer
            mainContainer.Dock = DockStyle.Fill;
            mainContainer.ColumnCount = 1;
            mainContainer.RowCount = 3;
            mainContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainContainer.Padding = new Padding(30, 20, 30, 20);
            mainContainer.BackColor = Color.FromArgb(247, 250, 252);

            // headerPanel
            headerPanel.Dock = DockStyle.Fill;
            headerPanel.BackColor = Color.Transparent;
            headerPanel.Controls.Add(lblSubtitle);
            headerPanel.Controls.Add(lblTitle);

            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(40, 42, 58);
            lblTitle.Location = new Point(0, 5);
            lblTitle.Text = "Entry & Exit";

            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.FromArgb(120, 125, 140);
            lblSubtitle.Location = new Point(2, 48);
            lblSubtitle.Text = "Register new entries or process vehicle exits.";

            // tabPanel
            tabPanel.Dock = DockStyle.Fill;
            tabPanel.BackColor = Color.Transparent;

            vehicleEntryBtn.BorderRadius = 10;
            vehicleEntryBtn.FlatStyle = FlatStyle.Flat;
            vehicleEntryBtn.FlatAppearance.BorderSize = 0;
            vehicleEntryBtn.Location = new Point(0, 5);
            vehicleEntryBtn.Size = new Size(180, 45);
            vehicleEntryBtn.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            vehicleEntryBtn.Text = "  ➕  Vehicle Entry";
            vehicleEntryBtn.BackColor = Color.FromArgb(236, 64, 122);
            vehicleEntryBtn.ForeColor = Color.White;
            vehicleEntryBtn.Cursor = Cursors.Hand;
            vehicleEntryBtn.Click += entryBtn;

            vehicleExitBtn.BorderRadius = 10;
            vehicleExitBtn.FlatStyle = FlatStyle.Flat;
            vehicleExitBtn.FlatAppearance.BorderSize = 0;
            vehicleExitBtn.Location = new Point(190, 5);
            vehicleExitBtn.Size = new Size(180, 45);
            vehicleExitBtn.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            vehicleExitBtn.Text = "  🚗  Vehicle Exit";
            vehicleExitBtn.BackColor = Color.White;
            vehicleExitBtn.ForeColor = Color.FromArgb(40, 42, 58);
            vehicleExitBtn.Cursor = Cursors.Hand;
            vehicleExitBtn.Click += exitBtn;

            tabPanel.Controls.Add(vehicleEntryBtn);
            tabPanel.Controls.Add(vehicleExitBtn);

            // contentPanel
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.BackColor = Color.Transparent;
            contentPanel.Controls.Add(exitPanel);
            contentPanel.Controls.Add(entryPanel);

            entryPanel.Dock = DockStyle.Fill;
            entryPanel.BackColor = Color.Transparent;

            exitPanel.Dock = DockStyle.Fill;
            exitPanel.BackColor = Color.Transparent;
            exitPanel.Visible = false;

            mainContainer.Controls.Add(headerPanel, 0, 0);
            mainContainer.Controls.Add(tabPanel, 0, 1);
            mainContainer.Controls.Add(contentPanel, 0, 2);

            // EntryExit
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(247, 250, 252);
            Controls.Add(mainContainer);
            Name = "EntryExit";
            Size = new Size(1264, 823);
            Load += EntryExit_Load;
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel mainContainer;
        private Panel headerPanel;
        private Label lblTitle, lblSubtitle;

        private Panel tabPanel;
        private RoundedButton vehicleEntryBtn;
        private RoundedButton vehicleExitBtn;

        private Panel contentPanel;

        internal Panel entryPanel;
        internal Panel exitPanel;

        // Entry form controls
        internal TextBox txtRegNum, txtCompany, txtOwnerName, txtContact;
        internal ComboBox cmbCategory;
        internal RoundedButton btnRegister;
        internal DataGridView dgvParked;

        // Exit controls
        internal TextBox txtSearchExit;
        internal RoundedButton btnSearchExit, btnProcessExit;
        internal DataGridView dgvSearchResults;
    }
}