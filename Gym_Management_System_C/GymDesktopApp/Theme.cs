using System.Drawing;
using System.Windows.Forms;

namespace GymDesktopApp
{
    public static class Theme
    {
        public static readonly Color Bg = Color.FromArgb(22, 24, 31);
        public static readonly Color Side = Color.FromArgb(32, 34, 45);
        public static readonly Color Card = Color.FromArgb(35, 37, 49);
        public static readonly Color CardAlt = Color.FromArgb(28, 30, 40);
        public static readonly Color Header = Color.FromArgb(26, 28, 38);
        public static readonly Color Primary = Color.FromArgb(255, 90, 0);
        public static readonly Color Primary2 = Color.FromArgb(255, 122, 24);
        public static readonly Color Text = Color.FromArgb(248, 250, 252);
        public static readonly Color Muted = Color.FromArgb(156, 163, 175);
        public static readonly Color Error = Color.FromArgb(239, 68, 68);
        public static readonly Color Success = Color.FromArgb(34, 197, 94);
        public static readonly Color Dark = Side;

        public static Button Btn(string t, bool primary = false)
        {
            var b = new Button
            {
                Text = t,
                Width = 128,
                Height = 40,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = primary ? Primary : Color.FromArgb(62, 47, 40),
                ForeColor = Color.White,
                UseVisualStyleBackColor = false,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter
            };

            b.FlatAppearance.BorderSize = 0;
            return b;
        }

        public static TextBox Input(string p)
        {
            return new TextBox
            {
                Width = 240,
                Height = 28,
                Font = new Font("Segoe UI", 10),
                BackColor = CardAlt,
                ForeColor = Text,
                BorderStyle = BorderStyle.FixedSingle
            };
        }

        public static Label Title(string text, int size = 16)
        {
            return new Label
            {
                Text = text,
                ForeColor = Text,
                Font = new Font("Segoe UI", size, FontStyle.Bold),
                AutoSize = true
            };
        }

        public static Panel CardPanel()
        {
            return new Panel
            {
                BackColor = Card,
                Padding = new Padding(16)
            };
        }

        public static void Grid(DataGridView g)
        {
            g.BackgroundColor = Card;
            g.BorderStyle = BorderStyle.FixedSingle;
            g.GridColor = Color.FromArgb(65, 67, 80);
            g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            g.RowHeadersVisible = false;
            g.AllowUserToAddRows = false;
            g.ReadOnly = true;
            g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            g.MultiSelect = false;
            g.EnableHeadersVisualStyles = false;
            g.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 32, 27);
            g.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(255, 221, 190);
            g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            g.DefaultCellStyle.BackColor = Card;
            g.DefaultCellStyle.ForeColor = Text;
            g.DefaultCellStyle.SelectionBackColor = Primary;
            g.DefaultCellStyle.SelectionForeColor = Color.White;
            g.DefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            g.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(39, 41, 54);
            g.RowTemplate.Height = 32;
        }
    }
}
