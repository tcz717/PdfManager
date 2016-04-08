using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfManager.Data
{
    public class PdfSearchResult
    {
        //public IList<PdfFile> ByTittle { get; set; }
        public ObservableCollection<PdfFile> ByTittle { get; set; }
        public ObservableCollection<PdfFile> ByYear { get; set; }
        public ObservableCollection<PdfFile> ByNumber { get; set; }
        public ObservableCollection<PdfFile> ByOther1 { get; set; }
        public ObservableCollection<PdfFile> ByOther2 { get; set; }

        public bool Remove(PdfFile pdf)
        {
            bool result = false;

            result |= ByNumber.Remove(pdf);
            result |= ByOther1.Remove(pdf);
            result |= ByOther2.Remove(pdf);
            result |= ByTittle.Remove(pdf);
            result |= ByYear.Remove(pdf);

            return result;
        }
    }
}
