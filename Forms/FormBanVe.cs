using QuanLyPhimVaLichChieu.BusinessLogic;
using QuanLyPhimVaLichChieu.Models;

namespace QuanLyPhimVaLichChieu.Forms
{
    public class FormBanVe : Form
    {
        private DataGridView dgvVe = null!;
        private ComboBox cboSuatChieu = null!, cboGhe = null!;
        private TextBox txtTenKH = null!, txtSDT = null!;
        private Label lblGiaVe = null!, lblThongTin = null!;
        private readonly VeBLL _veBLL = new VeBLL();
        private readonly SuatChieuBLL _suatBLL = new SuatChieuBLL();

        public FormBanVe()
        {
            InitializeComponent();
            LoadSuatChieu();
            LoadVe();
        }

        private void InitializeComponent()
        {
            UITheme.StyleForm(this, "Bán Vé");
            this.Size = new Size(950, 600);

            // === Header ===
            this.Controls.Add(UITheme.CreateFormHeader("BÁN VÉ", "Bán vé và quản lý vé đã bán"));

            // === TOP: Ban ve form ===
            var panelTop = new Panel { Dock = DockStyle.Top, Height = 230, Padding = new Padding(15), BackColor = UITheme.BgCard };

            int y = 10;
            panelTop.Controls.Add(UITheme.CreateLabel("Suất chiếu:", 15, y + 3));
            cboSuatChieu = UITheme.CreateComboBox(120, y, 400);
            cboSuatChieu.SelectedIndexChanged += CboSuatChieu_Changed;
            panelTop.Controls.Add(cboSuatChieu); y += 32;

            lblThongTin = new Label { Text = "", Location = new Point(120, y), AutoSize = true, ForeColor = UITheme.AccentCyan, Font = UITheme.FontBody };
            panelTop.Controls.Add(lblThongTin); y += 25;

            panelTop.Controls.Add(UITheme.CreateLabel("Tên KH:", 15, y + 3));
            txtTenKH = UITheme.CreateTextBox(120, y, 200);
            panelTop.Controls.Add(txtTenKH);

            panelTop.Controls.Add(UITheme.CreateLabel("SĐT:", 340, y + 3));
            txtSDT = UITheme.CreateTextBox(380, y, 140);
            panelTop.Controls.Add(txtSDT); y += 32;

            panelTop.Controls.Add(UITheme.CreateLabel("Ghế:", 15, y + 3));
            cboGhe = UITheme.CreateComboBox(120, y, 100);
            panelTop.Controls.Add(cboGhe);

            lblGiaVe = new Label
            {
                Text = "Giá vé: 0 VND",
                Location = new Point(240, y + 3),
                AutoSize = true,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = UITheme.AccentRed
            };
            panelTop.Controls.Add(lblGiaVe); y += 38;

            var btnBanVe = UITheme.CreateButton("XÁC NHẬN BÁN VÉ", UITheme.AccentGreen, 200);
            btnBanVe.Height = 38;
            btnBanVe.Location = new Point(120, y);
            btnBanVe.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnBanVe.Click += BtnBanVe_Click;
            panelTop.Controls.Add(btnBanVe);

            var btnHuyVe = UITheme.CreateButton("Hủy vé đã chọn", UITheme.AccentRed, 140);
            btnHuyVe.Height = 38;
            btnHuyVe.Location = new Point(340, y);
            btnHuyVe.Click += BtnHuyVe_Click;
            panelTop.Controls.Add(btnHuyVe);

            this.Controls.Add(panelTop);

            // === BOTTOM: DataGridView ===
            dgvVe = new DataGridView();
            UITheme.StyleDataGridView(dgvVe);
            dgvVe.Dock = DockStyle.Fill;
            dgvVe.DataBindingComplete += (s, e) => FormatVeGrid();
            this.Controls.Add(dgvVe);
            dgvVe.BringToFront();
        }

        private void LoadSuatChieu()
        {
            var list = _suatBLL.GetAll();
            var displayList = list.Select(s => new
            {
                s.MaSuat,
                Display = $"{s.TenPhim} | {s.TenPhong} | {s.NgayChieuStr} {s.GioChieuStr} | {s.GiaVeStr}"
            }).ToList();
            cboSuatChieu.DataSource = displayList;
            cboSuatChieu.DisplayMember = "Display";
            cboSuatChieu.ValueMember = "MaSuat";
        }

