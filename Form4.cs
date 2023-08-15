using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLiSinhVienFpoly
{
    public partial class frmSigUp : Form
    {
        public frmSigUp()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        DataStudentClasses1DataContext dataStudents = new DataStudentClasses1DataContext();
        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnSignup_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            if(txtEmail.Text ==""||txtUserName.Text ==""||txtPassWord.Text == "")
            {
                MessageBox.Show("Vui lòng kiểm tra lại các ô nhập liệu");
                frmSigUp frmSigUp = new frmSigUp();
                this.Hide();
                frmSigUp.ShowDialog();
            }
            else
            {
                var survivedUsers = dataStudents.USERs.ToList();
                string realEmail = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                bool checkedEmail = Regex.IsMatch((string)(txtEmail.Text), realEmail, RegexOptions.IgnoreCase);
                if (checkedEmail)
                {
                    var realUser = survivedUsers.FirstOrDefault(u => u.username.ToLower() == txtUserName.Text.ToLower());
                    if (realUser != null)
                    {
                        errorProvider1.SetError(txtUserName, "UserName bạn nhập đã thực sự tồn tại");
                    }
                    else
                    {
                        string realPassWord = @"^\d{4}$";
                        bool checkedPassWord = Regex.IsMatch((string)(txtPassWord.Text), realPassWord, RegexOptions.IgnoreCase);
                        if (checkedPassWord)
                        {
                            var newUser = new USER
                            {
                                username = txtUserName.Text,
                                password = txtPassWord.Text,
                                role = "user"
                            };
                            dataStudents.USERs.InsertOnSubmit(newUser);
                            dataStudents.SubmitChanges();
                            MessageBox.Show("Đã tạo tài khoản thành công");
                            frmLogin newLogin = new frmLogin();
                            this.Hide();
                            newLogin.ShowDialog();
                        }
                        else
                        {
                            errorProvider1.SetError(txtPassWord, "PassWord chỉ gồm 4 chữ số");
                        }
                    }
                }
                else
                {
                    errorProvider1.SetError(txtEmail, "Email không hợp lệ");
                }
            }
           
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmLogin newLogin = new frmLogin();
            this.Hide();
            newLogin.ShowDialog();
        }
    }
}
