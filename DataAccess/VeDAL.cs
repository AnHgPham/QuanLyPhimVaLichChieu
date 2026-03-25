using System.Data;
using MySqlConnector;
using QuanLyPhimVaLichChieu.Models;

namespace QuanLyPhimVaLichChieu.DataAccess
{
    public class VeDAL
    {
        public List<Ve> GetAll()
        {
            var list = new List<Ve>();
            string query = @"SELECT v.*, p.TenPhim, pc.TenPhong,
                             TIME_FORMAT(sc.GioChieu, '%H:%i') AS GioChieuStr,
                             DATE_FORMAT(sc.NgayChieu, '%d/%m/%Y') AS NgayChieuStr
                             FROM Ve v
                             INNER JOIN SuatChieu sc ON v.MaSuat = sc.MaSuat
                             INNER JOIN Phim p ON sc.MaPhim = p.MaPhim
                             INNER JOIN PhongChieu pc ON sc.MaPhong = pc.MaPhong
                             ORDER BY v.NgayBan DESC";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);
            foreach (DataRow row in dt.Rows)
                list.Add(MapFromDataRow(row));
            return list;
        }

        public List<Ve> GetBySuat(int maSuat)
        {
            var list = new List<Ve>();
            string query = @"SELECT v.*, p.TenPhim, pc.TenPhong,
                             TIME_FORMAT(sc.GioChieu, '%H:%i') AS GioChieuStr,
                             DATE_FORMAT(sc.NgayChieu, '%d/%m/%Y') AS NgayChieuStr
                             FROM Ve v
                             INNER JOIN SuatChieu sc ON v.MaSuat = sc.MaSuat
                             INNER JOIN Phim p ON sc.MaPhim = p.MaPhim
                             INNER JOIN PhongChieu pc ON sc.MaPhong = pc.MaPhong
                             WHERE v.MaSuat = @MaSuat
                             ORDER BY v.MaGhe";
            DataTable dt = DatabaseHelper.ExecuteQuery(query, new MySqlParameter("@MaSuat", maSuat));
            foreach (DataRow row in dt.Rows)
                list.Add(MapFromDataRow(row));
            return list;
        }

        public List<string> GetGheDaDat(int maSuat)
        {
            var list = new List<string>();
            DataTable dt = DatabaseHelper.ExecuteQuery(
                "SELECT MaGhe FROM Ve WHERE MaSuat = @MaSuat",
                new MySqlParameter("@MaSuat", maSuat));
            foreach (DataRow row in dt.Rows)
                list.Add(row["MaGhe"].ToString()!);
            return list;
        }

        public int Insert(Ve v)
        {
            string query = @"INSERT INTO Ve (MaSuat, TenKhachHang, SoDienThoai, MaGhe, GiaVe) 
                             VALUES (@MaSuat, @TenKhach, @SoDT, @MaGhe, @GiaVe);
                             SELECT LAST_INSERT_ID();";
            var result = DatabaseHelper.ExecuteScalar(query,
                new MySqlParameter("@MaSuat", v.MaSuat),
                new MySqlParameter("@TenKhach", v.TenKhachHang),
                new MySqlParameter("@SoDT", (object?)v.SoDienThoai ?? DBNull.Value),
                new MySqlParameter("@MaGhe", v.MaGhe),
                new MySqlParameter("@GiaVe", v.GiaVe));
            return Convert.ToInt32(result);
        }

        public bool Delete(int maVe)
        {
            string query = "DELETE FROM Ve WHERE MaVe = @MaVe";
            int rows = DatabaseHelper.ExecuteNonQuery(query, new MySqlParameter("@MaVe", maVe));
            return rows > 0;
        }

        private Ve MapFromDataRow(DataRow row)
        {
            return new Ve
            {
                MaVe = Convert.ToInt32(row["MaVe"]),
                MaSuat = Convert.ToInt32(row["MaSuat"]),
                TenKhachHang = row["TenKhachHang"].ToString()!,
                SoDienThoai = row["SoDienThoai"]?.ToString(),
                MaGhe = row["MaGhe"].ToString()!,
                GiaVe = Convert.ToInt32(row["GiaVe"]),
                NgayBan = Convert.ToDateTime(row["NgayBan"]),
                TenPhim = row["TenPhim"]?.ToString(),
                TenPhong = row["TenPhong"]?.ToString(),
                GioChieuStr = row["GioChieuStr"]?.ToString(),
                NgayChieuStr = row["NgayChieuStr"]?.ToString()
            };
        }
    }
}
