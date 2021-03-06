﻿using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfManager.Data
{
    public partial class PdfManageModelContainer
    {
        static readonly int BufferSize = 4096;
        public async Task ExpertAsync(string fileName)
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

                    foreach (var item in PdfFileSet)
                    {
                        using (FileStream fs = File.OpenRead(item.GetFullPath()))
                        {
                            ZipEntry file = new ZipEntry(item.FileName);
                            zip.PutNextEntry(file);

                            byte[] buffer = new byte[BufferSize];
                            int re = 0;
                            while ((re = await fs.ReadAsync(buffer, 0, BufferSize)) > 0)
                            {
                                zip.Write(buffer, 0, BufferSize);
                            }
                        }
                    }
                }
            }
        }
        public async Task ImportAsync(string fileName, Func<bool, PdfFile> conflictHandle = null)
        {
            using (FileStream stream = File.OpenRead(fileName))
            {
                ZipInputStream zip = new ZipInputStream(stream);

                ZipEntry next;
                while ((next = zip.GetNextEntry()) != null)
                {
                    if (next.Name == "index.json")
                    {
                        DecodePdfList(new StreamReader(zip), conflictHandle);
                        continue;
                    }
                    else if (Path.GetExtension(next.Name.ToLower()) != ".pdf")
                    {
                        continue;
                    }

                    using (FileStream fs = File.Create(
                        Path.Combine(PdfFile.StorePath, next.Name)))
                    {
                        byte[] buffer = new byte[BufferSize];
                        int re = 0;
                        while ((re = zip.Read(buffer, 0, BufferSize)) > 0)
                        {
                            await fs.WriteAsync(buffer, 0, BufferSize);
                            Trace.WriteLine(next.Name, nameof(ImportAsync));
                        }
                    }
                }
            }
        }

        public void EncodePdfList(StreamWriter sw)
        {
            JsonSerializer js = new JsonSerializer();
            js.NullValueHandling = NullValueHandling.Ignore;
            using (JsonTextWriter writer = new JsonTextWriter(sw) { CloseOutput = false })
            {
                js.Serialize(writer, PdfFileSet);
                writer.Flush();
            }
        }

        public void DecodePdfList(StreamReader sr, Func<bool, PdfFile> conflictHandle = null)
        {
            JsonSerializer js = new JsonSerializer();
            js.NullValueHandling = NullValueHandling.Ignore;
            using (JsonTextReader reader = new JsonTextReader(sr) { CloseInput = false })
            {
                //var pdfs = js.Deserialize<IEnumerable<PdfFile>>(reader);
                //foreach (var item in pdfs)
                //{
                //    if(PdfFileSet.Any())
                //}
                PdfFileSet.AddRange(js.Deserialize<List<PdfFile>>(reader));
            }
        }
    }
}
