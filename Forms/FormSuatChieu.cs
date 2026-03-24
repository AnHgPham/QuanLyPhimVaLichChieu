using QuanLyPhimVaLichChieu.BusinessLogic;
using QuanLyPhimVaLichChieu.Models;

namespace QuanLyPhimVaLichChieu.Forms
{
    public class FormSuatChieu : Form
    {
        private DataGridView dgv = null!;
        private ComboBox cboPhim = null!, cboPhong = null!, cboLocPhim = null!;
        private DateTimePicker dtpNgay = null!, dtpLocNgay = null!;
        private TextBox txtGio = null!, txtGiaVe = null!;
        private CheckBox chkLocNgay = null!;
        private readonly SuatChieuBLL _bll = new SuatChieuBLL();
        private readonly PhimBLL _phimBLL = new PhimBLL();
        private readonly PhongChieuBLL _phongBLL = new PhongChieuBLL();

        public FormSuatChieu()
        {
            InitializeComponent();
            LoadCombos();
            LoadData();
        }

        private void InitializeComponent()
        {
            UITheme.StyleForm(this, "Quản lý Suất chiếu");
            this.Size = new Size(1000, 600);

            // === Header ===
            this.Controls.Add(UITheme.CreateFormHeader("SUẤT CHIẾU", "Quản lý lịch chiếu phim"));

            // === TOP: Input Form ===
            var panelTop = new Panel { Dock = DockStyle.Top, Height = 220, Padding = new Padding(15), BackColor = UITheme.BgCard };

            int y = 10;
            panelTop.Controls.Add(UITheme.CreateLabel("Phim:", 15, y + 3));
            cboPhim = UITheme.CreateComboBox(110, y, 250);
            panelTop.Controls.Add(cboPhim); y += 32;

            panelTop.Controls.Add(UITheme.CreateLabel("Phòng:", 15, y + 3));
            cboPhong = UITheme.CreateComboBox(110, y, 180);
            panelTop.Controls.Add(cboPhong); y += 32;

            panelTop.Controls.Add(UITheme.CreateLabel("Ngày chiếu:", 15, y + 3));
            dtpNgay = new DateTimePicker { Location = new Point(110, y), Width = 150, Format = DateTimePickerFormat.Short };
            panelTop.Controls.Add(dtpNgay);

            panelTop.Controls.Add(UITheme.CreateLabel("Giờ:", 280, y + 3));
            txtGio = UITheme.CreateTextBox(320, y, 70);
            txtGio.Text = "10:00";
            panelTop.Controls.Add(txtGio); y += 32;

            panelTop.Controls.Add(UITheme.CreateLabel("Giá vé (VND):", 15, y + 3));
            txtGiaVe = UITheme.CreateTextBox(110, y, 120);
            txtGiaVe.Text = "75000";
            panelTop.Controls.Add(txtGiaVe); y += 35;

            // Buttons
            var btnThem = UITheme.CreateButton("Thêm", UITheme.AccentGreen, 70);
            btnThem.Location = new Point(15, y);
            var btnSua = UITheme.CreateButton("Sửa", UITheme.AccentBlue, 70);
            btnSua.Location = new Point(90, y);
            var btnXoa = UITheme.CreateButton("Xóa", UITheme.AccentRed, 70);
            btnXoa.Location = new Point(165, y);

            btnThem.Click += (s, e) => { var (ok, msg) = _bll.Insert(GetFormData()); Msg(ok, msg); if (ok) LoadData(); };
            btnSua.Click += (s, e) => { if (dgv.CurrentRow == null) return; var (ok, msg) = _bll.Update(GetFormData()); Msg(ok, msg); if (ok) LoadData(); };
            btnXoa.Click += (s, e) =>
            {
                if (dgv.CurrentRow == null) return;
                var sc = (SuatChieu)dgv.CurrentRow.DataBoundItem;
                if (MessageBox.Show($"Xóa suất chiếu {sc.TenPhim} - {sc.GioChieuStr}?", "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                var (ok, msg) = _bll.Delete(sc.MaSuat);
                Msg(ok, msg); if (ok) LoadData();
            };
            panelTop.Controls.AddRange(new Control[] { btnThem, btnSua, btnXoa });

            // Filter section (right side)
            var lblFilter = new Label { Text = "LỌC SUẤT CHIẾU", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = UITheme.Accent, Location = new Point(500, 10), AutoSize = true };
            panelTop.Controls.Add(lblFilter);

            panelTop.Controls.Add(new Label { Text = "Phim:", Location = new Point(500, 40), AutoSize = true, ForeColor = UITheme.TextSecondary });
            cboLocPhim = UITheme.CreateComboBox(560, 37, 200);
            panelTop.Controls.Add(cboLocPhim);

            chkLocNgay = new CheckBox { Text = "Lọc theo ngày:", Location = new Point(500, 72), AutoSize = true, ForeColor = UITheme.TextSecondary };
            panelTop.Controls.Add(chkLocNgay);
            dtpLocNgay = new DateTimePicker { Location = new Point(630, 69), Width = 130, Format = DateTimePickerFormat.Short };
            panelTop.Controls.Add(dtpLocNgay);

            var btnLoc = UITheme.CreateButton("Lọc", UITheme.AccentBlue, 80);
            btnLoc.Location = new Point(500, 102);
            btnLoc.Click += (s, e) =>
            {
                int? maPhim = cboLocPhim.SelectedValue is int v && v > 0 ? v : null;
                DateTime? ngay = chkLocNgay.Checked ? dtpLocNgay.Value.Date : null;
                dgv.DataSource = _bll.Search(maPhim, ngay);
            };
            panelTop.Controls.Add(btnLoc);

            this.Controls.Add(panelTop);

            // === DataGridView ===
            dgv = new DataGridView();
            UITheme.StyleDataGridView(dgv);
            dgv.Dock = DockStyle.Fill;
            dgv.SelectionChanged += (s, e) =>
            {
                if (dgv.CurrentRow == null) return;
                var sc = (SuatChieu)dgv.CurrentRow.DataBoundItem;
                cboPhim.SelectedValue = sc.MaPhim;
                cboPhong.SelectedValue = sc.MaPhong;
                dtpNgay.Value = sc.NgayChieu;
                txtGio.Text = sc.GioChieuStr;
                txtGiaVe.Text = sc.GiaVe.ToString();
            };
            dgv.DataBindingComplete += (s, e) => FormatGrid();
            this.Controls.Add(dgv);
            dgv.BringToFront();
        }

        private void LoadCombos()
        {
            var phimList = _phimBLL.GetAll();
            cboPhim.DataSource = phimList;
            cboPhim.DisplayMember = "TenPhim";
            cboPhim.ValueMember = "MaPhim";

            var filterPhim = new List<Phim> { new Phim { MaPhim = 0, TenPhim = "-- Tất cả --" } };
            filterPhim.AddRange(phimList);
            cboLocPhim.DataSource = filterPhim;
            cboLocPhim.DisplayMember = "TenPhim";
            cboLocPhim.ValueMember = "MaPhim";

            cboPhong.DataSource = _phongBLL.GetActive();
            cboPhong.DisplayMember = "TenPhong";
            cboPhong.ValueMember = "MaPhong";
        }

        private void LoadData()
        {
            dgv.DataSource = _bll.GetAll();
        }

        private void FormatGrid()
        {
            if (dgv.Columns.Count == 0) return;
            void SetCol(string name, Action<DataGridViewColumn> action)
            {
                var col = dgv.Columns[name];
                if (col != null) action(col);
            }
            SetCol("MaSuat", c => { c.HeaderText = "MÃ"; c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None; c.MinimumWidth = 40; c.Width = 50; });
            SetCol("TenPhim", c => c.HeaderText = "PHIM");
            SetCol("TenPhong", c => c.HeaderText = "PHÒNG");
            SetCol("NgayChieuStr", c => c.HeaderText = "NGÀY CHIẾU");
            SetCol("GioChieuStr", c => c.HeaderText = "GIỜ");
            SetCol("GiaVeStr", c => c.HeaderText = "GIÁ VÉ");
            SetCol("MaPhim", c => c.Visible = false);
            SetCol("MaPhong", c => c.Visible = false);
            SetCol("NgayChieu", c => c.Visible = false);
            SetCol("GioChieu", c => c.Visible = false);
            SetCol("GiaVe", c => c.Visible = false);
        }

        private SuatChieu GetFormData()
        {
            TimeSpan gio = TimeSpan.TryParse(txtGio.Text, out TimeSpan t) ? t : new TimeSpan(10, 0, 0);
            return new SuatChieu
            {
                MaSuat = dgv.CurrentRow != null ? ((SuatChieu)dgv.CurrentRow.DataBoundItem).MaSuat : 0,
                MaPhim = (int)(cboPhim.SelectedValue ?? 0),
                MaPhong = (int)(cboPhong.SelectedValue ?? 0),
                NgayChieu = dtpNgay.Value.Date,
                GioChieu = gio,
                GiaVe = int.TryParse(txtGiaVe.Text, out int g) ? g : 75000
            };
        }

        private void Msg(bool ok, string msg) => MessageBox.Show(msg, ok ? "Thành công" : "Lỗi", MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
    }
}
