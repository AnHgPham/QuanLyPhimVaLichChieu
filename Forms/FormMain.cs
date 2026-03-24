using System.Data;
using QuanLyPhimVaLichChieu.BusinessLogic;
using QuanLyPhimVaLichChieu.DataAccess;

namespace QuanLyPhimVaLichChieu.Forms
{
    public class FormMain : Form
    {
        private Panel panelSidebar = null!;
        private Panel panelContent = null!;
        private StatusStrip statusStrip = null!;
        private ToolStripStatusLabel toolStripStatus = null!;

        private static readonly Color SidebarBg = Color.FromArgb(25, 25, 45);
        private static readonly Color AccentColor = Color.FromArgb(229, 57, 53);
        private static readonly Color BgColor = Color.FromArgb(240, 243, 247);
        private static readonly Color CardBg = Color.White;

        public FormMain()
        {
            InitializeComponent();
            LoadDashboard();
        }

        private void InitializeComponent()
        {
            this.Text = "Quan Ly Phim & Lich Chieu - Movie Manager";
            this.Size = new Size(1200, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(1000, 600);
            this.BackColor = BgColor;
            this.Font = new Font("Segoe UI", 9.5F);

            // Status Strip
            statusStrip = new StatusStrip();
            statusStrip.BackColor = Color.FromArgb(18, 18, 35);
            statusStrip.SizingGrip = false;
            toolStripStatus = new ToolStripStatusLabel("San sang");
            toolStripStatus.ForeColor = Color.FromArgb(180, 200, 230);
            statusStrip.Items.Add(toolStripStatus);
            this.Controls.Add(statusStrip);

            // Sidebar
            panelSidebar = new Panel();
            panelSidebar.Dock = DockStyle.Left;
            panelSidebar.Width = 210;
            panelSidebar.BackColor = SidebarBg;

            var lblTitle = new Label();
            lblTitle.Text = "MOVIE\nMANAGER";
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = AccentColor;
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Height = 65;
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Padding = new Padding(0, 5, 0, 0);

            var accentLine = new Panel();
            accentLine.Dock = DockStyle.Top;
            accentLine.Height = 2;
            accentLine.BackColor = AccentColor;

            var lblNav = new Label();
            lblNav.Text = "MENU";
            lblNav.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblNav.ForeColor = Color.FromArgb(100, 120, 160);
            lblNav.Dock = DockStyle.Top;
            lblNav.Height = 30;
            lblNav.TextAlign = ContentAlignment.BottomLeft;
            lblNav.Padding = new Padding(20, 0, 0, 5);

            // Buttons (reverse order for Dock=Top)
            var btnThongKe = CreateSidebarButton("  Thong ke bao cao");
            btnThongKe.Click += (s, e) => OpenForm(new FormThongKe());

            var btnBanVe = CreateSidebarButton("  Ban ve");
            btnBanVe.Click += (s, e) => OpenForm(new FormBanVe());

            var btnSuatChieu = CreateSidebarButton("  Suat chieu");
            btnSuatChieu.Click += (s, e) => OpenForm(new FormSuatChieu());

            var btnPhongChieu = CreateSidebarButton("  Phong chieu");
            btnPhongChieu.Click += (s, e) => OpenForm(new FormPhongChieu());

            var btnPhim = CreateSidebarButton("  Quan ly phim");
            btnPhim.Click += (s, e) => OpenForm(new FormPhim());

            var btnDashboard = CreateSidebarButton("  Trang chu");
            btnDashboard.Click += (s, e) => LoadDashboard();

            panelSidebar.Controls.Add(btnThongKe);
            panelSidebar.Controls.Add(btnBanVe);
            panelSidebar.Controls.Add(btnSuatChieu);
            panelSidebar.Controls.Add(btnPhongChieu);
            panelSidebar.Controls.Add(btnPhim);
            panelSidebar.Controls.Add(btnDashboard);
            panelSidebar.Controls.Add(lblNav);
            panelSidebar.Controls.Add(accentLine);
            panelSidebar.Controls.Add(lblTitle);
            this.Controls.Add(panelSidebar);

            // Content Panel
            panelContent = new Panel();
            panelContent.Dock = DockStyle.Fill;
            panelContent.BackColor = BgColor;
            panelContent.Padding = new Padding(30, 25, 30, 15);
            this.Controls.Add(panelContent);
            panelContent.BringToFront();
        }

        private Button CreateSidebarButton(string text)
        {
            var btn = new Button();
            btn.Text = text;
            btn.Dock = DockStyle.Top;
            btn.Height = 45;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(45, 45, 75);
            btn.BackColor = SidebarBg;
            btn.ForeColor = Color.FromArgb(200, 210, 230);
            btn.Font = new Font("Segoe UI", 10.5F);
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(15, 0, 0, 0);
            btn.Cursor = Cursors.Hand;
            btn.MouseEnter += (s, e) => { btn.ForeColor = Color.White; };
            btn.MouseLeave += (s, e) => { btn.ForeColor = Color.FromArgb(200, 210, 230); };
            return btn;
        }

        private void LoadDashboard()
        {
            panelContent.Controls.Clear();

            var container = new Panel();
            container.Dock = DockStyle.Fill;
            container.AutoScroll = true;
            container.BackColor = BgColor;

            var lblHeading = new Label();
            lblHeading.Text = "TONG QUAN HE THONG";
            lblHeading.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblHeading.ForeColor = Color.FromArgb(25, 25, 45);
            lblHeading.AutoSize = true;
            lblHeading.Location = new Point(0, 0);
            container.Controls.Add(lblHeading);

            var lblDesc = new Label();
            lblDesc.Text = "Quan ly phim, lich chieu va ban ve noi bo";
            lblDesc.Font = new Font("Segoe UI", 10F);
            lblDesc.ForeColor = Color.FromArgb(130, 140, 160);
            lblDesc.AutoSize = true;
            lblDesc.Location = new Point(2, 42);
            container.Controls.Add(lblDesc);

            var panelCards = new FlowLayoutPanel();
            panelCards.Location = new Point(0, 80);
            panelCards.Size = new Size(900, 300);
            panelCards.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panelCards.WrapContents = true;
            panelCards.BackColor = Color.Transparent;

            try
            {
                var bll = new ThongKeBLL();
                DataTable dt = bll.GetTongQuan();
                if (dt.Rows.Count > 0)
                {
                    var row = dt.Rows[0];
                    panelCards.Controls.Add(CreateCard("Tong phim", row["TongPhim"].ToString()!, Color.FromArgb(52, 152, 219), "Trong he thong"));
                    panelCards.Controls.Add(CreateCard("Tong phong", row["TongPhong"].ToString()!, Color.FromArgb(155, 89, 182), "Phong chieu"));
                    panelCards.Controls.Add(CreateCard("Suat chieu", row["TongSuatChieu"].ToString()!, Color.FromArgb(46, 204, 113), "Tat ca"));
                    panelCards.Controls.Add(CreateCard("Ve hom nay", row["VeHomNay"].ToString()!, Color.FromArgb(241, 196, 15), DateTime.Now.ToString("dd/MM")));
                    panelCards.Controls.Add(CreateCard("Tong ve ban", row["TongVe"].ToString()!, Color.FromArgb(231, 76, 60), "Ve"));
                    panelCards.Controls.Add(CreateCard("Doanh thu", (Convert.ToInt64(row["TongDoanhThu"]) / 1000).ToString("N0") + "K", Color.FromArgb(26, 188, 156), "VND"));
                }
            }
            catch (Exception ex)
            {
                var lblError = new Label();
                lblError.Text = $"Khong the ket noi database!\n\nVui long kiem tra:\n1. SQL Server da khoi dong\n2. Database QuanLyPhimDB da duoc tao\n3. Connection string trong App.config\n\nLoi: {ex.Message}";
                lblError.Font = new Font("Segoe UI", 11F);
                lblError.ForeColor = Color.FromArgb(180, 60, 50);
                lblError.AutoSize = true;
                lblError.MaximumSize = new Size(600, 0);
                lblError.Location = new Point(0, 80);
                container.Controls.Add(lblError);
            }

            container.Controls.Add(panelCards);
            panelContent.Controls.Add(container);
            toolStripStatus.Text = "Trang chu - Dashboard";
        }

        private Panel CreateCard(string title, string value, Color color, string subtitle)
        {
            var card = new Panel();
            card.Size = new Size(210, 110);
            card.BackColor = CardBg;
            card.Margin = new Padding(0, 0, 15, 15);

            var colorBar = new Panel();
            colorBar.Dock = DockStyle.Top;
            colorBar.Height = 4;
            colorBar.BackColor = color;
            card.Controls.Add(colorBar);

            var lblT = new Label { Text = title, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(100, 110, 130), Location = new Point(15, 14), AutoSize = true };
            card.Controls.Add(lblT);

            var lblV = new Label { Text = value, Font = new Font("Segoe UI", 20F, FontStyle.Bold), ForeColor = color, Location = new Point(15, 36), AutoSize = true };
            card.Controls.Add(lblV);

            var lblS = new Label { Text = subtitle, Font = new Font("Segoe UI", 8.5F), ForeColor = Color.FromArgb(160, 170, 185), Location = new Point(15, 85), AutoSize = true };
            card.Controls.Add(lblS);

            card.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(245, 248, 255);
            card.MouseLeave += (s, e) => card.BackColor = CardBg;
            return card;
        }

        private void OpenForm(Form form)
        {
            form.ShowDialog();
            LoadDashboard();
        }
    }
}
