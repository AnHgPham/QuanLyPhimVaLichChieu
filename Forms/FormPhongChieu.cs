using QuanLyPhimVaLichChieu.BusinessLogic;
using QuanLyPhimVaLichChieu.Models;

namespace QuanLyPhimVaLichChieu.Forms
{
    public class FormPhongChieu : Form
    {
        private DataGridView dgv = null!;
        private TextBox txtTenPhong = null!, txtSucChua = null!;
        private ComboBox cboLoaiPhong = null!, cboTrangThai = null!;
        private Button btnThem = null!, btnSua = null!, btnXoa = null!, btnLamMoi = null!;
        private readonly PhongChieuBLL _bll = new PhongChieuBLL();

        public FormPhongChieu()
        {
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            UITheme.StyleForm(this, "Quản lý Phòng chiếu");
            this.Size = new Size(850, 550);

            // === Header ===
            this.Controls.Add(UITheme.CreateFormHeader("PHÒNG CHIẾU", "Quản lý phòng chiếu phim"));

            // === TOP: Input Form ===
            var panelTop = new Panel { Dock = DockStyle.Top, Height = 170, Padding = new Padding(15), BackColor = UITheme.BgCard };

            var flow = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = true, Padding = new Padding(0, 5, 0, 0), BackColor = Color.Transparent };

            flow.Controls.Add(new Label { Text = "Tên phòng:", Width = 80, TextAlign = ContentAlignment.MiddleRight, ForeColor = UITheme.TextSecondary, Font = UITheme.FontBold });
            txtTenPhong = new TextBox { Width = 180 }; UITheme.StyleTextBox(txtTenPhong); flow.Controls.Add(txtTenPhong);

            flow.Controls.Add(new Label { Text = "Sức chứa:", Width = 80, TextAlign = ContentAlignment.MiddleRight, ForeColor = UITheme.TextSecondary, Font = UITheme.FontBold });
            txtSucChua = new TextBox { Width = 80, Text = "50" }; UITheme.StyleTextBox(txtSucChua); flow.Controls.Add(txtSucChua);

            flow.Controls.Add(new Label { Text = "Loại:", Width = 80, TextAlign = ContentAlignment.MiddleRight, ForeColor = UITheme.TextSecondary, Font = UITheme.FontBold });
            cboLoaiPhong = new ComboBox { Width = 100, DropDownStyle = ComboBoxStyle.DropDownList };
            cboLoaiPhong.Items.AddRange(new object[] { "2D", "3D", "IMAX" });
            cboLoaiPhong.SelectedIndex = 0;
            UITheme.StyleComboBox(cboLoaiPhong);
            flow.Controls.Add(cboLoaiPhong);

            flow.Controls.Add(new Label { Text = "Trạng thái:", Width = 80, TextAlign = ContentAlignment.MiddleRight, ForeColor = UITheme.TextSecondary, Font = UITheme.FontBold });
            cboTrangThai = new ComboBox { Width = 120, DropDownStyle = ComboBoxStyle.DropDownList };
            cboTrangThai.Items.AddRange(new object[] { "Hoạt động", "Bảo trì" });
            cboTrangThai.SelectedIndex = 0;
            UITheme.StyleComboBox(cboTrangThai);
            flow.Controls.Add(cboTrangThai);

            // Buttons
            flow.Controls.Add(new Label { Width = 780, Height = 8 });
            btnThem = UITheme.CreateButton("Thêm", UITheme.AccentGreen, 70);
            btnSua = UITheme.CreateButton("Sửa", UITheme.AccentBlue, 70);
            btnXoa = UITheme.CreateButton("Xóa", UITheme.AccentRed, 70);
            btnLamMoi = UITheme.CreateButton("Làm mới", UITheme.BgInput, 80);
            btnLamMoi.ForeColor = UITheme.TextSecondary;
            flow.Controls.AddRange(new Control[] { btnThem, btnSua, btnXoa, btnLamMoi });

            btnThem.Click += (s, e) => { var (ok, msg) = _bll.Insert(GetFormData()); Show(ok, msg); if (ok) { LoadData(); ClearForm(); } };
            btnSua.Click += (s, e) => { if (dgv.CurrentRow == null) return; var (ok, msg) = _bll.Update(GetFormData()); Show(ok, msg); if (ok) LoadData(); };
            btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click += (s, e) => ClearForm();

            panelTop.Controls.Add(flow);
            flow.BringToFront();
            this.Controls.Add(panelTop);

            dgv = new DataGridView();
            UITheme.StyleDataGridView(dgv);
            dgv.Dock = DockStyle.Fill;
            dgv.SelectionChanged += (s, e) =>
            {
                if (dgv.CurrentRow == null) return;
                var p = (PhongChieu)dgv.CurrentRow.DataBoundItem;
                txtTenPhong.Text = p.TenPhong;
                txtSucChua.Text = p.SucChua.ToString();
                cboLoaiPhong.SelectedItem = p.LoaiPhong;
                cboTrangThai.SelectedItem = p.TrangThai;
            };
            dgv.DataBindingComplete += (s, e) => FormatGrid();
            this.Controls.Add(dgv);
            dgv.BringToFront();
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
            SetCol("MaPhong", c => { c.HeaderText = "MÃ"; c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None; c.MinimumWidth = 50; c.Width = 60; });
            SetCol("TenPhong", c => c.HeaderText = "TÊN PHÒNG");
            SetCol("SucChua", c => c.HeaderText = "SỨC CHỨA");
            SetCol("LoaiPhong", c => c.HeaderText = "LOẠI");
            SetCol("TrangThai", c => c.HeaderText = "TRẠNG THÁI");
        }

        private PhongChieu GetFormData() => new PhongChieu
        {
            MaPhong = dgv.CurrentRow != null ? ((PhongChieu)dgv.CurrentRow.DataBoundItem).MaPhong : 0,
            TenPhong = txtTenPhong.Text.Trim(),
            SucChua = int.TryParse(txtSucChua.Text, out int sc) ? sc : 0,
            LoaiPhong = cboLoaiPhong.SelectedItem?.ToString() ?? "2D",
            TrangThai = cboTrangThai.SelectedItem?.ToString() ?? "Hoạt động"
        };

        private void BtnXoa_Click(object? s, EventArgs e)
        {
            if (dgv.CurrentRow == null) return;
            var p = (PhongChieu)dgv.CurrentRow.DataBoundItem;
            if (MessageBox.Show($"Xóa phòng '{p.TenPhong}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            var (ok, msg) = _bll.Delete(p.MaPhong);
            Show(ok, msg); if (ok) { LoadData(); ClearForm(); }
        }

        private void ClearForm() { txtTenPhong.Text = ""; txtSucChua.Text = "50"; cboLoaiPhong.SelectedIndex = 0; cboTrangThai.SelectedIndex = 0; }
        private void Show(bool ok, string msg) => MessageBox.Show(msg, ok ? "Thành công" : "Lỗi", MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
    }
}
