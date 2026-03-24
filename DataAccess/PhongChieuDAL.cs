using System.Data;
using System.Data.SqlClient;
using QuanLyPhimVaLichChieu.Models;

namespace QuanLyPhimVaLichChieu.DataAccess
{
    public class PhongChieuDAL
    {
        public List<PhongChieu> GetAll()
        {
            var list = new List<PhongChieu>();
            DataTable dt = DatabaseHelper.ExecuteQuery("SELECT * FROM PhongChieu ORDER BY TenPhong");
            foreach (DataRow row in dt.Rows)
                list.Add(MapFromDataRow(row));
            return list;
        }

        public List<PhongChieu> GetActive()
        {
            var list = new List<PhongChieu>();
            DataTable dt = DatabaseHelper.ExecuteQuery("SELECT * FROM PhongChieu WHERE TrangThai = N'Hoat dong' ORDER BY TenPhong");
            foreach (DataRow row in dt.Rows)
                list.Add(MapFromDataRow(row));
            return list;
        }

        public int Insert(PhongChieu p)
        {
            string query = @"INSERT INTO PhongChieu (TenPhong, SucChua, LoaiPhong, TrangThai) 
                             VALUES (@TenPhong, @SucChua, @LoaiPhong, @TrangThai);
                             SELECT SCOPE_IDENTITY();";
            var result = DatabaseHelper.ExecuteScalar(query,
                new SqlParameter("@TenPhong", p.TenPhong),
                new SqlParameter("@SucChua", p.SucChua),
                new SqlParameter("@LoaiPhong", p.LoaiPhong),
                new SqlParameter("@TrangThai", p.TrangThai));
            return Convert.ToInt32(result);
        }

        public bool Update(PhongChieu p)
        {
            string query = @"UPDATE PhongChieu SET 
                             TenPhong = @TenPhong, SucChua = @SucChua, 
                             LoaiPhong = @LoaiPhong, TrangThai = @TrangThai
                             WHERE MaPhong = @MaPhong";
            int rows = DatabaseHelper.ExecuteNonQuery(query,
                new SqlParameter("@MaPhong", p.MaPhong),
                new SqlParameter("@TenPhong", p.TenPhong),
                new SqlParameter("@SucChua", p.SucChua),
                new SqlParameter("@LoaiPhong", p.LoaiPhong),
                new SqlParameter("@TrangThai", p.TrangThai));
            return rows > 0;
        }

        public bool Delete(int maPhong)
        {
            string query = "DELETE FROM PhongChieu WHERE MaPhong = @MaPhong";
            int rows = DatabaseHelper.ExecuteNonQuery(query, new SqlParameter("@MaPhong", maPhong));
            return rows > 0;
        }

        private PhongChieu MapFromDataRow(DataRow row)
        {
            return new PhongChieu
            {
                MaPhong = Convert.ToInt32(row["MaPhong"]),
                TenPhong = row["TenPhong"].ToString()!,
                SucChua = Convert.ToInt32(row["SucChua"]),
                LoaiPhong = row["LoaiPhong"].ToString()!,
                TrangThai = row["TrangThai"].ToString()!
            };
        }
    }
}
