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
            this.Text = "Quan ly Suat chieu";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Font = new Font("Segoe UI", 9.5F);

            // === TOP: Input Form ===
            var panelTop = new Panel { Dock = DockStyle.Top, Height = 220, Padding = new Padding(15) };

            var lblTitle = new Label { Text = "THONG TIN SUAT CHIEU", Font = new Font("Segoe UI", 13F, FontStyle.Bold), Dock = DockStyle.Top, Height = 35 };
            panelTop.Controls.Add(lblTitle);

            int y = 45;
            panelTop.Controls.Add(new Label { Text = "Phim:", Location = new Point(15, y + 3), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            cboPhim = new ComboBox { Location = new Point(110, y), Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
            panelTop.Controls.Add(cboPhim); y += 32;

            panelTop.Controls.Add(new Label { Text = "Phong:", Location = new Point(15, y + 3), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            cboPhong = new ComboBox { Location = new Point(110, y), Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };
            panelTop.Controls.Add(cboPhong); y += 32;

            panelTop.Controls.Add(new Label { Text = "Ngay chieu:", Location = new Point(15, y + 3), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            dtpNgay = new DateTimePicker { Location = new Point(110, y), Width = 150, Format = DateTimePickerFormat.Short };
            panelTop.Controls.Add(dtpNgay);

            panelTop.Controls.Add(new Label { Text = "Gio:", Location = new Point(280, y + 3), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            txtGio = new TextBox { Location = new Point(320, y), Width = 70, Text = "10:00" };
            panelTop.Controls.Add(txtGio); y += 32;

            panelTop.Controls.Add(new Label { Text = "Gia ve (VND):", Location = new Point(15, y + 3), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            txtGiaVe = new TextBox { Location = new Point(110, y), Width = 120, Text = "75000" };
            panelTop.Controls.Add(txtGiaVe); y += 35;

            // Buttons
            var btnThem = new Button { Text = "Them", Location = new Point(15, y), Width = 70, BackColor = Color.FromArgb(46, 204, 113), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            var btnSua = new Button { Text = "Sua", Location = new Point(90, y), Width = 70, BackColor = Color.FromArgb(52, 152, 219), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            var btnXoa = new Button { Text = "Xoa", Location = new Point(165, y), Width = 70, BackColor = Color.FromArgb(231, 76, 60), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnThem.FlatAppearance.BorderSize = 0; btnSua.FlatAppearance.BorderSize = 0; btnXoa.FlatAppearance.BorderSize = 0;

            btnThem.Click += (s, e) => { var (ok, msg) = _bll.Insert(GetFormData()); Msg(ok, msg); if (ok) LoadData(); };
            btnSua.Click += (s, e) => { if (dgv.CurrentRow == null) return; var (ok, msg) = _bll.Update(GetFormData()); Msg(ok, msg); if (ok) LoadData(); };
            btnXoa.Click += (s, e) =>
            {
                if (dgv.CurrentRow == null) return;
                var sc = (SuatChieu)dgv.CurrentRow.DataBoundItem;
                if (MessageBox.Show($"Xoa suat chieu {sc.TenPhim} - {sc.GioChieuStr}?", "Xac nhan", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                var (ok, msg) = _bll.Delete(sc.MaSuat);
                Msg(ok, msg); if (ok) LoadData();
            };
            panelTop.Controls.AddRange(new Control[] { btnThem, btnSua, btnXoa });

            // Filter section (right side of top panel)
            var lblFilter = new Label { Text = "LOC SUAT CHIEU", Font = new Font("Segoe UI", 10F, FontStyle.Bold), Location = new Point(500, 45), AutoSize = true };
            panelTop.Controls.Add(lblFilter);

            panelTop.Controls.Add(new Label { Text = "Phim:", Location = new Point(500, 75), AutoSize = true });
            cboLocPhim = new ComboBox { Location = new Point(560, 72), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            panelTop.Controls.Add(cboLocPhim);

            chkLocNgay = new CheckBox { Text = "Loc theo ngay:", Location = new Point(500, 105), AutoSize = true };
            panelTop.Controls.Add(chkLocNgay);
            dtpLocNgay = new DateTimePicker { Location = new Point(630, 102), Width = 130, Format = DateTimePickerFormat.Short };
            panelTop.Controls.Add(dtpLocNgay);

            var btnLoc = new Button { Text = "Loc", Location = new Point(500, 135), Width = 80, BackColor = Color.FromArgb(52, 152, 219), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnLoc.FlatAppearance.BorderSize = 0;
            btnLoc.Click += (s, e) =>
            {
                int? maPhim = cboLocPhim.SelectedValue is int v && v > 0 ? v : null;
                DateTime? ngay = chkLocNgay.Checked ? dtpLocNgay.Value.Date : null;
                dgv.DataSource = _bll.Search(maPhim, ngay);
                FormatGrid();
            };
            panelTop.Controls.Add(btnLoc);

            this.Controls.Add(panelTop);

            // === DataGridView ===
            dgv = new DataGridView
            {
                Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White, BorderStyle = BorderStyle.None, RowHeadersVisible = false
            };
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
            this.Controls.Add(dgv);
            dgv.BringToFront();
        }

        private void LoadCombos()
        {
            var phimList = _phimBLL.GetAll();
            cboPhim.DataSource = phimList;
            cboPhim.DisplayMember = "TenPhim";
            cboPhim.ValueMember = "MaPhim";

            var filterPhim = new List<Phim> { new Phim { MaPhim = 0, TenPhim = "-- Tat ca --" } };
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
            FormatGrid();
        }

        private void FormatGrid()
        {
            if (dgv.Columns.Count == 0) return;
            dgv.Columns["MaSuat"].HeaderText = "Ma"; dgv.Columns["MaSuat"].Width = 40;
            dgv.Columns["TenPhim"].HeaderText = "Phim";
            dgv.Columns["TenPhong"].HeaderText = "Phong";
            dgv.Columns["NgayChieuStr"].HeaderText = "Ngay chieu";
            dgv.Columns["GioChieuStr"].HeaderText = "Gio";
            dgv.Columns["GiaVeStr"].HeaderText = "Gia ve";
            dgv.Columns["MaPhim"].Visible = false;
            dgv.Columns["MaPhong"].Visible = false;
            dgv.Columns["NgayChieu"].Visible = false;
            dgv.Columns["GioChieu"].Visible = false;
            dgv.Columns["GiaVe"].Visible = false;
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

        private void Msg(bool ok, string msg) => MessageBox.Show(msg, ok ? "Thanh cong" : "Loi", MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
    }
}
