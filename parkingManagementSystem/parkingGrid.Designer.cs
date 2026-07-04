namespace parkingManagementSystem
{
    partial class parkingGrid
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
            statsPanel = new Panel();

            legendCard = new RoundedPanel();
            lblLegendTitle = new Label();
            legendContainer = new Panel();

            scrollPanel = new Panel();
            sectionsContainer = new TableLayoutPanel();

            SuspendLayout();

            // mainContainer
            mainContainer.Dock = DockStyle.Fill;
            mainContainer.ColumnCount = 1;
            mainContainer.RowCount = 3;
            mainContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainContainer.Padding = new Padding(30, 20, 30, 20);
            mainContainer.BackColor = Color.FromArgb(247, 250, 252);

            // headerPanel
            headerPanel.Dock = DockStyle.Fill;
            headerPanel.BackColor = Color.Transparent;
            headerPanel.Controls.Add(statsPanel);
            headerPanel.Controls.Add(lblSubtitle);
            headerPanel.Controls.Add(lblTitle);

            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(40, 42, 58);
            lblTitle.Location = new Point(0, 5);
            lblTitle.Text = "Parking Grid";

            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.FromArgb(120, 125, 140);
            lblSubtitle.Location = new Point(2, 48);
            lblSubtitle.Text = "Click a slot to manage its status.";

            statsPanel.AutoSize = true;
            statsPanel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            statsPanel.BackColor = Color.Transparent;

            // legendCard
            legendCard.Dock = DockStyle.Fill;
            legendCard.BackColor = Color.White;
            legendCard.BorderRadius = 12;
            legendCard.Margin = new Padding(0, 0, 0, 15);
            legendCard.Controls.Add(legendContainer);
            legendCard.Controls.Add(lblLegendTitle);

            lblLegendTitle.AutoSize = true;
            lblLegendTitle.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblLegendTitle.ForeColor = Color.FromArgb(40, 42, 58);
            lblLegendTitle.Location = new Point(20, 18);
            lblLegendTitle.Text = "Status:";

            legendContainer.Location = new Point(80, 8);
            legendContainer.Size = new Size(600, 40);
            legendContainer.BackColor = Color.Transparent;

            // scrollPanel
            scrollPanel.Dock = DockStyle.Fill;
            scrollPanel.BackColor = Color.Transparent;
            scrollPanel.AutoScroll = true;
            scrollPanel.Padding = new Padding(0, 5, 15, 5);
            scrollPanel.Controls.Add(sectionsContainer);

            // sectionsContainer
            sectionsContainer.Dock = DockStyle.Top;
            sectionsContainer.AutoSize = true;
            sectionsContainer.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            sectionsContainer.ColumnCount = 1;
            sectionsContainer.RowCount = 4;
            sectionsContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            sectionsContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            sectionsContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            sectionsContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            sectionsContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            sectionsContainer.BackColor = Color.Transparent;

            mainContainer.Controls.Add(headerPanel, 0, 0);
            mainContainer.Controls.Add(legendCard, 0, 1);
            mainContainer.Controls.Add(scrollPanel, 0, 2);

            // parkingGrid
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(247, 250, 252);
            Controls.Add(mainContainer);
            Name = "parkingGrid";
            Size = new Size(1222, 800);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel mainContainer;
        private Panel headerPanel;
        private Label lblTitle, lblSubtitle;
        private Panel statsPanel;

        private RoundedPanel legendCard;
        private Label lblLegendTitle;
        private Panel legendContainer;

        private Panel scrollPanel;
        private TableLayoutPanel sectionsContainer;

        // Category cards built dynamically in parkingGrid.cs
        internal TableLayoutPanel tlpMotor, tlpCompact, tlpSedan, tlpSUV;
    }
}