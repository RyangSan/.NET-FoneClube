using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections;
using Business.Commons;
using Business.Commons.Entities;
using Febraban;
using FoneClube.DataAccess;
using System.Linq;
using System.Text;
using FoneClube.Business.Commons.Entities;
using FoneClube.ParserFebraban;
using FoneClube.PagarmeStandard;
using PagarMe;

namespace FixedStringParser.Test
{
    [TestClass]
    public class ParserTests
    {

        [TestMethod]
        public void CanAcessDB()
        {
            Assert.IsTrue(new ContaAcesso().ValidaConexaoDB());
        }

        [TestMethod]
        public void ParseClaroV2_popula()
        {
            //1 claro
            //2 vivo
            var fileV2 = @"E:\Projects\FoneClub\FoneClube.NET\ParserFebraban\v2\abril\claro.txt"; //--100387217
            var conta = new FebrabanParser().Parse(fileV2);
            conta.TipoOperadora = 1;
            var salvaDB = new ContaAcesso().SaveConta(conta);

            Assert.IsTrue(salvaDB);

        }

        [TestMethod]
        public void ParseVivoV2_popula()
        {
            //1 claro
            //2 vivo
            var fileV2 = @"E:\Projects\FoneClub\FoneClube.NET\ParserFebraban\v2\abril\vivo_3mg.txt";
            var conta = new FebrabanParser().Parse(fileV2);
            conta.TipoOperadora = 2;
            var salvaDB = new ContaAcesso().SaveConta(conta);

            Assert.IsTrue(salvaDB);

        }

        [TestMethod]
        public void ParseClaroV2() {
            var fileV2 = @"C:\GitProjects\FoneClube.NET\ParserFebraban\v2\abril\claro.txt";
            var conta = new FebrabanParser().Parse(fileV2);
            
        }

        [TestMethod]
        public void ParseVivoV2()
        {
            var fileV2 = @"C:\GitProjects\FoneClube.NET\ParserFebraban\v2\VIVO_FEBRABAN_RJ_000000294335512_022017.txt";
            var conta = new FebrabanParser().Parse(fileV2);

        }

