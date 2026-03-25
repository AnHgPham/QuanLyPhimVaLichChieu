using System.Data;

namespace QuanLyPhimVaLichChieu.DataAccess
{
    public class TheLoaiDAL
    {
        public List<Models.TheLoai> GetAll()
        {
            var list = new List<Models.TheLoai>();
            DataTable dt = DatabaseHelper.ExecuteQuery("SELECT * FROM TheLoai ORDER BY TenTheLoai");
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new Models.TheLoai
                {
                    MaTheLoai = Convert.ToInt32(row["MaTheLoai"]),
                    TenTheLoai = row["TenTheLoai"].ToString()!
                });
            }
            return list;
        }
    }
}
