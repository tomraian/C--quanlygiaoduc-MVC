using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
namespace qlsv_hoangduy
{
    class lop_ctrl
    {
        DataProvider _provider;
        public lop_ctrl()
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
        public void XuLyDuLieuTrongDB(string storeProd,string malop, string tenlop, string makhoa)
        {
            _provider.XuLyDuLieuTrongDB(storeProd, malop,tenlop,makhoa);
        }  public void CapNhatSiSoLop(string malop, string pheptoan)
        {
            Connect();
            _provider.CapNhatSiSoLop("sp_UpdateSiSo", malop,pheptoan);
        }
        public void DeleteDataTable(string table,string dieukien)
        {
            _provider.DeleteDataTable("sp_delete", table,dieukien);
        }
    }
}
