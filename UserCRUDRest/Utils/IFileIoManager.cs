using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserCRUDRest.Utils
{
    public interface IFileIoManager
    {
        string GetFullPathFromFileInAssemblyDirectory(string path, string fileName);
        string ReadEmbeddedResource(string fullPath);
        void WriteTextFileToAssemblyDirectory(string path, string fileName, string content);
    }
}