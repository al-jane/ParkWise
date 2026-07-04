namespace parkingManagementSystem
{
    partial class DashBoard
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
            lblDateTime = new Label();

            welcomeCard = new RoundedPanel();
            lblWelcomeTitle = new Label();
            lblWelcomeDesc = new Label();

            statsContainer = new TableLayoutPanel();
            vehicleSection = new TableLayoutPanel();
            lblVehicleSection = new Label();
            vehicleContainer = new TableLayoutPanel();

            bottomContainer = new TableLayoutPanel();
            occupancyCard = new RoundedPanel();
            lblOccupancyTitle = new Label();
            donutChart = new DonutChart();
            legendPanel = new Panel();

            activityCard = new RoundedPanel();
            lblActivityTitle = new Label();
            activityListPanel = new Panel();

            SuspendLayout();

            // 
            // mainContainer
            // 
            mainContainer.Dock = DockStyle.Fill;
            mainContainer.ColumnCount = 1;
            mainContainer.RowCount = 5;
            mainContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 90F));
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 130F));
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 180F));
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainContainer.Padding = new Padding(30, 20, 30, 20);
            mainContainer.BackColor = Color.FromArgb(247, 250, 252);

            // 
            // headerPanel
            // 
            headerPanel.Dock = DockStyle.Fill;
            headerPanel.BackColor = Color.Transparent;
            headerPanel.Controls.Add(lblDateTime);
            headerPanel.Controls.Add(lblSubtitle);
            headerPanel.Controls.Add(lblTitle);

            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(40, 42, 58);
            lblTitle.Location = new Point(0, 5);
            lblTitle.Text = "Dashboard";

            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.FromArgb(120, 125, 140);
            lblSubtitle.Location = new Point(2, 48);
            lblSubtitle.Text = "Welcome back! Here's your parking overview.";

            lblDateTime.AutoSize = true;
            lblDateTime.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblDateTime.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblDateTime.ForeColor = Color.FromArgb(236, 64, 122);
            lblDateTime.Text = "";

            // 
            // welcomeCard
            // 
            welcomeCard.Dock = DockStyle.Fill;
            welcomeCard.BackColor = Color.FromArgb(40, 42, 58);
            welcomeCard.BorderRadius = 15;
            welcomeCard.Margin = new Padding(0, 0, 0, 15);
            welcomeCard.Controls.Add(lblWelcomeDesc);
            welcomeCard.Controls.Add(lblWelcomeTitle);

            lblWelcomeTitle.AutoSize = true;
            lblWelcomeTitle.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            lblWelcomeTitle.ForeColor = Color.White;
            lblWelcomeTitle.Location = new Point(25, 15);
            lblWelcomeTitle.Text = "👋  Welcome to ParkWise";

            lblWelcomeDesc.AutoSize = true;
            lblWelcomeDesc.Font = new Font("Segoe UI", 9.5F);
            lblWelcomeDesc.ForeColor = Color.FromArgb(200, 205, 220);
            lblWelcomeDesc.Location = new Point(27, 46);
            lblWelcomeDesc.Text = "Manage your mall parking efficiently — track slots, process entries, handle billing, and more.";

            // 
            // statsContainer
            // 
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

            // 
            // vehicleSection
            // 
            vehicleSection.Dock = DockStyle.Fill;
            vehicleSection.ColumnCount = 1;
            vehicleSection.RowCount = 2;
            vehicleSection.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            vehicleSection.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            vehicleSection.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            vehicleSection.BackColor = Color.Transparent;
            vehicleSection.Margin = new Padding(0);

            lblVehicleSection.AutoSize = true;
            lblVehicleSection.Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold);
            lblVehicleSection.ForeColor = Color.FromArgb(40, 42, 58);
            lblVehicleSection.Text = "Vehicle Type Availability";
            lblVehicleSection.Margin = new Padding(2, 0, 0, 5);

            vehicleContainer.Dock = DockStyle.Fill;
            vehicleContainer.ColumnCount = 4;
            vehicleContainer.RowCount = 1;
            vehicleContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            vehicleContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            vehicleContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            vehicleContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            vehicleContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            vehicleContainer.BackColor = Color.Transparent;
            vehicleContainer.Margin = new Padding(0);

            vehicleSection.Controls.Add(lblVehicleSection, 0, 0);
            vehicleSection.Controls.Add(vehicleContainer, 0, 1);

            // 
            // bottomContainer
            // 
            bottomContainer.Dock = DockStyle.Fill;
            bottomContainer.ColumnCount = 2;
            bottomContainer.RowCount = 1;
            bottomContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            bottomContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            bottomContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            bottomContainer.BackColor = Color.Transparent;
            bottomContainer.Margin = new Padding(0, 10, 0, 0);

            // 
            // occupancyCard
            // 
            occupancyCard.Dock = DockStyle.Fill;
            occupancyCard.BackColor = Color.White;
            occupancyCard.BorderRadius = 15;
            occupancyCard.Margin = new Padding(0, 0, 10, 0);
            occupancyCard.Controls.Add(legendPanel);
            occupancyCard.Controls.Add(donutChart);
            occupancyCard.Controls.Add(lblOccupancyTitle);

            lblOccupancyTitle.AutoSize = true;
            lblOccupancyTitle.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            lblOccupancyTitle.ForeColor = Color.FromArgb(40, 42, 58);
            lblOccupancyTitle.Location = new Point(25, 20);
            lblOccupancyTitle.Text = "Overall Occupancy";

            donutChart.Location = new Point(20, 55);
            donutChart.Size = new Size(280, 280);
            donutChart.BackColor = Color.White;
            donutChart.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;

            legendPanel.Location = new Point(310, 150);
            legendPanel.Size = new Size(140, 100);
            legendPanel.BackColor = Color.Transparent;
            legendPanel.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // 
            // activityCard
            // 
            activityCard.Dock = DockStyle.Fill;
            activityCard.BackColor = Color.White;
            activityCard.BorderRadius = 15;
            activityCard.Margin = new Padding(10, 0, 0, 0);
            activityCard.Controls.Add(activityListPanel);
            activityCard.Controls.Add(lblActivityTitle);

            lblActivityTitle.AutoSize = true;
            lblActivityTitle.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            lblActivityTitle.ForeColor = Color.FromArgb(40, 42, 58);
            lblActivityTitle.Location = new Point(25, 20);
            lblActivityTitle.Text = "Recent Activity";

            activityListPanel.Location = new Point(20, 55);
            activityListPanel.Size = new Size(500, 250);
            activityListPanel.BackColor = Color.Transparent;
            activityListPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            activityListPanel.AutoScroll = true;
            activityListPanel.Padding = new Padding(0, 0, 10, 0);

            bottomContainer.Controls.Add(occupancyCard, 0, 0);
            bottomContainer.Controls.Add(activityCard, 1, 0);

            mainContainer.Controls.Add(headerPanel, 0, 0);
            mainContainer.Controls.Add(welcomeCard, 0, 1);
            mainContainer.Controls.Add(statsContainer, 0, 2);
            mainContainer.Controls.Add(vehicleSection, 0, 3);
            mainContainer.Controls.Add(bottomContainer, 0, 4);

            // 
            // DashBoard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(247, 250, 252);
            Controls.Add(mainContainer);
            Name = "DashBoard";
            Size = new Size(1008, 681);
            Load += DashBoard_Load;
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel mainContainer;
        private Panel headerPanel;
        private Label lblTitle, lblSubtitle, lblDateTime;

        private RoundedPanel welcomeCard;
        private Label lblWelcomeTitle, lblWelcomeDesc;

        private TableLayoutPanel statsContainer;
        private TableLayoutPanel vehicleSection;
        private Label lblVehicleSection;
        private TableLayoutPanel vehicleContainer;

        private TableLayoutPanel bottomContainer;
        private RoundedPanel occupancyCard;
        private Label lblOccupancyTitle;
        private DonutChart donutChart;
        private Panel legendPanel;

        private RoundedPanel activityCard;
        private Label lblActivityTitle;
        private Panel activityListPanel;

        // Dynamic labels (populated in DashBoard.cs)
        internal Label lblTotalValue, lblAvailableValue, lblOccupiedValue, lblRateValue;
        internal Label lblMotorValue, lblCompactValue, lblSedanValue, lblSUVValue;
    }
}