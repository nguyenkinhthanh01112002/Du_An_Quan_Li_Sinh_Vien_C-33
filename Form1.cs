using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLiSinhVienFpoly
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }
        DataStudentClasses1DataContext dataStudents = new DataStudentClasses1DataContext();
        private void btnLogin_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
           
           
            string enteredUserName = txtUsername.Text.ToString();
            string enteredPassWord = txtPassword.Text.ToString();
            List<USER> users = new List<USER>();
            users = dataStudents.USERs.ToList();
            var foundUser = users.FirstOrDefault(u => u.username.Equals(enteredUserName));
           // var foundUser = dataStudents.USERs.FirstOrDefault(u => u.username.Equals(enteredUserName));
            if (foundUser != null)
            {
                if (foundUser.password.Equals(enteredPassWord))
                {
                    MessageBox.Show("Login Successed...", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (foundUser.role.Equals("canbodaotao"))
                    {
                        this.Hide();
                        frmQuanLiSinhVien form2 = new frmQuanLiSinhVien();
                        form2.ShowDialog();
                    }
                    else if(foundUser.role.Equals("giangvien"))
                    {
                        this.Hide();
                        Form3 form3 = new Form3();
                        form3.ShowDialog();
                    }
                    else
                    {
                        this.Hide();
                        frmQuanLiSinhVien form2 = new frmQuanLiSinhVien();
                        form2.ShowDialog();
                    }
                    
                }
                else
                {
                    MessageBox.Show("Login Failed...", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    errorProvider1.SetError(txtPassword, "Password is incorrect");
                }
            }
            else
            {
                MessageBox.Show("Login Failed...", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                errorProvider1.SetError(txtUsername, "User name is incorrect");
            }
        }

        private void grpLogin_Enter(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn chắc chắn muốn thoát không?","Confirm",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Close();
            }
            else if(result == DialogResult.No)
            {
                MessageBox.Show("Mời bạn tiếp tục sử dụng chương trình");
            }
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }

        private void btnSignup_Click(object sender, EventArgs e)
        {
            frmSigUp newSigUp = new frmSigUp();
            this.Hide();
            newSigUp.ShowDialog();
        }
    }
}
