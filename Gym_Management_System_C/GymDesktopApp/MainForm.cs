using System;
using System.Drawing;
using System.Windows.Forms;

namespace GymDesktopApp
{
    public class MainForm : Form
    {
        Panel side = new Panel();
        Panel right = new Panel();
        Panel header = new Panel();//new update
        Panel body = new Panel();//newupdate
        Label head = new Label();//newupdate
        Label sub = new Label();//newupdate

        public MainForm()//newupdate
        {
            Text = "FitFlow Gym Management System";
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Theme.Bg;
            MinimumSize = new Size(1366, 768);

            side.Dock = DockStyle.Left;
            side.Width = 260;
            side.BackColor = Theme.Side;

            right.Dock = DockStyle.Fill;
            right.BackColor = Theme.Bg;
            right.Padding = new Padding(18);

            header.Dock = DockStyle.Top;
            header.Height = 88;
            header.BackColor = Theme.Header;
            header.Padding = new Padding(22, 12, 22, 10);

            body.Dock = DockStyle.Fill;
            body.Padding = new Padding(0, 18, 0, 0);
            body.BackColor = Theme.Bg;

            right.Controls.Add(body);
            right.Controls.Add(header);
            Controls.Add(right);
            Controls.Add(side);

            BuildSidebar();
            BuildHeader();

            Show("Dashboard", new DashboardPage());
        }

        void BuildSidebar()
        {
            Label mark = new Label
            {
                Text = "F",
                BackColor = Theme.Primary,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(24, 24),
                Size = new Size(58, 52)
            };
            side.Controls.Add(mark);

            Label brand = new Label
            {
                Text = "FitFlow",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Location = new Point(95, 29),
                Size = new Size(145, 28)
            };
            side.Controls.Add(brand);

            Label brandSub = new Label
            {
                Text = "Gym Control",
                ForeColor = Theme.Muted,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                Location = new Point(98, 58),
                Size = new Size(130, 20)
            };
            side.Controls.Add(brandSub);

            Add("▦  Dashboard", 115, () => Show("Dashboard", new DashboardPage()));
            Add("☷  Members", 170, () => Show("Members", new CrudPage("members", new[] { "id", "name", "email", "phone", "gender", "address", "status", "created_at" })));
            Add("▣  Plans", 225, () => Show("Plans", new CrudPage("plans", new[] { "id", "name", "duration_days", "price", "created_at" })));
            Add("₱  Payments", 280, () => Show("Payments", new CrudPage("payments", new[] { "id", "member_id", "amount", "payment_date", "method", "status", "created_at" })));
            Add("◷  Attendance", 335, () => Show("Attendance", new CrudPage("attendance", new[] { "id", "member_id", "check_in", "check_out", "remarks", "created_at" })));
            Add("⌁  Trainers", 390, () => Show("Trainers", new CrudPage("trainers", new[] { "id", "name", "email", "phone", "specialty", "status", "created_at" })));
            Add("⚙  Equipment", 445, () => Show("Equipment", new CrudPage("equipment", new[] { "id", "name", "quantity", "condition_status", "purchase_date", "remarks", "created_at" })));

            Button logout = Theme.Btn("↩  Logout");
            logout.Left = 24;
            logout.Top = 610;
            logout.Width = 210;
            logout.Height = 42;
            logout.BackColor = Color.FromArgb(190, 18, 60);
            logout.Click += (s, e) =>
            {
                LoginForm login = new LoginForm();
                login.Show();
                Close();
            };
            side.Controls.Add(logout);
        }

        void BuildHeader()
        {
            head.Text = "Dashboard";
            head.ForeColor = Color.White;
            head.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            head.Location = new Point(22, 14);
            head.Size = new Size(420, 34);
            header.Controls.Add(head);

            sub.Text = "Find records, manage daily operations, and keep the gym updated.";
            sub.ForeColor = Theme.Muted;
            sub.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            sub.Location = new Point(25, 50);
            sub.Size = new Size(620, 24);
            header.Controls.Add(sub);

            Label profile = new Label
            {
                Text = "Admin",
                ForeColor = Color.White,
                BackColor = Theme.Card,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Dock = DockStyle.Right,
                Width = 120
            };
            header.Controls.Add(profile);
        }

        void Add(string t, int top, Action a)
        {
            Button b = Theme.Btn(t);
            b.Left = 24;
            b.Top = top;
            b.Width = 210;
            b.Height = 42;
            b.TextAlign = ContentAlignment.MiddleLeft;
            b.Padding = new Padding(14, 0, 0, 0);
            b.BackColor = Theme.Side;
            b.ForeColor = Color.FromArgb(220, 224, 235);
            b.Click += (s, e) => a();
            side.Controls.Add(b);
        }

        void Show(string t, UserControl p)
        {
            head.Text = t;
            body.Controls.Clear();
            p.Dock = DockStyle.Fill;
            body.Controls.Add(p);
        }
    }
}
