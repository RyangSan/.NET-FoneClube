using FoneClube.Business.Commons.Entities.Claro;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FoneClube.ParserFebraban
{
    public class ClaroParser
    {

        public ClaroConta Parse(string pathFile)
        {
            try
            {
                using (StreamReader file = new StreamReader(pathFile , Encoding.GetEncoding(1252) ))
                {
                   
                    string objectLine;
                    var lineList = new List<string>();

                    while ((objectLine = file.ReadLine()) != null)
                        lineList.Add(objectLine);

                    if (!IsContaClaro(lineList[0]))
                        return new ClaroConta();


                    var listaRegistros = BuldRegisterList(lineList);

                    var conta = new ClaroConta();

                    conta.DataCadastro = DateTime.Now;

                    
                    conta.IdCliente = BuildId(listaRegistros.Identificacao[0]);
                    conta.Nome = BuildNome(listaRegistros.Identificacao[0]);
                    conta.Endereco = BuildEndereco(listaRegistros.Identificacao);
                    conta.NumeroCliente = BuildNumeroCliente(listaRegistros.Identificacao.Last());

                    string[] vigencia = BuildDatas(listaRegistros.ResumoNotasList.First());
                    conta.DataReferenciaInicio = vigencia[0];
                    conta.DataReferenciaFim = vigencia[1];

                    conta.AnoMesCompetencia = BuildCompetencia(vigencia[1]);

                    conta.IdUnico = BuildIdUnico(conta.AnoMesCompetencia, conta.IdCliente);
                   
                                       

                    conta.DataVencimento = BuildDatas(listaRegistros.ResumoNotasList[1]).FirstOrDefault();
                                        
                    conta.Valor = BuildValor(listaRegistros.ResumoNotasList[2]);
                    conta.Notas = BuildNotas(listaRegistros.ResumoNotasList);

                    conta.IdReferenciaDebitoAutomatico = BuildIdDebitoAutomatico(listaRegistros.IdentificacaoDebitoAutomatico.FirstOrDefault());
                    conta.LinhasRegistro = BuildRegistros(listaRegistros.DetalhesList);


                    return conta;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("O Arquivo não pode ser lido.");
                Console.WriteLine(e.Message);
                return new ClaroConta();
            }
        }


        public ClaroConta ParserStreamReader(StreamReader file)
        {
            try
            {

                string objectLine;
                var lineList = new List<string>();

                while ((objectLine = file.ReadLine()) != null)
                    lineList.Add(objectLine);

                if (!IsContaClaro(lineList[0]))
                    return new ClaroConta();


                var listaRegistros = BuldRegisterList(lineList);

                var conta = new ClaroConta();

                conta.DataCadastro = DateTime.Now;


                conta.IdCliente = BuildId(listaRegistros.Identificacao[0]);
                conta.Nome = BuildNome(listaRegistros.Identificacao[0]);
                conta.Endereco = BuildEndereco(listaRegistros.Identificacao);
                conta.NumeroCliente = BuildNumeroCliente(listaRegistros.Identificacao.Last());

                string[] vigencia = BuildDatas(listaRegistros.ResumoNotasList.First());
                conta.DataReferenciaInicio = vigencia[0];
                conta.DataReferenciaFim = vigencia[1];

                conta.AnoMesCompetencia = BuildCompetencia(vigencia[1]);

                conta.IdUnico = BuildIdUnico(conta.AnoMesCompetencia, conta.IdCliente);



                conta.DataVencimento = BuildDatas(listaRegistros.ResumoNotasList[1]).FirstOrDefault();

                conta.Valor = BuildValor(listaRegistros.ResumoNotasList[2]);
                conta.Notas = BuildNotas(listaRegistros.ResumoNotasList);

                conta.IdReferenciaDebitoAutomatico = BuildIdDebitoAutomatico(listaRegistros.IdentificacaoDebitoAutomatico.FirstOrDefault());
                conta.LinhasRegistro = BuildRegistros(listaRegistros.DetalhesList);


                return conta;


            }
            catch (Exception e)
            {
                Console.WriteLine("O Arquivo não pode ser lido.");
                Console.WriteLine(e.Message);
                return new ClaroConta();
            }
        }



        private bool IsContaClaro(string line)
        {
            return ClearString(line).StartsWith("SOB MEDIDA RJ");          
        }

        private ListasRegistros BuldRegisterList(List<string> lineList)
        {

            var listaRegistros = new ListasRegistros
            {
                Identificacao = new List<string>(),
                ResumoNotasList = new List<string>(),
                IdentificacaoDebitoAutomatico = new List<string>(),
                DetalhesList = new List<string>()
            };




            EClaroContaRegions region = EClaroContaRegions.Identificacao;

            foreach (var item in lineList)
            {
                
                if (!string.IsNullOrWhiteSpace(item))
                {
               
                    switch (region)
                    {
                        case EClaroContaRegions.Identificacao:
                            listaRegistros.Identificacao.Add(ClearString(item));
                            break;
                        case EClaroContaRegions.ResumoNotas:
                            listaRegistros.ResumoNotasList.Add(ClearString(item));
                            break;
                        case EClaroContaRegions.DebitoAutomatico:
                            listaRegistros.IdentificacaoDebitoAutomatico.Add(ClearString(item));
                            break;
                        case EClaroContaRegions.Detalhes:
                            listaRegistros.DetalhesList.Add(item.Replace("\"", ""));
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    region++;
                    if (region == EClaroContaRegions.Final) break;
                }
            }

            return listaRegistros;


        }

      

        private string BuildId(string line)
        {
            int startIndex = line.IndexOf('-');
            int lenght = line.Length - startIndex;
            var id = line.Substring(startIndex, lenght).Trim();
            return GetNumeros(id);
        }

        private string BuildNome(string line)
        {
            int finalIndex = line.IndexOf('-');
            var nome = line.Substring(0, finalIndex).Trim();
            return nome;
        }

        private string BuildEndereco(List<string> linhasEndereco)
        {
            return string.Join(" - ", linhasEndereco.Skip(1).Take(linhasEndereco.Count - 2));
        }

        private string BuildIdUnico(string comp, string clientId)
        {
            return comp + clientId;
        }

        private string BuildCompetencia(string comp)
        {
            return GetDataYYYYMM(comp);
        }

        private string[] BuildDatas(string linhaVigencia)
        {
            return GetDatas(linhaVigencia);
        }

        private string BuildNumeroCliente(string line)
        {
            return GetNumeros(line);
        }

        private string BuildValor(string line)
        {
            return GetValor(line);
        }

        private List<RegistroNotaFiscal> BuildNotas(List<string> lineList)
        {
            var notasAgrupadas = lineList.Where(x => x.StartsWith("Nota"))
                                         .Select(x => new { indice = GetIndiceNota(x),
                                                            linha = x
                                                            }).ToList();


            var notas = new List<RegistroNotaFiscal>();

            foreach (var item in notasAgrupadas)
            {
                RegistroNotaFiscal notaFiscal = new RegistroNotaFiscal();

                notaFiscal.NotaFiscal = GetNomeNota(item.linha);
                notaFiscal.Codigo = GetNotaCodigoCompleto(item.linha, item.indice);
               
                var conteudo = item.linha.Substring(item.indice, item.linha.Length - item.indice);
                               
                notaFiscal.Aliquota = CheckString(GetValoresNotas(conteudo), 0);
                notaFiscal.BaseCalculo = CheckString(GetValoresNotas(conteudo), 1);
                notaFiscal.ValorImposto = CheckString(GetValoresNotas(conteudo), 2);
                notaFiscal.Tipo = CheckString(GetValoresNotas(conteudo), 3);

                notas.Add(notaFiscal);

            }

            return notas;
        }

        private string GetNotaCodigoCompleto(string linha, int index)
        {
            int indexI = (linha.IndexOf(":")+1) ;
            int lenght = index - indexI;
            return linha.Substring(indexI, lenght);
        }

        private int GetIndiceNota(string linha)
        {
            int index = linha.IndexOf(":");
            var subCodigo = linha.Substring(index, linha.Length - index);
            int indexCodigo = subCodigo.IndexOf(" ") + index;
            return indexCodigo;
            
        }

        private string BuildIdDebitoAutomatico(string line)
        {
            return GetNumeros(line);
        }

        private List<LinhaRegistro> BuildRegistros(List<string> lineList)
        {
            char[] separators = { ';', '\t' };
            List<LinhaRegistro> lista = new List<LinhaRegistro>();

            foreach (var item in lineList.Skip(1))
            {
                string[] line = item.Split(separators);
                var lenght = line.Length;

                LinhaRegistro linha = new LinhaRegistro();


                if (!string.IsNullOrEmpty(line[0].Trim()))
                {
                    var numero = GetNumeros(line[0]);
                    linha.DDD = numero.Substring(0, 2);
                    linha.Telefone = numero.Substring(2, numero.Length - 2);
                }
                else
                {
                    linha.DDD = string.Empty;
                    linha.Telefone = string.Empty;
                }

                linha.Secao =                            CheckString(line, 1);
                linha.Data =                             CheckString(line, 2);
                linha.Hora =                             CheckString(line, 3);
                linha.OrigemUFDestino =                  CheckString(line, 4);
                linha.Numero =                           CheckString(line, 5);
                linha.DuracaoQuantidade =                CheckString(line, 6);
                linha.Tarifa =                           CheckString(line, 7);
                linha.Valor =                   GetValor(CheckString(line, 8));
                linha.ValorCobrado =            GetValor(CheckString(line, 9));
                linha.Nome =                             CheckString(line, 10);
                linha.CC =                               CheckString(line, 11);
                linha.Matricula =                        CheckString(line, 12);
                linha.SubSecao =                         CheckString(line, 13);
                linha.TipoImposto =                      CheckString(line, 14);
                linha.Descricao =                        CheckString(line, 15);
                linha.Cargo =                            CheckString(line, 16);
                linha.NomeLocalOrigem =                  CheckString(line, 17);
                linha.NomeLocalDestino =                 CheckString(line, 18);
                linha.CodigoLocalOrigem =                CheckString(line, 19);
                linha.CodigoLocalDestino =               CheckString(line, 20);


                lista.Add(linha);

            }

            return lista;
        }

        private string CheckString(string[] line, int index)
        {
            return line.Length > index ? line[index].Trim() : string.Empty;
        }

        private string ClearString(string line)
        {
            return line.Replace("\"", "").Replace("\t", "");
        }



        #region Regex

        public string GetNumeros(string linha)
        {
            return Regex.Replace(linha, @"[\D]+" , "" ).ToString();   
        }

        public string GetValor(string linha)
        {
            var valor = Regex.Match(linha, @"[\d|\.|\,]+").Value;
            if (valor.StartsWith(","))
            {
                valor = "0" + valor;
            }
            return valor;
        }

        public string[] GetDatas(string linha)
        {
           
            return Regex.Matches(linha, @"[\d/]+")
                           .Cast<Match>()
                           .Select(m => m.Value)
                           .ToArray();
        }

        public string[] GetValoresNotas(string nota)
        {

           var tipo = nota.Split(':')[0];

           var valores = Regex.Matches(nota, @"[\d|\,|\.]+").Cast<Match>()
                              .Select(m => m.Value)
                              .ToList();
            valores.Add(tipo);
            return valores.ToArray();
           
        }

        public string GetNomeNota(string conteudo)
        {
            return conteudo.Substring(0, conteudo.IndexOf(':')).Replace("Nota Fiscal", string.Empty).Trim();
        }

        

        public string GetDataYYYYMM (string data)
        {
            DateTime dt = DateTime.ParseExact(data, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            return  dt.ToString("yyyyMM");
        }

        #endregion
    }
}
