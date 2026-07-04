using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace parkingManagementSystem
{
    public partial class EntryExit : UserControl
    {
        private parkingGrid parkingGridRef;
        private parkBilling parkBillingRef;
        private string connStr = "Server=localhost;Database=parking_db;Uid=root;Pwd=;";

        // Light theme colors
        private readonly Color BgColor = Color.FromArgb(247, 250, 252);
        private readonly Color CardBg = Color.White;
        private readonly Color InputBg = Color.FromArgb(248, 249, 251);
        private readonly Color InputBorder = Color.FromArgb(226, 229, 235);
        private readonly Color TextPrimary = Color.FromArgb(40, 42, 58);
        private readonly Color TextSecondary = Color.FromArgb(120, 125, 140);
        private readonly Color PinkAccent = Color.FromArgb(236, 64, 122);
        private readonly Color InactiveTab = Color.White;
        private readonly Color InactiveTabText = Color.FromArgb(80, 85, 100);

        public EntryExit()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            BuildEntryPanel();
            BuildExitPanel();
        }

        public void SetParkingGrid(parkingGrid grid) => parkingGridRef = grid;
        public void SetParkBilling(parkBilling billing) => parkBillingRef = billing;

        private void EntryExit_Load(object sender, EventArgs e)
        {
            ShowEntryPanel();
            LoadParkedVehicles();
        }

        // ========== BUILD ENTRY PANEL ==========
        private void BuildEntryPanel()
        {
            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = Color.Transparent
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 290F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // ===== FORM CARD =====
            RoundedPanel formCard = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                BackColor = CardBg,
                BorderRadius = 15,
                Margin = new Padding(0, 0, 0, 15)
            };

            Label lblFormTitle = new Label
            {
                AutoSize = true,
                Text = "📝  Register Vehicle Entry",
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                ForeColor = TextPrimary,
                Location = new Point(25, 20)
            };

            AddField(formCard, "Registration Number", 25, 60, out txtRegNum);
            AddField(formCard, "Vehicle's Company Name", 25, 130, out txtCompany);
            AddCombo(formCard, "Vehicle Category", 25, 200, out cmbCategory,
                new string[] { "Motorcycle", "Compact", "Sedan", "SUV" });

            AddField(formCard, "Owner's Full Name", 445, 60, out txtOwnerName);
            AddField(formCard, "Owner's Contact", 445, 130, out txtContact);

            btnRegister = new RoundedButton
            {
                Text = "✓  Register Entry and Assign Slot",
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                BackColor = PinkAccent,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                BorderRadius = 10,
                Size = new Size(360, 45),
                Location = new Point(505, 205),
                Cursor = Cursors.Hand
            };
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Click += btnRegister_Click;

            formCard.Controls.Add(lblFormTitle);
            formCard.Controls.Add(btnRegister);

            // ===== PARKED VEHICLES TABLE =====
            RoundedPanel tableCard = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                BackColor = CardBg,
                BorderRadius = 15
            };

            Label lblTableTitle = new Label
            {
                AutoSize = true,
                Text = "🚗  Currently Parked Vehicles",
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                ForeColor = TextPrimary,
                Location = new Point(25, 20)
            };

            dgvParked = CreateStyledDataGridView();
            dgvParked.Location = new Point(25, 60);
            dgvParked.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            dgvParked.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Registration #", Name = "colReg", FillWeight = 15 });
            dgvParked.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Company", Name = "colCompany", FillWeight = 15 });
            dgvParked.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Category", Name = "colCategory", FillWeight = 12 });
            dgvParked.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Owner", Name = "colOwner", FillWeight = 18 });
            dgvParked.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Contact", Name = "colContact", FillWeight = 12 });
            dgvParked.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Slot", Name = "colSlot", FillWeight = 8 });
            dgvParked.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Entry Time", Name = "colEntry", FillWeight = 20 });

            foreach (DataGridViewColumn col in dgvParked.Columns)
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            tableCard.Controls.Add(dgvParked);
            tableCard.Controls.Add(lblTableTitle);

            tableCard.Resize += (s, e) =>
            {
                dgvParked.Size = new Size(tableCard.Width - 50, tableCard.Height - 80);
            };

            layout.Controls.Add(formCard, 0, 0);
            layout.Controls.Add(tableCard, 0, 1);
            entryPanel.Controls.Add(layout);
        }

        // ========== BUILD EXIT PANEL ==========
        private void BuildExitPanel()
        {
            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = Color.Transparent
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 160F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // ===== SEARCH CARD =====
            RoundedPanel searchCard = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                BackColor = CardBg,
                BorderRadius = 15,
                Margin = new Padding(0, 0, 0, 15)
            };

            Label lblSearchTitle = new Label
            {
                AutoSize = true,
                Text = "🔍  Process Vehicle Exit",
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                ForeColor = TextPrimary,
                Location = new Point(25, 20)
            };

            Label lblSearchSubtitle = new Label
            {
                AutoSize = true,
                Text = "Search by registration number or owner name",
                Font = new Font("Segoe UI", 9F),
                ForeColor = TextSecondary,
                Location = new Point(25, 50)
            };

            txtSearchExit = new TextBox
            {
                Font = new Font("Segoe UI", 11F),
                Location = new Point(25, 85),
                Size = new Size(500, 35),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = InputBg,
                ForeColor = TextPrimary
            };
            txtSearchExit.KeyDown += txtSearchExit_KeyDown;

            btnSearchExit = new RoundedButton
            {
                Text = "🔍  Search",
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                BackColor = PinkAccent,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                BorderRadius = 8,
                Size = new Size(130, 35),
                Location = new Point(540, 85),
                Cursor = Cursors.Hand
            };
            btnSearchExit.FlatAppearance.BorderSize = 0;
            btnSearchExit.Click += btnSearchExit_Click;

            searchCard.Controls.Add(lblSearchTitle);
            searchCard.Controls.Add(lblSearchSubtitle);
            searchCard.Controls.Add(txtSearchExit);
            searchCard.Controls.Add(btnSearchExit);

            // ===== RESULTS CARD =====
            RoundedPanel resultsCard = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                BackColor = CardBg,
                BorderRadius = 15
            };

            Label lblResultsTitle = new Label
            {
                AutoSize = true,
                Text = "📋  Search Results",
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                ForeColor = TextPrimary,
                Location = new Point(25, 20)
            };

            dgvSearchResults = CreateStyledDataGridView();
            dgvSearchResults.Location = new Point(25, 60);
            dgvSearchResults.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            dgvSearchResults.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Registration #", Name = "sReg", FillWeight = 15 });
            dgvSearchResults.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Owner", Name = "sOwner", FillWeight = 20 });
            dgvSearchResults.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Contact", Name = "sContact", FillWeight = 15 });
            dgvSearchResults.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Category", Name = "sCategory", FillWeight = 12 });
            dgvSearchResults.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Slot", Name = "sSlot", FillWeight = 10 });
            dgvSearchResults.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Entry Time", Name = "sEntry", FillWeight = 28 });

            foreach (DataGridViewColumn col in dgvSearchResults.Columns)
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            Panel divider = new Panel
            {
                BackColor = Color.FromArgb(235, 238, 243),
                Height = 1,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            btnProcessExit = new RoundedButton
            {
                Text = "✓  Process Exit and Free Slot",
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                BackColor = PinkAccent,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                BorderRadius = 10,
                Size = new Size(300, 45),
                Location = new Point(0, 0),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                Cursor = Cursors.Hand
            };
            btnProcessExit.FlatAppearance.BorderSize = 0;
            btnProcessExit.Click += btnProcessExit_Click;

            resultsCard.Controls.Add(lblResultsTitle);
            resultsCard.Controls.Add(dgvSearchResults);
            resultsCard.Controls.Add(divider);
            resultsCard.Controls.Add(btnProcessExit);

            resultsCard.Resize += (s, e) =>
            {
                int bottomBarHeight = 80;
                dgvSearchResults.Size = new Size(resultsCard.Width - 50, resultsCard.Height - 60 - bottomBarHeight);
                divider.Location = new Point(25, resultsCard.Height - bottomBarHeight);
                divider.Size = new Size(resultsCard.Width - 50, 1);
                btnProcessExit.Location = new Point(resultsCard.Width - 325, resultsCard.Height - 62);
            };

            layout.Controls.Add(searchCard, 0, 0);
            layout.Controls.Add(resultsCard, 0, 1);
            exitPanel.Controls.Add(layout);
        }

        // ========== HELPERS ==========
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
                Size = new Size(400, 32),
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
                Size = new Size(400, 32),
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

            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 251, 253);

            return dgv;
        }

        // ========== TOGGLE PANELS ==========
        private void entryBtn(object sender, EventArgs e) => ShowEntryPanel();
        private void exitBtn(object sender, EventArgs e) => ShowExitPanel();

        private void ShowEntryPanel()
        {
            entryPanel.Visible = true;
            exitPanel.Visible = false;

            vehicleEntryBtn.BackColor = PinkAccent;
            vehicleEntryBtn.ForeColor = Color.White;
            vehicleExitBtn.BackColor = InactiveTab;
            vehicleExitBtn.ForeColor = InactiveTabText;

            LoadParkedVehicles();
        }

        private void ShowExitPanel()
        {
            entryPanel.Visible = false;
            exitPanel.Visible = true;

            vehicleExitBtn.BackColor = PinkAccent;
            vehicleExitBtn.ForeColor = Color.White;
            vehicleEntryBtn.BackColor = InactiveTab;
            vehicleEntryBtn.ForeColor = InactiveTabText;

            LoadAllParkedInGrid();
            txtSearchExit.Clear();
            txtSearchExit.Focus();
        }

        // ========== ENTRY LOGIC ==========
        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRegNum.Text))
            { MessageBox.Show("Please enter Registration Number!", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtRegNum.Focus(); return; }
            if (string.IsNullOrWhiteSpace(txtOwnerName.Text))
            { MessageBox.Show("Please enter Owner's Full Name!", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtOwnerName.Focus(); return; }
            if (string.IsNullOrWhiteSpace(txtContact.Text))
            { MessageBox.Show("Please enter Owner's Contact!", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtContact.Focus(); return; }
            if (cmbCategory.SelectedIndex == -1)
            { MessageBox.Show("Please select Vehicle Category!", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning); cmbCategory.Focus(); return; }

            string selectedCategory = cmbCategory.SelectedItem.ToString();
            string assignedSlot = GetFirstAvailableSlot(selectedCategory);

            if (string.IsNullOrEmpty(assignedSlot))
            { MessageBox.Show($"No available slots for {selectedCategory}!", "Full", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string insertQuery = @"INSERT INTO vehicles 
                        (reg_number, company, category, owner_name, owner_contact, slot_id, is_active) 
                        VALUES (@reg, @comp, @cat, @owner, @contact, @slot, 1)";

                    using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@reg", txtRegNum.Text.Trim());
                        cmd.Parameters.AddWithValue("@comp", txtCompany.Text.Trim());
                        cmd.Parameters.AddWithValue("@cat", selectedCategory);
                        cmd.Parameters.AddWithValue("@owner", txtOwnerName.Text.Trim());
                        cmd.Parameters.AddWithValue("@contact", txtContact.Text.Trim());
                        cmd.Parameters.AddWithValue("@slot", assignedSlot);
                        cmd.ExecuteNonQuery();
                    }

                    // Mark the slot as occupied
                    using (MySqlCommand cmd = new MySqlCommand(
                        "UPDATE slots SET status='occupied' WHERE slot_id=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", assignedSlot);
                        cmd.ExecuteNonQuery();
                    }
                }

                parkingGridRef?.RefreshSlots();
                parkBillingRef?.RefreshAll();

                MessageBox.Show($"Vehicle registered successfully!\n\nRegistration: {txtRegNum.Text}\nOwner: {txtOwnerName.Text}\nAssigned Slot: {assignedSlot}",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadParkedVehicles();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message, "Error");
            }
        }

        private string GetFirstAvailableSlot(string category)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    // Only pick slots that have NO active vehicle
                    string query = @"SELECT s.slot_id 
                             FROM slots s
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

        private void LoadParkedVehicles()
        {
            try
            {
                dgvParked.Rows.Clear();
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string query = @"SELECT v.reg_number, v.company, v.category, v.owner_name, 
                                     v.owner_contact, v.slot_id, v.entry_time 
                                     FROM vehicles v WHERE v.is_active = 1 ORDER BY v.entry_time DESC";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dgvParked.Rows.Add(
                                reader["reg_number"].ToString(),
                                reader["company"].ToString(),
                                reader["category"].ToString(),
                                reader["owner_name"].ToString(),
                                reader["owner_contact"].ToString(),
                                reader["slot_id"].ToString(),
                                Convert.ToDateTime(reader["entry_time"]).ToString("MMM dd, yyyy HH:mm")
                            );
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Load Error: " + ex.Message); }
        }

        private void ClearForm()
        {
            txtRegNum.Clear();
            txtCompany.Clear();
            txtOwnerName.Clear();
            txtContact.Clear();
            cmbCategory.SelectedIndex = -1;
            txtRegNum.Focus();
        }

        // ========== EXIT LOGIC ==========
        private void btnSearchExit_Click(object sender, EventArgs e)
        {
            string searchText = txtSearchExit.Text.Trim();
            if (string.IsNullOrWhiteSpace(searchText)) { LoadAllParkedInGrid(); return; }

            try
            {
                dgvSearchResults.Rows.Clear();
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string query = @"SELECT v.reg_number, v.owner_name, v.owner_contact, 
                                     v.category, v.slot_id, v.entry_time 
                                     FROM vehicles v WHERE v.is_active = 1 
                                     AND (v.reg_number LIKE @search OR v.owner_name LIKE @search)
                                     ORDER BY v.entry_time DESC";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            int count = 0;
                            while (reader.Read())
                            {
                                dgvSearchResults.Rows.Add(
                                    reader["reg_number"].ToString(),
                                    reader["owner_name"].ToString(),
                                    reader["owner_contact"].ToString(),
                                    reader["category"].ToString(),
                                    reader["slot_id"].ToString(),
                                    Convert.ToDateTime(reader["entry_time"]).ToString("MMM dd, yyyy HH:mm")
                                );
                                count++;
                            }
                            if (count == 0)
                                MessageBox.Show($"No vehicle found matching '{searchText}'", "No Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Search Error: " + ex.Message); }
        }

        private void LoadAllParkedInGrid()
        {
            try
            {
                dgvSearchResults.Rows.Clear();
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string query = @"SELECT v.reg_number, v.owner_name, v.owner_contact, 
                                     v.category, v.slot_id, v.entry_time 
                                     FROM vehicles v WHERE v.is_active = 1 ORDER BY v.entry_time DESC";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dgvSearchResults.Rows.Add(
                                reader["reg_number"].ToString(),
                                reader["owner_name"].ToString(),
                                reader["owner_contact"].ToString(),
                                reader["category"].ToString(),
                                reader["slot_id"].ToString(),
                                Convert.ToDateTime(reader["entry_time"]).ToString("MMM dd, yyyy HH:mm")
                            );
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Load Error: " + ex.Message); }
        }

        private void btnProcessExit_Click(object sender, EventArgs e)
        {
            if (dgvSearchResults.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a vehicle from the list!", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow row = dgvSearchResults.SelectedRows[0];
            string regNum = row.Cells[0].Value.ToString();
            string ownerName = row.Cells[1].Value.ToString();
            string category = row.Cells[3].Value.ToString();
            string slotId = row.Cells[4].Value.ToString();
            DateTime entryTime = Convert.ToDateTime(row.Cells[5].Value.ToString());
            DateTime exitTime = DateTime.Now;

            TimeSpan duration = exitTime - entryTime;
            decimal bill = BillingHelper.CalculateBill(category, entryTime, exitTime);
            string durationStr = BillingHelper.FormatDuration(duration);

            string billMessage = $"═══ PARKING RECEIPT ═══\n\n" +
                $"Registration: {regNum}\nOwner: {ownerName}\nCategory: {category}\nSlot: {slotId}\n\n" +
                $"Entry: {entryTime:yyyy-MM-dd HH:mm}\nExit:  {exitTime:yyyy-MM-dd HH:mm}\nDuration: {durationStr}\n\n" +
                $"═══════════════════\nTOTAL BILL: ₱{bill:N2}\n═══════════════════\n\n" +
                $"YES = Paid now\nNO  = Pay later\nCANCEL = Don't process exit";

            DialogResult result = MessageBox.Show(billMessage, "Parking Bill", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
            if (result == DialogResult.Cancel) return;
            bool markAsPaid = (result == DialogResult.Yes);

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string updateVehicle = @"UPDATE vehicles SET exit_time=@exit, duration_minutes=@dur, 
                        bill_amount=@bill, payment_status=@status, is_active=0
                        WHERE reg_number=@reg AND slot_id=@slot AND is_active=1
                        ORDER BY entry_time DESC LIMIT 1";

                    using (MySqlCommand cmd = new MySqlCommand(updateVehicle, conn))
                    {
                        cmd.Parameters.AddWithValue("@exit", exitTime);
                        cmd.Parameters.AddWithValue("@dur", (int)duration.TotalMinutes);
                        cmd.Parameters.AddWithValue("@bill", bill);
                        cmd.Parameters.AddWithValue("@status", markAsPaid ? "paid" : "unpaid");
                        cmd.Parameters.AddWithValue("@reg", regNum);
                        cmd.Parameters.AddWithValue("@slot", slotId);
                        cmd.ExecuteNonQuery();
                    }

                    // Free the slot ONLY if no other active vehicle is using it
                    using (MySqlCommand cmd = new MySqlCommand(
                        @"UPDATE slots 
                          SET status = 'available' 
                          WHERE slot_id = @id 
                          AND NOT EXISTS (
                              SELECT 1 FROM (SELECT slot_id FROM vehicles WHERE is_active = 1) v 
                              WHERE v.slot_id = @id
                          )", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", slotId);
                        cmd.ExecuteNonQuery();
                    }
                }

                parkingGridRef?.RefreshSlots();
                parkBillingRef?.RefreshAll();

                LoadParkedVehicles();
                LoadAllParkedInGrid();

                string statusMsg = markAsPaid ? "✅ PAID" : "⚠ UNPAID";
                MessageBox.Show($"Exit processed successfully!\n\nSlot {slotId} is now available.\nBill: ₱{bill:N2}\nStatus: {statusMsg}",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtSearchExit.Clear();
            }
            catch (Exception ex) { MessageBox.Show("Exit Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void txtSearchExit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearchExit_Click(sender, e);
                e.SuppressKeyPress = true;
            }
        }
    }
}