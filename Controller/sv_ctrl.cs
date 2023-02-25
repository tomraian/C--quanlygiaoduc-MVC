using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
namespace qlsv_hoangduy
{
    class sv_ctrl
    {
        DataProvider _provider;
        public sv_ctrl()
        {
            _provider = new DataProvider();
        }
        public bool Connect()
        {
            return _provider.Connect();
        }
        public void Disconnect()
        {
            _provider.Disconnect();
        }
        public DataTable GetDatatable(string table)
        {
            return _provider.ExcuteQuery("sp_Select", table);
        }
        public void ThemSinhVien(string mssv, string hoten, bool gioitinh, DateTime ngaysinh, string diachi, string hinhanh, string malop)
        {
            _provider.ThemSinhVien("sp_InsertSV",  mssv,  hoten,  gioitinh,  ngaysinh,  diachi,  hinhanh,  malop);
        }
        public void SuaSinhVien(string mssv, string hoten, bool gioitinh, DateTime ngaysinh, string diachi, string hinhanh, string malop)
        {
            _provider.SuaSinhVien("sp_UpdateSV", mssv, hoten, gioitinh, ngaysinh, diachi, hinhanh, malop);
        }
        public void DeleteDataTable(string table,string dieukien)
        {
            _provider.DeleteDataTable("sp_delete", table,dieukien);
        }
    }
}
