using PdfManager.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PdfManager
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txbError.Text = "";
            txtUsername.Focus();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            //if(e.Key==Key.Enter)
            
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                using (PdfManageModelContainer container = new PdfManageModelContainer())
                {
                    var name = txtUsername.Text;
                    var pwd = txtPassword.Password;
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        Trace.WriteLine(txbError.Text = "用户名不能为空");
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(pwd))
                    {
                        Trace.WriteLine(name, txbError.Text = "密码不能为空");
                        return;
                    }

                    if (!container.Login(name, pwd))
                    {
                        Trace.WriteLine(new { Name = name, Password = pwd }, txbError.Text = "密码错误");
                        return;
                    }

                    DialogResult = true;
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("请确认安装了x86或x64的Microsoft SQL 2014 Express LocalDb。",
                    "数据库启动失败", MessageBoxButton.OK, MessageBoxImage.Error);
                Trace.Fail(ex.ToString());
            }
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var name = txtUsername.Text;
            var pwd = txtPassword.Password;
            e.CanExecute = !(string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(pwd));
        }
    }
}
