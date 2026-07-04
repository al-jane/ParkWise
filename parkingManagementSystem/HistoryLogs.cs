using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Cmp;

namespace parkingManagementSystem
{
    public partial class HistoryLogs : UserControl
    {
        private string connStr = "Server=localhost;Database=parking_db;Uid=root;Pwd=;";

        // Theme colors
        private readonly Color CardBg = Color.White;
        private readonly Color InputBg = Color.FromArgb(248, 249, 251);
        private readonly Color TextPrimary = Color.FromArgb(40, 42, 58);
        private readonly Color TextSecondary = Color.FromArgb(120, 125, 140);
        private readonly Color PinkAccent = Color.FromArgb(236, 64, 122);
        private readonly Color GreenAccent = Color.FromArgb(34, 197, 94);
        private readonly Color RedAccent = Color.FromArgb(239, 68, 68);
        private readonly Color BlueAccent = Color.FromArgb(59, 130, 246);
        private readonly Color AmberAccent = Color.FromArgb(245, 158, 11);

        public HistoryLogs()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            BuildStatsCards();
            BuildFilters();
            BuildTable();

            this.Load += HistoryLogs_Load;
        }

        private void HistoryLogs_Load(object sender, EventArgs e)
        {
            LoadAllHistory();
            UpdateStats();
        }

        // ===== BUILD STAT CARDS =====
        private void BuildStatsCards()
        {
            statsContainer.Controls.Add(
                CreateStatCard(BlueAccent, "📋", "Total Transactions", "0", out lblTotalCount, 0), 0, 0);
            statsContainer.Controls.Add(
                CreateStatCard(GreenAccent, "💰", "Total Revenue", "₱0.00", out lblTotalRevenue, 1), 1, 0);
            statsContainer.Controls.Add(
                CreateStatCard(RedAccent, "⚠", "Unpaid Amount", "₱0.00", out lblUnpaidAmount, 2), 2, 0);
            statsContainer.Controls.Add(
                CreateStatCard(AmberAccent, "⏱", "Avg Duration", "0h", out lblAvgDuration, 3), 3, 0);
        }

        private RoundedPanel CreateStatCard(Color accent, string icon, string title, string value, out Label valueLabel, int index)
        {
            RoundedPanel card = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                BackColor = CardBg,
                BorderRadius = 12,
                Margin = new Padding(index == 0 ? 0 : 5, 0, index == 3 ? 0 : 5, 0)
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
                Location = new Point(72, 18)
            };

            valueLabel = new Label
            {
                AutoSize = true,
                Text = value,
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = TextPrimary,
                Location = new Point(72, 38)
            };

            card.Controls.Add(iconPnl);
            card.Controls.Add(titleLbl);
            card.Controls.Add(valueLabel);