        private void CboSuatChieu_Changed(object? s, EventArgs e)
        {
            if (cboSuatChieu.SelectedValue is not int maSuat) return;
            var allSuat = _suatBLL.GetAll();
            var suat = allSuat.FirstOrDefault(x => x.MaSuat == maSuat);
            if (suat == null) return;

            lblGiaVe.Text = $"Giá vé: {suat.GiaVeStr}";
            lblThongTin.Text = $"Phòng: {suat.TenPhong} | Ngày: {suat.NgayChieuStr} | Giờ: {suat.GioChieuStr}";

            var gheDaDat = _veBLL.GetGheDaDat(maSuat);
            var allGhe = new List<string>();
            foreach (char row in "ABCDEFGH")
                for (int col = 1; col <= 10; col++)
                    allGhe.Add($"{row}{col}");

            var gheTrong = allGhe.Where(g => !gheDaDat.Contains(g)).ToList();
            cboGhe.DataSource = gheTrong;
        }

        private void LoadVe()
        {
            var list = _veBLL.GetAll();
            dgvVe.DataSource = list;
        }

        private void FormatVeGrid()
        {
            if (dgvVe.Columns.Count == 0) return;
            void SetCol(string name, Action<DataGridViewColumn> action)
            {
                var col = dgvVe.Columns[name];
                if (col != null) action(col);
            }
            SetCol("MaVe", c => { c.HeaderText = "MÃ VÉ"; c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None; c.MinimumWidth = 50; c.Width = 60; });
            SetCol("TenKhachHang", c => c.HeaderText = "KHÁCH HÀNG");
            SetCol("SoDienThoai", c => c.HeaderText = "SĐT");
            SetCol("MaGhe", c => c.HeaderText = "GHẾ");
            SetCol("GiaVe", c => c.HeaderText = "GIÁ VÉ");
            SetCol("TenPhim", c => c.HeaderText = "PHIM");
            SetCol("TenPhong", c => c.HeaderText = "PHÒNG");
            SetCol("GioChieuStr", c => c.HeaderText = "GIỜ");
            SetCol("NgayChieuStr", c => c.HeaderText = "NGÀY");
            SetCol("NgayBan", c => c.HeaderText = "NGÀY BÁN");
            SetCol("MaSuat", c => c.Visible = false);
        }

        private void BtnBanVe_Click(object? s, EventArgs e)
        {
            if (cboSuatChieu.SelectedValue is not int maSuat) { MessageBox.Show("Chọn suất chiếu!"); return; }
            var allSuat = _suatBLL.GetAll();
            var suat = allSuat.FirstOrDefault(x => x.MaSuat == maSuat);

            var ve = new Ve
            {
                MaSuat = maSuat,
                TenKhachHang = txtTenKH.Text.Trim(),
                SoDienThoai = txtSDT.Text.Trim(),
                MaGhe = cboGhe.SelectedItem?.ToString() ?? "",
                GiaVe = suat?.GiaVe ?? 0
            };

            var (ok, msg) = _veBLL.Insert(ve);
            MessageBox.Show(msg, ok ? "Thành công" : "Lỗi", MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            if (ok)
            {
                LoadVe();
                CboSuatChieu_Changed(null, EventArgs.Empty);
            }
        }

        private void BtnHuyVe_Click(object? s, EventArgs e)
        {
            if (dgvVe.CurrentRow == null) { MessageBox.Show("Chọn vé cần hủy!"); return; }
            var ve = (Ve)dgvVe.CurrentRow.DataBoundItem;
            if (MessageBox.Show($"Hủy vé {ve.MaVe} (KH: {ve.TenKhachHang}, Ghế: {ve.MaGhe})?", "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            var (ok, msg) = _veBLL.Delete(ve.MaVe);
            MessageBox.Show(msg);
            if (ok)
            {
                LoadVe();
                CboSuatChieu_Changed(null, EventArgs.Empty);
            }
        }
    }
}
