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
using static PdfManager.Data.PdfSearchResult;

namespace PdfManager
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        PdfManageModelContainer container;
        PdfFile currentPdf;

        PdfiumViewer.PdfViewer pdfViewer = new PdfiumViewer.PdfViewer();

        public PdfFile CurrentPdf
        {
            get
            {
                return currentPdf;
            }

            set
            {
                currentPdf = value;
                labTittle.DataContext = value;
            }
        }

        public MainWindow()
        {
            Task.Run(new Action(PreLoadEF));
            InitializeComponent();
        }
        private bool CheckNext(TreeViewItem item)
        {
            int cid = trvResult.Items.IndexOf(item);
            for (int i = cid + 1; i < trvResult.Items.Count; i++)
            {
                if ((trvResult.Items[i] as TreeViewItem).Items.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckPrev(TreeViewItem item)
        {
            int cid = trvResult.Items.IndexOf(item);
            for (int i = 0; i < cid; i++)
            {
                if ((trvResult.Items[i] as TreeViewItem).Items.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        private void Delete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = trvResult?.SelectedItem is PdfSearchItem;
            Debug.WriteLine(e.CanExecute, nameof(Delete_CanExecute));
        }

        private async void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            int result;
            var select = trvResult.SelectedItem as PdfSearchItem;
            Trace.Assert(select != null);
            var pdf = select.PdfFile;

            if (MessageBox.Show("是否确认删除？", "删除文件", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }

            container.PdfFileSet.Remove(pdf);
            result = await container.SaveChangesAsync();
            Trace.Assert(result == 1);
            RemoveFileFromResult(pdf);

            if (CurrentPdf == pdf)
            {
                //PdfiumViewer目前没有更好解决方案
                pdfViewer.Document.Dispose();
                pdfViewer.Dispose();
                pdfViewer = new PdfiumViewer.PdfViewer();
                winfromHost.Child = pdfViewer;
            }

            await Task.Run(() => File.Delete(pdf.GetFullPath()));
        }

        private void Find_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !string.IsNullOrWhiteSpace(txtKeyword.Text);
        }

        private async void Find_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            PdfSearchResult result;
            result = await container.Search(txtKeyword.Text);
            RefushResult(result);
        }

        private TreeViewItem GetNext(TreeViewItem treeViewItem)
        {
            int cid = trvResult.Items.IndexOf(treeViewItem);
            for (int i = cid + 1; i < trvResult.Items.Count; i++)
            {
                if ((trvResult.Items[i] as TreeViewItem).Items.Count > 0)
                {
                    return trvResult.Items[i] as TreeViewItem;
                }
            }
            return null;
        }

        private TreeViewItem GetPrev(TreeViewItem treeViewItem)
        {
            int cid = trvResult.Items.IndexOf(treeViewItem);

            for (int i = cid - 1; i >= 0; i--)
            {
                if ((trvResult.Items[i] as TreeViewItem).Items.Count > 0)
                {
                    return trvResult.Items[i] as TreeViewItem;
                }
            }
            return null;
        }

        private async Task LoadPdf(PdfFile pdf)
        {
            PdfiumViewer.PdfDocument doc = await Task.Run(() =>
            {
                return PdfiumViewer.PdfDocument.Load(pdf.GetFullPath());
            });
            pdfViewer.Document?.Dispose();
            pdfViewer.Document = doc;
            CurrentPdf = pdf;
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
                addwindow.ShowDialog();
            }
        }

        private void Next_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var current = trvResult?.SelectedItem;
            if (current is TreeViewItem)
            {
                e.CanExecute = CheckNext(current as TreeViewItem);
            }
            else if (current is PdfSearchItem)
            {
                var item = current as PdfSearchItem;
                var father = item.Father;

                if (father.IndexOf(item) < father.Count - 1)
                {
                    e.CanExecute = true;
                    return;
                }

                foreach (var tree in trvResult.Items)
                {
                    var t = tree as TreeViewItem;
                    if (t.ItemsSource == father)
                    {
                        e.CanExecute = CheckNext(t);
                        return;
                    }
                }
            }
            else
                e.CanExecute = false;
        }
        private void Next_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            PdfSearchItem next = null;
            TreeViewItem node = null;
            var current = trvResult?.SelectedItem;
            if (current is TreeViewItem)
            {
                node = GetNext(current as TreeViewItem);
                next = node.Items[0] as PdfSearchItem;
            }
            else if (current is PdfSearchItem)
            {
                var item = current as PdfSearchItem;
                var father = item.Father;
                var id = father.IndexOf(item);
                var t = trvResult.ItemContainerGenerator.Items.First(n =>
                    (n as TreeViewItem)?.ItemsSource == father) as TreeViewItem;

                if (id < father.Count - 1)
                {
                    next = father[id + 1] as PdfSearchItem;
                    node = t;
                }
                else
                {
                    node = GetNext(t);
                    next = node?.Items[0] as PdfSearchItem;
                }
            }

            ((Control)node.ItemContainerGenerator.ContainerFromItem(next))?.Focus();
        }
        private void PreLoadEF()
        {
            using (var dbcontext = new PdfManageModelContainer())
            {
                Trace.WriteLine(dbcontext.UserSet.Any());
            }
        }

        private void Prev_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var current = trvResult?.SelectedItem;
            if (current is TreeViewItem)
            {
                e.CanExecute = CheckPrev(current as TreeViewItem);
            }
            else if (current is PdfSearchItem)
            {
                var item = current as PdfSearchItem;
                var father = item.Father;

                if (father.IndexOf(item) > 0)
                {
                    e.CanExecute = true;
                    return;
                }

                foreach (var tree in trvResult.Items)
                {
                    var t = tree as TreeViewItem;
                    if (t.ItemsSource == father)
                    {
                        e.CanExecute = CheckPrev(t);
                        return;
                    }
                }
            }
            else
                e.CanExecute = false;
        }
        
        private void Export_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog()
            {
                DefaultExt = "zip",
                DereferenceLinks = true,
                Title = "导出Pdf存档",
                Filter = "压缩文档|*.zip",
            };

            if (dialog.ShowDialog() ?? false)
            {
                container.Expert(dialog.FileName);
            }
        }
        private void Import_Executed(object sender, ExecutedRoutedEventArgs e)
        {
        }

        private void Prev_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            PdfSearchItem next = null;
            TreeViewItem node = null;
            var current = trvResult?.SelectedItem;
            if (current is TreeViewItem)
            {
                node = GetPrev(current as TreeViewItem);
                next = node.Items[node.Items.Count - 1] as PdfSearchItem;
            }
            else if (current is PdfSearchItem)
            {
                var item = current as PdfSearchItem;
                var father = item.Father;
                var id = father.IndexOf(item);
                var t = trvResult.ItemContainerGenerator.Items.First(n =>
                    (n as TreeViewItem)?.ItemsSource == father) as TreeViewItem;

                if (id > 0)
                {
                    next = father[id - 1] as PdfSearchItem;
                    node = t;
                }
                else
                {
                    node = GetPrev(t);
                    next = node?.Items[node.Items.Count - 1] as PdfSearchItem;
                }
            }

            ((Control)node.ItemContainerGenerator.ContainerFromItem(next))?.Focus();
        }
        private void RefushResult(PdfSearchResult result)
        {
            trvResult.DataContext = result;
            Trace.WriteLine(nameof(RefushResult));
        }

        private void RemoveFileFromResult(PdfFile pdf)
        {
            var result = trvResult.DataContext as PdfSearchResult;

            Trace.Assert(result != null);

            Trace.Assert(result.Remove(pdf));

            RefushResult(result);
        }

        private void True_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private async void trvResult_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Debug.WriteLine(e.NewValue);
            PdfSearchItem pdf = e.NewValue as PdfSearchItem;
            if (pdf == null)
                return;
            try
            {
                await LoadPdf(pdf.PdfFile);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            if (!(login.ShowDialog() ?? false))
                Close();

            winfromHost.Child = pdfViewer;

            container = new PdfManageModelContainer();
        }
    }
}