            return card;
        }

        // ===== BUILD FILTERS CARD =====
        private void BuildFilters()
        {
            RoundedPanel filterCard = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                BackColor = CardBg,
                BorderRadius = 12
            };

            // Search box
            Label lblSearch = new Label
            {
                AutoSize = true,
                Text = "🔍  Search",
                Font = new Font("Segoe UI", 9F),
                ForeColor = TextSecondary,
                Location = new Point(20, 15)
            };

            txtSearch = new TextBox
            {
                Font = new Font("Segoe UI", 10.5F),
                Location = new Point(20, 35),
                Size = new Size(300, 32),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = InputBg,
                ForeColor = TextPrimary,
                PlaceholderText = "Registration # or owner name..."
            };
            txtSearch.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter) { ApplyFilters(); e.SuppressKeyPress = true; }
            };

            // Status filter
            Label lblStatus = new Label
            {
                AutoSize = true,
                Text = "📊  Status",
                Font = new Font("Segoe UI", 9F),
                ForeColor = TextSecondary,
                Location = new Point(340, 15)
            };

            cmbStatus = new ComboBox
            {
                Font = new Font("Segoe UI", 10.5F),
                Location = new Point(340, 35),
                Size = new Size(150, 32),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = InputBg,
                ForeColor = TextPrimary,
                FlatStyle = FlatStyle.Flat
            };
            cmbStatus.Items.AddRange(new[] { "All", "Active", "Paid", "Unpaid" });
            cmbStatus.SelectedIndex = 0;

            // Category filter
            Label lblCategory = new Label
            {
                AutoSize = true,
                Text = "🚗  Category",
                Font = new Font("Segoe UI", 9F),
                ForeColor = TextSecondary,
                Location = new Point(510, 15)
            };

            cmbCategory = new ComboBox
            {
                Font = new Font("Segoe UI", 10.5F),
                Location = new Point(510, 35),
                Size = new Size(150, 32),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = InputBg,
                ForeColor = TextPrimary,
                FlatStyle = FlatStyle.Flat
            };
            cmbCategory.Items.AddRange(new[] { "All", "Motorcycle", "Compact", "Sedan", "SUV" });
            cmbCategory.SelectedIndex = 0;

            // Date From
            Label lblDateFrom = new Label
            {
                AutoSize = true,
                Text = "📅  From",
                Font = new Font("Segoe UI", 9F),
                ForeColor = TextSecondary,
                Location = new Point(680, 15)
            };

            dtpFrom = new DateTimePicker
            {
                Font = new Font("Segoe UI", 10.5F),
                Location = new Point(680, 35),
                Size = new Size(140, 32),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now.AddMonths(-1)
            };

            // Date To
            Label lblDateTo = new Label
            {
                AutoSize = true,
                Text = "📅  To",
                Font = new Font("Segoe UI", 9F),
                ForeColor = TextSecondary,
                Location = new Point(835, 15)
            };

            dtpTo = new DateTimePicker
            {
                Font = new Font("Segoe UI", 10.5F),
                Location = new Point(835, 35),
                Size = new Size(140, 32),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now
            };

            // Apply button
            RoundedButton btnApply = new RoundedButton
            {
                Text = "✓  Apply",
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                BackColor = PinkAccent,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                BorderRadius = 8,
                Size = new Size(110, 32),
                Location = new Point(995, 35),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnApply.FlatAppearance.BorderSize = 0;
            btnApply.Click += (s, e) => ApplyFilters();

            // Reset button
            RoundedButton btnReset = new RoundedButton
            {
                Text = "↺  Reset",
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                BackColor = Color.FromArgb(229, 231, 235),
                ForeColor = TextPrimary,
                FlatStyle = FlatStyle.Flat,
                BorderRadius = 8,
                Size = new Size(100, 32),
                Location = new Point(1115, 35),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnReset.FlatAppearance.BorderSize = 0;
            btnReset.Click += (s, e) => ResetFilters();

            filterCard.Controls.Add(lblSearch);
            filterCard.Controls.Add(txtSearch);
            filterCard.Controls.Add(lblStatus);
            filterCard.Controls.Add(cmbStatus);
            filterCard.Controls.Add(lblCategory);
            filterCard.Controls.Add(cmbCategory);
            filterCard.Controls.Add(lblDateFrom);
            filterCard.Controls.Add(dtpFrom);
            filterCard.Controls.Add(lblDateTo);
            filterCard.Controls.Add(dtpTo);
            filterCard.Controls.Add(btnApply);
            filterCard.Controls.Add(btnReset);

            // Anchor buttons to right
            filterCard.Resize += (s, e) =>
            {
                btnApply.Location = new Point(filterCard.Width - 230, 35);
                btnReset.Location = new Point(filterCard.Width - 115, 35);
            };

            filtersPanel.Controls.Add(filterCard);
        }

        // ===== BUILD TABLE =====
        private void BuildTable()
        {
            RoundedPanel tableCard = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                BackColor = CardBg,
                BorderRadius = 15
            };

            Label lblTitle = new Label
            {
                AutoSize = true,
                Text = "📜  Transaction History",
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                ForeColor = TextPrimary,
                Location = new Point(25, 20)
            };

            dgvHistory = CreateStyledDataGridView();
            dgvHistory.Location = new Point(25, 60);
            dgvHistory.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Registration #", FillWeight = 11 });
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Owner", FillWeight = 14 });
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Category", FillWeight = 9 });
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Slot", FillWeight = 6 });
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Entry Time", FillWeight = 13 });
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Exit Time", FillWeight = 13 });
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Duration", FillWeight = 10 });
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Amount", FillWeight = 10 });
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Status", FillWeight = 14 });

            foreach (DataGridViewColumn col in dgvHistory.Columns)
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            // Export button
            RoundedButton btnExport = new RoundedButton
            {
                Text = "📥  Export to CSV",
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                BackColor = GreenAccent,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                BorderRadius = 10,
                Size = new Size(170, 40),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.Click += (s, e) => ExportToCsv();

            tableCard.Controls.Add(dgvHistory);
            tableCard.Controls.Add(btnExport);
            tableCard.Controls.Add(lblTitle);

            tableCard.Resize += (s, e) =>
            {
                dgvHistory.Size = new Size(tableCard.Width - 50, tableCard.Height - 80);
                btnExport.Location = new Point(tableCard.Width - 195, 18);
            };

            tablePanel.Controls.Add(tableCard);
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
                Font = new Font("Segoe UI", 9.5F),
                EnableHeadersVisualStyles = false,
                GridColor = Color.FromArgb(235, 238, 243),
                RowTemplate = { Height = 40 },
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

        // ===== LOAD DATA =====
        private void LoadAllHistory() => LoadHistory("", "All", "All", null, null);

        private void LoadHistory(string search, string status, string category, DateTime? from, DateTime? to)
        {
            try
            {
                dgvHistory.Rows.Clear();
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string query = @"SELECT reg_number, owner_name, category, slot_id, 
                                     entry_time, exit_time, duration_minutes, bill_amount,
                                     payment_status, is_active
                                     FROM vehicles WHERE 1=1";

                    if (!string.IsNullOrWhiteSpace(search))
                        query += " AND (reg_number LIKE @search OR owner_name LIKE @search)";

                    if (status == "Active") query += " AND is_active=1";
                    else if (status == "Paid") query += " AND payment_status='paid' AND is_active=0";
                    else if (status == "Unpaid") query += " AND payment_status='unpaid' AND is_active=0";

                    if (category != "All") query += " AND category=@category";

                    if (from.HasValue) query += " AND entry_time >= @from";
                    if (to.HasValue) query += " AND entry_time <= @to";

                    query += " ORDER BY entry_time DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        if (!string.IsNullOrWhiteSpace(search))
                            cmd.Parameters.AddWithValue("@search", "%" + search + "%");
                        if (category != "All")
                            cmd.Parameters.AddWithValue("@category", category);
                        if (from.HasValue)
                            cmd.Parameters.AddWithValue("@from", from.Value.Date);
                        if (to.HasValue)
                            cmd.Parameters.AddWithValue("@to", to.Value.Date.AddDays(1));

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string regNum = reader["reg_number"].ToString();
                                string ownerName = reader["owner_name"].ToString();
                                string cat = reader["category"].ToString();
                                string slotId = reader["slot_id"].ToString();
                                DateTime entryTime = Convert.ToDateTime(reader["entry_time"]);
                                bool isActive = Convert.ToInt32(reader["is_active"]) == 1;
                                string paymentStatus = reader["payment_status"].ToString();

                                string exitTimeStr, durationStr, billStr, statusTag;
                                Color rowBg;

                                if (isActive)
                                {
                                    exitTimeStr = "—";
                                    TimeSpan dur = DateTime.Now - entryTime;
                                    decimal bill = BillingHelper.CalculateBill(cat, entryTime, DateTime.Now);
                                    durationStr = BillingHelper.FormatDuration(dur);
                                    billStr = $"₱{bill:N2}";
                                    statusTag = "🔵 ACTIVE";
                                    rowBg = Color.FromArgb(223, 242, 254);
                                }
                                else
                                {
                                    DateTime exitTime = Convert.ToDateTime(reader["exit_time"]);
                                    exitTimeStr = exitTime.ToString("MMM dd, yyyy HH:mm");
                                    int mins = Convert.ToInt32(reader["duration_minutes"]);
                                    decimal bill = Convert.ToDecimal(reader["bill_amount"]);
                                    durationStr = BillingHelper.FormatDuration(TimeSpan.FromMinutes(mins));
                                    billStr = $"₱{bill:N2}";

                                    if (paymentStatus == "paid")
                                    {
                                        statusTag = "✅ PAID";
                                        rowBg = Color.FromArgb(220, 252, 231);
                                    }
                                    else
                                    {
                                        statusTag = "❌ UNPAID";
                                        rowBg = Color.FromArgb(254, 226, 226);
                                    }
                                }

                                int rowIdx = dgvHistory.Rows.Add(
                                    regNum, ownerName, cat, slotId,
                                    entryTime.ToString("MMM dd, yyyy HH:mm"),
                                    exitTimeStr, durationStr, billStr, statusTag);
                                dgvHistory.Rows[rowIdx].DefaultCellStyle.BackColor = rowBg;
                            }
                        }
                    }
                }

                UpdateStats();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load Error: " + ex.Message);
            }
        }

        private void ApplyFilters()
        {
            LoadHistory(
                txtSearch.Text.Trim(),
                cmbStatus.SelectedItem?.ToString() ?? "All",
                cmbCategory.SelectedItem?.ToString() ?? "All",
                dtpFrom.Value,
                dtpTo.Value);
        }

        private void ResetFilters()
        {
            txtSearch.Clear();
            cmbStatus.SelectedIndex = 0;
            cmbCategory.SelectedIndex = 0;
            dtpFrom.Value = DateTime.Now.AddMonths(-1);
            dtpTo.Value = DateTime.Now;
            LoadAllHistory();
        }

        // ===== UPDATE STATS =====
        private void UpdateStats()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM vehicles", conn))
                        lblTotalCount.Text = cmd.ExecuteScalar().ToString();

                    using (MySqlCommand cmd = new MySqlCommand(
                        "SELECT COALESCE(SUM(bill_amount), 0) FROM vehicles WHERE payment_status='paid'", conn))
                        lblTotalRevenue.Text = $"₱{Convert.ToDecimal(cmd.ExecuteScalar()):N2}";

                    using (MySqlCommand cmd = new MySqlCommand(
                        "SELECT COALESCE(SUM(bill_amount), 0) FROM vehicles WHERE payment_status='unpaid' AND is_active=0", conn))
                        lblUnpaidAmount.Text = $"₱{Convert.ToDecimal(cmd.ExecuteScalar()):N2}";

                    using (MySqlCommand cmd = new MySqlCommand(
                        "SELECT COALESCE(AVG(duration_minutes), 0) FROM vehicles WHERE is_active=0", conn))
                    {
                        double avgMins = Convert.ToDouble(cmd.ExecuteScalar());
                        int hours = (int)(avgMins / 60);
                        int mins = (int)(avgMins % 60);
                        lblAvgDuration.Text = hours > 0 ? $"{hours}h {mins}m" : $"{mins}m";
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Stats error: " + ex.Message);
            }
        }

        // ===== EXPORT TO CSV =====
        private void ExportToCsv()
        {
            if (dgvHistory.Rows.Count == 0)
            {
                MessageBox.Show("No data to export!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv",
                FileName = $"ParkWise_History_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
            };

            if (dialog.ShowDialog() != DialogResult.OK) return;

            try
            {
                // ✅ Use UTF-8 WITH BOM so Excel recognizes it properly
                var utf8WithBom = new System.Text.UTF8Encoding(true);

                using (StreamWriter writer = new StreamWriter(dialog.FileName, false, utf8WithBom))
                {
                    // Header row
                    var headers = new System.Collections.Generic.List<string>();
                    foreach (DataGridViewColumn col in dgvHistory.Columns)
                        headers.Add($"\"{col.HeaderText}\"");
                    writer.WriteLine(string.Join(",", headers));

                    // Data rows
                    foreach (DataGridViewRow row in dgvHistory.Rows)
                    {
                        var cells = new System.Collections.Generic.List<string>();
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            string val = cell.Value?.ToString() ?? "";

                            // ✅ Remove emojis from status column so they don't mess up Excel
                            val = val.Replace("🔵 ", "")
                                     .Replace("✅ ", "")
                                     .Replace("❌ ", "")
                                     .Replace("₱", "PHP ");

                            // Escape quotes
                            val = val.Replace("\"", "\"\"");
                            cells.Add($"\"{val}\"");
                        }
                        writer.WriteLine(string.Join(",", cells));
                    }
                }

                // Ask to open the file
                DialogResult openFile = MessageBox.Show(
                    $"Exported {dgvHistory.Rows.Count} records successfully!\n\nSaved to:\n{dialog.FileName}\n\nOpen the file now?",
                    "Export Successful", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (openFile == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = dialog.FileName,
                        UseShellExecute = true
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Public refresh
        public void RefreshAll()
        {
            LoadAllHistory();
            UpdateStats();
        }
    }
}