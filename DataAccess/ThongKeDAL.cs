using System.Data;

namespace QuanLyPhimVaLichChieu.DataAccess
{
    public class ThongKeDAL
    {
        public DataTable GetTongQuan()
        {
            return DatabaseHelper.ExecuteStoredProcedure("sp_TongQuan");
        }

        public DataTable ThongKeVeTheoPhim()
        {
            return DatabaseHelper.ExecuteStoredProcedure("sp_ThongKeVeTheoPhim");
        }
    }
}
