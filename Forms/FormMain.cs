using System.Data;
using QuanLyPhimVaLichChieu.BusinessLogic;

namespace QuanLyPhimVaLichChieu.Forms
{
    public class FormMain : Form
    {
        private Panel panelSidebar = null!;
        private Panel panelContent = null!;
        private Button? _activeButton = null;
        private readonly List<Button> _navButtons = new List<Button>();

        public FormMain()
        {
            InitializeComponent();
            LoadDashboard();
        }

        private void InitializeComponent()
        {
            this.Text = "Quản Lý Phim & Lịch Chiếu - Movie Manager";
            this.Size = new Size(1200, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(1000, 600);
            this.BackColor = UITheme.BgDark;
            this.Font = UITheme.FontBody;

            // === Sidebar ===
            panelSidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 230,
                BackColor = UITheme.BgSidebar
            };

            // Logo
            var panelLogo = new Panel { Dock = DockStyle.Top, Height = 80, BackColor = UITheme.BgSidebar };
            var lblLogo = new Label
            {
                Text = "\U0001F3AC MOVIE",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = UITheme.Accent,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            panelLogo.Controls.Add(lblLogo);
            panelSidebar.Controls.Add(panelLogo);

            // Accent line under logo
            var logoLine = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = UITheme.Border };
            panelSidebar.Controls.Add(logoLine);
            logoLine.BringToFront();

            // Nav label
            var lblNav = new Label
            {
                Text = "  NAVIGATION",
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                ForeColor = UITheme.TextMuted,
                Dock = DockStyle.Top,
                Height = 35,
                TextAlign = ContentAlignment.BottomLeft,
                Padding = new Padding(20, 0, 0, 6)
            };
            panelSidebar.Controls.Add(lblNav);
            lblNav.BringToFront();

            // Nav buttons (reverse order for Dock=Top)
            var btnThongKe = CreateNavButton("\U0001F4CA  Thống kê");
            btnThongKe.Click += (s, e) => { SetActive(btnThongKe); OpenForm(new FormThongKe()); };

            var btnBanVe = CreateNavButton("\U0001F3AB  Bán vé");
            btnBanVe.Click += (s, e) => { SetActive(btnBanVe); OpenForm(new FormBanVe()); };

            var btnSuatChieu = CreateNavButton("\U0001F4C5  Suất chiếu");
            btnSuatChieu.Click += (s, e) => { SetActive(btnSuatChieu); OpenForm(new FormSuatChieu()); };

            var btnPhongChieu = CreateNavButton("\U0001F3E0  Phòng chiếu");
            btnPhongChieu.Click += (s, e) => { SetActive(btnPhongChieu); OpenForm(new FormPhongChieu()); };

            var btnPhim = CreateNavButton("\U0001F3AC  Quản lý phim");
            btnPhim.Click += (s, e) => { SetActive(btnPhim); OpenForm(new FormPhim()); };

            var btnDashboard = CreateNavButton("\U0001F3E0  Trang chủ");
            btnDashboard.Click += (s, e) => { SetActive(btnDashboard); LoadDashboard(); };

            // Add in reverse
            foreach (var btn in new[] { btnThongKe, btnBanVe, btnSuatChieu, btnPhongChieu, btnPhim, btnDashboard })
            {
                panelSidebar.Controls.Add(btn);
                btn.BringToFront();
                _navButtons.Add(btn);
            }

            // Bottom info
            var lblVersion = new Label
            {
                Text = "v1.0.0 • Movie Manager",
                Font = UITheme.FontSmall,
                ForeColor = UITheme.TextMuted,
                Dock = DockStyle.Bottom,
                Height = 35,
                TextAlign = ContentAlignment.MiddleCenter
            };
            panelSidebar.Controls.Add(lblVersion);

            this.Controls.Add(panelSidebar);

            // === Content Panel ===
            panelContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = UITheme.BgDark,
                Padding = new Padding(30, 25, 30, 15)
            };
            this.Controls.Add(panelContent);
            panelContent.BringToFront();

            // Set dashboard as active initially
            SetActive(btnDashboard);
        }

