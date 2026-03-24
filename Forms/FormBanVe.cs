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
            this.Text = "Ban Ve";
            this.Size = new Size(950, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Font = new Font("Segoe UI", 9.5F);

            // === TOP: Ban ve form ===
            var panelTop = new Panel { Dock = DockStyle.Top, Height = 240, Padding = new Padding(15), BackColor = Color.FromArgb(245, 248, 255) };

            var lblTitle = new Label { Text = "BAN VE", Font = new Font("Segoe UI", 14F, FontStyle.Bold), ForeColor = Color.FromArgb(229, 57, 53), Dock = DockStyle.Top, Height = 35 };
            panelTop.Controls.Add(lblTitle);

            int y = 45;
            panelTop.Controls.Add(new Label { Text = "Suat chieu:", Location = new Point(15, y + 3), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            cboSuatChieu = new ComboBox { Location = new Point(120, y), Width = 400, DropDownStyle = ComboBoxStyle.DropDownList };
            cboSuatChieu.SelectedIndexChanged += CboSuatChieu_Changed;
            panelTop.Controls.Add(cboSuatChieu); y += 32;

            lblThongTin = new Label { Text = "", Location = new Point(120, y), AutoSize = true, ForeColor = Color.FromArgb(52, 152, 219) };
            panelTop.Controls.Add(lblThongTin); y += 25;

            panelTop.Controls.Add(new Label { Text = "Ten KH:", Location = new Point(15, y + 3), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            txtTenKH = new TextBox { Location = new Point(120, y), Width = 200 };
            panelTop.Controls.Add(txtTenKH);

            panelTop.Controls.Add(new Label { Text = "SDT:", Location = new Point(340, y + 3), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            txtSDT = new TextBox { Location = new Point(380, y), Width = 140 };
            panelTop.Controls.Add(txtSDT); y += 32;

            panelTop.Controls.Add(new Label { Text = "Ghe:", Location = new Point(15, y + 3), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            cboGhe = new ComboBox { Location = new Point(120, y), Width = 100, DropDownStyle = ComboBoxStyle.DropDownList };
            panelTop.Controls.Add(cboGhe);

            lblGiaVe = new Label { Text = "Gia ve: 0 VND", Location = new Point(240, y + 3), AutoSize = true, Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = Color.FromArgb(229, 57, 53) };
            panelTop.Controls.Add(lblGiaVe); y += 35;

            var btnBanVe = new Button
            {
                Text = "XAC NHAN BAN VE", Location = new Point(120, y), Width = 200, Height = 35,
                BackColor = Color.FromArgb(46, 204, 113), ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold), FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand
            };
            btnBanVe.FlatAppearance.BorderSize = 0;
            btnBanVe.Click += BtnBanVe_Click;
            panelTop.Controls.Add(btnBanVe);

            var btnHuyVe = new Button
            {
                Text = "Huy ve da chon", Location = new Point(340, y), Width = 140, Height = 35,
                BackColor = Color.FromArgb(231, 76, 60), ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand
            };
            btnHuyVe.FlatAppearance.BorderSize = 0;
            btnHuyVe.Click += BtnHuyVe_Click;
            panelTop.Controls.Add(btnHuyVe);

            this.Controls.Add(panelTop);

            // === BOTTOM: List ve da ban ===
            dgvVe = new DataGridView
            {
                Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White, BorderStyle = BorderStyle.None, RowHeadersVisible = false
            };
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

            lblGiaVe.Text = $"Gia ve: {suat.GiaVeStr}";
            lblThongTin.Text = $"Phong: {suat.TenPhong} | Ngay: {suat.NgayChieuStr} | Gio: {suat.GioChieuStr}";

            // Load ghe chua dat
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
            if (dgvVe.Columns.Count > 0)
            {
                dgvVe.Columns["MaVe"].HeaderText = "Ma ve"; dgvVe.Columns["MaVe"].Width = 50;
                dgvVe.Columns["TenKhachHang"].HeaderText = "Khach hang";
                dgvVe.Columns["SoDienThoai"].HeaderText = "SDT";
                dgvVe.Columns["MaGhe"].HeaderText = "Ghe";
                dgvVe.Columns["GiaVe"].HeaderText = "Gia ve";
                dgvVe.Columns["TenPhim"].HeaderText = "Phim";
                dgvVe.Columns["TenPhong"].HeaderText = "Phong";
                dgvVe.Columns["GioChieuStr"].HeaderText = "Gio";
                dgvVe.Columns["NgayChieuStr"].HeaderText = "Ngay";
                dgvVe.Columns["NgayBan"].HeaderText = "Ngay ban";
                dgvVe.Columns["MaSuat"].Visible = false;
            }
        }

        private void BtnBanVe_Click(object? s, EventArgs e)
        {
            if (cboSuatChieu.SelectedValue is not int maSuat) { MessageBox.Show("Chon suat chieu!"); return; }
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
            MessageBox.Show(msg, ok ? "Thanh cong" : "Loi", MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            if (ok)
            {
                LoadVe();
                CboSuatChieu_Changed(null, EventArgs.Empty); // refresh ghe
            }
        }

        private void BtnHuyVe_Click(object? s, EventArgs e)
        {
            if (dgvVe.CurrentRow == null) { MessageBox.Show("Chon ve can huy!"); return; }
            var ve = (Ve)dgvVe.CurrentRow.DataBoundItem;
            if (MessageBox.Show($"Huy ve {ve.MaVe} (KH: {ve.TenKhachHang}, Ghe: {ve.MaGhe})?", "Xac nhan", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
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
