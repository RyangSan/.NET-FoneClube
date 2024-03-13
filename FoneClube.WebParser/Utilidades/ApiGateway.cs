using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net;
using System.Text;

namespace VideoUpload.Utilidades
{
    public class ApiGateway
    {
        public enum TipoRetorno { Xml, Json };
        private static string _url = Constants.apiAdress;

        private static string GetConteudo(string caminho)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Concat(_url, caminho));
                WebResponse response = request.GetResponse();
                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string SetConteudo(string caminho, string postData)
        {
            try
            {
                var request = WebRequest.Create(string.Concat(_url, caminho));
                request.Method = "POST";
                var byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "application/json";
                request.ContentLength = byteArray.Length;

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(postData);
                }

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    return responseText;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public class VideoUpload
        {
            private TipoRetorno tipo;
            public VideoUpload(TipoRetorno tipoRetorno)
            {
                tipo = tipoRetorno;
            }

            public string GetGuidQuestao(int questaoId)
            {
                return GetConteudo(string.Format(@"json/Concurso/Questao/{0}/Video/Guid", questaoId.ToString()));
            }

            public string SetVideoQuestao(string jsonPost) {
                return SetConteudo(@"json/Concurso/Questao/Video/Inserir/", jsonPost);
            }

            public string SendMail(string jsonPost)
            {
                return SetConteudo(@"json/Utilidades/EmailDireto/Enviar/", jsonPost);
            }

        }

    }
}