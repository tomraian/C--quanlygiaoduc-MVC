using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace qlsv_hoangduy
{
    class DataProvider
    {
        protected static string _connectionString;
        protected SqlConnection connection;
        protected SqlDataAdapter adapter;
        protected SqlCommand command;
        public string HashString(string text, string salt = "")
        {
            if (String.IsNullOrEmpty(text))
            {
                return String.Empty;
            }

            // Uses SHA256 to create the hash
            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                // Convert the string to a byte array first, to be processed
                byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(text + salt);
                byte[] hashBytes = sha.ComputeHash(textBytes);

                // Convert back to a string, removing the '-' that BitConverter adds
                string hash = BitConverter
                    .ToString(hashBytes)
                    .Replace("-", String.Empty);

                return hash;
            }
        }
        public static string ConnectionString
        {
            get
            {
                return _connectionString;
            }
            set
            {
                _connectionString = value;
            }
        }
        public bool Connect()
        {
            try
            {
                connection = new SqlConnection(_connectionString);
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public void Disconnect()
        {
            connection.Close();
        }
        public void executeNonQuery(string strStore,string table)
        {
            command = new SqlCommand(strStore, connection);
            command.Parameters.Add("@tenbang", SqlDbType.NVarChar).Value = table;
            command.CommandType = CommandType.StoredProcedure;
            command.ExecuteNonQuery();
        }
        public object executeScalar(string strStore)
        {
            command = new SqlCommand(strStore, connection);
            command.CommandType = CommandType.StoredProcedure;
            return command.ExecuteScalar();
        }
        public DataTable ExcuteQuery(string strStore, string table)
        {
            try
            {
                command = new SqlCommand(strStore, connection);
                command.Parameters.Add("@tenbang", SqlDbType.NVarChar).Value = table;
                command.CommandType = CommandType.StoredProcedure;
                adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
            catch
            {
                return null;
            }
        }
        public void ThemSinhVien(string storeProd, string mssv, string hoten, bool gioitinh, DateTime ngaysinh, string diachi, string hinhanh, string malop)
        {
                command = new SqlCommand(storeProd, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@mssv", SqlDbType.NChar).Value = mssv;
                command.Parameters.Add("@hoten", SqlDbType.NVarChar).Value = hoten;
                command.Parameters.Add("@gioitinh", SqlDbType.Bit).Value = gioitinh;
                command.Parameters.Add("@ngaysinh", SqlDbType.DateTime).Value = ngaysinh;
                command.Parameters.Add("@diachi", SqlDbType.Text).Value = diachi;
                command.Parameters.Add("@hinhanh", SqlDbType.Text).Value = hinhanh;
                command.Parameters.Add("@malop", SqlDbType.NChar).Value = malop;
                command.ExecuteNonQuery();
        }
        public void SuaSinhVien(string storeProd, string mssv, string hoten, bool gioitinh, DateTime ngaysinh, string diachi, string hinhanh, string malop)
        {
                command = new SqlCommand(storeProd, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@mssv", SqlDbType.NChar).Value = mssv;
                command.Parameters.Add("@hoten", SqlDbType.NVarChar).Value = hoten;
                command.Parameters.Add("@gioitinh", SqlDbType.Bit).Value = gioitinh;
                command.Parameters.Add("@ngaysinh", SqlDbType.DateTime).Value = ngaysinh;
                command.Parameters.Add("@diachi", SqlDbType.Text).Value = diachi;
                command.Parameters.Add("@hinhanh", SqlDbType.Text).Value = hinhanh;
                command.Parameters.Add("@malop", SqlDbType.NChar).Value = malop;
                command.ExecuteNonQuery();
        }
        public void DeleteDataTable(string storeProd,string table,string dieukien)
        {
            command = new SqlCommand(storeProd, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@tenbang", SqlDbType.NVarChar).Value = table;
            command.Parameters.Add("@dieukien", SqlDbType.NVarChar).Value = dieukien;
            command.ExecuteNonQuery();
        }
        public void XuLyDuLieuTrongDB(string storeProd, string malop, string tenlop, string makhoa)
        {
            command = new SqlCommand(storeProd, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@malop", SqlDbType.NChar).Value = malop;
            command.Parameters.Add("@tenlop", SqlDbType.NVarChar).Value = tenlop;
            command.Parameters.Add("@makhoa", SqlDbType.NChar).Value = makhoa;
            command.ExecuteNonQuery();
        }
        public void CapNhatSiSoLop(string storeProd, string malop, string pheptoan)
        {
            command = new SqlCommand(storeProd, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@malop", SqlDbType.NChar).Value = malop;
            command.Parameters.Add("@pheptoan", SqlDbType.NChar).Value = pheptoan;
            command.ExecuteNonQuery();
        }
    }
}
