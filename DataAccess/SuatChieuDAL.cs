using System.Data;
using MySqlConnector;
using QuanLyPhimVaLichChieu.Models;

namespace QuanLyPhimVaLichChieu.DataAccess
{
    public class SuatChieuDAL
    {
        public List<SuatChieu> GetAll()
        {
            var list = new List<SuatChieu>();
            string query = @"SELECT sc.*, p.TenPhim, pc.TenPhong
                             FROM SuatChieu sc
                             INNER JOIN Phim p ON sc.MaPhim = p.MaPhim
                             INNER JOIN PhongChieu pc ON sc.MaPhong = pc.MaPhong
                             ORDER BY sc.NgayChieu DESC, sc.GioChieu";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);
            foreach (DataRow row in dt.Rows)
                list.Add(MapFromDataRow(row));
            return list;
        }

        public List<SuatChieu> Search(int? maPhim, DateTime? ngayChieu)
        {
            var list = new List<SuatChieu>();
            string query = @"SELECT sc.*, p.TenPhim, pc.TenPhong
                             FROM SuatChieu sc
                             INNER JOIN Phim p ON sc.MaPhim = p.MaPhim
                             INNER JOIN PhongChieu pc ON sc.MaPhong = pc.MaPhong
                             WHERE 1=1";
            var parameters = new List<MySqlParameter>();

            if (maPhim.HasValue && maPhim.Value > 0)
            {
                query += " AND sc.MaPhim = @MaPhim";
                parameters.Add(new MySqlParameter("@MaPhim", maPhim.Value));
            }

            if (ngayChieu.HasValue)
            {
                query += " AND sc.NgayChieu = @NgayChieu";
                parameters.Add(new MySqlParameter("@NgayChieu", ngayChieu.Value.Date));
            }

            query += " ORDER BY sc.NgayChieu DESC, sc.GioChieu";
            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
            foreach (DataRow row in dt.Rows)
                list.Add(MapFromDataRow(row));
            return list;
        }

        public int Insert(SuatChieu sc)
        {
            string query = @"INSERT INTO SuatChieu (MaPhim, MaPhong, NgayChieu, GioChieu, GiaVe) 
                             VALUES (@MaPhim, @MaPhong, @NgayChieu, @GioChieu, @GiaVe);
                             SELECT LAST_INSERT_ID();";
            var result = DatabaseHelper.ExecuteScalar(query,
                new MySqlParameter("@MaPhim", sc.MaPhim),
                new MySqlParameter("@MaPhong", sc.MaPhong),
                new MySqlParameter("@NgayChieu", sc.NgayChieu.Date),
                new MySqlParameter("@GioChieu", sc.GioChieu),
                new MySqlParameter("@GiaVe", sc.GiaVe));
            return Convert.ToInt32(result);
        }

        public bool Update(SuatChieu sc)
        {
            string query = @"UPDATE SuatChieu SET 
                             MaPhim = @MaPhim, MaPhong = @MaPhong, 
                             NgayChieu = @NgayChieu, GioChieu = @GioChieu, GiaVe = @GiaVe
                             WHERE MaSuat = @MaSuat";
            int rows = DatabaseHelper.ExecuteNonQuery(query,
                new MySqlParameter("@MaSuat", sc.MaSuat),
                new MySqlParameter("@MaPhim", sc.MaPhim),
                new MySqlParameter("@MaPhong", sc.MaPhong),
                new MySqlParameter("@NgayChieu", sc.NgayChieu.Date),
                new MySqlParameter("@GioChieu", sc.GioChieu),
                new MySqlParameter("@GiaVe", sc.GiaVe));
            return rows > 0;
        }

        public bool Delete(int maSuat)
        {
            string query = "DELETE FROM SuatChieu WHERE MaSuat = @MaSuat";
            int rows = DatabaseHelper.ExecuteNonQuery(query, new MySqlParameter("@MaSuat", maSuat));
            return rows > 0;
        }

        private SuatChieu MapFromDataRow(DataRow row)
        {
            return new SuatChieu
            {
                MaSuat = Convert.ToInt32(row["MaSuat"]),
                MaPhim = Convert.ToInt32(row["MaPhim"]),
                TenPhim = row["TenPhim"].ToString()!,
                MaPhong = Convert.ToInt32(row["MaPhong"]),
                TenPhong = row["TenPhong"].ToString()!,
                NgayChieu = Convert.ToDateTime(row["NgayChieu"]),
                GioChieu = (TimeSpan)row["GioChieu"],
                GiaVe = Convert.ToInt32(row["GiaVe"])
            };
        }
    }
}
