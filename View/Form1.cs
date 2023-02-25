using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace qlsv_hoangduy
{
    public partial class Form1 : Form
    {
        OpenFileDialog openFile = new OpenFileDialog();
        lop_ctrl lop_ctrl = new lop_ctrl();
        sv_ctrl ctrl = new sv_ctrl();
        sv_info info = new sv_info();
        DataProvider data = new DataProvider();
        public Form1()
        {
            InitializeComponent();
        }

        //<hàm kết nối - đóng kết nối> 
        public void connect()
        {
            DataProvider.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\winform\qlsv-MVC\qlsv-hoangduy\app_data\qlsv.mdf;Integrated Security=True";
            if (!ctrl.Connect())
            {
                MessageBox.Show("Không thể kết nối đến với csdl");
                this.Close();
            }
            ctrl.Connect();
        }

        public void disconnect()
        {
            ctrl.Disconnect();

        }

        //<hàm hiển thị dữ liệu> 
        public void FormatColumnDataGridView()
        {
            dtgSinhVien.Rows.Clear();
            dtgSinhVien.ColumnCount = 8;
            dtgSinhVien.Columns[0].Name = "STT";
            dtgSinhVien.Columns[1].Name = "Mã sinh viên";
            dtgSinhVien.Columns[2].Name = "Tên sinh viên";
            dtgSinhVien.Columns[3].Name = "Giới tính";
            dtgSinhVien.Columns[4].Name = "Ngày sinh";
            dtgSinhVien.Columns[5].Name = "Địa chỉ";
            dtgSinhVien.Columns[6].Name = "Hình ảnh";
            dtgSinhVien.Columns[7].Name = "Lớp";
        }
        public void HienThiLop()
        {
            connect();
            cboLop.DisplayMember = "tenlop";
            cboLop.ValueMember = "malop";
            cboLop.DataSource = ctrl.GetDatatable("lop");
        }
        public void HienThiLopLoc()
        {
            connect();
            string makhoa = cboKhoaLoc.SelectedValue.ToString();
            cboLopLoc.DisplayMember = "tenlop";
            cboLopLoc.ValueMember = "malop";
            cboLopLoc.DataSource = ctrl.GetDatatable("lop where makhoa ='" + makhoa + "'");
        }
        public void HienThiKhoaLoc()
        {
            connect();
            cboKhoaLoc.DataSource = ctrl.GetDatatable("khoa");
            cboKhoaLoc.DisplayMember = "tenkhoa";
            cboKhoaLoc.ValueMember = "makhoa";
        }
        public void HienThiSinhVien(ComboBox cbo)
        {
            
            int stt = 0;
            string malop;
            FormatColumnDataGridView();
            malop = cbo.SelectedValue.ToString();
            foreach (DataRow dr in ctrl.GetDatatable("sinhvien where malop ='" + malop + "'").Rows)
            {
                stt += 1;
                string url = @"D:\winform\qlsv-MVC\qlsv-hoangduy\images\" + dr[5].ToString();
                string[] row = new string[] { stt.ToString(), dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString()};
                dtgSinhVien.Rows.Add(row);
            }
        }
        private void dtgSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dtgSinhVien.SelectedCells.Count > 0)
            {
                int selectedrowindex = dtgSinhVien.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dtgSinhVien.Rows[selectedrowindex];
                string cellValue = Convert.ToString(selectedRow.Cells["Mã sinh viên"].Value);
                DataTable tbl = ctrl.GetDatatable("SINHVIEN where mssv ='" + cellValue + "'");
                txtMssv.Text = tbl.Rows[0]["mssv"].ToString().Trim();
                txtMssv.ReadOnly = true;
                txtHoTen.Text = tbl.Rows[0]["hoten"].ToString();
                txtDiaChi.Text = tbl.Rows[0]["diachi"].ToString();
                cboLop.SelectedValue = tbl.Rows[0]["malop"].ToString();
                cboGioiTinh.SelectedIndex = int.Parse(tbl.Rows[0]["gioitinh"].ToString());
                txtHinhAnhDB.Text = tbl.Rows[0]["hinhanh"].ToString();
               // txtHinhAnh.Text = @"D:\winform\qlsv-MVC\qlsv-hoangduy\images\" + tbl.Rows[0]["hinhanh"].ToString();
                if (!System.IO.File.Exists(@"D:\winform\qlsv-MVC\qlsv-hoangduy\images\" + tbl.Rows[0]["hinhanh"].ToString()))
                {
                    pbHinhAnh.Image = Image.FromFile(@"D:\winform\qlsv-MVC\qlsv-hoangduy\images\no-image.jpg");
                }
                else
                    pbHinhAnh.Image = Image.FromFile(@"D:\winform\qlsv-MVC\qlsv-hoangduy\images\" + tbl.Rows[0]["hinhanh"].ToString());
            }
        }

        //<hàm lấy dữ liệu từ form >

        public void LayDuLieu()
        {
            info.mssv = txtMssv.Text;
            info.hoten = txtHoTen.Text;
            info.gioitinh = Convert.ToBoolean(cboGioiTinh.SelectedIndex);
            info.ngaysinh = dtNgaySinh.Value;
            info.diachi = txtDiaChi.Text;
            info.hinhanh = txtHinhAnhDB.Text;
            info.malop = cboLop.SelectedValue.ToString();
        }

        //<hàm xử lý thêm dữ liệu> 
        
        private void btnThemMoi_Click(object sender, EventArgs e)
        {
            connect();
            int count = ctrl.GetDatatable("sinhvien").Select(" mssv ='" + txtMssv.Text + "'").Length;
            txtHoTen.Focus();
            if (txtMssv.Text == "")
            {
                MessageBox.Show("Nhập vào mã số sinh viên");
                txtMssv.Focus();
                return;
            }
            else if (count > 0)
            {
                MessageBox.Show("Mã số sinh viên đã tồn tại");
                txtMssv.Focus();
                return;
            }
            else if (txtHoTen.Text == "")
            {
                MessageBox.Show("Nhập vào họ tên");
                txtHoTen.Focus();
                return;
            }
            else if (txtDiaChi.Text == "")
            {
                MessageBox.Show("Nhập vào địa chỉ");
                txtDiaChi.Focus();
                return;
            }
            if (txtHinhAnh.Text != "")
            {
                LuuHinh();
            }
            LayDuLieu();
            string malop = cboLop.SelectedValue.ToString();
            lop_ctrl.CapNhatSiSoLop(malop, "+1");
            ctrl.ThemSinhVien(info.mssv, info.hoten, info.gioitinh, info.ngaysinh, info.diachi, info.hinhanh, info.malop);
            MessageBox.Show("Thêm mới sinh viên thành công");
            RefreshForm();
            HienThiSinhVien(cboLop);
            disconnect();
        }

        //<hàm xử lý sửa dữ liệu> 

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMssv.Text == "")
            {
                MessageBox.Show("Chưa chọn sinh viên cần sửa");
                txtMssv.Focus();
                return;
            }
            else if (txtHoTen.Text == "")
            {
                MessageBox.Show("Nhập vào họ tên");
                txtHoTen.Focus();
                return;
            }
            else if (txtDiaChi.Text == "")
            {
                MessageBox.Show("Nhập vào địa chỉ");
                txtDiaChi.Focus();
                return;
            }
            connect();
            if (txtHinhAnh.Text != "" && dtgSinhVien.SelectedCells[6].Value.ToString() != txtHinhAnhDB.Text)
            {
                LuuHinh();
            }
            LayDuLieu();
            string maLopCu = dtgSinhVien.SelectedCells[7].Value.ToString();
            string maLopMoi = cboLop.SelectedValue.ToString() ;
            lop_ctrl.CapNhatSiSoLop(maLopCu, "-1");
            lop_ctrl.CapNhatSiSoLop(maLopMoi, "+1");
            ctrl.SuaSinhVien(info.mssv, info.hoten, info.gioitinh, info.ngaysinh, info.diachi, info.hinhanh, info.malop);
            MessageBox.Show("Sửa thông tin sinh viên thành công");
            RefreshForm();
            HienThiSinhVien(cboLop);
            disconnect();
        }

        //<hàm xử lý xóa dữ liệu> 

        private void btnXoa_Click(object sender, EventArgs e)
        {
            connect();
            DialogResult h = MessageBox.Show("Bạn có chắc muốn xóa sinh viên này không?", "Cảnh báo", MessageBoxButtons.OKCancel);
            if (h == DialogResult.OK)
            {
                if (dtgSinhVien.SelectedCells.Count > 0)
                {
                    int selectedrowindex = dtgSinhVien.SelectedCells[0].RowIndex;
                    DataGridViewRow selectedRow = dtgSinhVien.Rows[selectedrowindex];
                    string cellValue = Convert.ToString(selectedRow.Cells["Mã sinh viên"].Value);
                    ctrl.DeleteDataTable("SINHVIEN", "mssv = '" + cellValue + "'");
                    MessageBox.Show("Xóa thành công");
                    HienThiSinhVien(cboLopLoc);
                }
                else
                {
                    MessageBox.Show("Không còn gì để xóa");
                    HienThiSinhVien(cboLopLoc);
                }
            }
            disconnect();
        }

        //<hàm xử lý tìm kiếm dữ liệu> 
        public void TimKiem()
        {
            FormatColumnDataGridView();
            string TuKhoa;
            TuKhoa = txtTimKiem.Text;
            int stt = 0;
            FormatColumnDataGridView();
            string dkHoTen = "hoten like " + "'%" + TuKhoa + "%'";
            string dkMa = "mssv = '" + TuKhoa + "' OR ";
            string dk = dkMa + dkHoTen;
            //string dk = "mssv = '" + TuKhoa + "' OR hoten like N'%" + TuKhoa + '%' + "'";
            foreach (DataRow dr in ctrl.GetDatatable("SINHVIEN").Select(dk))
            {
                stt += 1;
                string[] row = new string[] { stt.ToString(), dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString() };
                dtgSinhVien.Rows.Add(row);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            TimKiem();
        }

        private void txtTimKiem_Enter(object sender, EventArgs e)
        {
            if (txtTimKiem.Text == "Nhập mssv hoặc họ tên để tìm kiếm")
            {
                txtTimKiem.Text = "";
            }
        }
        private void txtTimKiem_Leave(object sender, EventArgs e)
        {
            if (txtTimKiem.Text == "")
            {
                txtTimKiem.Text = "Nhập mssv hoặc họ tên để tìm kiếm";
            }
        }
        private void txtTimKiem_KeyUp(object sender, KeyEventArgs e)
        {
            TimKiem();
        }

        //<hàm xử lý upload ảnh> 

        public void ChonAnh()
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.png; *.jpeg;)| *.jpg; *.png; *.jpeg;";
            if(open.ShowDialog() == DialogResult.OK)
            {
                txtHinhAnh.Text = open.FileName;
                pbHinhAnh.Image = new Bitmap(open.FileName);
            }
            string[] arrListStr = Path.GetFileName(txtHinhAnh.Text).Split('.');
            string file_ext = arrListStr[arrListStr.Length - 1];
            string file_name_hash = data.HashString((Path.GetFileName(txtHinhAnh.Text)), DateTime.Now.ToString("d/m/y h:m:s")) + "." + file_ext;
            txtHinhAnhDB.Text = file_name_hash;
        }
        private void pbHinhAnh_Click(object sender, EventArgs e)
        {
            ChonAnh();
        }

        private void btnTaiAnh_Click(object sender, EventArgs e)
        {
            ChonAnh();
        }
        private void LuuHinh()
        {
            File.Copy(txtHinhAnh.Text, Path.Combine(@"D:\winform\qlsv-MVC\qlsv-hoangduy\images\", txtHinhAnhDB.Text), true);
        }

        //<hàm xử lý upload ảnh> 
        private void RefreshForm() {
            txtMssv.Text = "";
            txtHoTen.Text = "";
            txtDiaChi.Text = "";
            txtHinhAnh.Text = "";
            txtHinhAnhDB.Text = "no-image.jpg";
            cboGioiTinh.SelectedIndex = 0;
            pbHinhAnh.Image = null;
            txtMssv.ReadOnly = false;
        }
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            RefreshForm();
        }

        //<hàm form load> 
        private void Form1_Load(object sender, EventArgs e)
        {
            cboGioiTinh.SelectedIndex = 0;
            connect();
            HienThiLop();
            HienThiKhoaLoc();
            disconnect();
        }

        private void cboKhoaLoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            HienThiLopLoc();
        }

        private void cboLopLoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshForm();
            HienThiSinhVien(cboLopLoc);
        }

       
    }
}
