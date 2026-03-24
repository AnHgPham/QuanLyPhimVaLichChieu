namespace QuanLyPhimVaLichChieu.Models
{
    public class SuatChieu
    {
        public int MaSuat { get; set; }
        public int MaPhim { get; set; }
        public string TenPhim { get; set; } = string.Empty;
        public int MaPhong { get; set; }
        public string TenPhong { get; set; } = string.Empty;
        public DateTime NgayChieu { get; set; }
        public TimeSpan GioChieu { get; set; }
        public int GiaVe { get; set; } = 75000;

        // Thuoc tinh hien thi
        public string GioChieuStr => GioChieu.ToString(@"hh\:mm");
        public string NgayChieuStr => NgayChieu.ToString("dd/MM/yyyy");
        public string GiaVeStr => GiaVe.ToString("N0") + " VND";
    }
}
