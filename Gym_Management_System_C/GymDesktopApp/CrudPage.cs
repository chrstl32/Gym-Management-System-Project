using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GymDesktopApp
{
    public class CrudPage : UserControl
    {
        string table;
        string[] fallbackFields;

        DataGridView grid = new DataGridView();
        Panel toolbar = new Panel();
        Panel formPanel = new Panel();
        FlowLayoutPanel form = new FlowLayoutPanel();
        Label status = new Label();
        Label formTitle = new Label();

        Dictionary<string, TextBox> inputs = new Dictionary<string, TextBox>();

        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        bool loading = false;
        bool formBuilt = false;

        public CrudPage(string table, string[] fields)
        {
            this.table = table;
            this.fallbackFields = fields;

            BackColor = Theme.Bg;
            Padding = new Padding(0);

            BuildToolbar();
            BuildFormPanel();
            BuildGrid();

            Controls.Add(grid);
            Controls.Add(formPanel);
            Controls.Add(toolbar);

            timer.Interval = 5000;
            timer.Tick += async (s, e) =>
            {
                if (!Typing())
                {
                    await LoadData(false);
                }
            };

            Load += async (s, e) =>
            {
                await LoadData();
                timer.Start();
            };

            Disposed += (s, e) => timer.Stop();
        }

        void BuildToolbar()
        {
            toolbar.Dock = DockStyle.Top;
            toolbar.Height = 78;
            toolbar.BackColor = Theme.Bg;
            toolbar.Padding = new Padding(0, 12, 0, 10);

            Button refresh = Theme.Btn("REFRESH");
            refresh.Location = new Point(0, 14);
            refresh.Width = 140;
            refresh.Height = 42;
            refresh.Click += async (s, e) => await LoadData();

            Button add = Theme.Btn("NEW");
            add.Location = new Point(160, 14);
            add.Width = 140;
            add.Height = 42;
            add.Click += (s, e) => Clear();

            Button save = Theme.Btn("SAVE", true);
            save.Location = new Point(320, 14);
            save.Width = 140;
            save.Height = 42;
            save.Click += async (s, e) => await Save();

            Button del = Theme.Btn("DELETE");
            del.Location = new Point(480, 14);
            del.Width = 140;
            del.Height = 42;
            del.BackColor = Color.FromArgb(190, 18, 60);
            del.Click += async (s, e) => await Delete();

            status.Location = new Point(650, 22);
            status.Width = 850;
            status.Height = 30;
            status.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            status.ForeColor = Theme.Muted;
            status.TextAlign = ContentAlignment.MiddleLeft;

            toolbar.Controls.Add(refresh);
            toolbar.Controls.Add(add);
            toolbar.Controls.Add(save);
            toolbar.Controls.Add(del);
            toolbar.Controls.Add(status);
        }

        void BuildFormPanel()
        {
            formPanel.Dock = DockStyle.Top;
            formPanel.Height = 270;
            formPanel.BackColor = Theme.Card;
            formPanel.Padding = new Padding(18, 16, 18, 12);

            formTitle.Text = "Record Details";
            formTitle.ForeColor = Color.White;
            formTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            formTitle.Location = new Point(18, 12);
            formTitle.Size = new Size(500, 28);
            formPanel.Controls.Add(formTitle);

            Label hint = new Label();
            hint.Text = "Select a row to edit, or click New to add a record.";
            hint.ForeColor = Theme.Muted;
            hint.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            hint.Location = new Point(20, 40);
            hint.Size = new Size(700, 22);
            formPanel.Controls.Add(hint);

            form.Location = new Point(18, 68);
            form.Size = new Size(1450, 184);
            form.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            form.BackColor = Theme.Card;
            form.AutoScroll = false;
            form.WrapContents = true;
            form.FlowDirection = FlowDirection.LeftToRight;
            formPanel.Controls.Add(form);
        }

        void BuildGrid()
        {
            grid.Dock = DockStyle.Fill;
            Theme.Grid(grid);
            grid.Margin = new Padding(0, 16, 0, 0);
            grid.CellClick += (s, e) => Fill();
        }

        async Task LoadData(bool show = true)
        {
            if (loading) return;

            loading = true;

            if (show)
            {
                status.ForeColor = Theme.Muted;
                status.Text = "Loading records...";
            }

            ApiResult result = await Api.Get("list", table);

            if (result.Success)
            {
                grid.DataSource = result.Data;

                if (!formBuilt)
                {
                    BuildFormFromTable(result.Data);
                }

                status.ForeColor = Theme.Muted;
                status.Text = "Loaded " + result.Data.Rows.Count + " records | Auto refresh ON";
            }
            else
            {
                status.ForeColor = Theme.Error;
                status.Text = result.Message;

                if (!formBuilt)
                {
                    BuildForm(fallbackFields);
                }
            }

            loading = false;
        }

        void BuildFormFromTable(DataTable dt)
        {
            if (dt != null && dt.Columns.Count > 0)
            {
                List<string> fields = new List<string>();

                foreach (DataColumn col in dt.Columns)
                {
                    fields.Add(col.ColumnName);
                }

                BuildForm(fields.ToArray());
            }
            else
            {
                BuildForm(fallbackFields);
            }
        }

        string Pretty(string field)
        {
            return field.Replace("_", " ").ToUpper();
        }

        void BuildForm(string[] fields)
        {
            form.Controls.Clear();
            inputs.Clear();

            foreach (string field in fields)
            {
                Panel box = new Panel();
                box.Width = 255;
                box.Height = 82;
                box.Margin = new Padding(8, 4, 8, 10);
                box.BackColor = Theme.Card;

                Label lab = new Label();
                lab.Text = Pretty(field);
                lab.Location = new Point(0, 0);
                lab.Size = new Size(245, 24);
                lab.Font = new Font("Segoe UI", 8, FontStyle.Bold);
                lab.ForeColor = Color.FromArgb(190, 198, 215);
                lab.TextAlign = ContentAlignment.MiddleLeft;

                TextBox input = new TextBox();
                input.Location = new Point(0, 32);
                input.Size = new Size(245, 32);
                input.Font = new Font("Segoe UI", 10);
                input.BackColor = Theme.CardAlt;
                input.ForeColor = Color.White;
                input.BorderStyle = BorderStyle.FixedSingle;

                if (field == "id" || field == "created_at" || field == "updated_at")
                {
                    input.ReadOnly = true;
                    input.BackColor = Color.FromArgb(31, 33, 44);
                    input.ForeColor = Theme.Muted;
                }

                box.Controls.Add(lab);
                box.Controls.Add(input);
                form.Controls.Add(box);
                inputs[field] = input;
            }

            formBuilt = true;
        }

        bool Typing()
        {
            foreach (TextBox input in inputs.Values)
            {
                if (input.Focused) return true;
            }

            return false;
        }

        void Fill()
        {
            if (grid.CurrentRow == null) return;

            foreach (var item in inputs)
            {
                string field = item.Key;

                if (grid.Columns.Contains(field))
                {
                    item.Value.Text = grid.CurrentRow.Cells[field].Value?.ToString() ?? "";
                }
            }

            status.ForeColor = Theme.Muted;
            status.Text = "Selected record is ready for editing.";
        }

        void Clear()
        {
            foreach (var item in inputs)
            {
                item.Value.Text = "";
            }

            status.ForeColor = Theme.Muted;
            status.Text = "Ready for a new record.";
        }

        async Task Save()
        {
            timer.Stop();

            Dictionary<string, string> data = new Dictionary<string, string>();

            foreach (var item in inputs)
            {
                string field = item.Key;

                if (field == "created_at" || field == "updated_at")
                {
                    continue;
                }

                data[field] = item.Value.Text;
            }

            ApiResult result = await Api.Post("save", table, data);

            status.ForeColor = result.Success ? Theme.Success : Theme.Error;
            status.Text = result.Message;

            if (result.Success)
            {
                Clear();
                await LoadData(false);
            }

            timer.Start();
        }

        async Task Delete()
        {
            if (!inputs.ContainsKey("id") || string.IsNullOrWhiteSpace(inputs["id"].Text))
            {
                status.ForeColor = Theme.Error;
                status.Text = "Select a record first.";
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "Delete selected record?",
                "Confirm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm != DialogResult.Yes) return;

            timer.Stop();

            ApiResult result = await Api.Post("delete", table, new Dictionary<string, string>
            {
                { "id", inputs["id"].Text }
            });

            status.ForeColor = result.Success ? Theme.Success : Theme.Error;
            status.Text = result.Message;

            if (result.Success)
            {
                Clear();
                await LoadData(false);
            }

            timer.Start();
        }
    }
}
