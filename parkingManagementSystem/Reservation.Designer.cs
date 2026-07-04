namespace parkingManagementSystem
{
    partial class Reservations
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
            statsContainer = new TableLayoutPanel();
            formPanel = new Panel();
            tabsPanel = new Panel();
            contentPanel = new Panel();

            SuspendLayout();

            mainContainer.Dock = DockStyle.Fill;
            mainContainer.ColumnCount = 1;
            mainContainer.RowCount = 5;
            mainContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 110F));
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 200F));
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainContainer.Padding = new Padding(30, 20, 30, 20);
            mainContainer.BackColor = Color.FromArgb(247, 250, 252);

            headerPanel.Dock = DockStyle.Fill;
            headerPanel.BackColor = Color.Transparent;
            headerPanel.Controls.Add(lblSubtitle);
            headerPanel.Controls.Add(lblTitle);

            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(40, 42, 58);
            lblTitle.Location = new Point(0, 5);
            lblTitle.Text = "Reservations";

            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.FromArgb(120, 125, 140);
            lblSubtitle.Location = new Point(2, 48);
            lblSubtitle.Text = "Book slots in advance, manage holds, and check-in guests.";

            statsContainer.Dock = DockStyle.Fill;
            statsContainer.ColumnCount = 4;
            statsContainer.RowCount = 1;
            statsContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            statsContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            statsContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            statsContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            statsContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            statsContainer.BackColor = Color.Transparent;
            statsContainer.Margin = new Padding(0, 0, 0, 15);

            formPanel.Dock = DockStyle.Fill;
            formPanel.BackColor = Color.Transparent;
            formPanel.Margin = new Padding(0, 0, 0, 15);

            tabsPanel.Dock = DockStyle.Fill;
            tabsPanel.BackColor = Color.Transparent;

            contentPanel.Dock = DockStyle.Fill;
            contentPanel.BackColor = Color.Transparent;

            mainContainer.Controls.Add(headerPanel, 0, 0);
            mainContainer.Controls.Add(statsContainer, 0, 1);
            mainContainer.Controls.Add(formPanel, 0, 2);
            mainContainer.Controls.Add(tabsPanel, 0, 3);
            mainContainer.Controls.Add(contentPanel, 0, 4);

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(247, 250, 252);
            Controls.Add(mainContainer);
            Name = "Reservations";
            Size = new Size(1264, 823);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel mainContainer;
        private Panel headerPanel;
        private Label lblTitle, lblSubtitle;
        private TableLayoutPanel statsContainer;
        private Panel formPanel;
        private Panel tabsPanel;
        private Panel contentPanel;

        internal Label lblActiveCount, lblExpiringSoon, lblCheckedInToday, lblExpiredCount;
        internal TextBox txtRegNum, txtOwnerName, txtContact;
        internal ComboBox cmbCategory;
        internal DateTimePicker dtpReservation;
        internal NumericUpDown numDuration;
        internal RoundedButton btnCreate;
        internal RoundedButton tabBtnActive, tabBtnExpired, tabBtnAll;
        internal RoundedPanel activeCard, expiredCard, allCard;
        internal DataGridView dgvActive, dgvExpired, dgvAll;
        internal RoundedButton btnCheckIn, btnCancel;
        internal Panel bottomBar;
    }
}