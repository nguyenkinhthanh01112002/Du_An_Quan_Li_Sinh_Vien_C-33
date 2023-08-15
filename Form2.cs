using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace QuanLiSinhVienFpoly
{
    public partial class frmQuanLiSinhVien : Form
    {
        public frmQuanLiSinhVien()
        {
            InitializeComponent();
        }
        DataStudentClasses1DataContext dataStudents = new DataStudentClasses1DataContext();
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void frmQuanLiSinhVien_Load(object sender, EventArgs e)
        {
            Load_Data();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            txtMaSV.ReadOnly = false;
            txtMaSV.Text = string.Empty;
            txtHoTen.Text = string.Empty;
            txtDiaChi.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtSoDT.Text = string.Empty;
            radNam.Checked = false;
            radNu.Checked = false;
            picBox1.Image = null;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            bool gioiTinh = radNam.Checked ? true : false;

            if (picBox1.Image != null)
            {

                string fileName = txtMaSV.Text + "_" + Guid.NewGuid().ToString() + ".jpg";
                string folder = "D:\\Files";
                string pathString = Path.Combine(folder, fileName);

                System.Drawing.Image image = picBox1.Image;
                if (image != null)
                {
                    image.Save(pathString);
                }

                string realMaSV = @"(?i)^pd\d{5}$";


                bool checkMaSV = Regex.IsMatch((string)txtMaSV.Text, realMaSV);
                if (checkMaSV)
                {
                    var checkedSurvivedMSV = dataStudents.STUDENTs.FirstOrDefault(s => s.MaSV.ToLower() == (string)txtMaSV.Text.ToLower());
                    string realEmail = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                    if (checkedSurvivedMSV == null)
                    {
                        bool checkEmail = Regex.IsMatch((string)(txtEmail.Text), realEmail, RegexOptions.IgnoreCase);
                        if (checkEmail)
                        {
                            var checkedSurvivedEmail = dataStudents.STUDENTs.FirstOrDefault(s => s.Email.ToLower() == (string)txtEmail.Text.ToLower());
                            string realPhone = @"^0\d{9}$";
                            if (checkedSurvivedEmail == null)
                            {
                                bool CheckedPhone = Regex.IsMatch((string)txtSoDT.Text, realPhone, RegexOptions.IgnoreCase);
                                if (CheckedPhone)
                                {
                                    var st = new STUDENT
                                    {
                                        MaSV = txtMaSV.Text,
                                        HoTen = txtHoTen.Text,
                                        GioiTinh = gioiTinh,
                                        Email = txtEmail.Text,
                                        SoDT = txtSoDT.Text,
                                        DiaChi = txtDiaChi.Text,
                                        Hinh = pathString
                                    };
                                    dataStudents.STUDENTs.InsertOnSubmit(st);
                                    dataStudents.SubmitChanges();
                                    Load_Data();
                                    MessageBox.Show("Đã thêm dữ liệu thành công", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    errorProvider1.SetError(txtSoDT, "Số điện thoại bạn vừa nhập không hợp lệ");
                                }
                            }
                            else
                            {
                                errorProvider1.SetError(txtEmail, "Email bạn nhập đã tồn tại");
                            }
                        }
                        else
                        {
                            errorProvider1.SetError(txtEmail, "Email bạn vừa nhập không hợp lệ");
                        }
                    }
                    else
                    {
                        errorProvider1.SetError(txtMaSV, "Mã sinh viên bạn nhập đã tồn tại");
                    }
                }
                else
                {
                    errorProvider1.SetError(txtMaSV, "Mã sinh viên bạn vừa nhập không hợp lệ");
                }

            }
            else
            {
                MessageBox.Show("Choose a picture");
            }
        }
        void Load_Data()
        {

            string connectionString = "Data Source=NGUYENKINHTHANH\\SQLEXPRESS;Initial Catalog=FPOLY_STUDENT_DATA;Integrated Security=True";
            string query = "SELECT * FROM STUDENTs";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                    {
                        DataSet dataSet = new DataSet();
                        dataAdapter.Fill(dataSet, "STUDENTs");

                        dataGridView1.DataSource = dataSet.Tables["STUDENTs"];
                    }
                }
            }


        }

        private void picBox1_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            PictureBox p = sender as PictureBox;
            if (p != null)
            {
                openFileDialog.Filter = "(*.jpg;*.jpeg;*.bmp;)|*.jpg;*.jpeg;*.bmp; ";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    p.Image = System.Drawing.Image.FromFile(openFileDialog.FileName);
                }
            }

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            /* if (e.RowIndex >= 0 && e.ColumnIndex >= 0) // Đảm bảo người dùng chọn ô hợp lệ.
             {
                 DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                 DataGridViewColumn column = dataGridView1.Columns[e.ColumnIndex];

                 string columnName = column.HeaderText;
                 string cellValue = Convert.ToString(row.Cells[e.ColumnIndex].Value);

                 // Hiển thị tên ô và giá trị ô trong MessageBox
                 MessageBox.Show($"Tên ô: {columnName}\nGiá trị ô: {cellValue}");
             }*/
            errorProvider1.Clear();
            txtMaSV.ReadOnly = true;
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                txtHoTen.Text = Convert.ToString(row.Cells["HoTen"].Value);
                txtMaSV.Text = Convert.ToString(row.Cells["MaSV"].Value);
                txtEmail.Text = Convert.ToString(row.Cells["Email"].Value);
                txtDiaChi.Text = Convert.ToString(row.Cells["DiaChi"].Value);
                txtSoDT.Text = Convert.ToString(row.Cells["SoDT"].Value);
                string pathString = Convert.ToString(row.Cells["Hinh"].Value); // Giả sử đường dẫn ảnh lưu trong cột "imagePath"
                bool check = (bool)row.Cells["GioiTinh"].Value;
                if (check)
                {
                    radNam.Checked = true;
                }
                else
                {
                    radNu.Checked = true;
                }
                if (!string.IsNullOrEmpty(pathString))
                {
                    picBox1.Image = System.Drawing.Image.FromFile(pathString); // Hiển thị ảnh từ đường dẫn file
                }
                else
                {
                    // Nếu không có đường dẫn ảnh hoặc đường dẫn không hợp lệ, bạn có thể hiển thị một ảnh mặc định hoặc xử lý tùy ý.
                    picBox1.Image = null; // Hiển thị một hình ảnh mặc định hoặc ẩn ảnh nếu không có ảnh.
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                Close();
            }
            else
            {
                MessageBox.Show("Mời bạn tiếp tục sử dụng chương trình");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            var realStudent = dataStudents.STUDENTs.FirstOrDefault(s => s.MaSV.ToLower() == (string)txtMaSV.Text);
            if (realStudent != null)
            {
                bool gioiTinh = radNam.Checked;
                if (gioiTinh)
                {
                    realStudent.GioiTinh = true;
                }
                else
                {
                    realStudent.GioiTinh = false;
                }
                if (picBox1.Image != null)
                {

                    string fileName = txtMaSV.Text + "_" + Guid.NewGuid().ToString() + ".jpg";
                    string folder = "D:\\Files";
                    string pathString = Path.Combine(folder, fileName);
                    System.Drawing.Image image = picBox1.Image;
                    if (image != null)
                    {
                        image.Save(pathString);
                        realStudent.Hinh = pathString;
                    }
                    string realEmail = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                    bool checkedEmail = Regex.IsMatch((string)(txtEmail.Text), realEmail, RegexOptions.IgnoreCase);
                    if (checkedEmail)
                    {
                        var anotherStudent = dataStudents.STUDENTs.FirstOrDefault(s => s.Email.ToLower() == (string)txtEmail.Text);
                        string realPhone = @"^0\d{9}$";
                        if (txtEmail.Text.ToLower() == realStudent.Email.ToLower() || anotherStudent == null)
                        {
                            realStudent.Email = txtEmail.Text;
                            bool checkedPhone = Regex.IsMatch((string)txtSoDT.Text, realPhone, RegexOptions.IgnoreCase);
                            if (checkedPhone)
                            {
                                anotherStudent = dataStudents.STUDENTs.FirstOrDefault(s => s.SoDT == txtSoDT.Text.ToString());
                                if (txtSoDT.Text.ToString() == realStudent.SoDT.ToString() || anotherStudent == null)
                                {
                                    realStudent.SoDT = txtSoDT.Text;
                                    realStudent.DiaChi = txtDiaChi.Text;
                                    realStudent.HoTen = txtHoTen.Text;
                                    dataStudents.SubmitChanges();
                                    Load_Data();
                                    MessageBox.Show("Đã cập nhật dữ liệu thành công", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    errorProvider1.SetError(txtSoDT, "Số điện thoại bạn vừa nhập đã tồn tại");
                                }
                            }
                            else
                            {
                                errorProvider1.SetError(txtSoDT, "Số điện thoại bạn vừa nhập không hợp lệ");

                            }
                        }
                        else
                        {
                            errorProvider1.SetError(txtEmail, "Email bạn vừa nhập đã thực sự tồn tại");
                        }
                    }
                    else
                    {
                        errorProvider1.SetError(txtEmail, "Email không hợp lệ");
                    }


                }
                else
                {
                    MessageBox.Show("Choose a picture");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn thông tin bạn muốn cập nhất");
            }
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            txtMaSV.ReadOnly = false;
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xoá không?","Confirm",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                var realStudent = dataStudents.STUDENTs.FirstOrDefault(s => s.MaSV.ToLower() == txtMaSV.Text.ToLower());
                if (realStudent != null)
                {
                    var deletedGrade = dataStudents.GRADEs.FirstOrDefault(g => g.MASV.ToLower() == txtMaSV.Text.ToLower());
                    if (deletedGrade != null)
                    {
                        dataStudents.GRADEs.DeleteOnSubmit(deletedGrade);
                        dataStudents.SubmitChanges();
                       
                    }
                    dataStudents.STUDENTs.DeleteOnSubmit(realStudent);
                    dataStudents.SubmitChanges();
                    MessageBox.Show("Đã xoá dữ liệu thành công", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Load_Data();
                    txtMaSV.Text = string.Empty;
                    txtHoTen.Text = string.Empty;
                    txtDiaChi.Text = string.Empty;
                    txtEmail.Text = string.Empty;
                    txtSoDT.Text = string.Empty;
                    radNam.Checked = false;
                    radNu.Checked = false;
                    picBox1.Image = null;
                                                                       
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn dữ liệu trước khi xoá");

                }
            }
            else
            {
                MessageBox.Show("Mời bạn tiếp tục sử dụng chương trình");
            }
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
