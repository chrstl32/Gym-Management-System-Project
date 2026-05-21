using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GymDesktopApp
{
    public class LoginForm : Form
    {
        TextBox email = Theme.Input("Email or username");
        TextBox pass = Theme.Input("Password");
        TextBox url = Theme.Input("API URL");
        Label status = new Label();

        public LoginForm()
        {
            Text = "FitFlow Login";
            Width = 520;
            Height = 650;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Theme.Bg;

            Panel imagePanel = new Panel();
            imagePanel.Dock = DockStyle.Top;
            imagePanel.Height = 150;
            imagePanel.BackColor = Color.FromArgb(15, 16, 23);
            Controls.Add(imagePanel);

            Label logo = new Label
            {
                Text = "F",
                BackColor = Theme.Primary,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(45, 38),
                Size = new Size(76, 60)
            };
            imagePanel.Controls.Add(logo);

            Label app = new Label
            {
                Text = "FitFlow Gym",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                Location = new Point(140, 42),
                Size = new Size(320, 40)
            };
            imagePanel.Controls.Add(app);

            Label sub = new Label
            {
                Text = "Desktop operations access",
                ForeColor = Color.FromArgb(209, 213, 219),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Location = new Point(143, 82),
                Size = new Size(320, 25)
            };
            imagePanel.Controls.Add(sub);

            Panel card = new Panel
            {
                BackColor = Color.White,
                Location = new Point(45, 185),
                Size = new Size(410, 360)
            };
            Controls.Add(card);

            Label title = new Label
            {
                Text = "Welcome Back",
                ForeColor = Color.FromArgb(31, 41, 55),
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                Location = new Point(30, 24),
                Size = new Size(350, 40)
            };
            card.Controls.Add(title);

            Label hint = new Label
            {
                Text = "Sign in to continue",
                ForeColor = Color.FromArgb(107, 114, 128),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Location = new Point(34, 65),
                Size = new Size(350, 24)
            };
            card.Controls.Add(hint);

            email.Location = new Point(34, 110);
            email.Width = 340;
            email.Text = "admin@gym.test";
            email.BackColor = Color.FromArgb(245, 245, 245);
            email.ForeColor = Color.FromArgb(17, 24, 39);
            card.Controls.Add(email);

            pass.Location = new Point(34, 160);
            pass.Width = 340;
            pass.PasswordChar = '*';
            pass.Text = "admin123";
            pass.BackColor = Color.FromArgb(245, 245, 245);
            pass.ForeColor = Color.FromArgb(17, 24, 39);
            card.Controls.Add(pass);

            url.Location = new Point(34, 210);
            url.Width = 340;
            url.Text = Api.BaseUrl;
            url.BackColor = Color.FromArgb(245, 245, 245);
            url.ForeColor = Color.FromArgb(17, 24, 39);
            card.Controls.Add(url);

            Button test = Theme.Btn("TEST CONNECTION");
            test.Location = new Point(34, 263);
            test.Width = 160;
            test.BackColor = Color.FromArgb(82, 58, 44);
            test.Click += async (s, e) => await Test();
            card.Controls.Add(test);

            Button login = Theme.Btn("LOGIN", true);
            login.Location = new Point(214, 263);
            login.Width = 160;
            login.Click += async (s, e) => await Login();
            card.Controls.Add(login);

            status.Location = new Point(45, 560);
            status.Width = 410;
            status.Height = 55;
            status.ForeColor = Theme.Muted;
            status.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            status.TextAlign = ContentAlignment.TopCenter;
            Controls.Add(status);
        }

        void SetUrl()
        {
            Api.BaseUrl = url.Text.Trim();
        }

        async Task Test()
        {
            SetUrl();
            status.Text = "Testing connection...";
            var r = await Api.Get("ping");
            status.ForeColor = r.Success ? Theme.Success : Theme.Error;
            status.Text = r.Message;
        }

        async Task Login()
        {
            SetUrl();
            status.Text = "Logging in...";
            var r = await Api.Post("login", "", new Dictionary<string, string>
            {
                {"email", email.Text.Trim()},
                {"password", pass.Text}
            });

            if (r.Success)
            {
                MainForm main = new MainForm();
                main.Show();
                Hide();
            }
            else
            {
                status.ForeColor = Theme.Error;
                status.Text = r.Message;
            }
        }
    }
}
