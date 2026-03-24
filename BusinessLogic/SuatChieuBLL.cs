using QuanLyPhimVaLichChieu.DataAccess;
using QuanLyPhimVaLichChieu.Models;

namespace QuanLyPhimVaLichChieu.BusinessLogic
{
    public class SuatChieuBLL
    {
        private readonly SuatChieuDAL _dal = new SuatChieuDAL();

        public List<SuatChieu> GetAll() => _dal.GetAll();
        public List<SuatChieu> Search(int? maPhim, DateTime? ngayChieu) => _dal.Search(maPhim, ngayChieu);

        public (bool Success, string Message) Insert(SuatChieu sc)
        {
            if (sc.MaPhim <= 0)
                return (false, "Vui long chon phim!");
            if (sc.MaPhong <= 0)
                return (false, "Vui long chon phong chieu!");
            if (sc.GiaVe <= 0)
                return (false, "Gia ve phai lon hon 0!");
            if (sc.NgayChieu < DateTime.Today)
                return (false, "Ngay chieu khong duoc la ngay trong qua khu!");

            int id = _dal.Insert(sc);
            return id > 0 ? (true, $"Them suat chieu thanh cong! Ma suat: {id}") : (false, "Them suat chieu that bai!");
        }

        public (bool Success, string Message) Update(SuatChieu sc)
        {
            if (sc.MaPhim <= 0)
                return (false, "Vui long chon phim!");
            if (sc.MaPhong <= 0)
                return (false, "Vui long chon phong chieu!");
            if (sc.GiaVe <= 0)
                return (false, "Gia ve phai lon hon 0!");

            bool result = _dal.Update(sc);
            return result ? (true, "Cap nhat suat chieu thanh cong!") : (false, "Cap nhat suat chieu that bai!");
        }

        public (bool Success, string Message) Delete(int maSuat)
        {
            try
            {
                bool result = _dal.Delete(maSuat);
                return result ? (true, "Xoa suat chieu thanh cong!") : (false, "Xoa suat chieu that bai!");
            }
            catch (Exception ex)
            {
                return (false, $"Loi: {ex.Message}");
            }
        }
    }
}
