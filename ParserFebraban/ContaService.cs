
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoneClube.DataAccess;
using Febraban;

namespace FoneClube.ParserFebraban
{
    public class ContaService
    {
        public bool ProcessarConta(byte[] fileBytes)
        {

            switch (GetTipoConta(fileBytes))
            {
                case ETipoConta.Nenhum:
                    return false;
                case ETipoConta.FebrabanVivo:
                    return ProcessarFebrabanVivo(fileBytes);
                case ETipoConta.Claro:
                    return ProcessarClaro(fileBytes);
                default:
                    return false;
            }

        }

        private bool ProcessarFebrabanVivo(byte[] fileBytes)
        {
            var conta = new FebrabanParser().ParserStreamReader(GetStream(fileBytes, Encoding.UTF8));
            return new ContaAcesso().SaveConta(conta);

        }

        private bool ProcessarClaro(byte[] fileBytes)
        {
            var conta = new ClaroParser().ParserStreamReader(GetStream(fileBytes, Encoding.GetEncoding(1252)));
            return new ClaroContaAcesso().SaveConta(conta);
        }


        private StreamReader GetStream(byte[] fileBytes, Encoding encoding)
        {
            return new StreamReader(new MemoryStream(fileBytes), encoding);
        }


        private ETipoConta GetTipoConta(byte[] fileBytes)
        {
            string line = GetFirstLine(fileBytes);

            if (line.Contains("SOB MEDIDA RJ"))
            {
                return ETipoConta.Claro;
            }
            else if (line.Contains("VIVO"))
            {
                return ETipoConta.FebrabanVivo;
            }
            else
            {
                return ETipoConta.Nenhum;
            }

        }

        private string GetFirstLine(byte[] fileBytes)
        {
            var reader = new StreamReader(new MemoryStream(fileBytes), Encoding.UTF8);
            string objectLine = reader.ReadLine();
            return objectLine;

        }

        private enum ETipoConta
        {
            Nenhum,
            FebrabanVivo,
            Claro
        }



    }
}
