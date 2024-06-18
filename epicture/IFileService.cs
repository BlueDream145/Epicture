using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace epicture
{
    public interface IFileService
    {
        void SavePicture(string name, Stream data, string location = "temp");
    }
}
