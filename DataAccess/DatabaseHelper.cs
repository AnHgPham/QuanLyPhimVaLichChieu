using System.Configuration;
using System.Data;
using MySqlConnector;

namespace QuanLyPhimVaLichChieu.DataAccess
{
    public static class DatabaseHelper
    {
        private static string _connectionString = string.Empty;

        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = ConfigurationManager.ConnectionStrings["QuanLyPhimDB"]?.ConnectionString
                        ?? "Server=localhost;Port=3306;Database=QuanLyPhimDB;Uid=root;Pwd=;";
                }
                return _connectionString;
            }
            set => _connectionString = value;
        }

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public static bool TestConnection()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static DataTable ExecuteQuery(string query, params MySqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.CommandTimeout = 30;
                    if (parameters != null)
                    {
                        foreach (var p in parameters)
                            cmd.Parameters.Add(p);
                    }
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public static int ExecuteNonQuery(string query, params MySqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.CommandTimeout = 30;
                    if (parameters != null)
                    {
                        foreach (var p in parameters)
                            cmd.Parameters.Add(p);
                    }
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public static object? ExecuteScalar(string query, params MySqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.CommandTimeout = 30;
                    if (parameters != null)
                    {
                        foreach (var p in parameters)
                            cmd.Parameters.Add(p);
                    }
                    return cmd.ExecuteScalar();
                }
            }
        }

        public static DataTable ExecuteStoredProcedure(string spName, params MySqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 30;
                    if (parameters != null)
                    {
                        foreach (var p in parameters)
                            cmd.Parameters.Add(p);
                    }
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }
    }
}