        [TestMethod]
        public void ReadFile()
        {
            try
            {
                var fileV2 = @"C:\GitProjects\FoneClube.NET\ParserFebraban\v2\939144068_100387217_2017_02_03_Febraban.txt";
                //var fileV2 = @"C:\GitProjects\outros\FoneClubeFiles\v2\VIVO_FEBRABAN_RJ_000000294335512_022017.txt";

                using (StreamReader file = new StreamReader(fileV2))
                {

                    string objectLine;
                    var lineList = new List<string>();

                    while ((objectLine = file.ReadLine()) != null)
                        lineList.Add(objectLine);


                    var listaRegistros = BuildRegisterList(lineList);

                    var conta = new Conta
                    {
                        Head = BuildHead(listaRegistros.HeadList),
                        Resumos = BuildResumos(listaRegistros.ResumoList),
                        Enderecos = BuildEndereco(listaRegistros.EnderecoList),
                        Bilhetacoes = BuildBilhetacoes(listaRegistros.BilhetacaoList),
                        Servicos = BuildServicos(listaRegistros.ServicosList),
                        Descontos = BuildDescontos(listaRegistros.DescontosList),
                        Totalizadores = BuildTotalizador(listaRegistros.TotalizadorFinalList)

                    };

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("O Arquivo não pode ser lido.");
                Console.WriteLine(e.Message);
            }
        }

        private List<Endereco> BuildEndereco(List<string> enderecoList)
        {
            var enderecos = new List<Endereco>();
            foreach (var endereco in enderecoList)
            {
                enderecos.Add(new Endereco
                {
                    ControleSequencial = endereco.Substring(1,12).Trim(),
                    IDUnicoNRC = endereco.Substring(13,25).Trim(),
                    DDD = endereco.Substring(38,2).Trim(),
                    NumeroTelefone = endereco.Substring(40, 10).Trim(),
                    CaracteristicaRecurso = endereco.Substring(50,15).Trim(),
                    CNLRecursoEnderecoPontaA = endereco.Substring(65,5).Trim(),
                    NomeLocalidadePontaA = endereco.Substring(70,20).Trim(),
                    UFLocalidadePontaA = endereco.Substring(90,2).Trim(),
                    EnderecoPontaA = endereco.Substring(92, 30).Trim(),
                    NumeroEnderecoPontaA = endereco.Substring(122, 5).Trim(),
                    ComplementoPontaA = endereco.Substring(127, 10).Trim(),
                    BairroPontaA = endereco.Substring(137,20).Trim(),
                    CNLRecursoEnderecoPontaB = endereco.Substring(157, 5).Trim(),
                    NomeLocalidadePontaB = endereco.Substring(162, 20).Trim(),
                    UFLocalidadePontaB = endereco.Substring(182, 2).Trim(),
                    EnderecoPontaB = endereco.Substring(184,30).Trim(),
                    NumeroEnderecoPontaB = endereco.Substring(214, 5).Trim(),
                    ComplementoPontaB = endereco.Substring(219, 10).Trim(),
                    BairroPontaB = endereco.Substring(229, 20).Trim(),
                    Filler = endereco.Substring(239,101).Trim()

                });
            }


            return enderecos;
        }

        public Totalizador BuildTotalizador(List<string> totalizadorFinalList)
        {
            var totalizadores = new List<Totalizador>();
            foreach (var totalizador in totalizadorFinalList)
            {
                return new Totalizador
                {
                    ControleSequencial = totalizador.Substring(1, 12).Trim(),
                    CodigoCliente = totalizador.Substring(13, 15).Trim(),
                    ContaUnicaID = totalizador.Substring(28, 25).Trim(),
                    Vencimento = totalizador.Substring(53, 8).Trim(),
                    Emissao = totalizador.Substring(61, 8).Trim(),
                    QuantidadeRegistros = totalizador.Substring(69, 12).Trim(),
                    QuantidadeLinhas = totalizador.Substring(81, 12).Trim(),
                    SinalTotal = totalizador.Substring(93, 1).Trim(),
                    ValorTotal = totalizador.Substring(94, 13).Trim(),
                    Filler = totalizador.Substring(107, 243).Trim()
                };
            }
            return new Totalizador();
        }

        public List<Desconto> BuildDescontos(List<string> descontosList)
        {
            var descontos = new List<Desconto>();

            foreach (var desconto in descontosList)
            {
                descontos.Add(new Desconto
                {
                    ControleSequencial = desconto.Substring(1, 12).Trim(),
                    Vencimento = desconto.Substring(13, 8).Trim(),
                    Emissao = desconto.Substring(21, 8).Trim(),
                    IDUnicoNRC = desconto.Substring(29, 25).Trim(),
                    ContaUnicaID = desconto.Substring(54, 25).Trim(),
                    RecursoCNL = desconto.Substring(79, 5).Trim(),
                    DDD = desconto.Substring(84, 2).Trim(),
                    NumeroTelefone = desconto.Substring(86, 10).Trim(),
                    GrupoCategoria = desconto.Substring(96, 3).Trim(),
                    DescricaoGrupoCategoria = desconto.Substring(99, 80).Trim(),
                    SinalValorLigacao = desconto.Substring(179, 1).Trim(),
                    BaseCalculoDesconto = desconto.Substring(180, 13).Trim(),
                    PercentualDesconto = desconto.Substring(193, 5).Trim(),
                    ValorLigacao = desconto.Substring(198, 13).Trim(),
                    DataInicioAcerto = desconto.Substring(111, 8).Trim(),
                    HoraInicioAcerto = desconto.Substring(119, 6).Trim(),
                    DataFimAcerto = desconto.Substring(225, 8).Trim(),
                    HoraFimAcerto = desconto.Substring(233, 6).Trim(),
                    ClasseServico = desconto.Substring(239, 5).Trim(),
                    Filler = desconto.Substring(244, 106).Trim()
                });
            }

            return descontos;
        }

        public List<Servico> BuildServicos(List<string> servicosList)
        {
            var servicos = new List<Servico>();
            foreach (var servico in servicosList)
            {
                servicos.Add(new Servico
                {
                    ControleSequencial = servico.Substring(1, 12).Trim(),
                    Vencimento = servico.Substring(13, 8).Trim(),
                    Emissao = servico.Substring(21, 8).Trim(),
                    IDUnicoNRC = servico.Substring(29, 25).Trim(),
                    RecursoCNL = servico.Substring(54, 5).Trim(),
                    DDD = servico.Substring(59, 2).Trim(),
                    NumeroTelefone = servico.Substring(61, 10).Trim(),
                    CaracteristicaRecurso = servico.Substring(71, 15).Trim(),
                    DataServico = servico.Substring(86, 8).Trim(),
                    CNLLocalidadeChamada = servico.Substring(94, 5).Trim(),
                    NomeLocalidadeChamada = servico.Substring(99, 25).Trim(),
                    UFTelefoneChamado = servico.Substring(124, 2).Trim(),
                    CODNacionalInternacional = servico.Substring(126, 2).Trim(),
                    CODOperadora = servico.Substring(128, 2).Trim(),
                    DescricaoOperadora = servico.Substring(130, 20).Trim(),
                    CODPais = servico.Substring(150, 3).Trim(),
                    AreaDDD = servico.Substring(153, 4).Trim(),
                    NumeroTelefoneChamado = servico.Substring(157, 10).Trim(),
                    ConjugadoNumeroTelefoneChamado = servico.Substring(167, 2).Trim(),
                    DuracaoLigacao = servico.Substring(169, 6).Trim(),
                    HorarioLigacao = servico.Substring(175, 6).Trim(),
                    GrupoCategoria = servico.Substring(181, 3).Trim(),
                    DescricaoGrupoCategoria = servico.Substring(184, 30).Trim(),
                    Categoria = servico.Substring(214, 3).Trim(),
                    DescricaoCategoria = servico.Substring(217, 40).Trim(),
                    SinalValorLigacao = servico.Substring(257, 1).Trim(),
                    ValorLigacao = servico.Substring(258, 13).Trim(),
                    ClasseServico = servico.Substring(271, 5).Trim(),
                    Filler = servico.Substring(276, 74).Trim()
                });
            }

            return servicos;
        }

        public List<Bilhetacao> BuildBilhetacoes(List<string> bilhetacaoList)
        {
            var bilhetacoes = new List<Bilhetacao>();
            foreach (var bilhetacao in bilhetacaoList)
            {
                bilhetacoes.Add(new Bilhetacao
                {
                    ControleSequencial = bilhetacao.Substring(1, 12).Trim(),
                    Vencimento = bilhetacao.Substring(13, 8).Trim(),
                    Emissao = bilhetacao.Substring(21, 8).Trim(),
                    IDUnicoNRC = bilhetacao.Substring(29, 25).Trim(),
                    RecursoCNL = bilhetacao.Substring(54, 5).Trim(),
                    DDD = bilhetacao.Substring(59, 2).Trim(),
                    NumeroTelefone = bilhetacao.Substring(61, 10).Trim(),
                    CaracteristicaRecurso = bilhetacao.Substring(71, 15).Trim(),
                    DegrauRecurso = bilhetacao.Substring(86, 2).Trim(),
                    DataLigacao = bilhetacao.Substring(88, 8).Trim(),
                    CNLLocalidadeChamada = bilhetacao.Substring(96, 5).Trim(),
                    NomeLocalidadeChamada = bilhetacao.Substring(101, 25).Trim(),
                    UFTelefoneChamado = bilhetacao.Substring(126, 2).Trim(),
                    CODNacionalInternacional = bilhetacao.Substring(128, 2).Trim(),
                    CODOperadora = bilhetacao.Substring(130, 2).Trim(),
                    DescricaoOperadora = bilhetacao.Substring(132, 20).Trim(),
                    CODPais = bilhetacao.Substring(152, 3).Trim(),
                    AreaDDD = bilhetacao.Substring(155, 4).Trim(),
                    NumeroTelefoneChamado = bilhetacao.Substring(159, 10).Trim(),
                    ConjugadoNumeroTelefoneChamado = bilhetacao.Substring(169, 2).Trim(),
                    DuracaoLigacao = bilhetacao.Substring(171, 6).Trim(),
                    Categoria = bilhetacao.Substring(177, 3).Trim(),
                    DescricaoCategoria = bilhetacao.Substring(180, 50).Trim(),
                    HorarioLigacao = bilhetacao.Substring(230, 6).Trim(),
                    TipoChamada = bilhetacao.Substring(236, 1).Trim(),
                    GrupoHorarioTarifario = bilhetacao.Substring(237, 1).Trim(),
                    DescricaoHorarioTarifario = bilhetacao.Substring(238, 25).Trim(),
                    DegrauLigacao = bilhetacao.Substring(263, 2).Trim(),
                    SinalValorLigacao = bilhetacao.Substring(265, 1).Trim(),
                    AliquotaICMS = bilhetacao.Substring(266, 5).Trim(),
                    ValorLigacaoComImposto = bilhetacao.Substring(271, 13).Trim(),
                    ClasseServico = bilhetacao.Substring(284, 5).Trim(),
                    Filler = bilhetacao.Substring(289, 61).Trim()
                });
            }

            return bilhetacoes;
        }

        private List<Resumo> BuildResumos(List<string> resumoList)
        {
            var resumos = new List<Resumo>();
            foreach (var resumo in resumoList)
            {
                resumos.Add(new Resumo
                {
                    ControleSequencial = resumo.Substring(1, 12).Trim(),
                    ContaUnicaID = resumo.Substring(13, 25).Trim(),
                    Vencimento = resumo.Substring(38, 8).Trim(),
                    Emissao = resumo.Substring(46, 8).Trim(),
                    IDUnicoNRC = resumo.Substring(54, 25).Trim(),
                    RecursoCNL = resumo.Substring(79, 5).Trim(),
                    Localidade = resumo.Substring(84, 25).Trim(),
                    DDD = resumo.Substring(109, 2).Trim(),
                    NumeroTelefone = resumo.Substring(111, 10).Trim(),
                    TipoServico = resumo.Substring(121, 4).Trim(),
                    DescricaoServico = resumo.Substring(125, 35).Trim(),
                    CaracteristicaRecurso = resumo.Substring(160, 15).Trim(),
                    DegrauRecurso = resumo.Substring(175, 2).Trim(),
                    VelocidadeRecurso = resumo.Substring(177, 5).Trim(),
                    UnidadeVelocidadeRecurso = resumo.Substring(182, 4).Trim(),
                    InicioAssinatura = resumo.Substring(186, 8).Trim(),
                    FimAssinatura = resumo.Substring(194, 8).Trim(),
                    InicioPeriodoServico = resumo.Substring(202, 8).Trim(),
                    FimPeriodoServico = resumo.Substring(210, 8).Trim(),
                    UnidadeConsumo = resumo.Substring(218, 5).Trim(),
                    QuantidadeConsumo = resumo.Substring(223, 7).Trim(),
                    SinalValorConsumo = resumo.Substring(230, 1).Trim(),
                    ValorConsumo = resumo.Substring(231, 13).Trim(),
                    SinalAssinatura = resumo.Substring(244, 1).Trim(),
                    ValorAssinatura = resumo.Substring(245, 13).Trim(),
                    Aliquota = resumo.Substring(258, 2).Trim(),
                    SinalICMS = resumo.Substring(260, 1).Trim(),
                    ValorICMS = resumo.Substring(261, 13).Trim(),
                    SinalTotalOutrosImpostos = resumo.Substring(274, 1).Trim(),
                    ValorTotalImpostos = resumo.Substring(275, 13).Trim(),
                    NumeroNotaFiscal = resumo.Substring(288, 12).Trim(),
                    SinalValorConta = resumo.Substring(300, 1).Trim(),
                    ValorConta = resumo.Substring(301, 13).Trim(),
                    Filler = resumo.Substring(314, 36).Trim()
                });
            }

            return resumos;
        }


        public Head BuildHead(List<string> headList)
        {
            foreach (var head in headList)
            {
                return new Head
                {
                    ControleSequencial = head.Substring(1, 12).Trim(),
                    DataGeracaoArquivo = head.Substring(13, 8).Trim(),
                    IdentificadorEmpresa = head.Substring(21, 15).Trim(),
                    EmpresaUF = head.Substring(36, 2).Trim(),
                    CodigoCliente = head.Substring(38, 15).Trim(),
                    NomeCliente = head.Substring(53, 40).Trim(),
                    ClienteCGC = head.Substring(93, 15).Trim(),
                    ContaUnicaID = head.Substring(108, 25).Trim(),
                    Vencimento = head.Substring(133, 8).Trim(),
                    Emissao = head.Substring(141, 8).Trim(),
                    Filler = head.Substring(149, 201).Trim()
                };
            }

            return new Head();
        }

        public ListasRegistros BuildRegisterList(List<string> lineList)
        {

            var listaRegistros = new ListasRegistros
            {
                HeadList = new List<string>(),
                ResumoList = new List<string>(),
                EnderecoList = new List<string>(),
                BilhetacaoList = new List<string>(),
                ServicosList = new List<string>(),
                DescontosList = new List<string>(),
                TotalizadorFinalList = new List<string>()
            };

            foreach (var line in lineList)
            {
                if (line.StartsWith(TipoRegistro.Header))
                    listaRegistros.HeadList.Add(line);
                else if (line.StartsWith(TipoRegistro.Resumo))
                    listaRegistros.ResumoList.Add(line);
                else if (line.StartsWith(TipoRegistro.Endereco))
                    listaRegistros.EnderecoList.Add(line);
                else if (line.StartsWith(TipoRegistro.Bilhetacao))
                    listaRegistros.BilhetacaoList.Add(line);
                else if (line.StartsWith(TipoRegistro.Servicos))
                    listaRegistros.ServicosList.Add(line);
                else if (line.StartsWith(TipoRegistro.Descontos))
                    listaRegistros.DescontosList.Add(line);
                else if (line.StartsWith(TipoRegistro.TotalizadorFinal))
                    listaRegistros.TotalizadorFinalList.Add(line);

            }

            return listaRegistros;

        }

       
        [TestMethod]
        public void GetValorFromStringV1()
        {
            var line = @"Valor: R$ 7.933,57";
            var conta = new ClaroParser().GetValor(line);
            Assert.IsTrue(string.Equals("7.933,57", conta));

        }

        [TestMethod]
        public void GetValorFromStringV2()
        {
            var line = @"Valor: R$ 17.933,57";
            var conta = new ClaroParser().GetValor(line);
            Assert.IsTrue(string.Equals("17.933,57", conta));

        }

        [TestMethod]
        public void GetValorFromStringV3()
        {
            var line = @"Valor: R$ 217.933,57";
            var conta = new ClaroParser().GetValor(line);
            Assert.IsTrue(string.Equals("217.933,57", conta));

        }

        [TestMethod]
        public void GetValorFromStringV4()
        {
            var line = @"Valor: R$217.933,57";
            var conta = new ClaroParser().GetValor(line);
            Assert.IsTrue(string.Equals("217.933,57", conta));

        }

        [TestMethod]
        public void GetValorFromStringV5()
        {
            var line = @",57";
            var conta = new ClaroParser().GetValor(line);
            Assert.IsTrue(string.Equals("0,57", conta));

        }

        [TestMethod]
        public void GetNumberFromStringV1()
        {
            var line = @"21 9999-1124";
            var numero = new ClaroParser().GetNumeros(line);
            Assert.IsTrue(string.Equals("2199991124", numero));

        }


        [TestMethod]
        public void GetDateFromStringV1()
        {
            var line = @"Período de Referência: 10/05/2017 a 09/06/2017";
            var datas = new ClaroParser().GetDatas(line);
            Assert.IsTrue(string.Equals("10/05/2017", datas[0]));
            Assert.IsTrue(string.Equals("09/06/2017", datas[1]));

        }

        [TestMethod]
        public void GetDataYYYYMM()
        {
            var line = @"10/05/2017";
            var data = new ClaroParser().GetDataYYYYMM(line);
            Assert.IsTrue(string.Equals("201705", data));
            
        }


        [TestMethod]
        public void ParserClaroConta1()
        {
            var fileV2 = @"E:\Projects\FoneClub\Claro\2017.06.Claro.939144068_100387217_2017_6_3_1.txt";
            var conta = new ClaroParser().Parse(fileV2);
            Assert.IsTrue(conta.IdCliente != null);

        }

        [TestMethod]
        public void ParserClaroConta2()
        {
            var fileV2 = @"E:\Projects\FoneClub\Claro\2016.10.Claro.939144068_100387217_2016_10_3_1.txt";
            var conta = new ClaroParser().Parse(fileV2);
            Assert.IsTrue(conta.IdCliente != null);

        }

        [TestMethod]
        public void ParserClaroConta3()
        {
            var fileV2 = @"E:\Projects\FoneClub\Claro\2017.02.Claro.939144068_100387217_2017_2_3_1.txt";
            var conta = new ClaroParser().Parse(fileV2);
            Assert.IsTrue(conta.IdCliente != null);

        }

        [TestMethod]
        public void ParserClaroContaTab1()
        {
            var fileV2 = @"E:\Projects\FoneClub\Claro\2017.07.Claro.939144068_100387217_2017_7_3_1.txt";
            var conta = new ClaroParser().Parse(fileV2);
            Assert.IsTrue(conta.IdCliente  == "08453543000176");

        }

        [TestMethod]
        public void ParserClaroConta_ArquivoErrado()
        {
            var fileV2 = @"E:\Projects\FoneClub\FoneClube.NET\ParserFebraban\v2\abril\vivo_3mg.txt";
            var conta = new ClaroParser().Parse(fileV2);
            Assert.IsNull(conta.IdCliente);

        }


        [TestMethod]
        public void ParserClaroContaTab1_popula()
        {
            var fileV2 = @"E:\Projects\FoneClub\Claro\2017.07.Claro.939144068_100387217_2017_7_3_1.txt";
            var conta = new ClaroParser().Parse(fileV2);
            var salvaDB = new ClaroContaAcesso().SaveConta(conta);

            Assert.IsTrue(salvaDB);
            Assert.IsTrue(conta.IdCliente == "08453543000176");

        }

        [TestMethod]
        public void ClaroParser_popula1()
        {

            var fileV2 = @"C:\Users\Rodrigo\Desktop\claro1.txt";
            var conta = new ClaroParser().Parse(fileV2);
           
            var salvaDB = new ClaroContaAcesso().SaveConta(conta);

            Assert.IsTrue(salvaDB);

        }

        [TestMethod]
        public void ClaroParser_popula2()
        {

            var fileV2 = @"C:\Users\Rodrigo\Desktop\claro-fev\conta-claro-fev.txt";
            var conta = new ClaroParser().Parse(fileV2);

            var salvaDB = new ClaroContaAcesso().SaveConta(conta);

            Assert.IsTrue(salvaDB);

        }

        [TestMethod]
        public void ClaroParser_popula3()
        {

            var fileV2 = @"E:\Projects\FoneClub\Claro\2017.02.Claro.939144068_100387217_2017_2_3_1.txt";
            var conta = new ClaroParser().Parse(fileV2);

            var salvaDB = new ClaroContaAcesso().SaveConta(conta);

            Assert.IsTrue(salvaDB);

        }







    }
}
