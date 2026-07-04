using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace parkingManagementSystem
{
    public partial class DashBoard : UserControl
    {
        // ⚠️ UPDATE THIS if your MySQL username/password is different
        private readonly string connectionString = "server=127.0.0.1;user=root;password=;database=parking_db;";

        private System.Windows.Forms.Timer clockTimer;
        private System.Windows.Forms.Timer refreshTimer;

        // Totals loaded from slots table
        private int totalMotorcycle = 0;
        private int totalCompact = 0;
        private int totalSedan = 0;
        private int totalSUV = 0;
        private int totalSlots = 0;

        public DashBoard()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            LoadTotalsFromDB();

            BuildStatCards();
            BuildVehicleCards();
            BuildLegend();

            headerPanel.Resize += HeaderPanel_Resize;
            occupancyCard.Resize += OccupancyCard_Resize;
        }

        // ===== Load total slot counts =====
        private void LoadTotalsFromDB()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT category, COUNT(*) AS cnt FROM slots GROUP BY category";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string category = reader["category"].ToString();
                            int count = Convert.ToInt32(reader["cnt"]);
                            switch (category)
                            {
                                case "Motorcycle": totalMotorcycle = count; break;
                                case "Compact": totalCompact = count; break;
                                case "Sedan": totalSedan = count; break;
                                case "SUV": totalSUV = count; break;
                            }
                        }
                    }
                }
                totalSlots = totalMotorcycle + totalCompact + totalSedan + totalSUV;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadTotals error: " + ex.Message);
                totalMotorcycle = 15;
                totalCompact = 35;
                totalSedan = 25;
                totalSUV = 25;
                totalSlots = 100;
            }
        }

        // ===== Build Stat Cards =====
        private void BuildStatCards()
        {
            statsContainer.Controls.Add(
                CreateStatCard(Color.FromArgb(236, 64, 122), "P", "Total Slots", totalSlots.ToString(), out lblTotalValue, 0), 0, 0);
            statsContainer.Controls.Add(
                CreateStatCard(Color.FromArgb(34, 197, 94), "✓", "Available", "0", out lblAvailableValue, 1), 1, 0);
            statsContainer.Controls.Add(
                CreateStatCard(Color.FromArgb(239, 68, 68), "●", "Occupied", "0", out lblOccupiedValue, 2), 2, 0);
            statsContainer.Controls.Add(
                CreateStatCard(Color.FromArgb(245, 158, 11), "%", "Occupancy", "0%", out lblRateValue, 3), 3, 0);
        }

        private RoundedPanel CreateStatCard(Color accent, string icon, string title, string value, out Label valueLabel, int index)
        {
            RoundedPanel card = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderRadius = 15,
                Margin = new Padding(index == 0 ? 0 : 5, 0, index == 3 ? 0 : 5, 0)
            };

            RoundedPanel iconPnl = new RoundedPanel
            {
                BackColor = accent,
                BorderRadius = 10,
                Location = new Point(20, 22),
                Size = new Size(50, 50)
            };

            Label iconLbl = new Label
            {
                Text = icon,
                Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            iconPnl.Controls.Add(iconLbl);

            Label titleLbl = new Label
            {
                AutoSize = true,
                Text = title,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(120, 125, 140),
                Location = new Point(85, 25)
            };

            valueLabel = new Label
            {
                AutoSize = true,
                Text = value,
                Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 42, 58),
                Location = new Point(85, 45)
            };

            card.Controls.Add(iconPnl);
            card.Controls.Add(titleLbl);
            card.Controls.Add(valueLabel);

            return card;
        }

        // ===== Build Vehicle Cards =====
        private void BuildVehicleCards()
        {
            vehicleContainer.Controls.Add(
                CreateVehicleCard(Color.FromArgb(252, 231, 243), Color.FromArgb(236, 64, 122),
                    "🏍", "Motorcycle", $"{totalMotorcycle} / {totalMotorcycle}", out lblMotorValue, 0), 0, 0);
            vehicleContainer.Controls.Add(
                CreateVehicleCard(Color.FromArgb(203, 251, 241), Color.FromArgb(20, 184, 166),
                    "🚙", "Compact", $"{totalCompact} / {totalCompact}", out lblCompactValue, 1), 1, 0);
            vehicleContainer.Controls.Add(
                CreateVehicleCard(Color.FromArgb(223, 242, 254), Color.FromArgb(59, 130, 246),
                    "🚗", "Sedan", $"{totalSedan} / {totalSedan}", out lblSedanValue, 2), 2, 0);
            vehicleContainer.Controls.Add(
                CreateVehicleCard(Color.FromArgb(236, 252, 202), Color.FromArgb(132, 204, 22),
                    "🚐", "SUV", $"{totalSUV} / {totalSUV}", out lblSUVValue, 3), 3, 0);
        }

        private void OccupancyCard_Resize(object sender, EventArgs e)
        {
            // Calculate the best donut size based on card dimensions
            int availableHeight = occupancyCard.Height - 70; // minus title space
            int availableWidth = occupancyCard.Width - 160;  // minus legend space

            int donutSize = Math.Min(availableHeight, availableWidth);
            if (donutSize < 150) donutSize = 150;

            donutChart.Size = new Size(donutSize, donutSize);

            // Center the donut vertically
            donutChart.Location = new Point(20, (occupancyCard.Height - donutSize) / 2 + 15);

            // Position legend to the right of the donut, vertically centered
            int legendX = donutChart.Right + 20;
            int legendY = (occupancyCard.Height - legendPanel.Height) / 2 + 15;
            legendPanel.Location = new Point(legendX, legendY);
        }

        private RoundedPanel CreateVehicleCard(Color bgColor, Color accent, string icon, string title, string value, out Label valueLabel, int index)
        {
            RoundedPanel card = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                BackColor = bgColor,
                BorderRadius = 15,
                Margin = new Padding(index == 0 ? 0 : 5, 0, index == 3 ? 0 : 5, 0)
            };

            Label iconLbl = new Label
            {
                AutoSize = true,
                Text = icon,
                Font = new Font("Segoe UI Emoji", 24F),
                ForeColor = accent,
                Location = new Point(20, 15)
            };

            Label titleLbl = new Label
            {
                AutoSize = true,
                Text = title,
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 42, 58),
                Location = new Point(22, 65)
            };

            valueLabel = new Label
            {
                AutoSize = true,
                Text = value,
                Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold),
                ForeColor = accent,
                Location = new Point(22, 88)
            };

            card.Controls.Add(iconLbl);
            card.Controls.Add(titleLbl);
            card.Controls.Add(valueLabel);

            return card;
        }

        // ===== Build Legend =====
        private void BuildLegend()
        {
            Panel avBox = new Panel
            {
                BackColor = Color.FromArgb(34, 197, 94),
                Size = new Size(14, 14),
                Location = new Point(0, 14)
            };
            Label avLbl = new Label
            {
                AutoSize = true,
                Text = "Available",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(80, 85, 100),
                Location = new Point(22, 10)
            };

            Panel ocBox = new Panel
            {
                BackColor = Color.FromArgb(239, 68, 68),
                Size = new Size(14, 14),
                Location = new Point(0, 54)
            };
            Label ocLbl = new Label
            {
                AutoSize = true,
                Text = "Occupied",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(80, 85, 100),
                Location = new Point(22, 50)
            };

            legendPanel.Controls.Add(avBox);
            legendPanel.Controls.Add(avLbl);
            legendPanel.Controls.Add(ocBox);
            legendPanel.Controls.Add(ocLbl);
        }

        private void HeaderPanel_Resize(object sender, EventArgs e)
        {
            lblDateTime.Location = new Point(headerPanel.Width - lblDateTime.Width - 5, 10);
        }

        private void DashBoard_Load(object sender, EventArgs e)
        {
            clockTimer = new System.Windows.Forms.Timer();
            clockTimer.Interval = 1000;
            clockTimer.Tick += ClockTimer_Tick;
            clockTimer.Start();
            UpdateDateTime();

            refreshTimer = new System.Windows.Forms.Timer();
            refreshTimer.Interval = 5000;
            refreshTimer.Tick += (s, ev) => RefreshStats();
            refreshTimer.Start();

            RefreshStats();
        }

        private void ClockTimer_Tick(object sender, EventArgs e) => UpdateDateTime();

        private void UpdateDateTime()
        {
            lblDateTime.Text = DateTime.Now.ToString("dddd, MMM dd, yyyy  •  hh:mm:ss tt");
            HeaderPanel_Resize(headerPanel, EventArgs.Empty);
        }

        // ===== Refresh All Stats =====
        public void RefreshStats()
        {
            int motorOccupied = 0, compactOccupied = 0, sedanOccupied = 0, suvOccupied = 0;

            try
            {
                var occupiedCounts = GetOccupiedCountsFromDB();
                occupiedCounts.TryGetValue("Motorcycle", out motorOccupied);
                occupiedCounts.TryGetValue("Compact", out compactOccupied);
                occupiedCounts.TryGetValue("Sedan", out sedanOccupied);
                occupiedCounts.TryGetValue("SUV", out suvOccupied);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Refresh error: " + ex.Message);
            }

            int totalOccupied = motorOccupied + compactOccupied + sedanOccupied + suvOccupied;
            int totalAvailable = totalSlots - totalOccupied;
            double rate = totalSlots > 0 ? (double)totalOccupied / totalSlots * 100 : 0;

            lblTotalValue.Text = totalSlots.ToString();
            lblAvailableValue.Text = totalAvailable.ToString();
            lblOccupiedValue.Text = totalOccupied.ToString();
            lblRateValue.Text = $"{rate:F0}%";

            lblMotorValue.Text = $"{totalMotorcycle - motorOccupied} / {totalMotorcycle}";
            lblCompactValue.Text = $"{totalCompact - compactOccupied} / {totalCompact}";
            lblSedanValue.Text = $"{totalSedan - sedanOccupied} / {totalSedan}";
            lblSUVValue.Text = $"{totalSUV - suvOccupied} / {totalSUV}";

            donutChart.SetValues(totalAvailable, totalOccupied);

            LoadRecentActivity();
        }

        // ===== Occupied Counts =====
        private Dictionary<string, int> GetOccupiedCountsFromDB()
        {
            var counts = new Dictionary<string, int>();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT category, COUNT(*) AS cnt 
                                 FROM slots 
                                 WHERE status = 'occupied' 
                                 GROUP BY category";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        counts[reader["category"].ToString()] = Convert.ToInt32(reader["cnt"]);
                    }
                }
            }
            return counts;
        }

        // ===== Recent Activity =====
        private void LoadRecentActivity()
        {
            activityListPanel.Controls.Clear();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT reg_number, category, entry_time, exit_time 
                             FROM vehicles 
                             ORDER BY 
                                 CASE WHEN exit_time IS NULL THEN entry_time ELSE exit_time END DESC
                             LIMIT 10";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Collect all items first
                        var items = new List<Panel>();
                        while (reader.Read())
                        {
                            string plate = reader["reg_number"].ToString();
                            string type = reader["category"].ToString();
                            DateTime entryTime = Convert.ToDateTime(reader["entry_time"]);
                            bool isExit = reader["exit_time"] != DBNull.Value;
                            DateTime actionTime = isExit ? Convert.ToDateTime(reader["exit_time"]) : entryTime;

                            items.Add(CreateActivityItem(plate, type, actionTime, isExit, 0));
                        }

                        // Add in reverse so first record appears on top (Dock.Top stacks bottom-up)
                        for (int i = items.Count - 1; i >= 0; i--)
                        {
                            activityListPanel.Controls.Add(items[i]);
                        }

                        if (items.Count == 0)
                        {
                            activityListPanel.Controls.Add(new Label
                            {
                                Text = "📋  No recent activity yet.\nEntry and exit logs will appear here.",
                                Font = new Font("Segoe UI", 10F),
                                ForeColor = Color.FromArgb(160, 165, 180),
                                TextAlign = ContentAlignment.MiddleCenter,
                                Dock = DockStyle.Fill
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Activity error: " + ex.Message);
                activityListPanel.Controls.Add(new Label
                {
                    Text = "⚠️ Could not load activity.\nCheck database connection.",
                    Font = new Font("Segoe UI", 10F),
                    ForeColor = Color.FromArgb(160, 165, 180),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill
                });
            }
        }

        private Panel CreateActivityItem(string plate, string type, DateTime time, bool isExit, int y)
        {
            Panel item = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.FromArgb(247, 250, 252),
                Margin = new Padding(0, 0, 0, 5)
            };

            Label icon = new Label
            {
                Text = isExit ? "⬅" : "➡",
                Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold),
                ForeColor = isExit ? Color.FromArgb(239, 68, 68) : Color.FromArgb(34, 197, 94),
                Location = new Point(10, 12),
                AutoSize = true
            };

            Label plateLbl = new Label
            {
                Text = plate,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 42, 58),
                Location = new Point(40, 8),
                AutoSize = true
            };

            Label detailLbl = new Label
            {
                Text = $"{type} • {(isExit ? "Exited" : "Entered")}",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(120, 125, 140),
                Location = new Point(40, 27),
                AutoSize = true
            };

            Label timeLbl = new Label
            {
                Text = GetTimeAgo(time),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(160, 165, 180),
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            item.Controls.Add(icon);
            item.Controls.Add(plateLbl);
            item.Controls.Add(detailLbl);
            item.Controls.Add(timeLbl);

            // Position time label on the right side (after control is sized)
            item.Resize += (s, e) =>
            {
                timeLbl.Location = new Point(item.Width - timeLbl.Width - 15, 17);
            };

            return item;
        }

        private string GetTimeAgo(DateTime time)
        {
            TimeSpan diff = DateTime.Now - time;
            if (diff.TotalMinutes < 1) return "just now";
            if (diff.TotalMinutes < 60) return $"{(int)diff.TotalMinutes} min ago";
            if (diff.TotalHours < 24) return $"{(int)diff.TotalHours} hr ago";
            return time.ToString("MMM dd, hh:mm tt");
        }
    }
}