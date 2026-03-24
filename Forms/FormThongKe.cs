using System.Data;
using QuanLyPhimVaLichChieu.BusinessLogic;

namespace QuanLyPhimVaLichChieu.Forms
{
    public class FormThongKe : Form
    {
        private DataGridView dgv = null!;
        private Label lblTongVe = null!, lblTongDoanhThu = null!;
        private readonly ThongKeBLL _bll = new ThongKeBLL();

        public FormThongKe()
        {
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            UITheme.StyleForm(this, "Thống kê báo cáo");
            this.Size = new Size(800, 550);

            // === Header with stats ===
            var panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 110,
                BackColor = UITheme.BgSidebar,
                Padding = new Padding(25, 15, 20, 10)
            };

            var lblTitle = new Label
            {
                Text = "\U0001F4CA  THỐNG KÊ VÉ BÁN THEO PHIM",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = UITheme.TextPrimary,
                Location = new Point(25, 12),
                AutoSize = true
            };
            panelHeader.Controls.Add(lblTitle);

            lblTongVe = new Label
            {
                Text = "Tổng vé: 0",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = UITheme.AccentGreen,
                Location = new Point(25, 55),
                AutoSize = true
            };
            panelHeader.Controls.Add(lblTongVe);

            lblTongDoanhThu = new Label
            {
                Text = "Doanh thu: 0 VND",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = UITheme.AccentYellow,
                Location = new Point(250, 55),
                AutoSize = true
            };
            panelHeader.Controls.Add(lblTongDoanhThu);

            var btnRefresh = UITheme.CreateButton("Làm mới", UITheme.AccentBlue, 100);
            btnRefresh.Location = new Point(600, 52);
            btnRefresh.Click += (s, e) => LoadData();
            panelHeader.Controls.Add(btnRefresh);

            // Accent line
            var line = new Panel { Dock = DockStyle.Bottom, Height = 2, BackColor = UITheme.Accent };
            panelHeader.Controls.Add(line);

            this.Controls.Add(panelHeader);

            // === DataGridView ===
            dgv = new DataGridView();
            UITheme.StyleDataGridView(dgv);
            dgv.Dock = DockStyle.Fill;
            dgv.DataBindingComplete += (s, e) => FormatGrid();
            this.Controls.Add(dgv);
            dgv.BringToFront();
        }

        private void FormatGrid()
        {
            if (dgv.Columns.Count == 0) return;
            void SetCol(string name, Action<DataGridViewColumn> action)
            {
                var col = dgv.Columns[name];
                if (col != null) action(col);
            }
            SetCol("MaPhim", c => { c.HeaderText = "MÃ"; c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None; c.MinimumWidth = 40; c.Width = 50; });
            SetCol("TenPhim", c => c.HeaderText = "TÊN PHIM");
            SetCol("TenTheLoai", c => c.HeaderText = "THỂ LOẠI");
            SetCol("SoVeBan", c => { c.HeaderText = "SỐ VÉ BÁN"; c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; });
            SetCol("DoanhThu", c => { c.HeaderText = "DOANH THU (VND)"; c.DefaultCellStyle.Format = "N0"; c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; });
        }

        private void LoadData()
        {
            try
            {
                DataTable dt = _bll.ThongKeVeTheoPhim();
                dgv.DataSource = dt;

                int tongVe = 0;
                long tongDoanhThu = 0;
                foreach (DataRow row in dt.Rows)
                {
                    tongVe += Convert.ToInt32(row["SoVeBan"]);
                    tongDoanhThu += Convert.ToInt64(row["DoanhThu"]);
                }
                lblTongVe.Text = $"Tổng vé: {tongVe}";
                lblTongDoanhThu.Text = $"Doanh thu: {tongDoanhThu:N0} VND";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
