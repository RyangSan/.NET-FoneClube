using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Configuration;

namespace VideoUpload.Utilidades
{
    public class Util
    {
        public static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

     
        public static byte[] ObterByteArray(HttpPostedFileBase arquivo)
        {
            using (var binaryReader = new BinaryReader(arquivo.InputStream))
            {
                return binaryReader.ReadBytes(arquivo.ContentLength);
            }
        }

        public static string SerializeJson(object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return json;
        }

        public static T DeserializeJson<T>(string valor)
        {
            var objetodeSerializado = JsonConvert.DeserializeObject<T>(valor);
            return objetodeSerializado;
        }
    }
}