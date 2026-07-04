using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace parkingManagementSystem
{
    public partial class VehicleTracker : UserControl
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
        private readonly Color InactiveTab = Color.White;
        private readonly Color InactiveTabText = Color.FromArgb(80, 85, 100);

        private System.Windows.Forms.Timer refreshTimer;
        private int currentView = 0; // 0 = List, 1 = Map
        private string highlightedSlot = null;
        private Dictionary<string, Button> slotButtons = new Dictionary<string, Button>();

        public VehicleTracker()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            BuildStatsCards();
            BuildSearchAndTabs();
            BuildListView();
            BuildMapView();

            this.Load += VehicleTracker_Load;
        }

        private void VehicleTracker_Load(object sender, EventArgs e)
        {
            LoadAllVehicles();
            UpdateStats();
            ShowView(0);

            refreshTimer = new System.Windows.Forms.Timer();
            refreshTimer.Interval = 10000; // 10 seconds
            refreshTimer.Tick += (s, ev) => RefreshAll();
            refreshTimer.Start();
        }

        // ===== BUILD STAT CARDS =====
        private void BuildStatsCards()
        {
            statsContainer.Controls.Add(
                CreateStatCard(BlueAccent, "🚗", "Active Vehicles", "0", out lblActiveCount, 0), 0, 0);
            statsContainer.Controls.Add(
                CreateStatCard(PinkAccent, "🏍", "Motorcycles", "0", out lblMotorCount, 1), 1, 0);
            statsContainer.Controls.Add(
                CreateStatCard(GreenAccent, "🚙", "Cars (C/S)", "0", out lblCarCount, 2), 2, 0);
            statsContainer.Controls.Add(
                CreateStatCard(AmberAccent, "⏱", "Longest Stay", "0h", out lblLongestStay, 3), 3, 0);
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
                Font = new Font("Segoe UI Semibold", 17F, FontStyle.Bold),
                ForeColor = TextPrimary,
                Location = new Point(72, 38)
            };

            card.Controls.Add(iconPnl);
            card.Controls.Add(titleLbl);
            card.Controls.Add(valueLabel);

            return card;
        }

        // ===== BUILD SEARCH + TABS =====
        private void BuildSearchAndTabs()
        {
            RoundedPanel searchCard = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                BackColor = CardBg,
                BorderRadius = 12
            };

            Label lblSearch = new Label
            {
                AutoSize = true,
                Text = "🔍  Search Vehicle",
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = TextPrimary,
                Location = new Point(20, 15)
            };

            txtSearch = new TextBox
            {
                Font = new Font("Segoe UI", 10.5F),
                Location = new Point(20, 40),
                Size = new Size(400, 32),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = InputBg,
                ForeColor = TextPrimary,
                PlaceholderText = "Plate, owner, or slot number..."
            };
            txtSearch.TextChanged += (s, e) => LoadAllVehicles();

            // View toggle tabs
            tabBtnList = CreateTabButton("📋  List View", 450);
            tabBtnMap = CreateTabButton("🗺  Map View", 590);

            tabBtnList.Click += (s, e) => ShowView(0);
            tabBtnMap.Click += (s, e) => ShowView(1);

            searchCard.Controls.Add(lblSearch);
            searchCard.Controls.Add(txtSearch);
            searchCard.Controls.Add(tabBtnList);
            searchCard.Controls.Add(tabBtnMap);

            searchPanel.Controls.Add(searchCard);
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
                Size = new Size(130, 40),
                Location = new Point(x, 36),
                Cursor = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 }
            };
        }

        private void ShowView(int view)
        {
            currentView = view;

            tabBtnList.BackColor = InactiveTab;
            tabBtnList.ForeColor = InactiveTabText;
            tabBtnMap.BackColor = InactiveTab;
            tabBtnMap.ForeColor = InactiveTabText;

            if (view == 0)
            {
                tabBtnList.BackColor = PinkAccent;
                tabBtnList.ForeColor = Color.White;
                listCard.Visible = true;
                mapCard.Visible = false;
            }
            else
            {
                tabBtnMap.BackColor = PinkAccent;
                tabBtnMap.ForeColor = Color.White;
                listCard.Visible = false;
                mapCard.Visible = true;
                BuildMapGrid();
            }
        }

        // ===== LIST VIEW =====
        private void BuildListView()
        {
            listCard = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                BackColor = CardBg,
                BorderRadius = 15
            };

            Label lblTitle = new Label
            {
                AutoSize = true,
                Text = "🚗  Active Vehicles",
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                ForeColor = TextPrimary,
                Location = new Point(25, 20)
            };

            dgvVehicles = CreateStyledDataGridView();
            dgvVehicles.Location = new Point(25, 60);
            dgvVehicles.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            dgvVehicles.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Registration #", FillWeight = 13 });
            dgvVehicles.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Owner", FillWeight = 16 });
            dgvVehicles.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Contact", FillWeight = 13 });
            dgvVehicles.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Company", FillWeight = 12 });
            dgvVehicles.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Category", FillWeight = 10 });
            dgvVehicles.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Slot", FillWeight = 8 });
            dgvVehicles.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Entry Time", FillWeight = 15 });
            dgvVehicles.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Duration", FillWeight = 13 });

            foreach (DataGridViewColumn col in dgvVehicles.Columns)
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvVehicles.SelectionChanged += (s, e) =>
            {
                if (dgvVehicles.SelectedRows.Count > 0)
                {
                    highlightedSlot = dgvVehicles.SelectedRows[0].Cells[5].Value?.ToString();
                }
            };

            dgvVehicles.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    highlightedSlot = dgvVehicles.Rows[e.RowIndex].Cells[5].Value?.ToString();
                    ShowView(1);
                }
            };

            listCard.Controls.Add(dgvVehicles);
            listCard.Controls.Add(lblTitle);

            listCard.Resize += (s, e) =>
            {
                dgvVehicles.Size = new Size(listCard.Width - 50, listCard.Height - 80);
            };

            contentPanel.Controls.Add(listCard);
        }

        // ===== MAP VIEW =====
        private void BuildMapView()
        {
            mapCard = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                BackColor = CardBg,
                BorderRadius = 15,
                Visible = false
            };

            Label lblTitle = new Label
            {
                AutoSize = true,
                Text = "🗺  Live Parking Map",
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                ForeColor = TextPrimary,
                Location = new Point(25, 20)
            };

            // Legend
            Panel legendPanel = new Panel
            {
                Location = new Point(200, 23),
                Size = new Size(500, 25),
                BackColor = Color.Transparent
            };

            AddLegendItem(legendPanel, Color.FromArgb(220, 252, 231), GreenAccent, "Available", 0);
            AddLegendItem(legendPanel, Color.FromArgb(254, 226, 226), RedAccent, "Occupied", 120);
            AddLegendItem(legendPanel, Color.FromArgb(254, 243, 199), AmberAccent, "Reserved", 240);
            AddLegendItem(legendPanel, Color.FromArgb(252, 231, 243), PinkAccent, "Highlighted", 360);

            mapScrollPanel = new Panel
            {
                Location = new Point(25, 60),
                AutoScroll = true,
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            mapCard.Controls.Add(lblTitle);
            mapCard.Controls.Add(legendPanel);
            mapCard.Controls.Add(mapScrollPanel);

            mapCard.Resize += (s, e) =>
            {
                mapScrollPanel.Size = new Size(mapCard.Width - 50, mapCard.Height - 80);
            };

            contentPanel.Controls.Add(mapCard);
        }

        private void AddLegendItem(Panel parent, Color bg, Color accent, string text, int x)
        {
            RoundedPanel pill = new RoundedPanel
            {
                BackColor = bg,
                BorderRadius = 12,
                Location = new Point(x, 0),
                Size = new Size(110, 25)
            };

            Label lbl = new Label
            {
                AutoSize = true,
                Text = text,
                Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold),
                ForeColor = accent,
                Location = new Point(12, 5)
            };
            pill.Controls.Add(lbl);
            parent.Controls.Add(pill);
        }

        private void BuildMapGrid()
        {
            mapScrollPanel.Controls.Clear();
            slotButtons.Clear();

            int y = 10;
            y = BuildCategorySection("🏍  Motorcycle", "Motorcycle", 5, y, PinkAccent);
            y = BuildCategorySection("🚙  Compact", "Compact", 7, y, Color.FromArgb(20, 184, 166));
            y = BuildCategorySection("🚗  Sedan", "Sedan", 5, y, BlueAccent);
            y = BuildCategorySection("🚐  SUV", "SUV", 5, y, Color.FromArgb(132, 204, 22));
        }

        private int BuildCategorySection(string title, string category, int cols, int startY, Color accent)
        {
            Label lbl = new Label
            {
                AutoSize = true,
                Text = title,
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = accent,
                Location = new Point(0, startY)
            };
            mapScrollPanel.Controls.Add(lbl);

            int y = startY + 30;
            var slots = GetSlotsWithVehicles(category);

            int slotWidth = 140;
            int slotHeight = 60;
            int spacing = 10;

            for (int i = 0; i < slots.Count; i++)
            {
                int row = i / cols;
                int col = i % cols;
                int x = col * (slotWidth + spacing);
                int slotY = y + row * (slotHeight + spacing);

                var slot = slots[i];
                CreateSlotCard(slot, x, slotY, slotWidth, slotHeight);
            }

            int totalRows = (int)Math.Ceiling((double)slots.Count / cols);
            return y + totalRows * (slotHeight + spacing) + 20;
        }

        private void CreateSlotCard(SlotInfo slot, int x, int y, int w, int h)
        {
            Color bg, accent;
            bool isHighlighted = slot.SlotId == highlightedSlot;

            if (isHighlighted)
            {
                bg = Color.FromArgb(252, 231, 243);
                accent = PinkAccent;
            }
            else if (slot.Status == "occupied")
            {
                bg = Color.FromArgb(254, 226, 226);
                accent = RedAccent;
            }
            else if (slot.Status == "reserved")
            {
                bg = Color.FromArgb(254, 243, 199);
                accent = AmberAccent;
            }
            else
            {
                bg = Color.FromArgb(220, 252, 231);
                accent = GreenAccent;
            }

            RoundedPanel card = new RoundedPanel
            {
                Location = new Point(x, y),
                Size = new Size(w, h),
                BackColor = bg,
                BorderRadius = 10,
                Cursor = Cursors.Hand
            };

            Label lblSlot = new Label
            {
                AutoSize = true,
                Text = slot.SlotId,
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = accent,
                Location = new Point(10, 6)
            };

            Label lblInfo = new Label
            {
                AutoSize = false,
                Size = new Size(w - 20, 16),
                Location = new Point(10, 24),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = TextPrimary,
                Text = slot.Status == "occupied" ? slot.RegNumber : slot.Status.ToUpper(),
                AutoEllipsis = true
            };

            Label lblSub = new Label
            {
                AutoSize = false,
                Size = new Size(w - 20, 16),
                Location = new Point(10, 41),
                Font = new Font("Segoe UI", 8F),
                ForeColor = TextSecondary,
                Text = slot.Status == "occupied" ? slot.Duration : "—",
                AutoEllipsis = true
            };

            card.Controls.Add(lblSlot);
            card.Controls.Add(lblInfo);
            card.Controls.Add(lblSub);

            // Click to show details
            if (slot.Status == "occupied")
            {
                card.Click += (s, e) => ShowSlotDetails(slot);
                foreach (Control c in card.Controls)
                    c.Click += (s, e) => ShowSlotDetails(slot);
            }

            mapScrollPanel.Controls.Add(card);
            slotButtons[slot.SlotId] = null; // tracking
        }

        private void ShowSlotDetails(SlotInfo slot)
        {
            string msg = $"━━━ VEHICLE DETAILS ━━━\n\n" +
                         $"🅿 Slot: {slot.SlotId}\n" +
                         $"🚗 Plate: {slot.RegNumber}\n" +
                         $"👤 Owner: {slot.OwnerName}\n" +
                         $"📞 Contact: {slot.Contact}\n" +
                         $"🏷 Category: {slot.Category}\n" +
                         $"🕐 Entry: {slot.EntryTime:MMM dd, yyyy HH:mm}\n" +
                         $"⏱ Duration: {slot.Duration}";

            MessageBox.Show(msg, "Slot Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ===== DATA ACCESS =====
        private class SlotInfo
        {
            public string SlotId { get; set; }
            public string Status { get; set; }
            public string Category { get; set; }
            public string RegNumber { get; set; }
            public string OwnerName { get; set; }
            public string Contact { get; set; }
            public DateTime EntryTime { get; set; }
            public string Duration { get; set; }
        }

        private List<SlotInfo> GetSlotsWithVehicles(string category)
        {
            var list = new List<SlotInfo>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    // Use LEFT JOIN with is_active=1 as the source of truth
                    // This ignores slot.status and checks vehicles.is_active instead
                    string query = @"SELECT s.slot_id, s.category,
                             CASE 
                                WHEN v.reg_number IS NOT NULL THEN 'occupied'
                                WHEN s.status = 'reserved' THEN 'reserved'
                                ELSE 'available'
                             END AS real_status,
                             v.reg_number, v.owner_name, v.owner_contact, v.entry_time
                             FROM slots s
                             LEFT JOIN vehicles v ON s.slot_id = v.slot_id AND v.is_active = 1
                             WHERE s.category = @cat
                             ORDER BY s.slot_id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@cat", category);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var slot = new SlotInfo
                                {
                                    SlotId = reader["slot_id"].ToString(),
                                    Status = reader["real_status"].ToString(),
                                    Category = reader["category"].ToString(),
                                    RegNumber = reader["reg_number"]?.ToString() ?? "",
                                    OwnerName = reader["owner_name"]?.ToString() ?? "",
                                    Contact = reader["owner_contact"]?.ToString() ?? ""
                                };

                                if (reader["entry_time"] != DBNull.Value)
                                {
                                    slot.EntryTime = Convert.ToDateTime(reader["entry_time"]);
                                    TimeSpan dur = DateTime.Now - slot.EntryTime;
                                    slot.Duration = FormatDuration(dur);
                                }

                                list.Add(slot);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Map load error: " + ex.Message);
            }
            return list;
        }

        private string FormatDuration(TimeSpan ts)
        {
            if (ts.TotalHours >= 1) return $"{(int)ts.TotalHours}h {ts.Minutes}m";
            return $"{ts.Minutes}m";
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

            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 251, 253);

            return dgv;
        }

        // ===== LOAD DATA =====
        private void LoadAllVehicles()
        {
            string search = txtSearch?.Text?.Trim() ?? "";

            try
            {
                dgvVehicles.Rows.Clear();
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string query = @"SELECT reg_number, owner_name, owner_contact, company, 
                                     category, slot_id, entry_time
                                     FROM vehicles 
                                     WHERE is_active = 1";

                    if (!string.IsNullOrWhiteSpace(search))
                        query += " AND (reg_number LIKE @s OR owner_name LIKE @s OR slot_id LIKE @s)";

                    query += " ORDER BY entry_time DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        if (!string.IsNullOrWhiteSpace(search))
                            cmd.Parameters.AddWithValue("@s", "%" + search + "%");

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DateTime entry = Convert.ToDateTime(reader["entry_time"]);
                                TimeSpan dur = DateTime.Now - entry;

                                dgvVehicles.Rows.Add(
                                    reader["reg_number"].ToString(),
                                    reader["owner_name"].ToString(),
                                    reader["owner_contact"].ToString(),
                                    reader["company"].ToString(),
                                    reader["category"].ToString(),
                                    reader["slot_id"].ToString(),
                                    entry.ToString("MMM dd, yyyy HH:mm"),
                                    FormatDuration(dur));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load Error: " + ex.Message);
            }
        }

        private void UpdateStats()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM vehicles WHERE is_active=1", conn))
                        lblActiveCount.Text = cmd.ExecuteScalar().ToString();

                    using (MySqlCommand cmd = new MySqlCommand(
                        "SELECT COUNT(*) FROM vehicles WHERE is_active=1 AND category='Motorcycle'", conn))
                        lblMotorCount.Text = cmd.ExecuteScalar().ToString();

                    using (MySqlCommand cmd = new MySqlCommand(
                        "SELECT COUNT(*) FROM vehicles WHERE is_active=1 AND category IN ('Compact','Sedan','SUV')", conn))
                        lblCarCount.Text = cmd.ExecuteScalar().ToString();

                    using (MySqlCommand cmd = new MySqlCommand(
                        "SELECT MIN(entry_time) FROM vehicles WHERE is_active=1", conn))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != DBNull.Value && result != null)
                        {
                            DateTime oldest = Convert.ToDateTime(result);
                            TimeSpan dur = DateTime.Now - oldest;
                            lblLongestStay.Text = FormatDuration(dur);
                        }
                        else
                        {
                            lblLongestStay.Text = "0m";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Stats error: " + ex.Message);
            }
        }

        public void RefreshAll()
        {
            LoadAllVehicles();
            UpdateStats();
            if (currentView == 1) BuildMapGrid();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            refreshTimer?.Stop();
            base.OnHandleDestroyed(e);
        }
    }
}