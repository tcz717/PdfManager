using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfManager.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfManager.Data.Tests
{
    [TestClass()]
    public class PdfFileExtendTests
    {
        static string storePath = Path.Combine(Environment.CurrentDirectory, "Store");
        PdfManageModelContainer container;
        [ClassInitialize]
        public static void Preload(TestContext context)
        {
            using (PdfManageModelContainer con = new PdfManageModelContainer())
            {
                con.PdfFileSet.Any();
            }
        }
        [TestInitialize]
        public void Init()
        {
            container = new PdfManageModelContainer();
            if (!Directory.Exists(storePath))
                Directory.CreateDirectory(storePath);
        }
        [TestCleanup]
        public void Close()
        {
            container.PdfFileSet.RemoveRange(container.PdfFileSet);
            container.SaveChanges();
            container.Dispose();

            var files = Directory.EnumerateFiles(storePath);
            foreach (var item in files)
            {
                File.Delete(item);
            }
        }
        [TestMethod()]
        public void AddPdfAsyncTest()
        {
            var files = Directory.EnumerateFiles(Environment.CurrentDirectory);
            var pdfs = files.Where(n => Path.GetExtension(n).ToLower() == ".pdf");
            Assert.IsTrue(pdfs.Any());
            foreach (var item in pdfs)
            {
                PdfFile pdf = new PdfFile(item);
                var savePath = Path.Combine(storePath, pdf.FileName);
                var task = container.AddPdfAsync(pdf, item, savePath);
                task.Wait();
                Assert.IsTrue(task.Result);
                Assert.IsTrue(File.Exists(savePath));
                Assert.IsTrue(container.PdfFileSet.Any(n => n.FileName == pdf.FileName));
            }
        }
    }
}