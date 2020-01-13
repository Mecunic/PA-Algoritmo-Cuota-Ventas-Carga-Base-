using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;

namespace MVC_Project.Desktop
{
    public partial class LoginForm : Form
    {
        [Dependency]
        public IUserService _userService { get; set; }
        [Dependency("AuthService")]
        public IAuthService AuthService { get; set; }
        public LoginForm(/*IUserService userService,*/ IAuthService _authService)
        {
            //_userService = userService;
            AuthService = _authService;
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtUsername.Text) || String.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please provide userName and password");
                return;
            }

            string pass = Utils.Cryptography.EncryptPassword(txtPassword.Text.Trim());
            User user = AuthService.Authenticate(txtUsername.Text.Trim(), pass);

            if (user == null)
            {
                MessageBox.Show("ERROR: No se puede iniciar sesión");
                return;
            }
            else
            {
                this.Hide();
                MainForm mainForm = new MainForm();
                mainForm.Show();
            }
        }
    }
}
