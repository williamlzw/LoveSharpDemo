using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;


namespace Love.Awesome
{
    public class ClassZipResource
    {
        private ZipFile _zipFile;

        public ClassZipResource(string filePath, bool bEncrypt = false, string password = "")
        {
            _zipFile = new ZipFile(filePath, StringCodec.Default);
            if(bEncrypt)
            {
                _zipFile.Password = password;
            }
        }

        public ImageData ReadData(string fileName)
        {
            var ze = _zipFile.GetEntry(fileName);
            var bytes = new byte[ze.Size];
            var s = _zipFile.GetInputStream(ze);
            s.Read(bytes, 0, bytes.Length);
            var fileData = Love.FileSystem.NewFileData(bytes, fileName);
            var img = Image.NewImageData(fileData);
            return img;
        }
    }
}
