using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;

namespace UserCRUDRest.Utils
{

    public class FileIoManager : IFileIoManager
    {

        private static readonly Encoding ENCODING = Encoding.UTF8;

        public Assembly CurrentAssembly { get; set; }

        public string AssemblyDirectory
        {
            get
            {
                return Path.GetDirectoryName(CurrentAssembly.Location);
            }
        }

        public string GetFullPathFromFileInAssemblyDirectory(string path, string name)
        {
            if (this.ExistsFileInAssemblyDirectory(path, name))
            {
                return Path.Combine(this.AssemblyDirectory, Path.Combine(path, name));
            }
            return string.Empty;
        }

        public string ReadEmbeddedResource(string fullPath)
        {
            string result;

            using (Stream resourceStream = CurrentAssembly.GetManifestResourceStream(fullPath))
            {
                using (var resourceStreamReader = new StreamReader(resourceStream ?? new MemoryStream()))
                {
                    result = resourceStreamReader.ReadToEnd();
                }
            }

            return result;
        }

        public void WriteTextFileToAssemblyDirectory(string path, string fileName, string content)
        {
            string fullPath = Path.Combine(this.AssemblyDirectory, Path.Combine(path, fileName));

            new FileInfo(fullPath).Directory.Create();
            File.WriteAllText(fullPath, content, ENCODING);
        }

        private bool ExistsFileInAssemblyDirectory(string path, string fileName)
        {
            string fullPath = Path.Combine(this.AssemblyDirectory, Path.Combine(path, fileName));
            return File.Exists(fullPath);
        }
    }
}