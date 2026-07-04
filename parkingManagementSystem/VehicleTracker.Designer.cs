namespace parkingManagementSystem
{
    partial class VehicleTracker
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
            searchPanel = new Panel();
            contentPanel = new Panel();

            SuspendLayout();

            // mainContainer
            mainContainer.Dock = DockStyle.Fill;
            mainContainer.ColumnCount = 1;
            mainContainer.RowCount = 4;
            mainContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 110F));
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
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
            lblTitle.Text = "Vehicle Tracker";

            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.FromArgb(120, 125, 140);
            lblSubtitle.Location = new Point(2, 48);
            lblSubtitle.Text = "Real-time location and status of all active vehicles.";

            // statsContainer
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

            // searchPanel
            searchPanel.Dock = DockStyle.Fill;
            searchPanel.BackColor = Color.Transparent;
            searchPanel.Margin = new Padding(0, 0, 0, 15);

            // contentPanel
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.BackColor = Color.Transparent;

            mainContainer.Controls.Add(headerPanel, 0, 0);
            mainContainer.Controls.Add(statsContainer, 0, 1);
            mainContainer.Controls.Add(searchPanel, 0, 2);
            mainContainer.Controls.Add(contentPanel, 0, 3);

            // VehicleTracker
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(247, 250, 252);
            Controls.Add(mainContainer);
            Name = "VehicleTracker";
            Size = new Size(1264, 823);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel mainContainer;
        private Panel headerPanel;
        private Label lblTitle, lblSubtitle;
        private TableLayoutPanel statsContainer;
        private Panel searchPanel;
        private Panel contentPanel;

        // Built dynamically
        internal Label lblActiveCount, lblMotorCount, lblCarCount, lblLongestStay;
        internal TextBox txtSearch;
        internal RoundedButton tabBtnList, tabBtnMap;
        internal RoundedPanel listCard, mapCard;
        internal DataGridView dgvVehicles;
        internal Panel mapScrollPanel;
    }
}