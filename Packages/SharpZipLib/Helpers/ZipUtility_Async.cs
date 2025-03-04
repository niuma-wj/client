using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using UnityEngine;

namespace SharpZipLib.Unity.Helpers
{
    public static partial class ZipUtility
    {
        public async static Task UncompressZipAsync(string archivePath, string password, string outFolder, int parallel = 8)
        {
            if (Directory.Exists(outFolder))
            {
                Directory.Delete(outFolder, true);
            }

            Directory.CreateDirectory(outFolder);

            using (Stream fs = File.OpenRead(archivePath))
            {
                using (ZipFile zf = new ZipFile(fs))
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        // AES encrypted entries are handled automatically
                        zf.Password = password;
                    }

                    List<ZipEntry> entries = new List<ZipEntry>();
                    foreach (ZipEntry zipEntry in zf)
                    {
                        entries.Add(zipEntry);
                    }
                    
                    await GG.Extensions.AwaitExtensions.ForEachAsync(entries, parallel, async (zipEntry, i) =>
                    {
                        try
                        {
                            if (!zipEntry.IsFile)
                            {
                                // Ignore directories
                                await Task.CompletedTask;
                            }
                            else
                            {
                                Debug.Log(zipEntry.Name);
                                string entryFileName = zipEntry.Name;
                                // to remove the folder from the entry:
                                //entryFileName = Path.GetFileName(entryFileName);
                                // Optionally match entrynames against a selection list here
                                // to skip as desired.
                                // The unpacked length is available in the zipEntry.Size property.

                                // Manipulate the output filename here as desired.
                                string fullZipToPath = Path.Combine(outFolder, entryFileName);
                                string directoryName = Path.GetDirectoryName(fullZipToPath);
                                if (directoryName.Length > 0)
                                {
                                    Directory.CreateDirectory(directoryName);
                                }

                                // 4K is optimum
                                byte[] buffer = new byte[4096];

                                // Unzip file in buffered chunks. This is just as fast as unpacking
                                // to a buffer the full size of the file, but does not waste memory.
                                // The "using" will close the stream even if an exception occurs.
                                using (Stream zipStream = zf.GetInputStream(zipEntry))
                                {
                                    using (Stream fsOutput = File.Create(fullZipToPath))
                                    {
                                        StreamUtils.Copy(zipStream, fsOutput, buffer);
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                    });
                }
            }
        }
    }
}