using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace parkingManagementSystem
{
    public partial class parkingGrid : UserControl
    {
        private List<Button> allSlotButtons = new List<Button>();
        private string connStr = "Server=localhost;Database=parking_db;Uid=root;Pwd=;";

        // Status colors
        private readonly Color AvailableColor = Color.FromArgb(34, 197, 94);
        private readonly Color OccupiedColor = Color.FromArgb(239, 68, 68);
        private readonly Color ReservedColor = Color.FromArgb(245, 158, 11);
        private readonly Color AvailableBg = Color.FromArgb(220, 252, 231);
        private readonly Color OccupiedBg = Color.FromArgb(254, 226, 226);
        private readonly Color ReservedBg = Color.FromArgb(254, 243, 199);

        private Label lblStatAvailable, lblStatOccupied, lblStatReserved;
        private Dictionary<string, string> slotStatusCache = new Dictionary<string, string>();

        public parkingGrid()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            BuildCategoryCards();
            BuildLegend();
            BuildHeaderStats();

            this.Load += parkingGrid_Load;
        }

        private void parkingGrid_Load(object sender, EventArgs e)
        {
            CreateCategorySlots("Motorcycle", tlpMotor, 3, 5);
            CreateCategorySlots("Compact", tlpCompact, 5, 7);
            CreateCategorySlots("Sedan", tlpSedan, 5, 5);
            CreateCategorySlots("SUV", tlpSUV, 5, 5);
            RefreshSlots();
        }

        // ===== Build 4 Category Cards =====
        private void BuildCategoryCards()
        {
            tlpMotor = new TableLayoutPanel();
            tlpCompact = new TableLayoutPanel();
            tlpSedan = new TableLayoutPanel();
            tlpSUV = new TableLayoutPanel();

            RoundedPanel motorCard = CreateCategoryCard(
                Color.FromArgb(236, 64, 122), "🏍  Motorcycle", "15 slots", tlpMotor);
            RoundedPanel compactCard = CreateCategoryCard(
                Color.FromArgb(20, 184, 166), "🚙  Compact", "35 slots", tlpCompact);
            RoundedPanel sedanCard = CreateCategoryCard(
                Color.FromArgb(59, 130, 246), "🚗  Sedan", "25 slots", tlpSedan);
            RoundedPanel suvCard = CreateCategoryCard(
                Color.FromArgb(132, 204, 22), "🚐  SUV", "25 slots", tlpSUV);

            sectionsContainer.Controls.Add(motorCard, 0, 0);
            sectionsContainer.Controls.Add(compactCard, 0, 1);
            sectionsContainer.Controls.Add(sedanCard, 0, 2);
            sectionsContainer.Controls.Add(suvCard, 0, 3);
        }

        private RoundedPanel CreateCategoryCard(Color accent, string title, string count, TableLayoutPanel tlp)
        {
            RoundedPanel card = new RoundedPanel
            {
                Dock = DockStyle.Top,
                BackColor = Color.White,
                BorderRadius = 15,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Margin = new Padding(0, 0, 0, 15),
                Padding = new Padding(20, 15, 20, 20)
            };

            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 40,
                BackColor = Color.Transparent
            };

            Label titleLbl = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                ForeColor = accent,
                Location = new Point(0, 8),
                Text = title
            };

            Label countLbl = new Label
            {
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(120, 125, 140),
                Text = count
            };

            header.Controls.Add(titleLbl);
            header.Controls.Add(countLbl);

            header.Resize += (s, e) =>
            {
                countLbl.Location = new Point(header.Width - countLbl.Width, 10);
            };

            tlp.Dock = DockStyle.Top;
            tlp.AutoSize = true;
            tlp.BackColor = Color.Transparent;
            tlp.Margin = new Padding(0, 5, 0, 0);

            card.Controls.Add(tlp);
            card.Controls.Add(header);

            return card;
        }

        // ===== Build Legend =====
        private void BuildLegend()
        {
            legendContainer.Controls.Add(CreateLegendPill(AvailableColor, AvailableBg, "Available", 0));
            legendContainer.Controls.Add(CreateLegendPill(OccupiedColor, OccupiedBg, "Occupied", 140));
            legendContainer.Controls.Add(CreateLegendPill(ReservedColor, ReservedBg, "Reserved", 280));
        }

        private RoundedPanel CreateLegendPill(Color dotColor, Color bgColor, string text, int x)
        {
            RoundedPanel pill = new RoundedPanel
            {
                BackColor = bgColor,
                BorderRadius = 15,
                Location = new Point(x, 5),
                Size = new Size(125, 30)
            };

            Panel dot = new Panel
            {
                BackColor = dotColor,
                Size = new Size(10, 10),
                Location = new Point(12, 10)
            };

            Label lbl = new Label
            {
                AutoSize = true,
                Text = text,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                ForeColor = dotColor,
                Location = new Point(28, 7)
            };

            pill.Controls.Add(dot);
            pill.Controls.Add(lbl);
            return pill;
        }

        // ===== Build Header Stats =====
        private void BuildHeaderStats()
        {
            int x = 0;
            lblStatAvailable = AddStatBadge("✓ Available: 0", AvailableColor, ref x);
            lblStatOccupied = AddStatBadge("● Occupied: 0", OccupiedColor, ref x);
            lblStatReserved = AddStatBadge("◆ Reserved: 0", ReservedColor, ref x);
            statsPanel.Size = new Size(x, 40);
        }

        private Label AddStatBadge(string text, Color color, ref int x)
        {
            Label lbl = new Label
            {
                AutoSize = true,
                Text = text,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = color,
                Location = new Point(x, 8),
                BackColor = Color.Transparent
            };
            statsPanel.Controls.Add(lbl);
            x += lbl.PreferredWidth + 25;
            return lbl;
        }

        // ===== Create Slot Buttons =====
        private void CreateCategorySlots(string category, TableLayoutPanel tlp, int rows, int cols)
        {
            tlp.Controls.Clear();
            tlp.RowStyles.Clear();
            tlp.ColumnStyles.Clear();
            tlp.RowCount = rows;
            tlp.ColumnCount = cols;

            for (int i = 0; i < cols; i++)
                tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / cols));
            for (int i = 0; i < rows; i++)
                tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));

            tlp.Height = rows * 55 + 10;

            List<string> slotIds = GetSlotsByCategory(category);

            for (int i = 0; i < slotIds.Count; i++)
            {
                int row = i / cols;
                int col = i % cols;

                RoundedButton btn = new RoundedButton
                {
                    Text = slotIds[i],
                    Tag = slotIds[i],
                    Dock = DockStyle.Fill,
                    Margin = new Padding(4),
                    BorderRadius = 10,
                    Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                    BackColor = AvailableBg,
                    ForeColor = AvailableColor,
                    Cursor = Cursors.Hand,
                    FlatStyle = FlatStyle.Flat
                };
                btn.FlatAppearance.BorderSize = 0;

                btn.MouseEnter += (s, e) =>
                {
                    if (btn.Tag != null)
                        btn.BackColor = ControlPaint.Dark(btn.BackColor, 0.05f);
                };
                btn.MouseLeave += (s, e) => ApplySlotColor(btn);
                btn.Click += SlotButton_Click;

                allSlotButtons.Add(btn);
                tlp.Controls.Add(btn, col, row);
            }
        }

        private void ApplySlotColor(Button btn)
        {
            string status = GetSlotStatus(btn.Tag.ToString());
            switch (status)
            {
                case "occupied":
                    btn.BackColor = OccupiedBg;
                    btn.ForeColor = OccupiedColor;
                    break;
                case "reserved":
                    btn.BackColor = ReservedBg;
                    btn.ForeColor = ReservedColor;
                    break;
                default:
                    btn.BackColor = AvailableBg;
                    btn.ForeColor = AvailableColor;
                    break;
            }
        }

        private string GetSlotStatus(string slotId)
        {
            if (slotStatusCache.TryGetValue(slotId, out string status))
                return status;
            return "available";
        }

        private List<string> GetSlotsByCategory(string category)
        {
            List<string> result = new List<string>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string query = "SELECT slot_id FROM slots WHERE category=@cat ORDER BY slot_id";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@cat", category);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                result.Add(reader["slot_id"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading " + category + ": " + ex.Message);
            }
            return result;
        }

        public void RefreshSlots()
        {
            int available = 0, occupied = 0, reserved = 0;
            slotStatusCache.Clear();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    // Use is_active as source of truth, fallback to slot status for reserved
                    string query = @"SELECT s.slot_id, 
                             CASE 
                                WHEN v.reg_number IS NOT NULL THEN 'occupied'
                                WHEN s.status = 'reserved' THEN 'reserved'
                                ELSE 'available'
                             END AS real_status
                             FROM slots s
                             LEFT JOIN vehicles v ON s.slot_id = v.slot_id AND v.is_active = 1";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string slotId = reader["slot_id"].ToString();
                            string status = reader["real_status"].ToString();
                            slotStatusCache[slotId] = status;

                            switch (status)
                            {
                                case "occupied": occupied++; break;
                                case "reserved": reserved++; break;
                                default: available++; break;
                            }

                            Button btn = allSlotButtons.FirstOrDefault(b => b.Tag.ToString() == slotId);
                            if (btn != null) ApplySlotColor(btn);
                        }
                    }
                }

                lblStatAvailable.Text = $"✓ Available: {available}";
                lblStatOccupied.Text = $"● Occupied: {occupied}";
                lblStatReserved.Text = $"◆ Reserved: {reserved}";

                // Auto-sync slots table to match vehicles table (fix inconsistencies)
                SyncSlotsWithVehicles();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Refresh Error: " + ex.Message);
            }
        }

        // ===== AUTO-SYNC: Fix slot statuses to match vehicles =====
        private void SyncSlotsWithVehicles()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    // Step 1: Mark slots as 'occupied' where there's an active vehicle
                    string fixOccupied = @"UPDATE slots s
                                   INNER JOIN vehicles v ON s.slot_id = v.slot_id 
                                   SET s.status = 'occupied'
                                   WHERE v.is_active = 1 AND s.status != 'occupied'";
                    using (MySqlCommand cmd = new MySqlCommand(fixOccupied, conn))
                        cmd.ExecuteNonQuery();

                    // Step 2: Mark slots as 'available' where there's NO active vehicle (and not reserved)
                    string fixAvailable = @"UPDATE slots s
                                    LEFT JOIN vehicles v ON s.slot_id = v.slot_id AND v.is_active = 1
                                    SET s.status = 'available'
                                    WHERE v.reg_number IS NULL 
                                    AND s.status = 'occupied'";
                    using (MySqlCommand cmd = new MySqlCommand(fixAvailable, conn))
                        cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Sync error: " + ex.Message);
            }
        }

        private void SlotButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;
            string slotId = btn.Tag.ToString();
            string currentStatus = GetSlotStatus(slotId);

            switch (currentStatus)
            {
                case "occupied":
                    if (MessageBox.Show($"Free slot {slotId}?", "Vehicle Exit",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        UpdateSlotStatus(slotId, "available");
                        RefreshSlots();
                    }
                    break;

                case "available":
                    if (MessageBox.Show($"Reserve slot {slotId}?", "Reserve",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        UpdateSlotStatus(slotId, "reserved");
                        RefreshSlots();
                    }
                    break;

                case "reserved":
                    if (MessageBox.Show($"Cancel reservation for {slotId}?", "Cancel",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        UpdateSlotStatus(slotId, "available");
                        RefreshSlots();
                    }
                    break;
            }
        }

        private void UpdateSlotStatus(string slotId, string status)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("UPDATE slots SET status=@s WHERE slot_id=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@s", status);
                        cmd.Parameters.AddWithValue("@id", slotId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update Error: " + ex.Message);
            }
        }
    }
}