using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfManager.Data
{
    public partial class PdfManageModelContainer
    {
        public void Expert(string fileName)
        {
            using (FileStream stream = File.Create(fileName))
            {
                ZipOutputStream zip = new ZipOutputStream(stream);
                zip.SetLevel(9);

                ZipEntry indexFile = new ZipEntry("index.json");
                zip.PutNextEntry(indexFile);

                using (StreamWriter sw = new StreamWriter(zip))
                {
                    EncodePdfList(sw);
                }
            }
        }

        public void EncodePdfList(StreamWriter sw)
        {
            JsonSerializer js = new JsonSerializer();
            js.NullValueHandling = NullValueHandling.Ignore;
            using (JsonTextWriter writer = new JsonTextWriter(sw) { CloseOutput = false })
            {
                //    foreach (var item in PdfFileSet)
                //    {
                //        js.Serialize(writer, item);
                //    }

                js.Serialize(writer, PdfFileSet);
                writer.Flush();
            }
        }

        public void DecodePdfList(StreamReader sr)
        {
            JsonSerializer js = new JsonSerializer();
            js.NullValueHandling = NullValueHandling.Ignore;
            using (JsonTextReader reader = new JsonTextReader(sr) { CloseInput = false })
            {
                //PdfFile p = null;

                //while (true)
                //{
                //    p = js.Deserialize<PdfFile>(reader);
                //    if (p == null)
                //        continue;
                //    PdfFileSet.Add(p); 
                //}
                PdfFileSet.AddRange(js.Deserialize<List<PdfFile>>(reader));
            }
        }
    }
}
