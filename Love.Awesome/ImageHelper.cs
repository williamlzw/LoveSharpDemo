using Love;
using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text.Json;


namespace Love.Awesome
{
    public static class ImageHelper
    {
        public static Image GetImageFromBytes(byte[] bytes)
        {
            FileData fileData = Love.FileSystem.NewFileData(bytes, string.Empty);
            ImageData data = Love.Image.NewImageData(fileData);
            return Love.Graphics.NewImage(data);
        }

        static public byte[] BitmapToByte(System.Drawing.Bitmap Bitmap)
        {
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream();
                Bitmap.Save(ms, Bitmap.RawFormat);
                byte[] byteImage = new Byte[ms.Length];
                byteImage = ms.ToArray();
                return byteImage;
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            finally
            {
                ms.Close();
            }
        }
    }


    //序列化特性才能将字典的byte[]序列化
    [Serializable]
    public class ImagePackage
    {
        private Dictionary<string, byte[]> _dic = new Dictionary<string, byte[]>();

        public ImagePackage() { }

        public void AddImage(string name, byte[] data)
        {
            try
            {
                _dic[name] = data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public byte[] GetImage(string name)
        {
            byte[] ret = null;
            try
            {
                ret = _dic[name];
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return ret;
        }

        //public static ImagePackage LoadPackage(string path)
        //{
        //    return Love.Resource.LoadData<ImagePackage>(path);
        //}

        //public static void SavePackage(string path, ImagePackage obj)
        //{
        //    Love.Resource.SaveData(path, obj);
        //}

        public static ImagePackage LoadPackage(string path)
        {
            MemoryStream memorystreamd = new MemoryStream(FileSystem.Read(path));
            DataContractSerializer dcs = new DataContractSerializer(typeof(ImagePackage));
            return (ImagePackage)dcs.ReadObject(memorystreamd);
        }

        public static void SavePackage(string path, ImagePackage obj)
        {
            MemoryStream memorystream = new MemoryStream();
            DataContractSerializer dcs = new DataContractSerializer(typeof(ImagePackage));
            dcs.WriteObject(memorystream, obj);
            FileSystem.Write(path, memorystream.ToArray());
        }


    }

}
