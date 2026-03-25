using System.Data;
using MySqlConnector;
using QuanLyPhimVaLichChieu.Models;

namespace QuanLyPhimVaLichChieu.DataAccess
{
    public class PhimDAL
    {
        public List<Phim> GetAll()
        {
            var list = new List<Phim>();
            string query = @"SELECT p.*, tl.TenTheLoai 
                             FROM Phim p 
                             INNER JOIN TheLoai tl ON p.MaTheLoai = tl.MaTheLoai 
                             ORDER BY p.TenPhim";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);
            foreach (DataRow row in dt.Rows)
                list.Add(MapFromDataRow(row));
            return list;
        }

        public List<Phim> Search(string? keyword, int? maTheLoai)
        {
            var list = new List<Phim>();
            string query = @"SELECT p.*, tl.TenTheLoai 
                             FROM Phim p 
                             INNER JOIN TheLoai tl ON p.MaTheLoai = tl.MaTheLoai 
                             WHERE 1=1";
            var parameters = new List<MySqlParameter>();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query += " AND (p.TenPhim LIKE @Keyword OR p.MoTa LIKE @Keyword)";
                parameters.Add(new MySqlParameter("@Keyword", $"%{keyword}%"));
            }

            if (maTheLoai.HasValue && maTheLoai.Value > 0)
            {
                query += " AND p.MaTheLoai = @MaTheLoai";
                parameters.Add(new MySqlParameter("@MaTheLoai", maTheLoai.Value));
            }

            query += " ORDER BY p.TenPhim";
            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
            foreach (DataRow row in dt.Rows)
                list.Add(MapFromDataRow(row));
            return list;
        }

        public Phim? GetById(int maPhim)
        {
            string query = @"SELECT p.*, tl.TenTheLoai 
                             FROM Phim p 
                             INNER JOIN TheLoai tl ON p.MaTheLoai = tl.MaTheLoai 
                             WHERE p.MaPhim = @MaPhim";
            DataTable dt = DatabaseHelper.ExecuteQuery(query, new MySqlParameter("@MaPhim", maPhim));
            if (dt.Rows.Count > 0)
                return MapFromDataRow(dt.Rows[0]);
            return null;
        }

        public int Insert(Phim p)
        {
            string query = @"INSERT INTO Phim (TenPhim, MaTheLoai, ThoiLuong, QuocGia, MoTa, TrangThai) 
                             VALUES (@TenPhim, @MaTheLoai, @ThoiLuong, @QuocGia, @MoTa, @TrangThai);
                             SELECT LAST_INSERT_ID();";
            var result = DatabaseHelper.ExecuteScalar(query,
                new MySqlParameter("@TenPhim", p.TenPhim),
                new MySqlParameter("@MaTheLoai", p.MaTheLoai),
                new MySqlParameter("@ThoiLuong", p.ThoiLuong),
                new MySqlParameter("@QuocGia", (object?)p.QuocGia ?? DBNull.Value),
                new MySqlParameter("@MoTa", (object?)p.MoTa ?? DBNull.Value),
                new MySqlParameter("@TrangThai", p.TrangThai));
            return Convert.ToInt32(result);
        }

        public bool Update(Phim p)
        {
            string query = @"UPDATE Phim SET 
                             TenPhim = @TenPhim, MaTheLoai = @MaTheLoai, ThoiLuong = @ThoiLuong,
                             QuocGia = @QuocGia, MoTa = @MoTa, TrangThai = @TrangThai, NgayCapNhat = NOW()
                             WHERE MaPhim = @MaPhim";
            int rows = DatabaseHelper.ExecuteNonQuery(query,
                new MySqlParameter("@MaPhim", p.MaPhim),
                new MySqlParameter("@TenPhim", p.TenPhim),
                new MySqlParameter("@MaTheLoai", p.MaTheLoai),
                new MySqlParameter("@ThoiLuong", p.ThoiLuong),
                new MySqlParameter("@QuocGia", (object?)p.QuocGia ?? DBNull.Value),
                new MySqlParameter("@MoTa", (object?)p.MoTa ?? DBNull.Value),
                new MySqlParameter("@TrangThai", p.TrangThai));
            return rows > 0;
        }

        public bool Delete(int maPhim)
        {
            string query = "DELETE FROM Phim WHERE MaPhim = @MaPhim";
            int rows = DatabaseHelper.ExecuteNonQuery(query, new MySqlParameter("@MaPhim", maPhim));
            return rows > 0;
        }

        private Phim MapFromDataRow(DataRow row)
        {
            return new Phim
            {
                MaPhim = Convert.ToInt32(row["MaPhim"]),
                TenPhim = row["TenPhim"].ToString()!,
                MaTheLoai = Convert.ToInt32(row["MaTheLoai"]),
                TenTheLoai = row["TenTheLoai"].ToString()!,
                ThoiLuong = Convert.ToInt32(row["ThoiLuong"]),
                QuocGia = row["QuocGia"]?.ToString(),
                MoTa = row["MoTa"]?.ToString(),
                TrangThai = row["TrangThai"].ToString()!,
                NgayTao = Convert.ToDateTime(row["NgayTao"]),
                NgayCapNhat = Convert.ToDateTime(row["NgayCapNhat"])
            };
        }
    }
}
