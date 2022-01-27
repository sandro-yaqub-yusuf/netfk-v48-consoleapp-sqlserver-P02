using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using Dapper;
using KITAB.Dapper.BulkInsert.Domains;

namespace KITAB.Dapper.BulkInsert.ConsoleApp
{
    public class Program
    {
		public static List<Holerite> lstHolerites;
		public static List<Verba> lstVerbas;

		private static string strConSQL = ConfigurationManager.ConnectionStrings["strConSQL"].ConnectionString;
		private static string arquivo = ConfigurationManager.AppSettings["PastaArquivo"].ToString();
		private static string dataPagto = "";
		private static int seqID = 0;

		public static void Main(string[] args)
        {
			Console.WriteLine("Início - Processamento dos Holerites...\n\n");

			StepTakeCounter();

			StepFileReader();

			StepDelete();

			StepBulkInsertHolerites();

			StepBulkInsertVerbas();

			Console.WriteLine("Fim - Processamento dos Holerites...\n\n");
			Console.ReadLine();
		}

		private static void StepTakeCounter()
        {
			Console.WriteLine("Pegando a última contagem de Holerites já processados...\n\n");

			seqID = 0;

			using (var connection = new SqlConnection(strConSQL))
			{
				var retorno = connection.ExecuteScalar("SELECT TOP 1 ID_HOLERITE FROM HOLERITE (nolock) ORDER BY ID_HOLERITE DESC;");

				if (retorno != null) seqID = Convert.ToInt32(retorno);
			}

			seqID++;
        }

