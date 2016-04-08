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
        public class PdfSearchItem
        {
            public PdfFile PdfFile { get; set; }
            public ObservableCollection<PdfSearchItem> Father { get; set; }

            public void Release()
            {
                Father?.Remove(this);
                Father = null;
                PdfFile = null;
            }
        }

        public PdfSearchResult()
        {
            ByTittle = new ObservableCollection<PdfSearchItem>();
            ByYear = new ObservableCollection<PdfSearchItem>();
            ByNumber = new ObservableCollection<PdfSearchItem>();
            ByOther1 = new ObservableCollection<PdfSearchItem>();
            ByOther2 = new ObservableCollection<PdfSearchItem>();
        }

        public ObservableCollection<PdfSearchItem> ByNumber { get; private set; }
        public ObservableCollection<PdfSearchItem> ByOther1 { get; private set; }
        public ObservableCollection<PdfSearchItem> ByOther2 { get; private set; }
        public ObservableCollection<PdfSearchItem> ByTittle { get; private set; }
        public ObservableCollection<PdfSearchItem> ByYear { get; private set; }

        public bool Remove(PdfFile pdf)
        {
            bool result = false;

            result |= Remove(ByNumber, pdf);
            result |= Remove(ByOther1, pdf);
            result |= Remove(ByOther2, pdf);
            result |= Remove(ByTittle, pdf);
            result |= Remove(ByYear, pdf);

            return result;
        }

        private bool Remove(ObservableCollection<PdfSearchItem> collection, PdfFile pdf)
        {
            var set = collection.Where(n => n.PdfFile == pdf).ToArray();
            foreach (var item in set)
                item.Release();
            return set.Any();
        }

        public void SetByNumber(IList<PdfFile> pdfs)
        {
            SetCollection(ByNumber, pdfs);
        }
        public void SetByOther1(IList<PdfFile> pdfs)
        {
            SetCollection(ByOther1, pdfs);
        }
        public void SetByOther2(IList<PdfFile> pdfs)
        {
            SetCollection(ByOther2, pdfs);
        }
        public void SetByTittle(IList<PdfFile> pdfs)
        {
            SetCollection(ByTittle, pdfs);
        }
        public void SetByYear(IList<PdfFile> pdfs)
        {
            SetCollection(ByYear, pdfs);
        }

        public void SetCollection(ObservableCollection<PdfSearchItem> collection, IList<PdfFile> pdfs)
        {
            foreach (var item in pdfs)
            {
                collection.Add(new PdfSearchItem() { PdfFile = item, Father = collection });
            }
        }
    }
}
