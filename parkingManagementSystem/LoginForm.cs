using System;
using System.Windows.Forms;

namespace parkingManagementSystem
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        // LOGIN BUTTON CLICK - Main functionality here
        private void loginbtn_Click(object sender, EventArgs e)  // Change "loginbtn" to your button name if different
        {
            string enteredUser = username.Text.Trim();
            string enteredPass = password.Text.Trim();  // Assume textbox named "password"

            // Simple validation (for demo/presentation)
            if (enteredUser == "a" && enteredPass == "a")
            {
                // Success: Open MainForm
                Main main = new Main();
                main.Show();
                this.Hide();  // Hide login (don't close, so back button works if needed)
                // Or use this.Close(); if you want to fully close login
            }
            else
            {
                // Fail
                MessageBox.Show("Invalid username or password!", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                username.Clear();  // Clear fields
                password.Clear();
                username.Focus();  // Focus back to username
            }
        }

        // Optional: Password hide/show (if you have eye icon button)
        private void showPassword_Click(object sender, EventArgs e)
        {
            password.UseSystemPasswordChar = !password.UseSystemPasswordChar;
        }

        // Empty events - delete if not needed, or add code
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { this.Close(); }  // Close button
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void username_TextChanged(object sender, EventArgs e) { }

        // ENTER key on password = login (nice UX)
        private void password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                loginbtn_Click(sender, e);
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}