namespace QuanLyPhimVaLichChieu.Models
{
    public class Ve
    {
        public int MaVe { get; set; }
        public int MaSuat { get; set; }
        public string TenKhachHang { get; set; } = string.Empty;
        public string? SoDienThoai { get; set; }
        public string MaGhe { get; set; } = string.Empty;
        public int GiaVe { get; set; }
        public DateTime NgayBan { get; set; }

        // Thuoc tinh hien thi (JOIN)
        public string? TenPhim { get; set; }
        public string? TenPhong { get; set; }
        public string? GioChieuStr { get; set; }
        public string? NgayChieuStr { get; set; }
    }
}
