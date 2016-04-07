using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfManager.Data
{
    public class PdfSearchResult
    {
        public IList<PdfFile> ByTittle { get; set; }
        public IList<PdfFile> ByYear { get; set; }
        public IList<PdfFile> ByNumber { get; set; }
        public IList<PdfFile> ByOther1 { get; set; }
        public IList<PdfFile> ByOther2 { get; set; }
    }
}
