using System;
using System.Drawing;
using System.Windows.Forms;

namespace parkingManagementSystem
{
    public partial class Main : Form
    {
        private parkingGrid pgControl;
        private EntryExit eeControl;
        private parkBilling pbControl;
        private HistoryLogs hlControl;
        private VehicleTracker vtControl;
        private Reservations rsControl;

        private RoundedButton currentButton;
        private readonly Color activeBgColor = Color.FromArgb(236, 64, 122);
        private readonly Color activeTextColor = Color.White;
        private readonly Color hoverBgColor = Color.FromArgb(55, 57, 75);
        private readonly Color defaultBgColor = Color.Transparent;
        private readonly Color defaultTextColor = Color.White;

        public Main()
        {
            InitializeComponent();

            pgControl = new parkingGrid();
            eeControl = new EntryExit();
            pbControl = new parkBilling();
            hlControl = new HistoryLogs();
            vtControl = new VehicleTracker();
            rsControl = new Reservations();

            eeControl.SetParkingGrid(pgControl);
            eeControl.SetParkBilling(pbControl);
            rsControl.SetParkingGrid(pgControl);

            AttachHoverEffects(dashboardbtn);
            AttachHoverEffects(parkingbtn);
            AttachHoverEffects(entryexitbtn);
            AttachHoverEffects(billingbtn);
            AttachHoverEffects(historybtn);
            AttachHoverEffects(trackerbtn);
            AttachHoverEffects(reservationbtn);

            this.Load += Main_Load;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            ActivateButton(dashboardbtn);
            ShowDashboard();
        }

        private void AttachHoverEffects(RoundedButton btn)
        {
            btn.MouseEnter += (s, e) =>
            {
                if (btn != currentButton) { btn.BackColor = hoverBgColor; btn.Invalidate(); }
            };
            btn.MouseLeave += (s, e) =>
            {
                if (btn != currentButton) { btn.BackColor = defaultBgColor; btn.Invalidate(); }
            };
        }

        private void ActivateButton(RoundedButton btn)
        {
            if (currentButton != null)
            {
                currentButton.BackColor = defaultBgColor;
                currentButton.ForeColor = defaultTextColor;
                currentButton.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
                currentButton.Invalidate();
            }

            currentButton = btn;
            currentButton.BackColor = activeBgColor;
            currentButton.ForeColor = activeTextColor;
            currentButton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            currentButton.Invalidate();
        }

        private void ShowControl(UserControl uc)
        {
            panelMain.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            panelMain.Controls.Add(uc);
        }

        private void ShowDashboard()
        {
            panelMain.Controls.Clear();
            DashBoard dashboard = new DashBoard();
            dashboard.Dock = DockStyle.Fill;
            panelMain.Controls.Add(dashboard);
        }

        private void dashboardbtn_Click(object sender, EventArgs e)
        {
            ActivateButton((RoundedButton)sender);
            ShowDashboard();
        }

        private void parkingbtn_Click(object sender, EventArgs e)
        {
            ActivateButton((RoundedButton)sender);
            ShowControl(pgControl);
            pgControl.RefreshSlots();
        }

        private void entryexitbtn_Click(object sender, EventArgs e)
        {
            ActivateButton((RoundedButton)sender);
            ShowControl(eeControl);
        }

        private void billingbtn_Click(object sender, EventArgs e)
        {
            ActivateButton((RoundedButton)sender);
            ShowControl(pbControl);
            pbControl.RefreshAll();
        }

        private void historybtn_Click(object sender, EventArgs e)
        {
            ActivateButton((RoundedButton)sender);
            ShowControl(hlControl);
            hlControl.RefreshAll();
        }

        private void trackerbtn_Click(object sender, EventArgs e)
        {
            ActivateButton((RoundedButton)sender);
            ShowControl(vtControl);
            vtControl.RefreshAll();
        }

        private void reservationbtn_Click(object sender, EventArgs e)
        {
            ActivateButton((RoundedButton)sender);
            ShowControl(rsControl);
            rsControl.RefreshAll();
        }
    }
}