using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace parkingManagementSystem
{
    public partial class parkBilling : UserControl
    {
        private string connStr = "Server=localhost;Database=parking_db;Uid=root;Pwd=;";

        // Theme colors
        private readonly Color CardBg = Color.White;
        private readonly Color TextPrimary = Color.FromArgb(40, 42, 58);
        private readonly Color TextSecondary = Color.FromArgb(120, 125, 140);
        private readonly Color PinkAccent = Color.FromArgb(236, 64, 122);
        private readonly Color GreenAccent = Color.FromArgb(34, 197, 94);
        private readonly Color RedAccent = Color.FromArgb(239, 68, 68);
        private readonly Color BlueAccent = Color.FromArgb(59, 130, 246);
        private readonly Color AmberAccent = Color.FromArgb(245, 158, 11);
        private readonly Color InactiveTab = Color.White;
        private readonly Color InactiveTabText = Color.FromArgb(80, 85, 100);

        // Status row backgrounds (subtle)
        private readonly Color ActiveRowBg = Color.FromArgb(223, 242, 254);   // Light blue
        private readonly Color PaidRowBg = Color.FromArgb(220, 252, 231);     // Light green
        private readonly Color UnpaidRowBg = Color.FromArgb(254, 226, 226);   // Light red

        private int currentTab = 0;

        public parkBilling()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            BuildStatsCards();
            BuildTabs();
            BuildContent();

            this.Load += parkBilling_Load;
        }

        private void parkBilling_Load(object sender, EventArgs e)
        {
            RefreshAll();
            ShowTab(0);

            billTimer.Interval = 1000;
            billTimer.Tick += BillTimer_Tick;
            billTimer.Start();
        }

        // ===== BUILD STAT CARDS =====
        private void BuildStatsCards()
        {
            statsContainer.Controls.Add(
                CreateStatCard(BlueAccent, "🚗", "Active Vehicles", "0", out billLblActive, 0), 0, 0);
            statsContainer.Controls.Add(
                CreateStatCard(RedAccent, "⚠", "Unpaid Bills", "0", out billLblUnpaid, 1), 1, 0);
            statsContainer.Controls.Add(
                CreateStatCard(GreenAccent, "💰", "Today's Revenue", "₱0.00", out billLblRevenue, 2), 2, 0);

            // Refresh button in 4th slot
            RoundedPanel refreshHolder = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                BorderRadius = 0,
                Margin = new Padding(10, 0, 0, 0)
            };

            billBtnRefresh = new RoundedButton
            {
                Text = "🔄  Refresh",
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                BackColor = PinkAccent,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                BorderRadius = 10,
                Size = new Size(150, 45),
                Location = new Point(0, 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Cursor = Cursors.Hand
            };
            billBtnRefresh.FlatAppearance.BorderSize = 0;
            billBtnRefresh.Click += btnRefresh_Click;

            refreshHolder.Controls.Add(billBtnRefresh);
            refreshHolder.Resize += (s, e) =>
            {
                billBtnRefresh.Location = new Point(refreshHolder.Width - 160, 30);
            };

            statsContainer.Controls.Add(refreshHolder, 3, 0);
        }

        private RoundedPanel CreateStatCard(Color accent, string icon, string title, string value, out Label valueLabel, int index)
        {
            RoundedPanel card = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                BackColor = CardBg,
                BorderRadius = 12,
                Margin = new Padding(index == 0 ? 0 : 5, 0, 5, 0)
            };

            RoundedPanel iconPnl = new RoundedPanel
            {
                BackColor = accent,
                BorderRadius = 10,
                Location = new Point(15, 20),
                Size = new Size(45, 45)
            };

            Label iconLbl = new Label
            {
                Text = icon,
                Font = new Font("Segoe UI Emoji", 16F),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            iconPnl.Controls.Add(iconLbl);

            Label titleLbl = new Label
            {
                AutoSize = true,
                Text = title,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = TextSecondary,
                Location = new Point(72, 20)
            };

            valueLabel = new Label
            {
                AutoSize = true,
                Text = value,
                Font = new Font("Segoe UI Semibold", 17F, FontStyle.Bold),
                ForeColor = TextPrimary,
                Location = new Point(72, 40)
            };

            card.Controls.Add(iconPnl);
            card.Controls.Add(titleLbl);
            card.Controls.Add(valueLabel);

            return card;
        }

        // ===== BUILD TABS =====
        private void BuildTabs()
        {
            tabBtnAll = CreateTabButton("📊  All Vehicles", 0);
            tabBtnPaid = CreateTabButton("✅  Paid Bills", 175);
            tabBtnUnpaid = CreateTabButton("⚠  Unpaid Bills", 350);

            tabBtnAll.Click += (s, e) => ShowTab(0);
            tabBtnPaid.Click += (s, e) => ShowTab(1);
            tabBtnUnpaid.Click += (s, e) => ShowTab(2);

            tabPanel.Controls.Add(tabBtnAll);
            tabPanel.Controls.Add(tabBtnPaid);
            tabPanel.Controls.Add(tabBtnUnpaid);
        }

        private RoundedButton CreateTabButton(string text, int x)
        {
            return new RoundedButton
            {
                Text = text,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                BackColor = InactiveTab,
                ForeColor = InactiveTabText,
                FlatStyle = FlatStyle.Flat,
                BorderRadius = 10,
                Size = new Size(165, 40),
                Location = new Point(x, 5),
                Cursor = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 }
            };
        }

        private void ShowTab(int index)
        {
            currentTab = index;

            // Reset all tabs
            tabBtnAll.BackColor = InactiveTab;
            tabBtnAll.ForeColor = InactiveTabText;
            tabBtnPaid.BackColor = InactiveTab;
            tabBtnPaid.ForeColor = InactiveTabText;
            tabBtnUnpaid.BackColor = InactiveTab;
            tabBtnUnpaid.ForeColor = InactiveTabText;

            // Highlight selected
            switch (index)
            {
                case 0:
                    tabBtnAll.BackColor = PinkAccent;
                    tabBtnAll.ForeColor = Color.White;
                    allCard.Visible = true;
                    paidCard.Visible = false;
                    unpaidCard.Visible = false;
                    break;
                case 1:
                    tabBtnPaid.BackColor = GreenAccent;
                    tabBtnPaid.ForeColor = Color.White;
                    allCard.Visible = false;
                    paidCard.Visible = true;
                    unpaidCard.Visible = false;
                    break;
                case 2:
                    tabBtnUnpaid.BackColor = RedAccent;
                    tabBtnUnpaid.ForeColor = Color.White;
                    allCard.Visible = false;
                    paidCard.Visible = false;
                    unpaidCard.Visible = true;
                    break;
            }

            // Show/hide Mark Paid button based on tab
            billBtnMarkPaid.Visible = (index == 0 || index == 2);
        }

        // ===== BUILD CONTENT CARDS =====
        private void BuildContent()
        {
            // Container for cards (top) + button bar (bottom)
            Panel cardsHolder = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };

            Panel buttonBar = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 70,
                BackColor = Color.Transparent
            };

            // ===== ALL VEHICLES CARD =====
            allCard = BuildTableCard(out billDgvAll);
            AddColumns(billDgvAll, new[] {
        ("Registration #", 14), ("Owner", 15), ("Category", 11), ("Slot", 7),
        ("Entry Time", 15), ("Duration", 10), ("Current Bill", 12), ("Status", 16)
    });

            // ===== PAID BILLS CARD =====
            paidCard = BuildTableCard(out billDgvPaid);
            AddColumns(billDgvPaid, new[] {
        ("Registration #", 12), ("Owner", 14), ("Category", 10), ("Slot", 7),
        ("Entry Time", 14), ("Exit Time", 14), ("Duration", 10), ("Bill Amount", 12)
    });

            // ===== UNPAID BILLS CARD =====
            unpaidCard = BuildTableCard(out billDgvUnpaid);
            AddColumns(billDgvUnpaid, new[] {
        ("Registration #", 12), ("Owner", 14), ("Category", 10), ("Slot", 7),
        ("Entry Time", 14), ("Exit Time", 14), ("Duration", 10), ("Bill Amount", 12)
    });

            paidCard.Visible = false;
            unpaidCard.Visible = false;

            // Add cards to cardsHolder
            cardsHolder.Controls.Add(allCard);
            cardsHolder.Controls.Add(paidCard);
            cardsHolder.Controls.Add(unpaidCard);

            // ===== MARK AS PAID BUTTON =====
            billBtnMarkPaid = new RoundedButton
            {
                Text = "✓  Mark Selected as Paid",
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                BackColor = GreenAccent,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                BorderRadius = 10,
                Size = new Size(270, 45),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            billBtnMarkPaid.FlatAppearance.BorderSize = 0;
            billBtnMarkPaid.Click += btnMarkPaid_Click;

            buttonBar.Controls.Add(billBtnMarkPaid);
            buttonBar.Resize += (s, e) =>
            {
                billBtnMarkPaid.Location = new Point(buttonBar.Width - 280, 12);
            };

            // Add both sections to contentPanel
            contentPanel.Controls.Add(cardsHolder);
            contentPanel.Controls.Add(buttonBar);
        }

        private RoundedPanel BuildTableCard(out DataGridView dgv)
        {
            RoundedPanel card = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                BackColor = CardBg,
                BorderRadius = 15
            };

            dgv = CreateStyledDataGridView();
            dgv.Location = new Point(20, 20);
            dgv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            card.Controls.Add(dgv);

            var capturedDgv = dgv;
            card.Resize += (s, e) =>
            {
                capturedDgv.Size = new Size(card.Width - 40, card.Height - 100);
            };

            return card;
        }

        private void AddColumns(DataGridView dgv, (string header, int weight)[] columns)
        {
            foreach (var (header, weight) in columns)
            {
                dgv.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = header,
                    FillWeight = weight,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });
            }
        }

        private DataGridView CreateStyledDataGridView()
        {
            DataGridView dgv = new DataGridView
            {
                BackgroundColor = CardBg,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                AllowUserToResizeRows = false,
                AllowUserToResizeColumns = false,
                RowHeadersVisible = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                ColumnHeadersHeight = 42,
                Font = new Font("Segoe UI", 10F),
                EnableHeadersVisualStyles = false,
                GridColor = Color.FromArgb(235, 238, 243),
                RowTemplate = { Height = 42 },
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            };

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 251);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = TextPrimary;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(12, 0, 0, 0);

            dgv.DefaultCellStyle.BackColor = CardBg;
            dgv.DefaultCellStyle.ForeColor = TextPrimary;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(252, 231, 243);
            dgv.DefaultCellStyle.SelectionForeColor = PinkAccent;
            dgv.DefaultCellStyle.Padding = new Padding(12, 0, 0, 0);
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            return dgv;
        }

        // ===== TIMER =====
        private void BillTimer_Tick(object sender, EventArgs e)
        {
            UpdateActiveDurations();
        }

        private void UpdateActiveDurations()
        {
            foreach (DataGridViewRow row in billDgvAll.Rows)
            {
                try
                {
                    string status = row.Cells[7].Value?.ToString() ?? "";
                    if (!status.Contains("ACTIVE")) continue;

                    string category = row.Cells[2].Value.ToString();
                    DateTime entryTime = Convert.ToDateTime(row.Cells[4].Value.ToString());
                    DateTime now = DateTime.Now;

                    TimeSpan duration = now - entryTime;
                    decimal currentBill = BillingHelper.CalculateBill(category, entryTime, now);

                    row.Cells[5].Value = BillingHelper.FormatDuration(duration);
                    row.Cells[6].Value = $"₱{currentBill:N2}";
                }
                catch { }
            }
        }

        public void RefreshAll()
        {
            LoadAllVehicles();
            LoadPaidBills();
            LoadUnpaidBills();
            UpdateStats();
        }

        // ===== ALL VEHICLES =====
        private void LoadAllVehicles()
        {
            try
            {
                billDgvAll.Rows.Clear();
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string query = @"SELECT reg_number, owner_name, category, slot_id, 
                                     entry_time, exit_time, duration_minutes, bill_amount,
                                     payment_status, is_active
                                     FROM vehicles 
                                     ORDER BY 
                                        is_active DESC,
                                        CASE WHEN is_active=1 THEN entry_time ELSE exit_time END DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string regNum = reader["reg_number"].ToString();
                            string ownerName = reader["owner_name"].ToString();
                            string category = reader["category"].ToString();
                            string slotId = reader["slot_id"].ToString();
                            DateTime entryTime = Convert.ToDateTime(reader["entry_time"]);
                            bool isActive = Convert.ToInt32(reader["is_active"]) == 1;
                            string paymentStatus = reader["payment_status"].ToString();

                            string durationStr, billStr, statusTag;
                            Color rowBg;

                            if (isActive)
                            {
                                TimeSpan dur = DateTime.Now - entryTime;
                                decimal bill = BillingHelper.CalculateBill(category, entryTime, DateTime.Now);
                                durationStr = BillingHelper.FormatDuration(dur);
                                billStr = $"₱{bill:N2}";
                                statusTag = "🔵 ACTIVE";
                                rowBg = ActiveRowBg;
                            }
                            else
                            {
                                int mins = Convert.ToInt32(reader["duration_minutes"]);
                                decimal bill = Convert.ToDecimal(reader["bill_amount"]);
                                durationStr = BillingHelper.FormatDuration(TimeSpan.FromMinutes(mins));
                                billStr = $"₱{bill:N2}";

                                if (paymentStatus == "paid")
                                {
                                    statusTag = "✅ PAID";
                                    rowBg = PaidRowBg;
                                }
                                else
                                {
                                    statusTag = "❌ UNPAID";
                                    rowBg = UnpaidRowBg;
                                }
                            }

                            int rowIdx = billDgvAll.Rows.Add(
                                regNum, ownerName, category, slotId,
                                entryTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                durationStr, billStr, statusTag);
                            billDgvAll.Rows[rowIdx].DefaultCellStyle.BackColor = rowBg;
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Load All Error: " + ex.Message); }
        }

        // ===== PAID BILLS =====
        private void LoadPaidBills()
        {
            try
            {
                billDgvPaid.Rows.Clear();
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string query = @"SELECT reg_number, owner_name, category, slot_id, 
                                     entry_time, exit_time, duration_minutes, bill_amount
                                     FROM vehicles 
                                     WHERE payment_status='paid' AND is_active=0
                                     ORDER BY exit_time DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int mins = Convert.ToInt32(reader["duration_minutes"]);
                            TimeSpan dur = TimeSpan.FromMinutes(mins);

                            int rowIdx = billDgvPaid.Rows.Add(
                                reader["reg_number"].ToString(),
                                reader["owner_name"].ToString(),
                                reader["category"].ToString(),
                                reader["slot_id"].ToString(),
                                Convert.ToDateTime(reader["entry_time"]).ToString("MMM dd, yyyy HH:mm"),
                                Convert.ToDateTime(reader["exit_time"]).ToString("MMM dd, yyyy HH:mm"),
                                BillingHelper.FormatDuration(dur),
                                $"₱{Convert.ToDecimal(reader["bill_amount"]):N2}");
                            billDgvPaid.Rows[rowIdx].DefaultCellStyle.BackColor = PaidRowBg;
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Load Paid Error: " + ex.Message); }
        }

        // ===== UNPAID BILLS =====
        private void LoadUnpaidBills()
        {
            try
            {
                billDgvUnpaid.Rows.Clear();
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string query = @"SELECT reg_number, owner_name, category, slot_id, 
                                     entry_time, exit_time, duration_minutes, bill_amount
                                     FROM vehicles 
                                     WHERE payment_status='unpaid' AND is_active=0
                                     ORDER BY exit_time DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int mins = Convert.ToInt32(reader["duration_minutes"]);
                            TimeSpan dur = TimeSpan.FromMinutes(mins);

                            int rowIdx = billDgvUnpaid.Rows.Add(
                                reader["reg_number"].ToString(),
                                reader["owner_name"].ToString(),
                                reader["category"].ToString(),
                                reader["slot_id"].ToString(),
                                Convert.ToDateTime(reader["entry_time"]).ToString("MMM dd, yyyy HH:mm"),
                                Convert.ToDateTime(reader["exit_time"]).ToString("MMM dd, yyyy HH:mm"),
                                BillingHelper.FormatDuration(dur),
                                $"₱{Convert.ToDecimal(reader["bill_amount"]):N2}");
                            billDgvUnpaid.Rows[rowIdx].DefaultCellStyle.BackColor = UnpaidRowBg;
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Load Unpaid Error: " + ex.Message); }
        }

        // ===== STATS =====
        private void UpdateStats()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM vehicles WHERE is_active=1", conn))
                        billLblActive.Text = cmd.ExecuteScalar().ToString();

                    using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM vehicles WHERE payment_status='unpaid' AND is_active=0", conn))
                        billLblUnpaid.Text = cmd.ExecuteScalar().ToString();

                    using (MySqlCommand cmd = new MySqlCommand(
                        @"SELECT COALESCE(SUM(bill_amount), 0) FROM vehicles 
                          WHERE payment_status='paid' AND DATE(exit_time) = CURDATE()", conn))
                    {
                        decimal revenue = Convert.ToDecimal(cmd.ExecuteScalar());
                        billLblRevenue.Text = $"₱{revenue:N2}";
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Stats Error: " + ex.Message); }
        }

        // ===== BUTTONS =====
        private void btnRefresh_Click(object sender, EventArgs e) => RefreshAll();

        private void btnMarkPaid_Click(object sender, EventArgs e)
        {
            DataGridView activeDgv = null;
            if (currentTab == 0) activeDgv = billDgvAll;
            else if (currentTab == 2) activeDgv = billDgvUnpaid;
            else
            {
                MessageBox.Show("Go to 'All Vehicles' or 'Unpaid' tab to mark as paid.",
                    "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (activeDgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a vehicle first!", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selected = activeDgv.SelectedRows[0];
            string regNum = selected.Cells[0].Value.ToString();
            string ownerName = selected.Cells[1].Value.ToString();
            string slotId = selected.Cells[3].Value.ToString();

            if (currentTab == 0)
            {
                string status = selected.Cells[7].Value?.ToString() ?? "";
                if (status.Contains("ACTIVE"))
                {
                    MessageBox.Show("This vehicle is still ACTIVE. Process exit first.",
                        "Cannot Mark", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (status.Contains("PAID"))
                {
                    MessageBox.Show("This bill is already PAID.",
                        "Already Paid", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            string billAmount = selected.Cells[currentTab == 0 ? 6 : 7].Value.ToString();

            DialogResult result = MessageBox.Show(
                $"Mark this bill as PAID?\n\nRegistration: {regNum}\nOwner: {ownerName}\nSlot: {slotId}\nAmount: {billAmount}",
                "Confirm Payment", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string query = @"UPDATE vehicles SET payment_status='paid' 
                                     WHERE reg_number=@reg AND slot_id=@slot 
                                     AND payment_status='unpaid' AND is_active=0
                                     ORDER BY exit_time DESC LIMIT 1";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@reg", regNum);
                        cmd.Parameters.AddWithValue("@slot", slotId);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show($"Payment confirmed!\n\nAmount: {billAmount}",
                    "Paid", MessageBoxButtons.OK, MessageBoxIcon.Information);

                RefreshAll();
            }
            catch (Exception ex) { MessageBox.Show("Payment Error: " + ex.Message); }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            if (billTimer != null) billTimer.Stop();
            base.OnHandleDestroyed(e);
        }
    }
}