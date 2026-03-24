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
            this.Text = "Thong ke bao cao";
            this.Size = new Size(800, 550);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Font = new Font("Segoe UI", 9.5F);

            // === Header ===
            var panelHeader = new Panel { Dock = DockStyle.Top, Height = 100, BackColor = Color.FromArgb(25, 25, 45), Padding = new Padding(20) };

            var lblTitle = new Label { Text = "THONG KE VE BAN THEO PHIM", Font = new Font("Segoe UI", 16F, FontStyle.Bold), ForeColor = Color.White, Location = new Point(20, 10), AutoSize = true };
            panelHeader.Controls.Add(lblTitle);

            lblTongVe = new Label { Text = "Tong ve: 0", Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = Color.FromArgb(46, 204, 113), Location = new Point(20, 50), AutoSize = true };
            panelHeader.Controls.Add(lblTongVe);

            lblTongDoanhThu = new Label { Text = "Doanh thu: 0 VND", Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = Color.FromArgb(241, 196, 15), Location = new Point(250, 50), AutoSize = true };
            panelHeader.Controls.Add(lblTongDoanhThu);

            var btnRefresh = new Button
            {
                Text = "Lam moi", Location = new Point(600, 50), Width = 100, Height = 30,
                BackColor = Color.FromArgb(52, 152, 219), ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += (s, e) => LoadData();
            panelHeader.Controls.Add(btnRefresh);

            this.Controls.Add(panelHeader);

            // === DataGridView ===
            dgv = new DataGridView
            {
                Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White, BorderStyle = BorderStyle.None, RowHeadersVisible = false
            };
            this.Controls.Add(dgv);
            dgv.BringToFront();
        }

        private void LoadData()
        {
            try
            {
                DataTable dt = _bll.ThongKeVeTheoPhim();
                dgv.DataSource = dt;

                if (dgv.Columns.Count > 0)
                {
                    dgv.Columns["MaPhim"].HeaderText = "Ma";
                    dgv.Columns["MaPhim"].Width = 40;
                    dgv.Columns["TenPhim"].HeaderText = "Ten phim";
                    dgv.Columns["TenTheLoai"].HeaderText = "The loai";
                    dgv.Columns["SoVeBan"].HeaderText = "So ve ban";
                    dgv.Columns["SoVeBan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgv.Columns["DoanhThu"].HeaderText = "Doanh thu (VND)";
                    dgv.Columns["DoanhThu"].DefaultCellStyle.Format = "N0";
                    dgv.Columns["DoanhThu"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                // Calculate totals
                int tongVe = 0;
                long tongDoanhThu = 0;
                foreach (DataRow row in dt.Rows)
                {
                    tongVe += Convert.ToInt32(row["SoVeBan"]);
                    tongDoanhThu += Convert.ToInt64(row["DoanhThu"]);
                }
                lblTongVe.Text = $"Tong ve: {tongVe}";
                lblTongDoanhThu.Text = $"Doanh thu: {tongDoanhThu:N0} VND";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Loi: {ex.Message}", "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
