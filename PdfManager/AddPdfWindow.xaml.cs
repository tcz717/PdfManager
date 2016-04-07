using PdfManager.Data;
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
        private string path;

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
            var data = new PdfFile()
            {
                FileName = IO.Path.GetFileName(path),
                Tittle = IO.Path.GetFileNameWithoutExtension(path),

            };
            grdMain.DataContext = data;
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }
    }
}
