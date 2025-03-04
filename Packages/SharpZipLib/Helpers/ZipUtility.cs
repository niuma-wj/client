#region

using System.IO;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

#endregion

namespace SharpZipLib.Unity.Helpers
{
    public static partial class ZipUtility
    { 
        public static void UncompressZip(string archivePath, string password, string outFolder)
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

                    foreach (ZipEntry zipEntry in zf)
                    {
                        if (!zipEntry.IsFile)
                        {
                            // Ignore directories
                            continue;
                        }

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
            }
        }

        /// <summary>
        ///  Compresses the files in the nominated folder, and creates a zip file 
        ///  on disk named as outPathname
        /// </summary>
        /// <param name="outPathname">The path of the created zip file</param>
        /// <param name="password">The password required to open the zip file. Set to null if not required.</param>
        /// <param name="folderName">The folder to be compressed</param>
        public static void CompressZip(string outPathname, string password, string folderName)
        {
            using (FileStream fsOut = File.Create(outPathname))
            {
                using (ZipOutputStream zipStream = new ZipOutputStream(fsOut))
                {
                    //0-9, 9 being the highest level of compression
                    zipStream.SetLevel(3);

                    // optional. Null is the same as not setting. Required if using AES.
                    zipStream.Password = password;

                    // This setting will strip the leading part of the folder path in the entries, 
                    // to make the entries relative to the starting folder.
                    // To include the full path for each entry up to the drive root, assign to 0.
                    int folderOffset = folderName.Length + (folderName.EndsWith("\\") ? 0 : 1);

                    CompressFolder(folderName, zipStream, folderOffset);
                }
            }
        }

        // Recursively compresses a folder structure
        static void CompressFolder(string path, ZipOutputStream zipStream, int folderOffset)
        {
            string[] files = Directory.GetFiles(path);

            foreach (string filename in files)
            {
                FileInfo fi = new FileInfo(filename);

                // Make the name in zip based on the folder
                string entryName = filename.Substring(folderOffset);

                // Remove drive from name and fixe slash direction
                entryName = ZipEntry.CleanName(entryName);

                ZipEntry newEntry = new ZipEntry(entryName);

                // Note the zip format stores 2 second granularity
                newEntry.DateTime = fi.LastWriteTime;

                // Specifying the AESKeySize triggers AES encryption. 
                // Allowable values are 0 (off), 128 or 256.
                // A password on the ZipOutputStream is required if using AES.
                //   newEntry.AESKeySize = 256;

                // To permit the zip to be unpacked by built-in extractor in WinXP and Server2003,
                // WinZip 8, Java, and other older code, you need to do one of the following: 
                // Specify UseZip64.Off, or set the Size.
                // If the file may be bigger than 4GB, or you do not need WinXP built-in compatibility, 
                // you do not need either, but the zip will be in Zip64 format which
                // not all utilities can understand.
                //   zipStream.UseZip64 = UseZip64.Off;
                newEntry.Size = fi.Length;

                zipStream.PutNextEntry(newEntry);

                // Zip the file in buffered chunks
                // the "using" will close the stream even if an exception occurs
                byte[] buffer = new byte[4096];
                using (FileStream fsInput = File.OpenRead(filename))
                {
                    StreamUtils.Copy(fsInput, zipStream, buffer);
                }

                zipStream.CloseEntry();
            }

            // Recursively call CompressFolder on all folders in path
            string[] folders = Directory.GetDirectories(path);
            foreach (string folder in folders)
            {
                CompressFolder(folder, zipStream, folderOffset);
            }
        }
    }
}