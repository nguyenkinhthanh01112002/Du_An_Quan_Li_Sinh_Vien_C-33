using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLiSinhVienFpoly
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            Load_Data_Combobox();
            Load_DataGridview();
        }
        DataStudentClasses1DataContext dataStudentClasses1 = new DataStudentClasses1DataContext();
        void Load_Data_Combobox()
        {
            var listStudents = dataStudentClasses1.STUDENTs.ToList();
            lblNameStudent.Text = listStudents[0].HoTen.ToString();
            var realStudent = dataStudentClasses1.GRADEs.FirstOrDefault(s => s.MASV == listStudents[0].MaSV);
            if (realStudent != null)
            {
                txtEnglish.Text = realStudent.TiengAnh.ToString();
                txtGDQP.Text = realStudent.TinHoc.ToString();
                txtTinHoc.Text = realStudent.GDTC.ToString();
                double diemTB = Math.Round(realStudent.DiemTrungBinh(), 1);
                lblDiemTB.Text = diemTB.ToString();
            }          
            comboBox2.DataSource = listStudents;           
            comboBox2.DisplayMember = "MaSV"; // Hiển thị mã sinh viên trong ComboBox
            comboBox2.ValueMember = "MaSV";
        }     
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            if (comboBox2.SelectedValue != null)
            {
                string selectedMASVShow = comboBox2.SelectedValue.ToString();
                var foundStudent = dataStudentClasses1.STUDENTs.FirstOrDefault(s => s.MaSV == selectedMASVShow);
                if (foundStudent != null)
                {
                    lblNameStudent.Text = foundStudent.HoTen.ToString();
                    var realStudent = dataStudentClasses1.GRADEs.FirstOrDefault(s => s.MASV == selectedMASVShow);
                    if (realStudent != null)
                    {
                        txtEnglish.Text = realStudent.TiengAnh.ToString();
                        txtGDQP.Text = realStudent.TinHoc.ToString();
                        txtTinHoc.Text = realStudent.GDTC.ToString();
                        double diemTB = Math.Round(realStudent.DiemTrungBinh(), 1);
                        lblDiemTB.Text = diemTB.ToString();
                    }
                    else
                    {
                        txtEnglish.Text = string.Empty;
                        txtTinHoc.Text = string.Empty;
                        txtGDQP.Text = string.Empty;
                        lblDiemTB.Text = "";
                        MessageBox.Show("Bạn có thể nhập điểm cho sinh viên");
                    }
                }
            }

        }

        private void lblNameStudent_Click(object sender, EventArgs e)
        {

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            txtSearchedMsv.Text = "PD";
            txtEnglish.Text = string.Empty;
            txtTinHoc.Text = string.Empty;
            txtGDQP.Text = string.Empty;
            txtEnglish.ReadOnly = false;
            txtGDQP.ReadOnly = false;
            txtTinHoc.ReadOnly = false;
        }
        static bool checkMark(string mark)
        {
            if (string.IsNullOrEmpty(mark))
            {
                return false;
            }
            float diemHopLe;
            if (float.TryParse(mark, out diemHopLe))
            {
                if (diemHopLe < 0 || diemHopLe > 10)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            txtSearchedMsv.Text = "PD";
            if (comboBox2.SelectedValue != null)
            {
                string selectedMASVShow = comboBox2.SelectedValue.ToString();
                var foundStudent = dataStudentClasses1.STUDENTs.FirstOrDefault(s => s.MaSV == selectedMASVShow);
                if (foundStudent != null)
                {
                    lblNameStudent.Text = foundStudent.HoTen.ToString();
                    var savedStudent = dataStudentClasses1.GRADEs.FirstOrDefault(g => g.MASV == selectedMASVShow);
                    if (savedStudent != null)
                    {
                        txtEnglish.ReadOnly = true;
                        txtGDQP.ReadOnly = true;
                        txtTinHoc.ReadOnly = true;
                        errorProvider1.SetError(txtEnglish, "điểm sinh viên bạn muốn nhập đã thực sự tồn tại vui lòng chọn tính năng update để chỉnh sửa");
                        errorProvider1.SetError(txtGDQP, "điểm sinh viên bạn muốn nhập đã thực sự tồn tại vui lòng chọn tính năng update để chỉnh sửa");
                        errorProvider1.SetError(txtTinHoc, "điểm sinh viên bạn muốn nhập đã thực sự tồn tại vui lòng chọn tính năng update để chỉnh sửa");
                    }
                    else
                    {
                        bool checkDiem = checkMark(txtEnglish.Text);

                        if (checkDiem)
                        {
                            if (checkMark(txtTinHoc.Text))
                            {
                                if (checkMark(txtGDQP.Text))
                                {
                                    var newStudent = new GRADE
                                    {
                                        MASV = selectedMASVShow,
                                        TiengAnh = float.Parse(txtEnglish.Text),
                                        TinHoc = float.Parse(txtTinHoc.Text),
                                        GDTC = float.Parse(txtGDQP.Text)
                                    };
                                    dataStudentClasses1.GRADEs.InsertOnSubmit(newStudent);
                                    dataStudentClasses1.SubmitChanges();
                                    MessageBox.Show("Đã thêm dữ liệu thành công", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    double diemTB = Math.Round(newStudent.DiemTrungBinh(), 1);
                                    lblDiemTB.Text = diemTB.ToString();
                                    Load_DataGridview();
                                   
                                }
                                else
                                {
                                    errorProvider1.SetError(txtEnglish, "Điểm phải thuộc [0;10]");
                                }
                            }
                            else
                            {
                                errorProvider1.SetError(txtEnglish, "Điểm phải thuộc [0;10]");
                            }

                        }
                        else
                        {
                            errorProvider1.SetError(txtEnglish, "Điểm phải thuộc [0;10]");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Không tồn tại bất cứ sinh viên nào trong hệ thống");
                }
            }
        }
        void Load_DataGridview()
        {
            string connectionString = "Data Source=NGUYENKINHTHANH\\SQLEXPRESS;Initial Catalog=FPOLY_STUDENT_DATA;Integrated Security=True";
            string query = "SELECT TOP 3 STUDENTS.MaSV, STUDENTS.HoTen, GRADE.TiengAnh, GRADE.TinHoc, GRADE.GDTC, ROUND((GRADE.TiengAnh + GRADE.TinHoc + GRADE.GDTC) / 3.0, 1) AS DiemTB FROM GRADE INNER JOIN STUDENTS ON GRADE.MaSV = STUDENTS.MaSV ORDER BY DiemTB DESC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                    {
                        DataSet dataSet = new DataSet();
                        dataAdapter.Fill(dataSet, "TOPSTUDENTS");

                        dataGridView1.DataSource = dataSet.Tables["TOPSTUDENTS"];
                    }
                }
            }

        }
        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            txtSearchedMsv.Text = "PD";
            txtEnglish.ReadOnly = false;
            txtGDQP.ReadOnly = false;
            txtTinHoc.ReadOnly = false;
            //kiểm tra lỗi chỗ này
            var firstStudents = dataStudentClasses1.STUDENTs.ToList();
            var firstGrade = dataStudentClasses1.GRADEs.FirstOrDefault(s => s.MASV == firstStudents[0].MaSV);
            if (comboBox2.SelectedValue != null || firstGrade!=null)
            {
                string selectedMsvShow = comboBox2.SelectedValue.ToString();
                if (comboBox2.SelectedValue != null)
                {
                    selectedMsvShow = comboBox2.SelectedValue.ToString();
                }
                else
                {
                    selectedMsvShow = firstGrade.MASV.ToString();
                }
                var foundStudent = dataStudentClasses1.GRADEs.FirstOrDefault(s => s.MASV == selectedMsvShow);
                DialogResult result = MessageBox.Show("Bạn có chắc muốn cập điểm của sinh viên","Confirm",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if(result==DialogResult.Yes)
                {
                    if(!checkMark(txtEnglish.Text)||!checkMark(txtGDQP.Text)||!checkMark(txtTinHoc.Text) )
                    {
                        MessageBox.Show("Vui lòng kiểm tra lại các ô điểm bạn vừa nhập");
                    }
                    else
                    {
                        foundStudent.TiengAnh = float.Parse(txtEnglish.Text);
                        foundStudent.GDTC = float.Parse(txtGDQP.Text);
                        foundStudent.TinHoc = float.Parse(txtTinHoc.Text);
                        dataStudentClasses1.SubmitChanges();
                        double diemTB = Math.Round(foundStudent.DiemTrungBinh(), 1);
                        lblDiemTB.Text = diemTB.ToString();
                        Load_DataGridview();
                        MessageBox.Show("Đã cập nhật thành công","Notification",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }
                    
                }
                else
                {
                    MessageBox.Show("Mời bạn tiếp tục sử dụng chương trình");
                }
                

               
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            var firstStudents = dataStudentClasses1.STUDENTs.ToList();
            var firstGrade = dataStudentClasses1.GRADEs.FirstOrDefault(s => s.MASV == firstStudents[0].MaSV);
            if (comboBox2.SelectedValue != null || firstGrade != null)
            {
                string selectedMsvShow = comboBox2.SelectedValue.ToString();
                if (comboBox2.SelectedValue != null)
                {
                    selectedMsvShow = comboBox2.SelectedValue.ToString();
                }
                else
                {
                    selectedMsvShow = firstGrade.MASV.ToString();
                }
                var foundStudent = dataStudentClasses1.GRADEs.FirstOrDefault(s => s.MASV == selectedMsvShow);
                
                DialogResult result = MessageBox.Show("Bạn có chắc muốn xoá không", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(result == DialogResult.Yes)
                {
                    if (foundStudent != null)
                    {
                        dataStudentClasses1.GRADEs.DeleteOnSubmit(foundStudent);
                        dataStudentClasses1.SubmitChanges();
                        MessageBox.Show("Dữ liệu đã được xoá thành công", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Load_DataGridview();
                    }
                    else
                    {
                       
                        MessageBox.Show("Sinh viên bạn muốn xoá không tồn tại trong hệ thống");
                        
                    }
                }
                else
                {
                    MessageBox.Show("Mời bạn tiếp tục sử dụng chương trình");
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
                MessageBox.Show("Mời bạn tiếp tục sử dụng chương trình", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            string realMaSV = @"(?i)^pd\d{5}$";
            bool checkMaSV = Regex.IsMatch((string)txtSearchedMsv.Text, realMaSV);
            if (checkMaSV)
            {
                var foundGrade = dataStudentClasses1.GRADEs.FirstOrDefault(g => g.MASV.ToLower() == txtSearchedMsv.Text.ToLower());
                var foundStudent = dataStudentClasses1.STUDENTs.FirstOrDefault(s => s.MaSV.ToLower() == txtSearchedMsv.Text.ToLower());
                if (foundGrade != null)
                {
                    comboBox2.SelectedValue = foundStudent.MaSV;
                    lblNameStudent.Text = foundStudent.HoTen.ToString();
                    txtEnglish.Text = foundGrade.TiengAnh.ToString();
                    txtGDQP.Text = foundGrade.TinHoc.ToString();
                    txtTinHoc.Text = foundGrade.GDTC.ToString();
                    double diemTB = Math.Round(foundGrade.DiemTrungBinh(), 1);
                    lblDiemTB.Text = diemTB.ToString();
                }
                else
                {
                    MessageBox.Show("Dữ liệu bạn đã tìm trống", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    comboBox2.SelectedValue = "";
                    lblNameStudent.Text = "";
                    txtEnglish.Text = "";
                    txtGDQP.Text = "";
                    txtTinHoc.Text = "";
                    lblDiemTB.Text = "";
                    txtSearchedMsv.Text = "PD";
                }
            }
            else
            {
                errorProvider1.SetError(txtSearchedMsv, "MSV phải có dạng PDxxxxx với x thuộc [0;9]");
            }
        }
    }
}
