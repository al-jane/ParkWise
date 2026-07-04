namespace parkingManagementSystem
{
    partial class parkBilling
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
            components = new System.ComponentModel.Container();
            mainContainer = new TableLayoutPanel();
            headerPanel = new Panel();
            lblTitle = new Label();
            lblSubtitle = new Label();
            statsContainer = new TableLayoutPanel();
            tabPanel = new Panel();
            contentPanel = new Panel();
            billTimer = new System.Windows.Forms.Timer(components);

            SuspendLayout();

            // mainContainer
            mainContainer.Dock = DockStyle.Fill;
            mainContainer.ColumnCount = 1;
            mainContainer.RowCount = 4;
            mainContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 110F));
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
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
            lblTitle.Text = "Park Billing";

            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.FromArgb(120, 125, 140);
            lblSubtitle.Location = new Point(2, 48);
            lblSubtitle.Text = "Track active bills, payments, and outstanding balances.";

            // statsContainer
            statsContainer.Dock = DockStyle.Fill;
            statsContainer.ColumnCount = 4;
            statsContainer.RowCount = 1;
            statsContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28F));
            statsContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28F));
            statsContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28F));
            statsContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));
            statsContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            statsContainer.BackColor = Color.Transparent;
            statsContainer.Margin = new Padding(0, 0, 0, 15);

            // tabPanel
            tabPanel.Dock = DockStyle.Fill;
            tabPanel.BackColor = Color.Transparent;

            // contentPanel
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.BackColor = Color.Transparent;

            // timer
            billTimer.Enabled = false;
            billTimer.Interval = 1000;

            mainContainer.Controls.Add(headerPanel, 0, 0);
            mainContainer.Controls.Add(statsContainer, 0, 1);
            mainContainer.Controls.Add(tabPanel, 0, 2);
            mainContainer.Controls.Add(contentPanel, 0, 3);

            // parkBilling
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(247, 250, 252);
            Controls.Add(mainContainer);
            Name = "parkBilling";
            Size = new Size(1322, 711);
            Load += parkBilling_Load;
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel mainContainer;
        private Panel headerPanel;
        private Label lblTitle, lblSubtitle;
        private TableLayoutPanel statsContainer;
        private Panel tabPanel;
        private Panel contentPanel;
        private System.Windows.Forms.Timer billTimer;

        // Dynamic — created in code
        internal Label billLblActive, billLblUnpaid, billLblRevenue;
        internal RoundedButton billBtnRefresh;
        internal RoundedButton tabBtnAll, tabBtnPaid, tabBtnUnpaid;
        internal RoundedPanel allCard, paidCard, unpaidCard;
        internal DataGridView billDgvAll, billDgvPaid, billDgvUnpaid;
        internal RoundedButton billBtnMarkPaid;
    }
}