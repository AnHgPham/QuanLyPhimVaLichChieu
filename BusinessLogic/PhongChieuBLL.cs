using QuanLyPhimVaLichChieu.DataAccess;
using QuanLyPhimVaLichChieu.Models;

namespace QuanLyPhimVaLichChieu.BusinessLogic
{
    public class PhongChieuBLL
    {
        private readonly PhongChieuDAL _dal = new PhongChieuDAL();

        public List<PhongChieu> GetAll() => _dal.GetAll();
        public List<PhongChieu> GetActive() => _dal.GetActive();

        public (bool Success, string Message) Insert(PhongChieu p)
        {
            if (string.IsNullOrWhiteSpace(p.TenPhong))
                return (false, "Ten phong khong duoc de trong!");
            if (p.SucChua <= 0)
                return (false, "Suc chua phai lon hon 0!");

            int id = _dal.Insert(p);
            return id > 0 ? (true, $"Them phong thanh cong! Ma phong: {id}") : (false, "Them phong that bai!");
        }

        public (bool Success, string Message) Update(PhongChieu p)
        {
            if (string.IsNullOrWhiteSpace(p.TenPhong))
                return (false, "Ten phong khong duoc de trong!");
            if (p.SucChua <= 0)
                return (false, "Suc chua phai lon hon 0!");

            bool result = _dal.Update(p);
            return result ? (true, "Cap nhat phong thanh cong!") : (false, "Cap nhat phong that bai!");
        }

        public (bool Success, string Message) Delete(int maPhong)
        {
            try
            {
                bool result = _dal.Delete(maPhong);
                return result ? (true, "Xoa phong thanh cong!") : (false, "Xoa phong that bai!");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("REFERENCE"))
                    return (false, "Khong the xoa phong dang co suat chieu!");
                return (false, $"Loi: {ex.Message}");
            }
        }
    }
}