        public static void StepFileReader()
		{
			Console.WriteLine("Efetuando a leitura do arquivo PAGAMENTOS.TXT...\n\n");

			lstHolerites = new List<Holerite>();
			lstVerbas = new List<Verba>();

			Holerite holerite = new Holerite();
			Verba verba = new Verba();
			DateTime validaData;
			string cnpj = "";
			int numeroDalinha = 0;
			int tipoArquivo = 0;

			try
			{
				if (File.Exists(arquivo))
				{
					// STEP FILE READER
					using (StreamReader sr = new StreamReader(arquivo))
					{
						string linha;

						while ((linha = sr.ReadLine()) != null)
						{
							numeroDalinha++;

							switch (linha.Substring(13, 1))
							{
								case "A":
									holerite.cnpj = cnpj.Trim();
									holerite.tipo = tipoArquivo;
									holerite.cpf = linha.Substring(206, 11).Trim();
									holerite.funcionario = linha.Substring(43, 30).Trim();
									holerite.cod_banco = Convert.ToInt32(linha.Substring(20, 3));
									holerite.agencia = Convert.ToInt32(linha.Substring(24, 5));
									holerite.conta_corrente = Convert.ToInt32(linha.Substring(35, 6));
									holerite.liquido_pagamento = (Convert.ToDouble(linha.Substring(119, 15)) / 100);
									holerite.data_pgto = DateTime.ParseExact(linha.Substring(93, 2) + "/" + linha.Substring(95, 2) + "/" + linha.Substring(97, 4), "dd/MM/yyyy", null);

									dataPagto = (linha.Substring(97, 4) + linha.Substring(95, 2) + linha.Substring(93, 2));

									break;

								case "D":
									holerite.id_holerite = seqID;
									holerite.id_tel = linha.Substring(38, 15).Trim();
									holerite.centro_custo = linha.Substring(23, 15).Trim();
									holerite.salario_contribuicao_inss = (Convert.ToDouble(linha.Substring(105, 15)) / 100);
									holerite.cargo = linha.Substring(53, 30).Trim();
									holerite.depir = Convert.ToInt32(linha.Substring(99, 2));
									holerite.depsf = Convert.ToInt32(linha.Substring(101, 2));

									if (DateTime.TryParseExact(linha.Substring(83, 2) + "/" + linha.Substring(85, 2) + "/" + linha.Substring(87, 4), "dd/MM/yyyy", null, DateTimeStyles.None, out validaData))
									{
										holerite.ferias_inicio = DateTime.ParseExact(linha.Substring(83, 2) + "/" + linha.Substring(85, 2) + "/" + linha.Substring(87, 4), "dd/MM/yyyy", null);
									}
									else 
									{ 
										holerite.ferias_inicio = null; 
									}

									if (DateTime.TryParseExact(linha.Substring(83, 2) + "/" + linha.Substring(85, 2) + "/" + linha.Substring(87, 4), "dd/MM/yyyy", null, DateTimeStyles.None, out validaData))
									{
										holerite.ferias_fim = DateTime.ParseExact(linha.Substring(91, 2) + "/" + linha.Substring(93, 2) + "/" + linha.Substring(95, 4), "dd/MM/yyyy", null);
									}
									else 
									{ 
										holerite.ferias_fim = null; 
									}

									holerite.horas_semanais = Convert.ToInt32(linha.Substring(103, 2));
									holerite.valor_total_credito = (Convert.ToDouble(linha.Substring(135, 15)) / 100);
									holerite.valor_total_desconto = (Convert.ToDouble(linha.Substring(150, 15)) / 100);
									holerite.base_calculo_ir = (Convert.ToDouble(linha.Substring(195, 15)) / 100);
									holerite.base_calculo_fgts = (Convert.ToDouble(linha.Substring(210, 15)) / 100);
									holerite.salario_base = (Convert.ToDouble(linha.Substring(180, 15)) / 100);
									holerite.fgts_mes = (Convert.ToDouble(linha.Substring(120, 15)) / 100);
									holerite.mes_ano_referencia = DateTime.ParseExact("01/" + linha.Substring(17, 2) + "/" + linha.Substring(19, 4), "dd/MM/yyyy", null);

									break;

								case "E":
									string cv = linha.Substring(17, 1);
									int codigoDaVerba = Convert.ToInt32(cv);

									verba = null;
									verba = new Verba();

									if (linha.Substring(18, 30).Trim().Length > 0)
                                    {
										verba = new Verba();
										verba.id_holerite = seqID;
										verba.codverba = codigoDaVerba;
										verba.descricao_verba = linha.Substring(18, 30).Trim();
										verba.valor = (Convert.ToDouble(linha.Substring(53, 15)) / 100);

										lstVerbas.Add(verba);
									}

									if (linha.Substring(68, 30).Trim().Length > 0)
									{
										verba = new Verba();
										verba.id_holerite = seqID;
										verba.codverba = codigoDaVerba;
										verba.descricao_verba = linha.Substring(68, 30).Trim();
										verba.valor = (Convert.ToDouble(linha.Substring(103, 15)) / 100);

										lstVerbas.Add(verba);
									}

									if (linha.Substring(118, 30).Trim().Length > 0)
									{
										verba = new Verba();
										verba.id_holerite = seqID;
										verba.codverba = codigoDaVerba;
										verba.descricao_verba = linha.Substring(118, 30).Trim();
										verba.valor = (Convert.ToDouble(linha.Substring(153, 15)) / 100);

										lstVerbas.Add(verba);
									}

									if (linha.Substring(168, 30).Trim().Length > 0)
									{
										verba = new Verba();
										verba.id_holerite = seqID;
										verba.codverba = codigoDaVerba;
										verba.descricao_verba = linha.Substring(168, 30).Trim();
										verba.valor = (Convert.ToDouble(linha.Substring(203, 15)) / 100);

										lstVerbas.Add(verba);
									}

									break;

								case "F":
									lstHolerites.Add(holerite);

									holerite = null;
									holerite = new Holerite();

									seqID++;

									break;

								default:
									if (linha.Substring(3, 4).Equals("0000")) cnpj = linha.Substring(18, 14);
									if (linha.Substring(8, 1).Equals("C")) tipoArquivo = int.Parse(linha.Substring(102, 5));

									break;
							}
						}

						sr.Close();
					}

					Console.WriteLine("Leitura do arquivo efetuado com sucesso !\n\n");
				}
				else
				{
					throw new Exception("Arquivo PAGAMENTOS.TXT não foi localizado !\n\n");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("ERRO => " + ex.Message);
			}
		}

		public static void StepDelete()
		{
			Console.WriteLine("Excluindo os dados anteriores...\n\n");

			if (lstHolerites.Count > 0)
            {
				using (var connection = new SqlConnection(strConSQL))
				{
					connection.Execute("DELETE FROM VERBAS WHERE ID_HOLERITE IN (SELECT ID_HOLERITE FROM HOLERITE (nolock) WHERE DATA_PGTO = '" + dataPagto + "');");
					connection.Execute("DELETE FROM HOLERITE WHERE DATA_PGTO = '" + dataPagto + "';");
				}
			}
		}

		public static void StepBulkInsertHolerites()
		{
			Console.WriteLine("Inserindo os dados de Holerites...\n\n");

			using (IDbConnection con = new SqlConnection(strConSQL))
			{
				if (con.State == ConnectionState.Closed) con.Open();

				var trans = con.BeginTransaction();
				
				try
				{
					con.Execute(@"INSERT  INTO HOLERITE (ID_TEL, CENTRO_CUSTO, FUNCIONARIO, CARGO, COD_BANCO, AGENCIA, CONTA_CORRENTE, DEPIR, 
										  DEPSF, FERIAS_INICIO, FERIAS_FIM, HORAS_SEMANAIS, BASE_CALCULO_IR, BASE_CALCULO_FGTS, 
										  SALARIO_BASE, SALARIO_CONTRIBUICAO_INSS, FGTS_MES, LIQUIDO_PAGAMENTO, CNPJ, MES_ANO_REFERENCIA, 
										  DATA_PGTO, ID_HOLERITE, VALOR_TOTAL_CREDITO, VALOR_TOTAL_DESCONTO, CPF, TIPO)
                                  VALUES (@ID_TEL, @CENTRO_CUSTO, @FUNCIONARIO, @CARGO, @COD_BANCO, @AGENCIA, @CONTA_CORRENTE, @DEPIR, 
                                          @DEPSF, @FERIAS_INICIO, @FERIAS_FIM, @HORAS_SEMANAIS, @BASE_CALCULO_IR, @BASE_CALCULO_FGTS, 
                                          @SALARIO_BASE, @SALARIO_CONTRIBUICAO_INSS, @FGTS_MES, @LIQUIDO_PAGAMENTO, @CNPJ, @MES_ANO_REFERENCIA, 
                                          @DATA_PGTO, @ID_HOLERITE, @VALOR_TOTAL_CREDITO, @VALOR_TOTAL_DESCONTO, @CPF, @TIPO)", lstHolerites, transaction: trans);

					trans.Commit();
				}
				catch (Exception ex)
                {
					Console.WriteLine("Ocorreu um erro ao inserir os dados novos... ERRO: " + ex.Message + "\n\n");

					trans.Rollback();
                }
			}
		}

		public static void StepBulkInsertVerbas()
		{
			Console.WriteLine("Inserindo os dados de Verbas...\n\n");

			using (IDbConnection con = new SqlConnection(strConSQL))
			{
				if (con.State == ConnectionState.Closed) con.Open();

				var trans = con.BeginTransaction();

				try
				{
					con.Execute(@"INSERT INTO VERBAS (ID_HOLERITE, VALOR, DESCRICAO_VERBA, CODVERBA) 
                                  VALUES (@ID_HOLERITE, @VALOR, @DESCRICAO_VERBA, @CODVERBA)", lstVerbas, transaction: trans);

					trans.Commit();
				}
				catch (Exception ex)
				{
					Console.WriteLine("Ocorreu um erro ao inserir os dados novos... ERRO: " + ex.Message + "\n\n");

					trans.Rollback();
				}
			}
		}
	}
}
