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
            UITheme.StyleForm(this, "Quản lý Phim");
            this.Size = new Size(1050, 650);

            // === Header ===
            this.Controls.Add(UITheme.CreateFormHeader("QUẢN LÝ PHIM", "Thêm, sửa, xóa và tìm kiếm phim"));

            // === LEFT: Input Form ===
            var panelLeft = UITheme.CreateInputPanel(330);

            var lblSection = new Label
            {
                Text = "THÔNG TIN PHIM",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = UITheme.Accent,
                Dock = DockStyle.Top,
                Height = 40,
                TextAlign = ContentAlignment.BottomLeft,
                Padding = new Padding(5, 0, 0, 5)
            };
            panelLeft.Controls.Add(lblSection);

            int y = 50;
            panelLeft.Controls.Add(UITheme.CreateLabel("Tên phim", 15, y)); y += 22;
            txtTenPhim = UITheme.CreateTextBox(15, y); panelLeft.Controls.Add(txtTenPhim); y += 35;

            panelLeft.Controls.Add(UITheme.CreateLabel("Thể loại", 15, y)); y += 22;
            cboTheLoai = UITheme.CreateComboBox(15, y); panelLeft.Controls.Add(cboTheLoai); y += 35;

            panelLeft.Controls.Add(UITheme.CreateLabel("Thời lượng (phút)", 15, y)); y += 22;
            txtThoiLuong = UITheme.CreateTextBox(15, y); panelLeft.Controls.Add(txtThoiLuong); y += 35;

            panelLeft.Controls.Add(UITheme.CreateLabel("Quốc gia", 15, y)); y += 22;
            txtQuocGia = UITheme.CreateTextBox(15, y); panelLeft.Controls.Add(txtQuocGia); y += 35;

            panelLeft.Controls.Add(UITheme.CreateLabel("Trạng thái", 15, y)); y += 22;
            cboTrangThai = UITheme.CreateComboBox(15, y);
            cboTrangThai.Items.AddRange(new object[] { "Đang chiếu", "Sắp chiếu", "Ngưng chiếu" });
            cboTrangThai.SelectedIndex = 0;
            panelLeft.Controls.Add(cboTrangThai); y += 35;

            panelLeft.Controls.Add(UITheme.CreateLabel("Mô tả", 15, y)); y += 22;
            txtMoTa = new TextBox { Location = new Point(15, y), Width = 280, Height = 60, Multiline = true };
            UITheme.StyleTextBox(txtMoTa);
            panelLeft.Controls.Add(txtMoTa); y += 70;

            // Buttons
            var panelBtn = new FlowLayoutPanel { Location = new Point(15, y), Width = 290, Height = 40, BackColor = Color.Transparent };
            btnThem = UITheme.CreateButton("Thêm", UITheme.AccentGreen, 70);
            btnSua = UITheme.CreateButton("Sửa", UITheme.AccentBlue, 70);
            btnXoa = UITheme.CreateButton("Xóa", UITheme.AccentRed, 70);
            btnLamMoi = UITheme.CreateButton("Làm mới", UITheme.BgInput, 75);
            btnLamMoi.ForeColor = UITheme.TextSecondary;
            panelBtn.Controls.AddRange(new Control[] { btnThem, btnSua, btnXoa, btnLamMoi });
            panelLeft.Controls.Add(panelBtn);

            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click += (s, e) => ClearForm();

            this.Controls.Add(panelLeft);

            // === RIGHT: DataGridView + Search ===
            var panelRight = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10), BackColor = UITheme.BgDark };

            var panelSearch = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 42, WrapContents = false, BackColor = Color.Transparent };
            txtTimKiem = new TextBox { Width = 200, PlaceholderText = "Tìm theo tên phim..." };
            UITheme.StyleTextBox(txtTimKiem);
            cboLocTheLoai = UITheme.CreateComboBox(0, 0, 150);
            var btnTim = UITheme.CreateButton("Tìm", UITheme.AccentBlue, 65);
            btnTim.Click += (s, e) => SearchData();
            txtTimKiem.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) SearchData(); };
            panelSearch.Controls.AddRange(new Control[] { txtTimKiem, cboLocTheLoai, btnTim });
            panelRight.Controls.Add(panelSearch);

            dgv = new DataGridView();
            UITheme.StyleDataGridView(dgv);
            dgv.Dock = DockStyle.Fill;
            dgv.SelectionChanged += Dgv_SelectionChanged;
            dgv.DataBindingComplete += (s, e) => FormatGrid();
            panelRight.Controls.Add(dgv);
            dgv.BringToFront();

            this.Controls.Add(panelRight);
            panelRight.BringToFront();
        }

        private void LoadTheLoai()
        {
            var list = _theLoaiBLL.GetAll();
            cboTheLoai.DataSource = list;
            cboTheLoai.DisplayMember = "TenTheLoai";
            cboTheLoai.ValueMember = "MaTheLoai";

            var filterList = new List<TheLoai> { new TheLoai { MaTheLoai = 0, TenTheLoai = "-- Tất cả thể loại --" } };
            filterList.AddRange(list);
            cboLocTheLoai.DataSource = filterList;
            cboLocTheLoai.DisplayMember = "TenTheLoai";
            cboLocTheLoai.ValueMember = "MaTheLoai";
        }

        private void LoadData()
        {
            var list = _bll.GetAll();
            dgv.DataSource = list;
        }

        private void SearchData()
        {
            string? keyword = string.IsNullOrWhiteSpace(txtTimKiem.Text) ? null : txtTimKiem.Text;
            int? maTheLoai = cboLocTheLoai.SelectedValue is int v && v > 0 ? v : null;
            dgv.DataSource = _bll.Search(keyword, maTheLoai);
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
            SetCol("ThoiLuong", c => { c.HeaderText = "PHÚT"; c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None; c.MinimumWidth = 50; c.Width = 60; });
            SetCol("QuocGia", c => c.HeaderText = "QUỐC GIA");
            SetCol("TrangThai", c => c.HeaderText = "TRẠNG THÁI");
            SetCol("MaTheLoai", c => c.Visible = false);
            SetCol("MoTa", c => c.Visible = false);
            SetCol("NgayTao", c => c.Visible = false);
            SetCol("NgayCapNhat", c => c.Visible = false);
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
                TrangThai = cboTrangThai.SelectedItem?.ToString() ?? "Đang chiếu"
            };
        }

        private void BtnThem_Click(object? s, EventArgs e)
        {
            var (ok, msg) = _bll.Insert(GetFormData());
            MessageBox.Show(msg, ok ? "Thành công" : "Lỗi", MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            if (ok) { LoadData(); ClearForm(); }
        }

        private void BtnSua_Click(object? s, EventArgs e)
        {
            if (dgv.CurrentRow == null) { MessageBox.Show("Chọn phim cần sửa!"); return; }
            var (ok, msg) = _bll.Update(GetFormData());
            MessageBox.Show(msg, ok ? "Thành công" : "Lỗi", MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            if (ok) LoadData();
        }

        private void BtnXoa_Click(object? s, EventArgs e)
        {
            if (dgv.CurrentRow == null) { MessageBox.Show("Chọn phim cần xóa!"); return; }
            var p = (Phim)dgv.CurrentRow.DataBoundItem;
            if (MessageBox.Show($"Xóa phim '{p.TenPhim}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            var (ok, msg) = _bll.Delete(p.MaPhim);
            MessageBox.Show(msg, ok ? "Thành công" : "Lỗi", MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
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
