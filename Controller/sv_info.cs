using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qlsv_hoangduy
{
    class sv_info
    {
        private string _mssv;
        private string _hoten;
        private bool _gioitinh;
        private DateTime _ngaysinh;
        private string _diachi;
        private string _hinhanh;
        private string _malop;
        public string mssv
        {
            get
            {
                return _mssv;
            }
            set
            {
                if (value == null)
                    throw new Exception("Ma sinh vien khong duoc rong");
                _mssv = value;
            }
        }
        public string hoten
        {
            get
            {
                return _hoten;
            }
            set
            {
                if (value == null)
                    throw new Exception("ho ten sinh vien khong duoc rong");
                _hoten = value;
            }
        }
        public bool gioitinh
        {
            get
            {
                return _gioitinh;
            }
            set
            {
                _gioitinh = value;
            }
        }
        public DateTime ngaysinh
        {
            get
            {
                return _ngaysinh;
            }
            set
            {
                if (value == null)
                    throw new Exception("ngay sinh cua sinh vien khong duoc rong");
                _ngaysinh = value;
            }
        }
        public string diachi
        {
            get
            {
                return _diachi;
            }
            set
            {
                if (value == null)
                    throw new Exception("dia chi sinh vien khong duoc rong");
                _diachi = value;
            }
        }
        public string hinhanh
        {
            get
            {
                return _hinhanh;
            }
            set
            {
                _hinhanh = value;
            }
        }
        public string malop
        {
            get
            {
                return _malop;
            }
            set
            {
                if (value == null)
                    throw new Exception("lop cua sinh vien khong duoc rong");
                _malop = value;
            }
        }
    }
}
