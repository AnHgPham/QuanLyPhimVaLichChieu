using QuanLyPhimVaLichChieu.BusinessLogic;
using QuanLyPhimVaLichChieu.Models;

namespace QuanLyPhimVaLichChieu.Forms
{
    public class FormPhim : Form
    {
        private DataGridView dgv = null!;
        private TextBox txtTenPhim = null!, txtThoiLuong = null!, txtQuocGia = null!, txtMoTa = null!, txtTimKiem = null!;
        private ComboBox cboTheLoai = null!, cboTrangThai = null!, cboLocTheLoai = null!;
        private Button btnThem = null!, btnSua = null!, btnXoa = null!, btnLamMoi = null!;
        private readonly PhimBLL _bll = new PhimBLL();
        private readonly TheLoaiBLL _theLoaiBLL = new TheLoaiBLL();

        public FormPhim()
        {
            InitializeComponent();
            LoadTheLoai();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Text = "Quan ly Phim";
            this.Size = new Size(1050, 650);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Font = new Font("Segoe UI", 9.5F);

            // === LEFT: Input Form ===
            var panelLeft = new Panel { Dock = DockStyle.Left, Width = 320, Padding = new Padding(15) };

            var lblTitle = new Label { Text = "THONG TIN PHIM", Font = new Font("Segoe UI", 13F, FontStyle.Bold), Dock = DockStyle.Top, Height = 40 };
            panelLeft.Controls.Add(lblTitle);

            int y = 50;
            panelLeft.Controls.Add(CreateLabel("Ten phim:", y)); y += 22;
            txtTenPhim = CreateTextBox(y); panelLeft.Controls.Add(txtTenPhim); y += 35;

            panelLeft.Controls.Add(CreateLabel("The loai:", y)); y += 22;
            cboTheLoai = new ComboBox { Location = new Point(15, y), Width = 270, DropDownStyle = ComboBoxStyle.DropDownList };
            panelLeft.Controls.Add(cboTheLoai); y += 35;

            panelLeft.Controls.Add(CreateLabel("Thoi luong (phut):", y)); y += 22;
            txtThoiLuong = CreateTextBox(y); panelLeft.Controls.Add(txtThoiLuong); y += 35;

            panelLeft.Controls.Add(CreateLabel("Quoc gia:", y)); y += 22;
            txtQuocGia = CreateTextBox(y); panelLeft.Controls.Add(txtQuocGia); y += 35;

            panelLeft.Controls.Add(CreateLabel("Trang thai:", y)); y += 22;
            cboTrangThai = new ComboBox { Location = new Point(15, y), Width = 270, DropDownStyle = ComboBoxStyle.DropDownList };
            cboTrangThai.Items.AddRange(new object[] { "Dang chieu", "Sap chieu", "Ngung chieu" });
            cboTrangThai.SelectedIndex = 0;
            panelLeft.Controls.Add(cboTrangThai); y += 35;

            panelLeft.Controls.Add(CreateLabel("Mo ta:", y)); y += 22;
            txtMoTa = new TextBox { Location = new Point(15, y), Width = 270, Height = 60, Multiline = true };
            panelLeft.Controls.Add(txtMoTa); y += 70;

            // Buttons
            var panelBtn = new FlowLayoutPanel { Location = new Point(15, y), Width = 280, Height = 40 };
            btnThem = new Button { Text = "Them", Width = 65, BackColor = Color.FromArgb(46, 204, 113), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnSua = new Button { Text = "Sua", Width = 65, BackColor = Color.FromArgb(52, 152, 219), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnXoa = new Button { Text = "Xoa", Width = 65, BackColor = Color.FromArgb(231, 76, 60), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnLamMoi = new Button { Text = "Lam moi", Width = 70, FlatStyle = FlatStyle.Flat };
            btnThem.FlatAppearance.BorderSize = 0; btnSua.FlatAppearance.BorderSize = 0;
            btnXoa.FlatAppearance.BorderSize = 0; btnLamMoi.FlatAppearance.BorderSize = 0;
            panelBtn.Controls.AddRange(new Control[] { btnThem, btnSua, btnXoa, btnLamMoi });
            panelLeft.Controls.Add(panelBtn);

            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click += (s, e) => ClearForm();

            this.Controls.Add(panelLeft);

            // === RIGHT: DataGridView + Search ===
            var panelRight = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };

            var panelSearch = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 40, WrapContents = false };
            txtTimKiem = new TextBox { Width = 200, PlaceholderText = "Tim theo ten phim..." };
            cboLocTheLoai = new ComboBox { Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            var btnTim = new Button { Text = "Tim", Width = 60, BackColor = Color.FromArgb(52, 152, 219), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnTim.FlatAppearance.BorderSize = 0;
            btnTim.Click += (s, e) => SearchData();
            txtTimKiem.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) SearchData(); };
            panelSearch.Controls.AddRange(new Control[] { txtTimKiem, cboLocTheLoai, btnTim });
            panelRight.Controls.Add(panelSearch);

            dgv = new DataGridView
            {
                Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White, BorderStyle = BorderStyle.None,
                RowHeadersVisible = false
            };
            dgv.SelectionChanged += Dgv_SelectionChanged;
            panelRight.Controls.Add(dgv);
            dgv.BringToFront();

            this.Controls.Add(panelRight);
            panelRight.BringToFront();
        }

        private Label CreateLabel(string text, int y) => new Label { Text = text, Location = new Point(15, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
        private TextBox CreateTextBox(int y) => new TextBox { Location = new Point(15, y), Width = 270 };

        private void LoadTheLoai()
        {
            var list = _theLoaiBLL.GetAll();
            cboTheLoai.DataSource = list;
            cboTheLoai.DisplayMember = "TenTheLoai";
            cboTheLoai.ValueMember = "MaTheLoai";

            var filterList = new List<TheLoai> { new TheLoai { MaTheLoai = 0, TenTheLoai = "-- Tat ca the loai --" } };
            filterList.AddRange(list);
            cboLocTheLoai.DataSource = filterList;
            cboLocTheLoai.DisplayMember = "TenTheLoai";
            cboLocTheLoai.ValueMember = "MaTheLoai";
        }

        private void LoadData()
        {
            var list = _bll.GetAll();
            dgv.DataSource = list;
            FormatGrid();
        }

        private void SearchData()
        {
            string? keyword = string.IsNullOrWhiteSpace(txtTimKiem.Text) ? null : txtTimKiem.Text;
            int? maTheLoai = cboLocTheLoai.SelectedValue is int v && v > 0 ? v : null;
            dgv.DataSource = _bll.Search(keyword, maTheLoai);
            FormatGrid();
        }

        private void FormatGrid()
        {
            if (dgv.Columns.Count == 0) return;
            dgv.Columns["MaPhim"].HeaderText = "Ma";
            dgv.Columns["MaPhim"].Width = 40;
            dgv.Columns["TenPhim"].HeaderText = "Ten phim";
            dgv.Columns["TenTheLoai"].HeaderText = "The loai";
            dgv.Columns["ThoiLuong"].HeaderText = "Phut";
            dgv.Columns["ThoiLuong"].Width = 50;
            dgv.Columns["QuocGia"].HeaderText = "Quoc gia";
            dgv.Columns["TrangThai"].HeaderText = "Trang thai";
            dgv.Columns["MaTheLoai"].Visible = false;
            dgv.Columns["MoTa"].Visible = false;
            dgv.Columns["NgayTao"].Visible = false;
            dgv.Columns["NgayCapNhat"].Visible = false;
        }

        private void Dgv_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgv.CurrentRow == null) return;
            var p = (Phim)dgv.CurrentRow.DataBoundItem;
            txtTenPhim.Text = p.TenPhim;
            txtThoiLuong.Text = p.ThoiLuong.ToString();
            txtQuocGia.Text = p.QuocGia;
            txtMoTa.Text = p.MoTa;
            cboTheLoai.SelectedValue = p.MaTheLoai;
            cboTrangThai.SelectedItem = p.TrangThai;
        }

        private Phim GetFormData()
        {
            return new Phim
            {
                MaPhim = dgv.CurrentRow != null ? ((Phim)dgv.CurrentRow.DataBoundItem).MaPhim : 0,
                TenPhim = txtTenPhim.Text.Trim(),
                MaTheLoai = (int)(cboTheLoai.SelectedValue ?? 0),
                ThoiLuong = int.TryParse(txtThoiLuong.Text, out int tl) ? tl : 0,
                QuocGia = txtQuocGia.Text.Trim(),
                MoTa = txtMoTa.Text.Trim(),
                TrangThai = cboTrangThai.SelectedItem?.ToString() ?? "Dang chieu"
            };
        }

        private void BtnThem_Click(object? s, EventArgs e)
        {
            var (ok, msg) = _bll.Insert(GetFormData());
            MessageBox.Show(msg, ok ? "Thanh cong" : "Loi", MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            if (ok) { LoadData(); ClearForm(); }
        }

        private void BtnSua_Click(object? s, EventArgs e)
        {
            if (dgv.CurrentRow == null) { MessageBox.Show("Chon phim can sua!"); return; }
            var (ok, msg) = _bll.Update(GetFormData());
            MessageBox.Show(msg, ok ? "Thanh cong" : "Loi", MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            if (ok) LoadData();
        }

        private void BtnXoa_Click(object? s, EventArgs e)
        {
            if (dgv.CurrentRow == null) { MessageBox.Show("Chon phim can xoa!"); return; }
            var p = (Phim)dgv.CurrentRow.DataBoundItem;
            if (MessageBox.Show($"Xoa phim '{p.TenPhim}'?", "Xac nhan", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            var (ok, msg) = _bll.Delete(p.MaPhim);
            MessageBox.Show(msg, ok ? "Thanh cong" : "Loi", MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            if (ok) { LoadData(); ClearForm(); }
        }

        private void ClearForm()
        {
            txtTenPhim.Text = txtThoiLuong.Text = txtQuocGia.Text = txtMoTa.Text = "";
            cboTrangThai.SelectedIndex = 0;
            if (cboTheLoai.Items.Count > 0) cboTheLoai.SelectedIndex = 0;
        }
    }
}
