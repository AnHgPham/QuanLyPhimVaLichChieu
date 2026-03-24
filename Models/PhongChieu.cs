namespace QuanLyPhimVaLichChieu.Models
{
    public class PhongChieu
    {
        public int MaPhong { get; set; }
        public string TenPhong { get; set; } = string.Empty;
        public int SucChua { get; set; } = 50;
        public string LoaiPhong { get; set; } = "2D";
        public string TrangThai { get; set; } = "Hoat dong";
    }
}
