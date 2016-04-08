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

        static PdfFile[] GenarateTestPdf(int count)
        {
            PdfFile[] pdfs = new PdfFile[count];

            for (int i = 0; i < count; i++)
            {
                pdfs[i] = new PdfFile()
                {
                    Tittle = Faker.TextFaker.Sentence(),
                    FileId = Faker.NumberFaker.Number(),
                    Year = Faker.NumberFaker.Number(1000, 3000),
                    FileName = Faker.StringFaker.AlphaNumeric(20),
                    Other1 = Faker.StringFaker.AlphaNumeric(20),
                    Other2 = Faker.PhoneFaker.InternationalPhone()
                };
            }

            return pdfs;
        }

        [TestMethod()]
        public void SearchTest()
        {
            var pdfs = GenarateTestPdf(100);
            container.PdfFileSet.AddRange(pdfs);
            container.SaveChanges();

            for (int i = 0; i < 100; i++)
            {
                var result = container.Search(pdfs[i].Tittle.Substring(2));
                result.Wait(100);
                Assert.IsTrue(result.Result.ByTittle.Any());
            }
            for (int i = 0; i < 100; i++)
            {
                var result = container.Search(pdfs[i].Other1.Substring(2));
                result.Wait(100);
                Assert.IsTrue(result.Result.ByOther1.Any());
            }
            for (int i = 0; i < 100; i++)
            {
                var result = container.Search(pdfs[i].Other2.Substring(2));
                result.Wait(100);
                Assert.IsTrue(result.Result.ByOther2.Any());
            }
            for (int i = 0; i < 100; i++)
            {
                var result = container.Search(pdfs[i].Year / 10);
                result.Wait(100);
                Assert.IsTrue(result.Result.ByYear.Any());
            }
            for (int i = 0; i < 100; i++)
            {
                var result = container.Search(pdfs[i].FileId);
                result.Wait(100);
                Assert.IsTrue(result.Result.ByNumber.Any());
            }
        }
        [TestMethod()]
        public void EncodeAndDecodeTest()
        {
            var pdfs = GenarateTestPdf(100);
            container.PdfFileSet.AddRange(pdfs);
            container.SaveChanges();

            using (StreamWriter sw = new StreamWriter("test.json"))
            {
                container.EncodePdfList(sw);
            }

            container.PdfFileSet.RemoveRange(container.PdfFileSet);
            container.SaveChanges();


            using (StreamReader sr = new StreamReader("test.json"))
            {
                container.DecodePdfList(sr);
            }
            container.SaveChanges();

            foreach (var item in pdfs)
            {
                Assert.IsTrue(container.PdfFileSet.Any(n =>
                    n.Tittle == item.Tittle));
                    //&&
                    //n.Year == item.Year &&
                    //n.FileId == item.FileId &&
                    //n.CreateTime == item.CreateTime &&
                    //n.FileName == item.FileName &&
                    //n.Other1 == item.Other1 &&
                    //n.Other2 == item.Other2));
            }
        }
    }
}