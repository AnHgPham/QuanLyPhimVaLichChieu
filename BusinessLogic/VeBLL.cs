using QuanLyPhimVaLichChieu.DataAccess;
using QuanLyPhimVaLichChieu.Models;

namespace QuanLyPhimVaLichChieu.BusinessLogic
{
    public class VeBLL
    {
        private readonly VeDAL _dal = new VeDAL();

        public List<Ve> GetAll() => _dal.GetAll();
        public List<Ve> GetBySuat(int maSuat) => _dal.GetBySuat(maSuat);
        public List<string> GetGheDaDat(int maSuat) => _dal.GetGheDaDat(maSuat);

        public (bool Success, string Message) Insert(Ve v)
        {
            if (string.IsNullOrWhiteSpace(v.TenKhachHang))
                return (false, "Ten khach hang khong duoc de trong!");
            if (v.MaSuat <= 0)
                return (false, "Vui long chon suat chieu!");
            if (string.IsNullOrWhiteSpace(v.MaGhe))
                return (false, "Vui long chon ghe!");
            if (v.GiaVe <= 0)
                return (false, "Gia ve phai lon hon 0!");

            // Kiem tra ghe da dat chua
            var gheDaDat = _dal.GetGheDaDat(v.MaSuat);
            if (gheDaDat.Contains(v.MaGhe))
                return (false, $"Ghe {v.MaGhe} da duoc dat!");

            try
            {
                int id = _dal.Insert(v);
                return id > 0 ? (true, $"Ban ve thanh cong! Ma ve: {id}") : (false, "Ban ve that bai!");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("UQ_Ve_Ghe"))
                    return (false, $"Ghe {v.MaGhe} da duoc dat cho suat chieu nay!");
                return (false, $"Loi: {ex.Message}");
            }
        }

        public (bool Success, string Message) Delete(int maVe)
        {
            bool result = _dal.Delete(maVe);
            return result ? (true, "Huy ve thanh cong!") : (false, "Huy ve that bai!");
        }
    }
}