        private Button CreateNavButton(string text)
        {
            var btn = new Button
            {
                Text = text,
                Dock = DockStyle.Top,
                Height = 46,
                FlatStyle = FlatStyle.Flat,
                BackColor = UITheme.BgSidebar,
                ForeColor = UITheme.TextSecondary,
                Font = new Font("Segoe UI", 10.5F),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(18, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(35, 35, 55);
            btn.MouseEnter += (s, e) => { if (btn != _activeButton) btn.ForeColor = UITheme.TextPrimary; };
            btn.MouseLeave += (s, e) => { if (btn != _activeButton) btn.ForeColor = UITheme.TextSecondary; };
            return btn;
        }

        private void SetActive(Button btn)
        {
            // Reset all
            foreach (var b in _navButtons)
            {
                b.BackColor = UITheme.BgSidebar;
                b.ForeColor = UITheme.TextSecondary;
                b.Font = new Font("Segoe UI", 10.5F);
            }
            // Highlight active
            _activeButton = btn;
            btn.BackColor = Color.FromArgb(40, 40, 65);
            btn.ForeColor = UITheme.Accent;
            btn.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        }

        private void LoadDashboard()
        {
            panelContent.Controls.Clear();

            var container = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = UITheme.BgDark };

            var lblHeading = new Label
            {
                Text = "TỔNG QUAN HỆ THỐNG",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = UITheme.TextPrimary,
                AutoSize = true,
                Location = new Point(0, 0)
            };
            container.Controls.Add(lblHeading);

            var lblDesc = new Label
            {
                Text = "Quản lý phim, lịch chiếu và bán vé nội bộ",
                Font = new Font("Segoe UI", 10.5F),
                ForeColor = UITheme.TextMuted,
                AutoSize = true,
                Location = new Point(2, 38)
            };
            container.Controls.Add(lblDesc);

            var panelCards = new FlowLayoutPanel
            {
                Location = new Point(0, 85),
                Size = new Size(900, 300),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                WrapContents = true,
                BackColor = Color.Transparent
            };

            try
            {
                var bll = new ThongKeBLL();
                DataTable dt = bll.GetTongQuan();
                if (dt.Rows.Count > 0)
                {
                    var row = dt.Rows[0];
                    panelCards.Controls.Add(CreateCard("TỔNG PHIM", row["TongPhim"].ToString()!, UITheme.AccentBlue, "Trong hệ thống"));
                    panelCards.Controls.Add(CreateCard("PHÒNG CHIẾU", row["TongPhong"].ToString()!, UITheme.Accent, "Phòng"));
                    panelCards.Controls.Add(CreateCard("SUẤT CHIẾU", row["TongSuatChieu"].ToString()!, UITheme.AccentGreen, "Tất cả"));
                    panelCards.Controls.Add(CreateCard("VÉ HÔM NAY", row["VeHomNay"].ToString()!, UITheme.AccentYellow, DateTime.Now.ToString("dd/MM")));
                    panelCards.Controls.Add(CreateCard("TỔNG VÉ BÁN", row["TongVe"].ToString()!, UITheme.AccentRed, "Vé"));
                    panelCards.Controls.Add(CreateCard("DOANH THU", (Convert.ToInt64(row["TongDoanhThu"]) / 1000).ToString("N0") + "K", UITheme.AccentCyan, "VND"));
                }
            }
            catch (Exception ex)
            {
                var lblError = new Label
                {
                    Text = $"Không thể kết nối database!\n\nVui lòng kiểm tra:\n1. SQL Server đã khởi động\n2. Database QuanLyPhimDB đã được tạo\n3. Connection string trong App.config\n\nLỗi: {ex.Message}",
                    Font = new Font("Segoe UI", 11F),
                    ForeColor = UITheme.AccentRed,
                    AutoSize = true,
                    MaximumSize = new Size(600, 0),
                    Location = new Point(0, 80)
                };
                container.Controls.Add(lblError);
            }

            container.Controls.Add(panelCards);
            panelContent.Controls.Add(container);
        }

        private Panel CreateCard(string title, string value, Color accentColor, string subtitle)
        {
            var card = new Panel
            {
                Size = new Size(210, 120),
                BackColor = UITheme.BgCard,
                Margin = new Padding(0, 0, 15, 15)
            };

            // Top accent bar
            var bar = new Panel { Dock = DockStyle.Top, Height = 3, BackColor = accentColor };
            card.Controls.Add(bar);

            var lblT = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = UITheme.TextMuted,
                Location = new Point(16, 16),
                AutoSize = true
            };
            card.Controls.Add(lblT);

            var lblV = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                ForeColor = accentColor,
                Location = new Point(16, 38),
                AutoSize = true
            };
            card.Controls.Add(lblV);

            var lblS = new Label
            {
                Text = subtitle,
                Font = UITheme.FontSmall,
                ForeColor = UITheme.TextMuted,
                Location = new Point(16, 92),
                AutoSize = true
            };
            card.Controls.Add(lblS);

            // Hover effect
            card.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(50, 52, 72);
            card.MouseLeave += (s, e) => card.BackColor = UITheme.BgCard;
            foreach (Control c in card.Controls)
            {
                c.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(50, 52, 72);
                c.MouseLeave += (s, e) => card.BackColor = UITheme.BgCard;
            }

            return card;
        }

        private void OpenForm(Form form)
        {
            form.ShowDialog();
            LoadDashboard();
        }
    }
}
