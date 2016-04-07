using Microsoft.Win32;
using PdfManager.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PdfManager
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(new Action(PreLoadEF));

            LoginWindow login = new LoginWindow();
            if (!(login.ShowDialog() ?? false))
                Close();
        }

        private void PreLoadEF()
        {
            using (var dbcontext = new PdfManageModelContainer())
            {
                Trace.WriteLine(dbcontext.UserSet.Any());
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Multiselect = false,
                DefaultExt = "pdf",
                DereferenceLinks = true,
                Title = "打开PDF文件",
                Filter = "PDF文件|*.pdf",
            };
            if (dialog.ShowDialog() ?? false)
            {
                var path = dialog.FileName;
                AddPdfWindow addwindow = new AddPdfWindow(path);
                if (addwindow.ShowDialog() ?? false)
                {

                }
            }
        }
    }
}
