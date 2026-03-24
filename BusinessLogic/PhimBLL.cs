using QuanLyPhimVaLichChieu.DataAccess;
using QuanLyPhimVaLichChieu.Models;

namespace QuanLyPhimVaLichChieu.BusinessLogic
{
    public class PhimBLL
    {
        private readonly PhimDAL _dal = new PhimDAL();

        public List<Phim> GetAll() => _dal.GetAll();
        public List<Phim> Search(string? keyword, int? maTheLoai) => _dal.Search(keyword, maTheLoai);
        public Phim? GetById(int maPhim) => _dal.GetById(maPhim);

        public (bool Success, string Message) Insert(Phim p)
        {
            if (string.IsNullOrWhiteSpace(p.TenPhim))
                return (false, "Ten phim khong duoc de trong!");
            if (p.MaTheLoai <= 0)
                return (false, "Vui long chon the loai phim!");
            if (p.ThoiLuong <= 0)
                return (false, "Thoi luong phai lon hon 0!");

            int id = _dal.Insert(p);
            return id > 0 ? (true, $"Them phim thanh cong! Ma phim: {id}") : (false, "Them phim that bai!");
        }

        public (bool Success, string Message) Update(Phim p)
        {
            if (string.IsNullOrWhiteSpace(p.TenPhim))
                return (false, "Ten phim khong duoc de trong!");
            if (p.MaTheLoai <= 0)
                return (false, "Vui long chon the loai phim!");
            if (p.ThoiLuong <= 0)
                return (false, "Thoi luong phai lon hon 0!");

            bool result = _dal.Update(p);
            return result ? (true, "Cap nhat phim thanh cong!") : (false, "Cap nhat phim that bai!");
        }

        public (bool Success, string Message) Delete(int maPhim)
        {
            try
            {
                bool result = _dal.Delete(maPhim);
                return result ? (true, "Xoa phim thanh cong!") : (false, "Xoa phim that bai!");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("REFERENCE"))
                    return (false, "Khong the xoa phim da co suat chieu!");
                return (false, $"Loi: {ex.Message}");
            }
        }
    }
}
