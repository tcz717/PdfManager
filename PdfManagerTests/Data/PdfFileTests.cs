using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfManager.Data.Tests
{
    [TestClass()]
    public class PdfFileTests
    {
        [TestMethod()]
        public void MakeTagsTest()
        {
            PdfFile testFile = new PdfFile()
            {
                Tittle = "whatever, this is just a docment file used for test",
                FileName = "test.pdf"
            };
            var keys = testFile.MakeTags();
            Assert.IsTrue(keys.All(n => testFile.Tittle.Contains(n) && !string.IsNullOrWhiteSpace(n)));
        }
    }
}