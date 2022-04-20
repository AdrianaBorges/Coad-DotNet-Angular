ALTER TABLE [COADCORP].[dbo].[CLIENTES] 
ADD [DATA_EXCLUSAO] [DateTime] NULL

GO

USE [COADCORP]
GO
/****** Object:  Table [dbo].[TIPO_ENDERECO]    Script Date: 08/06/2015 15:17:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TIPO_ENDERECO](
	[TP_END_ID] [int] IDENTITY(1,1) NOT NULL,
	[TP_END_DESCRICAO] [nvarchar](25) NULL,
 CONSTRAINT [PK_TIPO_ENDERECO] PRIMARY KEY CLUSTERED 
(
	[TP_END_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET IDENTITY_INSERT [dbo].[TIPO_ENDERECO] ON
INSERT [dbo].[TIPO_ENDERECO] ([TP_END_ID], [TP_END_DESCRICAO]) VALUES (1, N'Entrega')
INSERT [dbo].[TIPO_ENDERECO] ([TP_END_ID], [TP_END_DESCRICAO]) VALUES (2, N'Faturamento')
SET IDENTITY_INSERT [dbo].[TIPO_ENDERECO] OFF
GO


ALTER TABLE [dbo].[CLIENTES_ENDERECO]  WITH CHECK ADD  CONSTRAINT [FK_CLIENTES_ENDERECO_TIPO_ENDERECO] FOREIGN KEY([END_TIPO])
REFERENCES [dbo].[TIPO_ENDERECO] ([TP_END_ID])
GO

ALTER TABLE [CLIENTES_ENDERECO] CHECK CONSTRAINT [FK_CLIENTES_ENDERECO_TIPO_ENDERECO]
GO

GO

/****** Object:  Table [dbo].[REGIAO]    Script Date: 08/20/2015 11:51:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REGIAO](
	[RG_ID] [int] IDENTITY(1,1) NOT NULL,
	[RG_DESCRICAO] [nvarchar](25) NULL,
 CONSTRAINT [PK_REGIAO] PRIMARY KEY CLUSTERED 
(
	[RG_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [COADCORP]
GO

/****** Object:  Table [dbo].[TABELA_PRECO]    Script Date: 08/20/2015 12:33:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TABELA_PRECO](
	[CMP_ID] [int] NOT NULL,
	[CO_PG_ID] [int] NOT NULL,
	[TPG_ID] [int] NOT NULL,
	[TP_MARGEM_NEGOCIACAO] [int] NULL,
	[TP_MARGEM_LUCRO] [int] NULL,
	[TP_PRECO_VENDA] [numeric](11, 2) NULL,
 CONSTRAINT [PK_TABELA_PRECO] PRIMARY KEY CLUSTERED 
(
	[CMP_ID] ASC,
	[CO_PG_ID] ASC,
	[TPG_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [COADCORP]
GO
/****** Object:  Table [dbo].[CONDICAO_PAGAMENTO]    Script Date: 09/29/2015 12:27:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CONDICAO_PAGAMENTO](
	[CO_PG_ID] [int] IDENTITY(1,1) NOT NULL,
	[CO_PG_DESCRICAO] [nvarchar](25) NULL,
 CONSTRAINT [PK_CONDICAO_PAGAMENTO] PRIMARY KEY CLUSTERED 
(
	[CO_PG_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[CONDICAO_PAGAMENTO] ON
INSERT [dbo].[CONDICAO_PAGAMENTO] ([CO_PG_ID], [CO_PG_DESCRICAO]) VALUES (1, N'A Vista')
INSERT [dbo].[CONDICAO_PAGAMENTO] ([CO_PG_ID], [CO_PG_DESCRICAO]) VALUES (2, N'Parcelado')
SET IDENTITY_INSERT [dbo].[CONDICAO_PAGAMENTO] OFF


ALTER TABLE [dbo].[TABELA_PRECO]  WITH CHECK ADD  CONSTRAINT [FK_TABELA_PRECO_CONDICAO_PAGAMENTO] FOREIGN KEY([CO_PG_ID])
REFERENCES [dbo].[CONDICAO_PAGAMENTO] ([CO_PG_ID])
GO

ALTER TABLE [dbo].[TABELA_PRECO] CHECK CONSTRAINT [FK_TABELA_PRECO_CONDICAO_PAGAMENTO]
GO

ALTER TABLE [dbo].[TABELA_PRECO]  WITH CHECK ADD  CONSTRAINT [FK_TABELA_PRECO_PRODUTO_COMPOSICAO] FOREIGN KEY([CMP_ID])
REFERENCES [dbo].[PRODUTO_COMPOSICAO] ([CMP_ID])
GO

ALTER TABLE [dbo].[TABELA_PRECO] CHECK CONSTRAINT [FK_TABELA_PRECO_PRODUTO_COMPOSICAO]
GO

ALTER TABLE [dbo].[TABELA_PRECO]  WITH CHECK ADD  CONSTRAINT [FK_TABELA_PRECO_TIPO_PAGAMENTO] FOREIGN KEY([TPG_ID])
REFERENCES [dbo].[TIPO_PAGAMENTO] ([TPG_ID])
GO

ALTER TABLE [dbo].[TABELA_PRECO] CHECK CONSTRAINT [FK_TABELA_PRECO_TIPO_PAGAMENTO]
GO

USE [COADCORP]
GO

/****** Object:  Table [dbo].[REGIAO_TABELA_PRECO]    Script Date: 08/20/2015 14:55:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[REGIAO_TABELA_PRECO](
	[RG_ID] [int] NOT NULL,
	[CMP_ID] [int] NOT NULL,
	[CO_PG_ID] [int] NOT NULL,
	[TPG_ID] [int] NOT NULL,
	[RG_TP_PRECO_VENDA] [numeric](11, 2) NULL,
	[DATA_ASSOCIACAO] [datetime] NULL,
 CONSTRAINT [PK_REGIAO_TABELA_PRECO] PRIMARY KEY CLUSTERED 
(
	[RG_ID] ASC,
	[CMP_ID] ASC,
	[CO_PG_ID] ASC,
	[TPG_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[REGIAO_TABELA_PRECO]  WITH CHECK ADD  CONSTRAINT [FK_REGIAO_TABELA_PRECO_REGIAO] FOREIGN KEY([RG_ID])
REFERENCES [dbo].[REGIAO] ([RG_ID])
GO

ALTER TABLE [dbo].[REGIAO_TABELA_PRECO] CHECK CONSTRAINT [FK_REGIAO_TABELA_PRECO_REGIAO]
GO

ALTER TABLE [dbo].[REGIAO_TABELA_PRECO]  WITH CHECK ADD  CONSTRAINT [FK_REGIAO_TABELA_PRECO_TABELA_PRECO] FOREIGN KEY([CMP_ID], [CO_PG_ID], [TPG_ID])
REFERENCES [dbo].[TABELA_PRECO] ([CMP_ID], [CO_PG_ID], [TPG_ID])
GO

ALTER TABLE [dbo].[REGIAO_TABELA_PRECO] CHECK CONSTRAINT [FK_REGIAO_TABELA_PRECO_TABELA_PRECO]
GO

