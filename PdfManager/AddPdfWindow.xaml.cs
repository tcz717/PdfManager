using PdfManager.Data;
using PdfManager.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
using IO = System.IO;

namespace PdfManager
{
    /// <summary>
    /// AddPdfWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddPdfWindow : Window
    {
        public PdfFile Pdf { get; private set; }
        public string SavePath { get; private set; }
        private string path;
        static string storePath = PdfFile.StorePath;

        public AddPdfWindow()
        {
            InitializeComponent();
        }

        public AddPdfWindow(string path)
            : this()
        {
            this.path = path;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var data = new PdfFile(path);
            grdMain.DataContext = data;
        }

        private async void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Pdf = grdMain.DataContext as PdfFile;
            SavePath = IO.Path.Combine(storePath, Pdf.FileName);

            if (IO.File.Exists(SavePath))
            {
                if (MessageBox.Show("已存在同名文件是否继续？", "存在同名文件", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    return;
                }
            }

            using (PdfManageModelContainer container = new PdfManageModelContainer())
            {
                txbNotice.Text = "正在复制文件...";
                if (await container.AddPdfAsync(Pdf, path, SavePath))
                {
                    DialogResult = true;
                    Close();
                }
            }
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var context = new ValidationContext(grdMain.DataContext);
            var result = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            e.CanExecute = Validator.TryValidateObject(grdMain.DataContext, context, result);
        }
    }
}
