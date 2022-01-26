CREATE TABLE [dbo].[TIPO_HOLERITE] (
    [ID]        INT           NOT NULL,
    [DESCRICAO] VARCHAR (100) NOT NULL,
    [ATIVO]     BIT           NOT NULL,
    CONSTRAINT [PK_TIPO_HOLERITE] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);
GO

INSERT INTO [dbo].[TIPO_HOLERITE] ([ID], [DESCRICAO], [ATIVO]) VALUES (1, N'Folha Mensal', 1)
INSERT INTO [dbo].[TIPO_HOLERITE] ([ID], [DESCRICAO], [ATIVO]) VALUES (2, N'Folha Quinzenal', 1)
INSERT INTO [dbo].[TIPO_HOLERITE] ([ID], [DESCRICAO], [ATIVO]) VALUES (3, N'Folha Complementar', 0)
INSERT INTO [dbo].[TIPO_HOLERITE] ([ID], [DESCRICAO], [ATIVO]) VALUES (4, N'13 Salario', 1)
INSERT INTO [dbo].[TIPO_HOLERITE] ([ID], [DESCRICAO], [ATIVO]) VALUES (5, N'Participacao de Resultados', 0)
INSERT INTO [dbo].[TIPO_HOLERITE] ([ID], [DESCRICAO], [ATIVO]) VALUES (6, N'Informe de Rendimentos', 0)
INSERT INTO [dbo].[TIPO_HOLERITE] ([ID], [DESCRICAO], [ATIVO]) VALUES (7, N'Férias', 0)
INSERT INTO [dbo].[TIPO_HOLERITE] ([ID], [DESCRICAO], [ATIVO]) VALUES (8, N'Rescisao', 0)
INSERT INTO [dbo].[TIPO_HOLERITE] ([ID], [DESCRICAO], [ATIVO]) VALUES (9, N'Rescisao Complementar', 0)
INSERT INTO [dbo].[TIPO_HOLERITE] ([ID], [DESCRICAO], [ATIVO]) VALUES (10, N'Outros', 0)
INSERT INTO [dbo].[TIPO_HOLERITE] ([ID], [DESCRICAO], [ATIVO]) VALUES (85, N'Debito Conta Investimento', 0)
GO

CREATE TABLE [dbo].[HOLERITE] (
    [ID_TEL]                    VARCHAR (50)    NOT NULL,
    [CENTRO_CUSTO]              VARCHAR (15)    NOT NULL,
    [FUNCIONARIO]               VARCHAR (100)   NOT NULL,
    [CARGO]                     VARCHAR (30)    NOT NULL,
    [COD_BANCO]                 INT             NOT NULL,
    [AGENCIA]                   INT             NOT NULL,
    [CONTA_CORRENTE]            INT             NOT NULL,
    [DEPIR]                     INT             NULL,
    [DEPSF]                     INT             NULL,
    [FERIAS_INICIO]             DATETIME        NULL,
    [FERIAS_FIM]                DATETIME        NULL,
    [HORAS_SEMANAIS]            INT             NULL,
    [BASE_CALCULO_IR]           DECIMAL (18, 2) NULL,
    [BASE_CALCULO_FGTS]         DECIMAL (18, 2) NULL,
    [SALARIO_BASE]              DECIMAL (18, 2) NOT NULL,
    [SALARIO_CONTRIBUICAO_INSS] DECIMAL (18, 2) NULL,
    [FGTS_MES]                  DECIMAL (18, 2) NULL,
    [LIQUIDO_PAGAMENTO]         DECIMAL (18, 2) NULL,
    [CNPJ]                      VARCHAR (14)    NULL,
    [MES_ANO_REFERENCIA]        DATETIME        NOT NULL,
    [DATA_PGTO]                 DATETIME        NOT NULL,
    [ID_HOLERITE]               INT             NOT NULL,
    [VALOR_TOTAL_CREDITO]       DECIMAL (18, 2) NULL,
    [VALOR_TOTAL_DESCONTO]      DECIMAL (18, 2) NULL,
    [CPF]                       VARCHAR (11)    NULL,
    [TIPO]                      INT             NOT NULL,
    CONSTRAINT [PK_HOLERITE] PRIMARY KEY CLUSTERED ([ID_HOLERITE] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_TIPO_HOLERITE] FOREIGN KEY ([TIPO]) REFERENCES [dbo].[TIPO_HOLERITE] ([ID])
);
GO

CREATE TABLE [dbo].[VERBAS] (
    [ID_HOLERITE]     INT             NOT NULL,
    [VALOR]           DECIMAL (18, 2) NOT NULL,
    [DESCRICAO_VERBA] VARCHAR (100)   NULL,
    [CODVERBA]        INT             NULL,
    CONSTRAINT [FK_ID_HOLERITE] FOREIGN KEY ([ID_HOLERITE]) REFERENCES [dbo].[HOLERITE] ([ID_HOLERITE])
);
GO

