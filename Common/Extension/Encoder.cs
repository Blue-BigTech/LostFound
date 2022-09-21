using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Abstract.Extension
{
    public static class Extensions
    {
        public static string Encode(this object text)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(text)))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(Convert.ToString(text));
                var ms = new MemoryStream();
                using (var stream = new GZipStream(ms, CompressionMode.Compress, true))
                    stream.Write(buffer, 0, buffer.Length);

                ms.Position = 0;

                var rawData = new byte[ms.Length];
                ms.Read(rawData, 0, rawData.Length);

                var compressedData = new byte[rawData.Length + 4];
                Buffer.BlockCopy(rawData, 0, compressedData, 4, rawData.Length);
                Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, compressedData, 0, 4);
                string convertedData = Convert.ToBase64String(compressedData);
                convertedData = convertedData.Replace("+", "_@_");
                convertedData = convertedData.Replace("=", "_!_");
                convertedData = convertedData.Replace("/", "_~_");
                return convertedData;
            }
            else
                return string.Empty;
        }
        public static string Decode(this string compressedText)
        {
            if (!string.IsNullOrEmpty(compressedText))
            {
                compressedText = compressedText.Replace("_@_", "+");
                compressedText = compressedText.Replace("_!_", "=");
                compressedText = compressedText.Replace("_~_", "/");
                byte[] compressedData = Convert.FromBase64String(compressedText);
                using var ms = new MemoryStream();
                int dataLength = BitConverter.ToInt32(compressedData, 0);
                ms.Write(compressedData, 4, compressedData.Length - 4);

                var buffer = new byte[dataLength];

                ms.Position = 0;
                using (var stream = new GZipStream(ms, CompressionMode.Decompress))
                {
                    stream.Read(buffer, 0, buffer.Length);
                }
                return Encoding.UTF8.GetString(buffer);
            }
            else
                return string.Empty;
        }
    }
   
}
