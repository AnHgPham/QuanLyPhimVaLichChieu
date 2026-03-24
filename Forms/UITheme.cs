namespace QuanLyPhimVaLichChieu.Forms
{
    public static class UITheme
    {
        // === Color Palette ===
        public static readonly Color BgDark = Color.FromArgb(30, 30, 46);
        public static readonly Color BgCard = Color.FromArgb(40, 42, 58);
        public static readonly Color BgInput = Color.FromArgb(50, 52, 70);
        public static readonly Color BgSidebar = Color.FromArgb(22, 22, 38);
        public static readonly Color BgHeader = Color.FromArgb(35, 35, 52);

        public static readonly Color Accent = Color.FromArgb(139, 92, 246);     // purple
        public static readonly Color AccentRed = Color.FromArgb(239, 68, 68);
        public static readonly Color AccentGreen = Color.FromArgb(34, 197, 94);
        public static readonly Color AccentBlue = Color.FromArgb(59, 130, 246);
        public static readonly Color AccentYellow = Color.FromArgb(250, 204, 21);
        public static readonly Color AccentCyan = Color.FromArgb(6, 182, 212);
        public static readonly Color AccentOrange = Color.FromArgb(249, 115, 22);

        public static readonly Color TextPrimary = Color.FromArgb(226, 232, 240);
        public static readonly Color TextSecondary = Color.FromArgb(148, 163, 184);
        public static readonly Color TextMuted = Color.FromArgb(100, 116, 139);
        public static readonly Color Border = Color.FromArgb(55, 58, 78);

        public static readonly Font FontTitle = new Font("Segoe UI", 14F, FontStyle.Bold);
        public static readonly Font FontSubtitle = new Font("Segoe UI", 11F, FontStyle.Bold);
        public static readonly Font FontBody = new Font("Segoe UI", 9.5F);
        public static readonly Font FontSmall = new Font("Segoe UI", 8.5F);
        public static readonly Font FontBold = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        public static readonly Font FontLarge = new Font("Segoe UI", 22F, FontStyle.Bold);

        // === Style Methods ===

        public static void StyleForm(Form form, string title)
        {
            form.Text = title;
            form.BackColor = BgDark;
            form.ForeColor = TextPrimary;
            form.Font = FontBody;
            form.StartPosition = FormStartPosition.CenterParent;
        }

        public static Button CreateButton(string text, Color bgColor, int width = 90)
        {
            var btn = new Button
            {
                Text = text,
                Width = width,
                Height = 34,
                FlatStyle = FlatStyle.Flat,
                BackColor = bgColor,
                ForeColor = Color.White,
                Font = FontBold,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Lighten(bgColor, 30);
            btn.FlatAppearance.MouseDownBackColor = Darken(bgColor, 20);
            return btn;
        }

        public static void StyleTextBox(TextBox txt)
        {
            txt.BackColor = BgInput;
            txt.ForeColor = TextPrimary;
            txt.BorderStyle = BorderStyle.FixedSingle;
            txt.Font = FontBody;
        }

        public static void StyleComboBox(ComboBox cbo)
        {
            cbo.BackColor = BgInput;
            cbo.ForeColor = TextPrimary;
            cbo.FlatStyle = FlatStyle.Flat;
            cbo.Font = FontBody;
        }

        public static void StyleDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = BgDark;
            dgv.GridColor = Border;
            dgv.BorderStyle = BorderStyle.None;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.EnableHeadersVisualStyles = false;

            // Header style
            dgv.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = BgHeader,
                ForeColor = Accent,
                Font = FontBold,
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 0, 0),
                SelectionBackColor = BgHeader,
                SelectionForeColor = Accent
            };
            dgv.ColumnHeadersHeight = 40;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Row style
            dgv.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = BgCard,
                ForeColor = TextPrimary,
                Font = FontBody,
                SelectionBackColor = Color.FromArgb(60, 62, 90),
                SelectionForeColor = Color.White,
                Padding = new Padding(8, 0, 0, 0)
            };
            dgv.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(45, 47, 64),
                ForeColor = TextPrimary,
                SelectionBackColor = Color.FromArgb(60, 62, 90),
                SelectionForeColor = Color.White,
                Padding = new Padding(8, 0, 0, 0)
            };
            dgv.RowTemplate.Height = 36;
        }

        public static Panel CreateFormHeader(string title, string subtitle = "")
        {
            var panel = new Panel
            {
                Dock = DockStyle.Top,
                Height = subtitle.Length > 0 ? 78 : 50,
                BackColor = BgHeader,
                Padding = new Padding(20, 12, 20, 8)
            };

            var lbl = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = TextPrimary,
                AutoSize = true,
                Location = new Point(20, 14)
            };
            panel.Controls.Add(lbl);

            if (subtitle.Length > 0)
            {
                var lblSub = new Label
                {
                    Text = subtitle,
                    Font = FontSmall,
                    ForeColor = TextMuted,
                    AutoSize = true,
                    Location = new Point(22, 46)
                };
                panel.Controls.Add(lblSub);
            }

            // accent line
            var line = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 2,
                BackColor = Accent
            };
            panel.Controls.Add(line);

            return panel;
        }

        public static Label CreateLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Font = FontBold,
                ForeColor = TextSecondary
            };
        }

        public static TextBox CreateTextBox(int x, int y, int width = 270)
        {
            var txt = new TextBox
            {
                Location = new Point(x, y),
                Width = width
            };
            StyleTextBox(txt);
            return txt;
        }

        public static ComboBox CreateComboBox(int x, int y, int width = 270)
        {
            var cbo = new ComboBox
            {
                Location = new Point(x, y),
                Width = width,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            StyleComboBox(cbo);
            return cbo;
        }

        public static Panel CreateInputPanel(int width = 320)
        {
            return new Panel
            {
                Dock = DockStyle.Left,
                Width = width,
                BackColor = BgCard,
                Padding = new Padding(15)
            };
        }

        // === Utility ===
        public static Color Lighten(Color c, int amount)
        {
            return Color.FromArgb(c.A,
                Math.Min(255, c.R + amount),
                Math.Min(255, c.G + amount),
                Math.Min(255, c.B + amount));
        }

        public static Color Darken(Color c, int amount)
        {
            return Color.FromArgb(c.A,
                Math.Max(0, c.R - amount),
                Math.Max(0, c.G - amount),
                Math.Max(0, c.B - amount));
        }
    }
}
