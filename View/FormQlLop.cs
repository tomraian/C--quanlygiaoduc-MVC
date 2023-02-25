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
    public partial class FormQlLop : Form
    {
        lop_ctrl ctrl = new lop_ctrl();
        lop_info info = new lop_info();
        DataProvider data = new DataProvider();
        private void FormQlLop_Load(object sender, EventArgs e)
        {
            HienThiDSLop();
            HienThiKhoa();
        }
        public FormQlLop()
        {
            InitializeComponent();
        }
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
        private void txtSiSo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.' ||
                (e.KeyChar == '.' && (txtMaLop.Text.Length == 0 || txtMaLop.Text.IndexOf('.') != -1))))
                e.Handled = true;
        }
        public void HienThiDSLop()
        {
            connect();
            dtgLop.DataSource = ctrl.GetDatatable( "LOP");
            disconnect();
        }
        public void HienThiKhoa()
        {
            connect();
            cboKhoa.DisplayMember = "tenkhoa";
            cboKhoa.ValueMember = "makhoa";
            cboKhoa.DataSource = ctrl.GetDatatable("KHOA");
        }
        public void LayDuLieu()
        {
            info.makhoa = cboKhoa.SelectedValue.ToString();
            info.malop = txtMaLop.Text.ToString();
            info.tenlop = txtTenLop.Text.ToString();
            info.siso = Convert.ToInt32(txtSiSo.Text);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            connect();
                if (dtgLop.SelectedCells.Count > 0)
                {
                    int selectedrowindex = dtgLop.SelectedCells[0].RowIndex;
                    DataGridViewRow selectedRow = dtgLop.Rows[selectedrowindex];
                    string cellValue = Convert.ToString(selectedRow.Cells["malop"].Value);
                    int count = ctrl.GetDatatable("sinhvien where malop ='" + cellValue + "'").Rows.Count;
                    if (count > 0){
                    DialogResult h = MessageBox.Show("Lớp này đã có sinh viên, bạn có muốn xóa không. \nNếu xóa sẽ xóa tất cả thông tin sinh viên có trong lớp ?", "Cảnh báo", MessageBoxButtons.OKCancel);
                        if (h == DialogResult.OK){
                        ctrl.DeleteDataTable("SINHVIEN", "malop = '" + cellValue + "'");
                        ctrl.DeleteDataTable("LOP", "malop = '" + cellValue + "'");
                        MessageBox.Show("Xóa thành công");
                        HienThiDSLop();
                        }
                    }
                else
                {
                    DialogResult h = MessageBox.Show("Bạn có muốn xóa lớp này không.?", "Cảnh báo", MessageBoxButtons.OKCancel);
                    if (h == DialogResult.OK)
                    {
                        ctrl.DeleteDataTable("LOP", "malop = '" + cellValue + "'");
                        MessageBox.Show("Xóa thành công");
                        HienThiDSLop();
                    }
                }
                }
                else
                {
                    MessageBox.Show("Không còn gì để xóa");
                    HienThiDSLop();
                }
        }

        private void dtgLop_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dtgLop.SelectedCells.Count > 0)
            {
                int selectedrowindex = dtgLop.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dtgLop.Rows[selectedrowindex];
                string cellValue = Convert.ToString(selectedRow.Cells["malop"].Value);
                DataTable tbl = ctrl.GetDatatable("LOP where malop ='" + cellValue + "'");
                cboKhoa.SelectedValue = tbl.Rows[0]["makhoa"].ToString();
                txtMaLop.Text = tbl.Rows[0]["malop"].ToString().Trim();
                txtMaLop.ReadOnly = true;
                txtTenLop.Text = tbl.Rows[0]["tenlop"].ToString();
                txtSiSo.Text = tbl.Rows[0]["siso"].ToString();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            connect();
            LayDuLieu();
            if (txtMaLop.Text == "")
            {
                MessageBox.Show("Chưa nhập mã lớp");
                txtMaLop.Focus();
                return;
            }
            else if (txtTenLop.Text == "")
            {
                MessageBox.Show("Chưa nhập tên lớp");
                txtTenLop.Focus();
                return;
            }
            ctrl.XuLyDuLieuTrongDB("sp_UpdateLop",info.malop,info.tenlop, info.makhoa);
            MessageBox.Show("Sửa lớp mới thành công");
            HienThiDSLop();
        }
        public void TimKiem()
        {
            string TuKhoa;
            TuKhoa = txtMaLop.Text;
            DataTable tbl = ctrl.GetDatatable("LOP where malop ='" + TuKhoa + "'");
            if(tbl.Rows.Count > 0)
            {
                txtTenLop.Text = tbl.Rows[0]["tenlop"].ToString();
                txtSiSo.Text = tbl.Rows[0]["siso"].ToString();
                cboKhoa.SelectedValue = tbl.Rows[0]["makhoa"].ToString();
            }
            else
            {
                txtTenLop.Text = "";
                txtSiSo.Text = "";
                cboKhoa.SelectedIndex = 0 ;
            }

        }

        private void txtMaLop_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                TimKiem();
            }
        }

        private void btnThemMoi_Click(object sender, EventArgs e)
        {
            connect();
            LayDuLieu();
            int count = ctrl.GetDatatable("lop").Select(" malop ='" + txtMaLop.Text + "'").Length;
            if (txtMaLop.Text == "")
            {
                MessageBox.Show("Chưa nhập mã lớp");
                txtMaLop.Focus();
                return;
            }else if (count > 0)
            {
                MessageBox.Show("Mã lớp đã tồn tại");
                txtMaLop.Focus();
                return;
            }
            else if (txtTenLop.Text == "")
            {
                MessageBox.Show("Chưa nhập tên lớp");
                txtTenLop.Focus();
                return;
            }
            ctrl.XuLyDuLieuTrongDB("sp_InsertLop",info.malop,info.tenlop, info.makhoa);
            MessageBox.Show("Thêm lớp mới thành công");
            HienThiDSLop();
        }
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaLop.Text = "";
            txtTenLop.Text = "";
            txtMaLop.ReadOnly = false;
            cboKhoa.SelectedIndex = 0;
        }
    }
}
