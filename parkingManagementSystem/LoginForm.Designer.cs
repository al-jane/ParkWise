namespace parkingManagementSystem
{
    partial class LoginForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            panelMain = new Panel();
            label2 = new Label();
            label1 = new Label();
            password = new TextBox();
            username = new TextBox();
            loginbtn = new Button();
            pictureBox1 = new PictureBox();
            panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panelMain
            // 
            panelMain.BackColor = Color.FromArgb(246, 248, 250);
            panelMain.Controls.Add(label2);
            panelMain.Controls.Add(label1);
            panelMain.Controls.Add(password);
            panelMain.Controls.Add(username);
            panelMain.Controls.Add(loginbtn);
            panelMain.Controls.Add(pictureBox1);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(0, 0);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(1232, 603);
            panelMain.TabIndex = 0;
            panelMain.Paint += panel1_Paint;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(719, 368);
            label2.Name = "label2";
            label2.Size = new Size(57, 15);
            label2.TabIndex = 7;
            label2.Text = "Password";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(719, 299);
            label1.Name = "label1";
            label1.Size = new Size(60, 15);
            label1.TabIndex = 6;
            label1.Text = "Username";
            // 
            // password
            // 
            password.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            password.Location = new Point(719, 386);
            password.Name = "password";
            password.Size = new Size(315, 33);
            password.TabIndex = 5;
            password.TextChanged += textBox1_TextChanged;
            // 
            // username
            // 
            username.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            username.Location = new Point(719, 317);
            username.Name = "username";
            username.Size = new Size(315, 33);
            username.TabIndex = 3;
            username.TextChanged += username_TextChanged;
            // 
            // loginbtn
            // 
            loginbtn.BackColor = Color.White;
            loginbtn.FlatAppearance.BorderSize = 0;
            loginbtn.FlatStyle = FlatStyle.Flat;
            loginbtn.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            loginbtn.ForeColor = SystemColors.ActiveCaptionText;
            loginbtn.Location = new Point(690, 456);
            loginbtn.Name = "loginbtn";
            loginbtn.Size = new Size(368, 33);
            loginbtn.TabIndex = 2;
            loginbtn.Text = "Login";
            loginbtn.UseVisualStyleBackColor = false;
            loginbtn.Click += loginbtn_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(-3, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1232, 600);
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1232, 603);
            Controls.Add(panelMain);
            Name = "LoginForm";
            Text = "Form1";
            WindowState = FormWindowState.Minimized;
            Load += LoginForm_Load;
            panelMain.ResumeLayout(false);
            panelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelMain;
        private PictureBox pictureBox1;
        private Button loginbtn;
        private TextBox username;
        private TextBox password;
        private Label label2;
        private Label label1;
    }
}
