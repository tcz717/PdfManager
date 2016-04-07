using PdfManager.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfManager.Data
{
    public partial class PdfFile
    {
        public static string StorePath = Path.Combine(Environment.CurrentDirectory,
            Settings.Default.StoreDirectory.ToString());
        public PdfFile(string filePath)
            : this()
        {
            FileName = Path.GetFileName(filePath);
            Tittle = Path.GetFileNameWithoutExtension(filePath);
        }
        public string[] MakeTags()
        {
            //var analyzer = new Lucene.Net.Analysis.PanGu.PanGuAnalyzer();
            //var indexer=new Lucene.Net.Index.IndexWriter()

            throw new NotImplementedException();
        }

        public string GetFullPath()
        {
            return Path.Combine(StorePath, FileName);
        }
    }
    public static class PdfFileExtend
    {
        public static async Task<bool> AddPdfAsync(this PdfManageModelContainer container, PdfFile pdf, string from, string to)
        {
            try
            {
                await Task.Run(() => File.Copy(from, to, true));
                container.PdfFileSet.Add(pdf);
                await container.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
