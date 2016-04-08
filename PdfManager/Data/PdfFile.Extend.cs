using PdfManager.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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

        //public override bool Equals(object obj)
        //{
        //    PdfFile pdf = obj as PdfFile;
        //    if (pdf == null)
        //        return false;
        //    return pdf.Tittle == pdf.Tittle;
        //}
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

        public static async Task<PdfSearchResult> Search(this PdfManageModelContainer container, object keyword)
        {
            var kw = keyword.ToString().ToLower();
            var set = container.PdfFileSet;
            var result = new PdfSearchResult();
            result.SetByTittle(await set.Where(n =>
                    n.Tittle.ToLower().Contains(kw)).ToArrayAsync());
            result.SetByOther1(await set.Where(n =>
                    n.Other1.ToLower().Contains(kw)).ToArrayAsync());
            result.SetByOther2(await set.Where(n =>
                    n.Other2.ToLower().Contains(kw)).ToArrayAsync());
            result.SetByNumber(await set.Where(n =>
                    n.FileId.ToString().Contains(kw)).ToArrayAsync());
            result.SetByYear(await set.Where(n =>
                    n.Year.ToString().Contains(kw)).ToArrayAsync());
            return result;
        }
    }
}
