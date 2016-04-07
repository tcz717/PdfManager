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

        PdfiumViewer.PdfViewer pdfViewr = new PdfiumViewer.PdfViewer();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(new Action(PreLoadEF));

            LoginWindow login = new LoginWindow();
            if (!(login.ShowDialog() ?? false))
                Close();
            CollectionViewSource pdfFileViewSource = ((CollectionViewSource)(FindResource("pdfFileViewSource")));
            // 通过设置 CollectionViewSource.Source 属性加载数据: 
            // pdfFileViewSource.Source = [一般数据源]
            winfromHost.Child = pdfViewr;
        }

        private void PreLoadEF()
        {
            using (var dbcontext = new PdfManageModelContainer())
            {
                Trace.WriteLine(dbcontext.UserSet.Any());
            }
        }

        private void RefushResult()
        {
            throw new NotImplementedException();
        }

        #region TmepCode
        private void RefushResultTest()
        {
            using (PdfManageModelContainer container = new PdfManageModelContainer())
            {
                var list = container.PdfFileSet;
                PdfSearchResult result = new PdfSearchResult()
                {
                    ByNumber = list.ToList(),
                    ByOther1 = list.ToList(),
                    ByOther2 = list.ToList(),
                    ByTittle = list.ToList(),
                    ByYear = list.ToList(),
                };
                trvResult.DataContext = result;
            }
        }


        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Debug.WriteLine(e.Parameter, e.Command.ToString());
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
        } 
        #endregion

        private async void trvResult_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Debug.WriteLine(e.NewValue);
            PdfFile pdf = e.NewValue as PdfFile;
            if (pdf == null)
                return;
            try
            {
                PdfiumViewer.PdfDocument doc = await Task.Run<PdfiumViewer.PdfDocument>(() =>
                {
                    return PdfiumViewer.PdfDocument.Load(pdf.GetFullPath());
                });
                //var fs = File.OpenRead(pdf.GetFullPath());
                pdfViewr.Document = doc;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
            }
        }

        private void New_Executed(object sender, ExecutedRoutedEventArgs e)
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
                    RefushResultTest();
                }
            }
        }
        private void True_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void Find_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            PdfSearchResult result;
            using (PdfManageModelContainer container = new PdfManageModelContainer())
            {

            }
        }
    }
}
