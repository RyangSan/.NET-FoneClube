using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace VideoUpload.Utilidades
{
    public class LoggerManager
    {
        public static LoggerManager Logger { get; set; }
        public string caminhoLogFile { get; set; }

        public LoggerManager()
        {
            var usuario = HttpContext.Current.Request.LogonUserIdentity.Name.Replace(@"\", "");
            var pastaLog = System.Web.HttpContext.Current.Server.MapPath("~/Log");
            var nomeArquivoLog = string.Format("{0}_{1}.log", DateTime.Now.ToString("yyyyMMdd_HHmm tt"), usuario);
            caminhoLogFile = string.Format("{0}{1}{2}", pastaLog, @"/", nomeArquivoLog);

            using (StreamWriter txtWriter = File.AppendText(caminhoLogFile))
            {
                txtWriter.WriteLine(string.Format("Dado início processo de upload por {0}", usuario));
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("-------------------------------");
            } 
        }

        public void IncluirLog(string logMessage)
        {
            using (StreamWriter txtWriter = File.AppendText(caminhoLogFile))
            {
                txtWriter.WriteLine(string.Format("{0} | {1} | {2}", DateTime.Now.ToString("yyyy.MM.dd HH:mm tt"), HttpContext.Current.Request.LogonUserIdentity.Name.Replace(@"\", ""), logMessage));
                txtWriter.WriteLine("-------------------------------");
            }
        }
    }
}