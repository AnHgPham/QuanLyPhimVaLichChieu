namespace QuanLyPhimVaLichChieu.Models
{
    public class Phim
    {
        public int MaPhim { get; set; }
        public string TenPhim { get; set; } = string.Empty;
        public int MaTheLoai { get; set; }
        public string TenTheLoai { get; set; } = string.Empty;
        public int ThoiLuong { get; set; }
        public string? QuocGia { get; set; }
        public string? MoTa { get; set; }
        public string TrangThai { get; set; } = "Dang chieu";
        public DateTime NgayTao { get; set; }
        public DateTime NgayCapNhat { get; set; }
    }
}
