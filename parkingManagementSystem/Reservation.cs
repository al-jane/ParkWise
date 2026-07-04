using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace parkingManagementSystem
{
    public partial class Reservations : UserControl
    {
        private parkingGrid parkingGridRef;
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

        // Status row backgrounds
        private readonly Color ActiveRowBg = Color.FromArgb(223, 242, 254);
        private readonly Color CheckedInRowBg = Color.FromArgb(220, 252, 231);
        private readonly Color ExpiredRowBg = Color.FromArgb(254, 226, 226);
        private readonly Color CancelledRowBg = Color.FromArgb(243, 244, 246);

        private System.Windows.Forms.Timer refreshTimer;
        private int currentTab = 0;

        public Reservations()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            BuildStatsCards();
            BuildFormCard();
            BuildTabs();
            BuildTables();

            this.Load += Reservations_Load;
        }

        public void SetParkingGrid(parkingGrid grid) => parkingGridRef = grid;

        private void Reservations_Load(object sender, EventArgs e)
        {
            AutoExpireReservations();
            RefreshAll();
            ShowTab(0);

            refreshTimer = new System.Windows.Forms.Timer();
            refreshTimer.Interval = 30000; // 30 seconds
            refreshTimer.Tick += (s, ev) =>
            {
                AutoExpireReservations();
                RefreshAll();
            };
            refreshTimer.Start();
        }

        // ===== BUILD STAT CARDS =====
        private void BuildStatsCards()
        {
            statsContainer.Controls.Add(
                CreateStatCard(BlueAccent, "📅", "Active", "0", out lblActiveCount, 0), 0, 0);
            statsContainer.Controls.Add(
                CreateStatCard(AmberAccent, "⏳", "Expiring Soon", "0", out lblExpiringSoon, 1), 1, 0);
            statsContainer.Controls.Add(
                CreateStatCard(GreenAccent, "✓", "Checked In Today", "0", out lblCheckedInToday, 2), 2, 0);
            statsContainer.Controls.Add(
                CreateStatCard(RedAccent, "⚠", "Expired", "0", out lblExpiredCount, 3), 3, 0);
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

        // ===== BUILD FORM =====
        private void BuildFormCard()
        {
            RoundedPanel formCard = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                BackColor = CardBg,
                BorderRadius = 15
            };

            Label lblTitle = new Label
            {
                AutoSize = true,
                Text = "➕  Create New Reservation",
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                ForeColor = TextPrimary,
                Location = new Point(25, 18)
            };

            AddField(formCard, "Registration Number", 25, 55, out txtRegNum);
            AddField(formCard, "Owner's Full Name", 315, 55, out txtOwnerName);
            AddField(formCard, "Owner's Contact", 605, 55, out txtContact);

            AddCombo(formCard, "Vehicle Category", 25, 120, out cmbCategory,
                new string[] { "Motorcycle", "Compact", "Sedan", "SUV" });

            // Date/time picker
            Label lblDate = new Label
            {
                AutoSize = true,
                Text = "Reservation Date & Time",
                Font = new Font("Segoe UI", 9F),
                ForeColor = TextSecondary,
                Location = new Point(315, 120)
            };

            dtpReservation = new DateTimePicker
            {
                Font = new Font("Segoe UI", 10.5F),
                Location = new Point(315, 142),
                Size = new Size(270, 32),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "MMM dd, yyyy  hh:mm tt",
                Value = DateTime.Now.AddHours(1),
                MinDate = DateTime.Now
            };

            // Duration
            Label lblDuration = new Label
            {
                AutoSize = true,
                Text = "Hold Duration (hours)",
                Font = new Font("Segoe UI", 9F),
                ForeColor = TextSecondary,
                Location = new Point(605, 120)
            };

            numDuration = new NumericUpDown
            {
                Font = new Font("Segoe UI", 10.5F),
                Location = new Point(605, 142),
                Size = new Size(120, 32),
                BackColor = InputBg,
                ForeColor = TextPrimary,
                Minimum = 1,
                Maximum = 24,
                Value = 2,
                BorderStyle = BorderStyle.FixedSingle
            };

            btnCreate = new RoundedButton
            {
                Text = "📅  Create Reservation",
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                BackColor = PinkAccent,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                BorderRadius = 10,
                Size = new Size(250, 40),
                Location = new Point(895, 140),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnCreate.FlatAppearance.BorderSize = 0;
            btnCreate.Click += BtnCreate_Click;

            formCard.Controls.Add(lblTitle);
            formCard.Controls.Add(lblDate);
            formCard.Controls.Add(dtpReservation);
            formCard.Controls.Add(lblDuration);
            formCard.Controls.Add(numDuration);
            formCard.Controls.Add(btnCreate);

            formCard.Resize += (s, e) =>
            {
                btnCreate.Location = new Point(formCard.Width - 275, 140);
            };

            formPanel.Controls.Add(formCard);
        }

        // ===== BUILD TABS =====
        private void BuildTabs()
        {
            tabBtnActive = CreateTabButton("📅  Active", 0);
            tabBtnExpired = CreateTabButton("⚠  Expired", 170);
            tabBtnAll = CreateTabButton("📊  All Reservations", 340);

            tabBtnActive.Click += (s, e) => ShowTab(0);
            tabBtnExpired.Click += (s, e) => ShowTab(1);
            tabBtnAll.Click += (s, e) => ShowTab(2);

            tabsPanel.Controls.Add(tabBtnActive);
            tabsPanel.Controls.Add(tabBtnExpired);
            tabsPanel.Controls.Add(tabBtnAll);
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
                Size = new Size(160, 40),
                Location = new Point(x, 5),
                Cursor = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 }
            };
        }

        private void ShowTab(int index)
        {
            currentTab = index;

            tabBtnActive.BackColor = InactiveTab;
            tabBtnActive.ForeColor = InactiveTabText;
            tabBtnExpired.BackColor = InactiveTab;
            tabBtnExpired.ForeColor = InactiveTabText;
            tabBtnAll.BackColor = InactiveTab;
            tabBtnAll.ForeColor = InactiveTabText;

            switch (index)
            {
                case 0:
                    tabBtnActive.BackColor = BlueAccent;
                    tabBtnActive.ForeColor = Color.White;
                    activeCard.Visible = true;
                    expiredCard.Visible = false;
                    allCard.Visible = false;
                    break;
                case 1:
                    tabBtnExpired.BackColor = RedAccent;
                    tabBtnExpired.ForeColor = Color.White;
                    activeCard.Visible = false;
                    expiredCard.Visible = true;
                    allCard.Visible = false;
                    break;
                case 2:
                    tabBtnAll.BackColor = PinkAccent;
                    tabBtnAll.ForeColor = Color.White;
                    activeCard.Visible = false;
                    expiredCard.Visible = false;
                    allCard.Visible = true;
                    break;
            }

            // Show action buttons only on active tab
            bottomBar.Visible = (index == 0);
        }

        // ===== BUILD TABLES =====
        private void BuildTables()
        {
            Panel cardsHolder = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };

            bottomBar = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 65,
                BackColor = Color.Transparent
            };

            // Active reservations
            activeCard = BuildTableCard(out dgvActive);
            AddColumns(dgvActive, new[] {
                ("Registration #", 13), ("Owner", 15), ("Contact", 13), ("Category", 10),
                ("Slot", 7), ("Reserved For", 17), ("Expires", 17), ("Status", 8)
            });

            // Expired
            expiredCard = BuildTableCard(out dgvExpired);
            AddColumns(dgvExpired, new[] {
                ("Registration #", 13), ("Owner", 15), ("Contact", 13), ("Category", 10),
                ("Slot", 7), ("Reserved For", 17), ("Expired At", 17), ("Status", 8)
            });

            // All
            allCard = BuildTableCard(out dgvAll);
            AddColumns(dgvAll, new[] {
                ("Registration #", 12), ("Owner", 14), ("Category", 10), ("Slot", 7),
                ("Reserved For", 16), ("Expiry", 16), ("Created", 15), ("Status", 10)
            });

            expiredCard.Visible = false;
            allCard.Visible = false;

            cardsHolder.Controls.Add(activeCard);
            cardsHolder.Controls.Add(expiredCard);
            cardsHolder.Controls.Add(allCard);

            // Action buttons
            btnCheckIn = new RoundedButton
            {
                Text = "✓  Check In (Convert to Active)",
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                BackColor = GreenAccent,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                BorderRadius = 10,
                Size = new Size(260, 42),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnCheckIn.FlatAppearance.BorderSize = 0;
            btnCheckIn.Click += BtnCheckIn_Click;

            btnCancel = new RoundedButton
            {
                Text = "✗  Cancel Reservation",
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                BackColor = RedAccent,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                BorderRadius = 10,
                Size = new Size(200, 42),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += BtnCancel_Click;

            bottomBar.Controls.Add(btnCheckIn);
            bottomBar.Controls.Add(btnCancel);

            bottomBar.Resize += (s, e) =>
            {
                btnCheckIn.Location = new Point(bottomBar.Width - 275, 10);
                btnCancel.Location = new Point(bottomBar.Width - 490, 10);
            };

            contentPanel.Controls.Add(cardsHolder);
            contentPanel.Controls.Add(bottomBar);
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
                capturedDgv.Size = new Size(card.Width - 40, card.Height - 40);
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

        // ===== HELPERS =====
        private void AddField(Control parent, string labelText, int x, int y, out TextBox textBox)
        {
            Label lbl = new Label
            {
                AutoSize = true,
                Text = labelText,
                Font = new Font("Segoe UI", 9F),
                ForeColor = TextSecondary,
                Location = new Point(x, y)
            };

            textBox = new TextBox
            {
                Font = new Font("Segoe UI", 10.5F),
                Location = new Point(x, y + 22),
                Size = new Size(270, 32),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = InputBg,
                ForeColor = TextPrimary
            };

            parent.Controls.Add(lbl);
            parent.Controls.Add(textBox);
        }

        private void AddCombo(Control parent, string labelText, int x, int y, out ComboBox combo, string[] items)
        {
            Label lbl = new Label
            {
                AutoSize = true,
                Text = labelText,
                Font = new Font("Segoe UI", 9F),
                ForeColor = TextSecondary,
                Location = new Point(x, y)
            };

            combo = new ComboBox
            {
                Font = new Font("Segoe UI", 10.5F),
                Location = new Point(x, y + 22),
                Size = new Size(270, 32),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = InputBg,
                ForeColor = TextPrimary,
                FlatStyle = FlatStyle.Flat
            };
            combo.Items.AddRange(items);

            parent.Controls.Add(lbl);
            parent.Controls.Add(combo);
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

        // ========== CREATE RESERVATION ==========
        private void BtnCreate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRegNum.Text))
            { MessageBox.Show("Please enter Registration Number!", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (string.IsNullOrWhiteSpace(txtOwnerName.Text))
            { MessageBox.Show("Please enter Owner's Name!", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (string.IsNullOrWhiteSpace(txtContact.Text))
            { MessageBox.Show("Please enter Owner's Contact!", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (cmbCategory.SelectedIndex == -1)
            { MessageBox.Show("Please select Category!", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            string category = cmbCategory.SelectedItem.ToString();
            string slot = GetAvailableSlot(category);

            if (string.IsNullOrEmpty(slot))
            {
                MessageBox.Show($"No available slots for {category}!", "Full", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DateTime reservationTime = dtpReservation.Value;
            DateTime expiryTime = reservationTime.AddHours((double)numDuration.Value);

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    // Insert reservation
                    string insertQuery = @"INSERT INTO reservations 
                        (reg_number, owner_name, owner_contact, category, slot_id, reservation_time, expiry_time, status) 
                        VALUES (@reg, @owner, @contact, @cat, @slot, @rtime, @etime, 'active')";

                    using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@reg", txtRegNum.Text.Trim());
                        cmd.Parameters.AddWithValue("@owner", txtOwnerName.Text.Trim());
                        cmd.Parameters.AddWithValue("@contact", txtContact.Text.Trim());
                        cmd.Parameters.AddWithValue("@cat", category);
                        cmd.Parameters.AddWithValue("@slot", slot);
                        cmd.Parameters.AddWithValue("@rtime", reservationTime);
                        cmd.Parameters.AddWithValue("@etime", expiryTime);
                        cmd.ExecuteNonQuery();
                    }

                    // Mark slot as reserved
                    using (MySqlCommand cmd = new MySqlCommand(
                        "UPDATE slots SET status='reserved' WHERE slot_id=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", slot);
                        cmd.ExecuteNonQuery();
                    }
                }

                parkingGridRef?.RefreshSlots();

                MessageBox.Show($"Reservation created!\n\nRegistration: {txtRegNum.Text}\nSlot: {slot}\nReserved for: {reservationTime:MMM dd, yyyy hh:mm tt}\nExpires: {expiryTime:MMM dd, yyyy hh:mm tt}",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearForm();
                RefreshAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private string GetAvailableSlot(string category)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    // Only pick slots not occupied and not already reserved
                    string query = @"SELECT s.slot_id FROM slots s
                                     LEFT JOIN vehicles v ON s.slot_id = v.slot_id AND v.is_active = 1
                                     WHERE s.category = @cat 
                                     AND s.status = 'available'
                                     AND v.reg_number IS NULL
                                     ORDER BY s.slot_id LIMIT 1";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@cat", category);
                        object result = cmd.ExecuteScalar();
                        return result?.ToString();
                    }
                }
            }
            catch { return null; }
        }

        private void ClearForm()
        {
            txtRegNum.Clear();
            txtOwnerName.Clear();
            txtContact.Clear();
            cmbCategory.SelectedIndex = -1;
            dtpReservation.Value = DateTime.Now.AddHours(1);
            numDuration.Value = 2;
            txtRegNum.Focus();
        }

        // ========== AUTO-EXPIRE ==========
        private void AutoExpireReservations()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    // Find active reservations past expiry
                    string findQuery = "SELECT slot_id FROM reservations WHERE status='active' AND expiry_time < NOW()";
                    var expiredSlots = new System.Collections.Generic.List<string>();

                    using (MySqlCommand cmd = new MySqlCommand(findQuery, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            expiredSlots.Add(reader["slot_id"].ToString());
                    }

                    if (expiredSlots.Count == 0) return;

                    // Mark as expired
                    using (MySqlCommand cmd = new MySqlCommand(
                        "UPDATE reservations SET status='expired' WHERE status='active' AND expiry_time < NOW()", conn))
                        cmd.ExecuteNonQuery();

                    // Free the slots (if not occupied by active vehicle)
                    foreach (string slot in expiredSlots)
                    {
                        using (MySqlCommand cmd = new MySqlCommand(
                            @"UPDATE slots SET status='available' 
                              WHERE slot_id=@id AND status='reserved'
                              AND NOT EXISTS (SELECT 1 FROM (SELECT slot_id FROM vehicles WHERE is_active=1) v WHERE v.slot_id=@id)", conn))
                        {
                            cmd.Parameters.AddWithValue("@id", slot);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    parkingGridRef?.RefreshSlots();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Auto-expire error: " + ex.Message);
            }
        }

        // ========== LOAD DATA ==========
        public void RefreshAll()
        {
            LoadActive();
            LoadExpired();
            LoadAll();
            UpdateStats();
        }

        private void LoadActive()
        {
            try
            {
                dgvActive.Rows.Clear();
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string query = @"SELECT reg_number, owner_name, owner_contact, category, slot_id,
                                     reservation_time, expiry_time 
                                     FROM reservations 
                                     WHERE status = 'active'
                                     ORDER BY reservation_time ASC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime expiry = Convert.ToDateTime(reader["expiry_time"]);
                            bool expiringSoon = (expiry - DateTime.Now).TotalMinutes < 30;
                            string statusTag = expiringSoon ? "⏳ SOON" : "📅 ACTIVE";

                            int idx = dgvActive.Rows.Add(
                                reader["reg_number"].ToString(),
                                reader["owner_name"].ToString(),
                                reader["owner_contact"].ToString(),
                                reader["category"].ToString(),
                                reader["slot_id"].ToString(),
                                Convert.ToDateTime(reader["reservation_time"]).ToString("MMM dd, yyyy hh:mm tt"),
                                expiry.ToString("MMM dd, yyyy hh:mm tt"),
                                statusTag);
                            dgvActive.Rows[idx].DefaultCellStyle.BackColor = expiringSoon ? ExpiredRowBg : ActiveRowBg;
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Load active error: " + ex.Message); }
        }

        private void LoadExpired()
        {
            try
            {
                dgvExpired.Rows.Clear();
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string query = @"SELECT reg_number, owner_name, owner_contact, category, slot_id,
                                     reservation_time, expiry_time 
                                     FROM reservations 
                                     WHERE status = 'expired'
                                     ORDER BY expiry_time DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int idx = dgvExpired.Rows.Add(
                                reader["reg_number"].ToString(),
                                reader["owner_name"].ToString(),
                                reader["owner_contact"].ToString(),
                                reader["category"].ToString(),
                                reader["slot_id"].ToString(),
                                Convert.ToDateTime(reader["reservation_time"]).ToString("MMM dd, yyyy hh:mm tt"),
                                Convert.ToDateTime(reader["expiry_time"]).ToString("MMM dd, yyyy hh:mm tt"),
                                "⚠ EXPIRED");
                            dgvExpired.Rows[idx].DefaultCellStyle.BackColor = ExpiredRowBg;
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Load expired error: " + ex.Message); }
        }

        private void LoadAll()
        {
            try
            {
                dgvAll.Rows.Clear();
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string query = @"SELECT reg_number, owner_name, category, slot_id,
                                     reservation_time, expiry_time, created_at, status 
                                     FROM reservations 
                                     ORDER BY created_at DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string status = reader["status"].ToString();
                            string statusTag;
                            Color rowBg;

                            switch (status)
                            {
                                case "active": statusTag = "📅 ACTIVE"; rowBg = ActiveRowBg; break;
                                case "checked_in": statusTag = "✅ CHECKED IN"; rowBg = CheckedInRowBg; break;
                                case "expired": statusTag = "⚠ EXPIRED"; rowBg = ExpiredRowBg; break;
                                case "cancelled": statusTag = "✗ CANCELLED"; rowBg = CancelledRowBg; break;
                                default: statusTag = status.ToUpper(); rowBg = Color.White; break;
                            }

                            int idx = dgvAll.Rows.Add(
                                reader["reg_number"].ToString(),
                                reader["owner_name"].ToString(),
                                reader["category"].ToString(),
                                reader["slot_id"].ToString(),
                                Convert.ToDateTime(reader["reservation_time"]).ToString("MMM dd hh:mm tt"),
                                Convert.ToDateTime(reader["expiry_time"]).ToString("MMM dd hh:mm tt"),
                                Convert.ToDateTime(reader["created_at"]).ToString("MMM dd hh:mm tt"),
                                statusTag);
                            dgvAll.Rows[idx].DefaultCellStyle.BackColor = rowBg;
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Load all error: " + ex.Message); }
        }

        private void UpdateStats()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM reservations WHERE status='active'", conn))
                        lblActiveCount.Text = cmd.ExecuteScalar().ToString();

                    using (MySqlCommand cmd = new MySqlCommand(
                        "SELECT COUNT(*) FROM reservations WHERE status='active' AND expiry_time < DATE_ADD(NOW(), INTERVAL 30 MINUTE)", conn))
                        lblExpiringSoon.Text = cmd.ExecuteScalar().ToString();

                    using (MySqlCommand cmd = new MySqlCommand(
                        "SELECT COUNT(*) FROM reservations WHERE status='checked_in' AND DATE(created_at) = CURDATE()", conn))
                        lblCheckedInToday.Text = cmd.ExecuteScalar().ToString();

                    using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM reservations WHERE status='expired'", conn))
                        lblExpiredCount.Text = cmd.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Stats error: " + ex.Message);
            }
        }

        // ========== CHECK-IN ==========
        private void BtnCheckIn_Click(object sender, EventArgs e)
        {
            if (dgvActive.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a reservation!", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow row = dgvActive.SelectedRows[0];
            string regNum = row.Cells[0].Value.ToString();
            string owner = row.Cells[1].Value.ToString();
            string contact = row.Cells[2].Value.ToString();
            string category = row.Cells[3].Value.ToString();
            string slot = row.Cells[4].Value.ToString();

            DialogResult result = MessageBox.Show(
                $"Check in this reservation?\n\nRegistration: {regNum}\nOwner: {owner}\nSlot: {slot}\n\nThis will convert it to an active vehicle entry.",
                "Confirm Check-In", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    // Insert into vehicles
                    string insertVehicle = @"INSERT INTO vehicles 
                        (reg_number, company, category, owner_name, owner_contact, slot_id, is_active) 
                        VALUES (@reg, '', @cat, @owner, @contact, @slot, 1)";

                    using (MySqlCommand cmd = new MySqlCommand(insertVehicle, conn))
                    {
                        cmd.Parameters.AddWithValue("@reg", regNum);
                        cmd.Parameters.AddWithValue("@cat", category);
                        cmd.Parameters.AddWithValue("@owner", owner);
                        cmd.Parameters.AddWithValue("@contact", contact);
                        cmd.Parameters.AddWithValue("@slot", slot);
                        cmd.ExecuteNonQuery();
                    }

                    // Update reservation
                    using (MySqlCommand cmd = new MySqlCommand(
                        "UPDATE reservations SET status='checked_in' WHERE reg_number=@reg AND slot_id=@slot AND status='active'", conn))
                    {
                        cmd.Parameters.AddWithValue("@reg", regNum);
                        cmd.Parameters.AddWithValue("@slot", slot);
                        cmd.ExecuteNonQuery();
                    }

                    // Update slot status
                    using (MySqlCommand cmd = new MySqlCommand(
                        "UPDATE slots SET status='occupied' WHERE slot_id=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", slot);
                        cmd.ExecuteNonQuery();
                    }
                }

                parkingGridRef?.RefreshSlots();

                MessageBox.Show($"Check-in successful!\n\n{regNum} is now parked at {slot}.",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                RefreshAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Check-in error: " + ex.Message, "Error");
            }
        }

        // ========== CANCEL ==========
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (dgvActive.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a reservation!", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow row = dgvActive.SelectedRows[0];
            string regNum = row.Cells[0].Value.ToString();
            string slot = row.Cells[4].Value.ToString();

            DialogResult result = MessageBox.Show(
                $"Cancel this reservation?\n\nRegistration: {regNum}\nSlot: {slot}",
                "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(
                        "UPDATE reservations SET status='cancelled' WHERE reg_number=@reg AND slot_id=@slot AND status='active'", conn))
                    {
                        cmd.Parameters.AddWithValue("@reg", regNum);
                        cmd.Parameters.AddWithValue("@slot", slot);
                        cmd.ExecuteNonQuery();
                    }

                    // Free slot if not occupied
                    using (MySqlCommand cmd = new MySqlCommand(
                        @"UPDATE slots SET status='available' 
                          WHERE slot_id=@id AND status='reserved'
                          AND NOT EXISTS (SELECT 1 FROM (SELECT slot_id FROM vehicles WHERE is_active=1) v WHERE v.slot_id=@id)", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", slot);
                        cmd.ExecuteNonQuery();
                    }
                }

                parkingGridRef?.RefreshSlots();

                MessageBox.Show("Reservation cancelled.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cancel error: " + ex.Message, "Error");
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            refreshTimer?.Stop();
            base.OnHandleDestroyed(e);
        }
    }
}