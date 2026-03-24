using System.Data;
using QuanLyPhimVaLichChieu.DataAccess;

namespace QuanLyPhimVaLichChieu.BusinessLogic
{
    public class ThongKeBLL
    {
        private readonly ThongKeDAL _dal = new ThongKeDAL();
        public DataTable GetTongQuan() => _dal.GetTongQuan();
        public DataTable ThongKeVeTheoPhim() => _dal.ThongKeVeTheoPhim();
    }
}
