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
            this.Text = "Quan ly Phong chieu";
            this.Size = new Size(850, 550);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Font = new Font("Segoe UI", 9.5F);

            var panelTop = new Panel { Dock = DockStyle.Top, Height = 200, Padding = new Padding(15) };

            var lblTitle = new Label { Text = "THONG TIN PHONG CHIEU", Font = new Font("Segoe UI", 13F, FontStyle.Bold), Dock = DockStyle.Top, Height = 35 };
            panelTop.Controls.Add(lblTitle);

            var flow = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = true, Padding = new Padding(0, 10, 0, 0) };

            flow.Controls.Add(new Label { Text = "Ten phong:", Width = 80, TextAlign = ContentAlignment.MiddleRight });
            txtTenPhong = new TextBox { Width = 180 }; flow.Controls.Add(txtTenPhong);

            flow.Controls.Add(new Label { Text = "Suc chua:", Width = 80, TextAlign = ContentAlignment.MiddleRight });
            txtSucChua = new TextBox { Width = 80, Text = "50" }; flow.Controls.Add(txtSucChua);

            flow.Controls.Add(new Label { Text = "Loai:", Width = 80, TextAlign = ContentAlignment.MiddleRight });
            cboLoaiPhong = new ComboBox { Width = 100, DropDownStyle = ComboBoxStyle.DropDownList };
            cboLoaiPhong.Items.AddRange(new object[] { "2D", "3D", "IMAX" });
            cboLoaiPhong.SelectedIndex = 0;
            flow.Controls.Add(cboLoaiPhong);

            flow.Controls.Add(new Label { Text = "Trang thai:", Width = 80, TextAlign = ContentAlignment.MiddleRight });
            cboTrangThai = new ComboBox { Width = 120, DropDownStyle = ComboBoxStyle.DropDownList };
            cboTrangThai.Items.AddRange(new object[] { "Hoat dong", "Bao tri" });
            cboTrangThai.SelectedIndex = 0;
            flow.Controls.Add(cboTrangThai);

            // Buttons
            flow.Controls.Add(new Label { Width = 780, Height = 5 }); // spacer
            btnThem = new Button { Text = "Them", Width = 70, BackColor = Color.FromArgb(46, 204, 113), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnSua = new Button { Text = "Sua", Width = 70, BackColor = Color.FromArgb(52, 152, 219), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnXoa = new Button { Text = "Xoa", Width = 70, BackColor = Color.FromArgb(231, 76, 60), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnLamMoi = new Button { Text = "Lam moi", Width = 80, FlatStyle = FlatStyle.Flat };
            btnThem.FlatAppearance.BorderSize = 0; btnSua.FlatAppearance.BorderSize = 0;
            btnXoa.FlatAppearance.BorderSize = 0; btnLamMoi.FlatAppearance.BorderSize = 0;
            flow.Controls.AddRange(new Control[] { btnThem, btnSua, btnXoa, btnLamMoi });

            btnThem.Click += (s, e) => { var (ok, msg) = _bll.Insert(GetFormData()); Show(ok, msg); if (ok) { LoadData(); ClearForm(); } };
            btnSua.Click += (s, e) => { if (dgv.CurrentRow == null) return; var (ok, msg) = _bll.Update(GetFormData()); Show(ok, msg); if (ok) LoadData(); };
            btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click += (s, e) => ClearForm();

            panelTop.Controls.Add(flow);
            flow.BringToFront();
            this.Controls.Add(panelTop);

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
                var p = (PhongChieu)dgv.CurrentRow.DataBoundItem;
                txtTenPhong.Text = p.TenPhong;
                txtSucChua.Text = p.SucChua.ToString();
                cboLoaiPhong.SelectedItem = p.LoaiPhong;
                cboTrangThai.SelectedItem = p.TrangThai;
            };
            this.Controls.Add(dgv);
            dgv.BringToFront();
        }

        private void LoadData()
        {
            dgv.DataSource = _bll.GetAll();
            if (dgv.Columns.Count > 0)
            {
                dgv.Columns["MaPhong"].HeaderText = "Ma";
                dgv.Columns["MaPhong"].Width = 50;
                dgv.Columns["TenPhong"].HeaderText = "Ten phong";
                dgv.Columns["SucChua"].HeaderText = "Suc chua";
                dgv.Columns["LoaiPhong"].HeaderText = "Loai";
                dgv.Columns["TrangThai"].HeaderText = "Trang thai";
            }
        }

        private PhongChieu GetFormData() => new PhongChieu
        {
            MaPhong = dgv.CurrentRow != null ? ((PhongChieu)dgv.CurrentRow.DataBoundItem).MaPhong : 0,
            TenPhong = txtTenPhong.Text.Trim(),
            SucChua = int.TryParse(txtSucChua.Text, out int sc) ? sc : 0,
            LoaiPhong = cboLoaiPhong.SelectedItem?.ToString() ?? "2D",
            TrangThai = cboTrangThai.SelectedItem?.ToString() ?? "Hoat dong"
        };

        private void BtnXoa_Click(object? s, EventArgs e)
        {
            if (dgv.CurrentRow == null) return;
            var p = (PhongChieu)dgv.CurrentRow.DataBoundItem;
            if (MessageBox.Show($"Xoa phong '{p.TenPhong}'?", "Xac nhan", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            var (ok, msg) = _bll.Delete(p.MaPhong);
            Show(ok, msg); if (ok) { LoadData(); ClearForm(); }
        }

        private void ClearForm() { txtTenPhong.Text = ""; txtSucChua.Text = "50"; cboLoaiPhong.SelectedIndex = 0; cboTrangThai.SelectedIndex = 0; }
        private void Show(bool ok, string msg) => MessageBox.Show(msg, ok ? "Thanh cong" : "Loi", MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
    }
}
