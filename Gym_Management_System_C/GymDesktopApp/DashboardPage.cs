using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GymDesktopApp
{
    public class DashboardPage : UserControl
    {
        FlowLayoutPanel cards = new FlowLayoutPanel();
        Label status = new Label();
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        string[] tables = { "members", "plans", "payments", "attendance", "trainers", "equipment" };

        public DashboardPage()
        {
            BackColor = Theme.Bg;

            Panel hero = new Panel
            {
                Dock = DockStyle.Top,
                Height = 150,
                BackColor = Theme.Card,
                Padding = new Padding(22)
            };

            Label welcome = new Label
            {
                Text = "Ready to manage today's gym activity?",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                Location = new Point(24, 26),
                Size = new Size(700, 45)
            };
            hero.Controls.Add(welcome);

            Label desc = new Label
            {
                Text = "Live module summary updates every five seconds.",
                ForeColor = Theme.Muted,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(28, 78),
                Size = new Size(600, 25)
            };
            hero.Controls.Add(desc);

            Button refresh = Theme.Btn("REFRESH", true);
            refresh.Location = new Point(28, 108);
            refresh.Width = 130;
            refresh.Click += async (s, e) => await LoadCards();
            hero.Controls.Add(refresh);

            status.Location = new Point(175, 115);
            status.Width = 700;
            status.ForeColor = Theme.Muted;
            status.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            hero.Controls.Add(status);

            cards.Dock = DockStyle.Fill;
            cards.Padding = new Padding(0, 18, 0, 0);
            cards.AutoScroll = true;
            cards.BackColor = Theme.Bg;

            Controls.Add(cards);
            Controls.Add(hero);

            timer.Interval = 5000;
            timer.Tick += async (s, e) => await LoadCards();

            Load += async (s, e) =>
            {
                await LoadCards();
                timer.Start();
            };

            Disposed += (s, e) => timer.Stop();
        }

        async Task LoadCards()
        {
            cards.Controls.Clear();

            foreach (var t in tables)
            {
                await AddCard(t);
            }

            status.Text = "Auto refresh ON  |  " + DateTime.Now.ToString("hh:mm:ss tt");
        }

        async Task AddCard(string t)
        {
            var r = await Api.Get("list", t);

            Panel p = new Panel
            {
                Width = 260,
                Height = 138,
                BackColor = Theme.Card,
                Margin = new Padding(0, 0, 18, 18),
                Padding = new Padding(18)
            };

            Label name = new Label
            {
                Text = t.ToUpper(),
                ForeColor = Theme.Muted,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                Location = new Point(18, 18),
                Size = new Size(210, 18)
            };
            p.Controls.Add(name);

            Label num = new Label
            {
                Text = (r.Success ? r.Data.Rows.Count : 0).ToString(),
                Font = new Font("Segoe UI", 27, FontStyle.Bold),
                ForeColor = r.Success ? Color.White : Theme.Error,
                Location = new Point(16, 42),
                Size = new Size(190, 48)
            };
            p.Controls.Add(num);

            Label change = new Label
            {
                Text = r.Success ? "Live records" : "Connection issue",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = r.Success ? Theme.Success : Theme.Error,
                Location = new Point(20, 94),
                Size = new Size(200, 24)
            };
            p.Controls.Add(change);

            cards.Controls.Add(p);
        }
    }
}
