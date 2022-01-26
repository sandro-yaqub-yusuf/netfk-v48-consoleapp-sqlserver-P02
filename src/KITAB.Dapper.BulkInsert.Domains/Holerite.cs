using System;

namespace KITAB.Dapper.BulkInsert.Domains
{
    public class Holerite
    {
		public string id_tel { get; set; }
		public string centro_custo { get; set; }
		public string funcionario { get; set; }
		public string cargo { get; set; }
		public int cod_banco { get; set; }
		public int agencia { get; set; }
		public int conta_corrente { get; set; }
		public int depir { get; set; }
		public int depsf { get; set; }
		public DateTime? ferias_inicio { get; set; }
		public DateTime? ferias_fim { get; set; }
		public int horas_semanais { get; set; }
		public double base_calculo_ir { get; set; }
		public double base_calculo_fgts { get; set; }
		public double salario_base { get; set; }
		public double salario_contribuicao_inss { get; set; }
		public double fgts_mes { get; set; }
		public double liquido_pagamento { get; set; }
		public string cnpj { get; set; }
		public DateTime mes_ano_referencia { get; set; }
		public DateTime data_pgto { get; set; }
		public int id_holerite { get; set; }
		public double valor_total_credito { get; set; }
		public double valor_total_desconto { get; set; }
		public string cpf { get; set; }
		public int tipo { get; set; }
	}
}
