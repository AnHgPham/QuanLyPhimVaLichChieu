using QuanLyPhimVaLichChieu.DataAccess;
using QuanLyPhimVaLichChieu.Models;

namespace QuanLyPhimVaLichChieu.BusinessLogic
{
    public class TheLoaiBLL
    {
        private readonly TheLoaiDAL _dal = new TheLoaiDAL();
        public List<TheLoai> GetAll() => _dal.GetAll();
    }
}
