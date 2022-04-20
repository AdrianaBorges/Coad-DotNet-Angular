/****** Script do comando SelectTopNRows de SSMS  ******/
/*USE COADCORP

GO
	TRUNCATE TABLE 
	dbo.CARTEIRA_REPRESENTANTE
GO
	TRUNCATE TABLE
	dbo.PEDIDO_PAGAMENTO
GO
	DELETE FROM
	dbo.PEDIDO
GO
	TRUNCATE TABLE
	dbo.PEDIDO
GO
	DELETE FROM
	dbo.PRE_PEDIDO
GO

DELETE FROM dbo.REPRESENTANTE

GO
TRUNCATE TABLE dbo.REPRESENTANTE
GO
*/
---- INSERE TODOS OS REPRESENTANTES QUE AINDA NÃO EXISTEM NO BANCO DO CORPORATIVO
INSERT INTO 
COADCORP.dbo.REPRESENTANTE 
	   ([REP_NOME]
      ,[REGIAO_UF]
      ,[AREA_ID]
      ,[REP_ATIVO]
      ,[REP_VARIAS_CARTEIRAS]
      ,[UEN_ID]
      ,[CODIGO_ANTIGO]
      ,[REP_OPER_ID]      
	  ,[CAR_ID]
      ,[REGIAO_ID_ANTIGO])
SELECT 
	 OP.SlpName AS REP_NOME
	 ,(CASE (select top 1 1 from COADCORP.dbo.UF uf -- verifico se o nome da região existe de verdade
		WHERE uf.UF_SIGLA = substring(OP.U_GRUPO,1,2))
	   WHEN 1 THEN substring(OP.U_GRUPO,1,2)
	   ELSE 'RJ' END) AS REGIAO_UF,
	1 AS AREA_ID,
	1 AS REP_ATIVO,
	1 AS REP_VARIAS_CARTEIRAS,
	2 AS UEN_ID,
	OP.SlpCode as CODIGO_ANTIGO,
	OP.SlpCode as REP_OPER_ID,
	'0000000' AS CARTEIRA,
	OP.REGIAO_ID
FROM COADSAT.dbo.SAT_OPERADOR OP 
WHERE NOT EXISTS (
	SELECT 1 FROM COADCORP.dbo.REPRESENTANTE REP
		WHERE OP.SlpName = REP.REP_NOME 
)
AND U_ASISTEMAS = '1000000000'
---------------------------------------------------------------------------------
GO
-- CLIENTES QUE JÁ EXISTEM NO OUTRO BANCO----------------------------------------
UPDATE CLI
SET CLI.CODIGO_ANTIGO = SUS.CardCode
FROM COADCORP.dbo.CLIENTES CLI
	
WHERE EXISTS (
	SELECT 1 FROM COADSAT.dbo.SAT_SUSPECT SUS LEFT JOIN 
	COADCORP.dbo.ASSINATURA_EMAIL EMAIL ON EMAIL.CLI_ID = CLI.CLI_ID
		WHERE SUS.CardName = CLI.CLI_NOME 
)
GO
----------- Procura os clientes da agenda de cursos no corporativo
SELECT T.CardName,
T.CLI_CPF_CNPJ,
count(T.CardName) AS QTD
FROM
(
SELECT 	
	SUS.CardCode,
	CLI.CLI_ID,
	SUS.CardName,
	CLI.CLI_NOME,
	SUS.U_TAXID,
	CLI.CLI_CPF_CNPJ
FROM
	COADSAT.dbo.SAT_SUSPECT SUS 
	INNER JOIN COADCORP.dbo.CLIENTES_VW CLI 
		ON (SUS.CardName = CLI.CLI_NOME) AND 
			((SUS.U_TAXID IS NULL AND CLI.CLI_CPF_CNPJ IS NULL) OR 
			 (SUS.U_TAXID = CLI.CLI_CPF_CNPJ))

)	AS T
GROUP BY T.CardName, T.CLI_CPF_CNPJ
ORDER BY CardName 

		
GO

--------------------------------------------------------------------------
SELECT 
	SUS.CardName
	,(
	SELECT 1 FROM COADCORP.dbo.CLIENTES CLI
		WHERE SUS.CardName = CLI.CLI_NOME 
)
FROM COADSAT.dbo.SAT_SUSPECT SUS

-- Mostra quantos clientes não estão no banco da agenda nova
SELECT 
		----------------Quantidade de Suspects--------------------
		 (SELECT COUNT(*) FROM
			COADSAT_HOMOL.dbo.SAT_SUSPECT) AS TOTAL_SUSPECT
		----------------Quantidade de suspects no banco da agenda nova------
		,T.Achados AS ACHADOS
	---------------Calcula a sobra------------------
		,(SELECT COUNT(*) FROM
			COADSAT_HOMOL.dbo.SAT_SUSPECT) - T.Achados as Sobra
	-----------------------------------------------------
FROM 
(
SELECT COUNT(CLI.CLI_ID) AS Achados
	FROM
	COADSAT_HOMOL.dbo.SAT_SUSPECT SUS INNER JOIN 
	COADCORP_TEST.dbo.CLIENTES_VW CLI 
		ON (SUS.CardName = CLI.CLI_NOME) AND 
			((SUS.U_TAXID IS NULL AND CLI.CLI_CPF_CNPJ IS NULL) OR 
			 (SUS.U_TAXID = CLI.CLI_CPF_CNPJ))
) AS T


GO 

UPDATE SUS 
SET SUS.CLI_ID = CLI.CLI_ID
FROM
	COADSAT.dbo.SAT_SUSPECT SUS 
	INNER JOIN COADCORP.dbo.CLIENTES_VW CLI 
		ON (SUS.CardName = CLI.CLI_NOME) AND 
			((SUS.U_TAXID IS NULL AND CLI.CLI_CPF_CNPJ IS NULL) OR 
			 (SUS.U_TAXID = CLI.CLI_CPF_CNPJ))
GO

UPDATE OP 
SET OP.REP_ID = REP.REP_ID
  FROM [COADCORP].[dbo].[REPRESENTANTE] REP
  INNER JOIN COADSAT.dbo.SAT_OPERADOR OP ON REP.CODIGO_ANTIGO = OP.SlpCode
 

GO
--------- CRIA AS CARTEIRAS DOS REPRESENTANTES IMPORTADOS
BEGIN TRANSACTION TX1;

INSERT INTO dbo.CARTEIRA (
	CAR_ID,
	REGIAO_ID,
	SEQ_REG,
	AREA_ID,
	REGIAO_UF,
	CAR_VARIOS_REPRESENTANTES,
	UEN_ID,
	REP_REF
)
SELECT 
	((select TOP 1 seq from dbo.TAB_SEQ tb WHERE tb.TABELA = 'CARTEIRA')) + 
	ROW_NUMBER() OVER(ORDER BY REP.REP_NOME) AS CAR_ID
	,REP.REGIAO_UF AS REGIAO_ID
	,01 AS SEQ_REG
	,1 AS AREA_ID
	,REP.REGIAO_UF AS REGIAO_UF
	,0 AS CAR_VARIOS_REPRESENTANTES
	,1 AS UEN_ID  
	,OP.REP_ID AS REP_REF

FROM [COADCORP].[dbo].[REPRESENTANTE] REP INNER JOIN 
[COADSAT].[dbo].[SAT_OPERADOR] OP ON REP.REP_ID = OP.REP_ID


UPDATE dbo.TAB_SEQ 
SET SEQ = (select 
	(SELECT SEQ FROM dbo.TAB_SEQ WHERE TABELA = 'CARTEIRA')
	 + COUNT(*)
				FROM [COADCORP].[dbo].[REPRESENTANTE] REP INNER JOIN 
					[COADSAT].[dbo].[SAT_OPERADOR] OP ON REP.REP_ID = OP.REP_ID)
WHERE TABELA = 'CARTEIRA';



COMMIT TRANSACTION TX1
GO

-- Cria a associação entre a carteira e a representante ----------------------
INSERT INTO COADCORP.dbo.CARTEIRA_REPRESENTANTE (CAR_ID, REP_ID, DATA_ASSOCIACAO)
SELECT
	CAR_ID, 
	REP_REF,
	GETDATE()
FROM [COADCORP_TEST].dbo.CARTEIRA CAR
WHERE CAR.REP_REF IS NOT NULL
------------------------------------------------------------------------

BEGIN TRANSACTION TX1------ CRIA ASSINATURAS PARA OS CLIENTES QUE NÃO POSSUIEM NENHUMA

--- CLIENTES QUE AINDA NÃO POSSUEM ASSINATURA
INSERT INTO dbo.ASSINATURA (
	ASN_ANO_COAD,
	ASN_CORTESIA,
	CLI_ID,
	UEN_ID,
	ASN_NUM_ASSINATURA
)
SELECT 	
	49 AS ASN_ANO_COAD,
	0 AS ASN_CORTESIA,
	CLI.CLI_ID,
	1 AS UEN_ID,
	(SELECT SEQ FROM dbo.TAB_SEQ WHERE TABELA = 'ASSINATURA') +
	ROW_NUMBER() OVER(ORDER BY CLI.CLI_ID ASC) AS ASN_NUM_ASSINATURA
FROM
		COADSAT.dbo.SAT_SUSPECT SUS 
		INNER JOIN COADCORP.dbo.CLIENTES_VW CLI ON SUS.CLI_ID = CLI.CLI_ID	
WHERE NOT EXISTS (
	SELECT  1
	FROM COADCORP.dbo.ASSINATURA ASS
	WHERE ASS.CLI_ID = CLI.CLI_ID 
	AND UEN_ID = 1
)
	
UPDATE dbo.TAB_SEQ 
SET SEQ = (SELECT 
			(SELECT SEQ FROM dbo.TAB_SEQ WHERE TABELA = 'ASSINATURA')
	 + COUNT(*) AS COUNT 
FROM
		COADSAT.dbo.SAT_SUSPECT SUS 
		INNER JOIN COADCORP.dbo.CLIENTES_VW CLI ON SUS.CLI_ID = CLI.CLI_ID	
WHERE NOT EXISTS (
	SELECT  1
	FROM COADCORP.dbo.ASSINATURA ASS
	WHERE ASS.CLI_ID = CLI.CLI_ID 
))
WHERE TABELA = 'ASSINATURA'

COMMIT TRANSACTION TX1----------------------------

----- ATUALIZA TODAS AS ASSINATURAS MAIS ATUAIS DOS CLIENTES IMPORTADOS COMO UEN_ID 1 -

BEGIN TRANSACTION TX2

UPDATE ASS 
SET UEN_ID = 1
FROM COADCORP.dbo.ASSINATURA ASS 
WHERE EXISTS (	
	SELECT 	
		1
	FROM
		COADSAT.dbo.SAT_SUSPECT SUS 
		INNER JOIN COADCORP.dbo.CLIENTES_VW CLI ON SUS.CLI_ID = CLI.CLI_ID		
	WHERE CLI.CLI_ID = ASS.CLI_ID
) 
AND ASS.ASN_NUM_ASSINATURA = (

	SELECT MIN(ASS1.ASN_NUM_ASSINATURA) FROM dbo.ASSINATURA ASS1 
	WHERE ASS1.CLI_ID = ASS.CLI_ID
	GROUP BY (ASS1.CLI_ID)
)

COMMIT TRANSACTION TX2

-----------------------------------------------------------------------
--- UNE O CLIENTE PELA SUA ASSINATURA MAIS ATUAL, A CARTEIRA MAIS ATUAL DO REPRESENTANTE DO BANCO DA AGENDA ANTIGA
BEGIN TRANSACTION TX3
INSERT INTO 
	dbo.CARTEIRA_ASSINATURA	(ASN_NUM_ASSINATURA, CAR_ID, CLA_ID)
SELECT DISTINCT
	ASN_NUM_ASSINATURA,
	CART02.CAR_ID,
	1
	FROM COADCORP.dbo.ASSINATURA ASS 
		INNER JOIN 	COADSAT.dbo.SAT_SUSPECT SUS ON ASS.CLI_ID  = SUS.CLI_ID
		INNER JOIN COADCORP.dbo.CLIENTES_VW CLI ON SUS.CLI_ID = CLI.CLI_ID		
		INNER JOIN COADSAT.dbo.SAT_CARTEIRAMENTO CART ON SUS.CardCode = CART.CARDCODE
		INNER JOIN COADSAT.dbo.SAT_OPERADOR OP ON CART.SLPCODE = OP.SlpCode
		INNER JOIN COADCORP.dbo.REPRESENTANTE REP ON REP.REP_ID = OP.REP_ID
		INNER JOIN COADCORP.dbo.CARTEIRA_REPRESENTANTE CAR_REP ON REP.REP_ID = CAR_REP.REP_ID
		INNER JOIN COADCORP.dbo.CARTEIRA CART02 ON CART02.CAR_ID = CAR_REP.CAR_ID
WHERE ASS.ASN_NUM_ASSINATURA = (

	SELECT MIN(ASS1.ASN_NUM_ASSINATURA) FROM dbo.ASSINATURA ASS1 
	WHERE ASS1.CLI_ID = ASS.CLI_ID AND ASS1.UEN_ID = 1
	GROUP BY (ASS1.CLI_ID)
)
AND CART.[STATUS] = 'A'
AND CART02.UEN_ID = 1
AND NOT EXISTS (SELECT 1 FROM dbo.CARTEIRA_ASSINATURA CA WHERE CA.CAR_ID = CART02.CAR_ID AND CA.ASN_NUM_ASSINATURA = ASN_NUM_ASSINATURA )


GO 
COMMIT TRANSACTION TX3

------------ CRIA PRODUTOS COMPOSICAO PARA SERVIR DE PRODUTOS DE INTERESSE 
  INSERT INTO dbo.PRODUTO_COMPOSICAO 
  (
	  [PRO_ID]
	  ,[UEN_ID]
      ,[CMP_DESCRICAO]
      ,[CMP_NOME_ESTRANGEIRO]
      ,[TIPO_PRO_ID]
      ,[TIPO_ENVIO_ID] 
      ,[CMP_PRO_INTERESSE]
  )
  SELECT
  40,
  'PB' AS UEN_ID,
  ItemName AS CMP_DESCRICACAO,
  FrgnName AS CMP_NOME_ESTRANGEIRO,
  3 AS TIPO_PRO_ID,
  2 AS TIPO_ENVIO_ID,
  1 AS CMP_PRO_INTERESSE
  FROM  
  CURSOS.dbo.OITM CURSO
  WHERE NOT EXISTS 
	  (SELECT 1 
	  FROM dbo.PRODUTO_COMPOSICAO CMP 
	  WHERE CMP.CMP_DESCRICAO = CURSO.ItemName COLLATE SQL_Latin1_General_CP850_CI_AS)
	  
GO

-- Associa os cursos da agenda antiga ao produto composição inseridas a partir dela
UPDATE CURSO
SET CMP_ID = CMP.CMP_ID
FROM dbo.PRODUTO_COMPOSICAO CMP
INNER JOIN CURSOS.dbo.OITM CURSO ON 
	CMP.CMP_DESCRICAO = CURSO.ItemName COLLATE SQL_Latin1_General_CP850_CI_AS
	AND CMP.CMP_NOME_ESTRANGEIRO = CURSO.FrgnName COLLATE SQL_Latin1_General_CP850_CI_AS
WHERE PRO_ID = 40

GO
UPDATE dbo.AREAS SET AREA_NOME = 'Área Contábil' 
WHERE AREA_ID = 0

UPDATE dbo.AREAS SET AREA_NOME = 'Área Tributária IR/LC' 
WHERE AREA_ID = 1

UPDATE dbo.AREAS SET AREA_NOME = 'Área Trabalhista (LTPS)' 
WHERE AREA_ID = 2

UPDATE dbo.AREAS SET AREA_NOME = 'Área Jurídica' 
WHERE AREA_ID = 3

UPDATE dbo.AREAS SET AREA_NOME = 'Área Tributária ICMS/ISS/ST' 
WHERE AREA_ID = 4

GO
  UPDATE dbo.PRODUTOS 
  SET AREA_ID = 0 
  WHERE AREA_ID IN (5,6)
GO

UPDATE dbo.CONTRATOS 
  SET AREA_ID = 0 
WHERE AREA_ID IN (5,6)

GO

DELETE dbo.AREAS 
WHERE AREA_ID IN (5, 6)

GO

UPDATE 
dbo.PRODUTO_COMPOSICAO 
SET AREA_ID = 3
WHERE
CMP_PRO_INTERESSE = 1
AND AREA_ID IS NULL

GO


INSERT INTO dbo.PRODUTO_COMPOSICAO_ITEM
(
	CMP_ID,
	PRO_ID,
	CMI_QTDE,
	CMI_PRECO_UNIT,
	CMI_QTDE_PERIODO,
	TTP_ID
)
SELECT 
	CMP_ID,
	40 AS PRO_ID,
	0 AS CMI_QTDE,
	0 AS CMI_PRECO_UNIT,
	0 AS CMI_QTDE_PERIODO,
	1 AS TTP_ID

FROM
dbo.PRODUTO_COMPOSICAO CMP
WHERE
CMP_PRO_INTERESSE = 1 AND
	NOT EXISTS 
		(SELECT 1
		FROM dbo.PRODUTO_COMPOSICAO_ITEM item 
		WHERE item.CMP_ID = CMP.CMP_ID)

GO

-- Importa o email do cliente da agenda velha a agenda nova por meio da primeira assinatura de franquia encontrado
INSERT INTO COADCORP.dbo.ASSINATURA_EMAIL (AEM_EMAIL, ASN_NUM_ASSINATURA)
SELECT 
	E_Mail,
	MIN(ASS1.ASN_NUM_ASSINATURA) AS NUMERO_ASSINATURA_FRANQUIA 
 FROM
	[COADSAT].dbo.SAT_SUSPECT SUS INNER JOIN 
	[COADCORP].[dbo].[CLIENTES] CLI ON CLI.CLI_ID = SUS.CLI_ID INNER JOIN 
	COADCORP.dbo.ASSINATURA ASS1 ON ASS1.CLI_ID = CLI.CLI_ID
WHERE 
	SUS.CLI_ID IS NOT NULL AND
	SUS.E_Mail IS NOT NULL AND
	SUS.E_Mail <> '' AND
	SUS.E_Mail <> '(null)' AND
	ASS1.UEN_ID = 1
	
	GROUP BY E_Mail
HAVING
	
	NOT EXISTS (
	   
	   SELECT 1 FROM COADCORP.dbo.ASSINATURA_EMAIL EMAIL
	   WHERE 
			EMAIL.ASN_NUM_ASSINATURA = MIN(ASS1.ASN_NUM_ASSINATURA) AND
			EMAIL.AEM_EMAIL = SUS.E_Mail
	)

GO

-- TODO: COLOCAR HAVIN AO INVEZ DE TABELA DERIVADA

-- Importa o telefone 1 do cliente da agenda velha a agenda nova por meio da primeira assinatura de franquia encontrado
INSERT INTO COADCORP.dbo.ASSINATURA_TELEFONE (ATE_TELEFONE, ASN_NUM_ASSINATURA, TIPO_TEL_ID)
SELECT 
		SUS.Phone1,
		MIN(ASS1.ASN_NUM_ASSINATURA) AS NUMERO_ASSINATURA_FRANQUIA,
		4 AS TIPO_TELEFONE 
	 FROM
		[COADSAT].dbo.SAT_SUSPECT SUS INNER JOIN 
		[COADCORP].[dbo].[CLIENTES] CLI ON CLI.CLI_ID = SUS.CLI_ID INNER JOIN 
		COADCORP.dbo.ASSINATURA ASS1 ON ASS1.CLI_ID = CLI.CLI_ID
	WHERE 
		SUS.CLI_ID IS NOT NULL AND
		SUS.Phone1 IS NOT NULL AND
		SUS.Phone1 <> '' AND
		SUS.Phone1 <> '( )            ' AND
		ASS1.UEN_ID = 1	
		
		GROUP BY Phone1 
	 
	HAVING  NOT EXISTS (
	   
	   SELECT 1 FROM COADCORP.dbo.ASSINATURA_TELEFONE TEL
	   WHERE 
			TEL.ASN_NUM_ASSINATURA = MIN(ASS1.ASN_NUM_ASSINATURA) AND
			TEL.ATE_TELEFONE = Phone1
	)
	
	
-- Importa o celular do cliente da agenda velha a agenda nova por meio da primeira assinatura de franquia encontrado
INSERT INTO COADCORP.dbo.ASSINATURA_TELEFONE (ATE_TELEFONE, ASN_NUM_ASSINATURA, TIPO_TEL_ID)
SELECT 
		SUS.Phone2,
		MIN(ASS1.ASN_NUM_ASSINATURA) AS NUMERO_ASSINATURA_FRANQUIA,
		1 AS TIPO_TELEFONE 
	 FROM
		[COADSAT].dbo.SAT_SUSPECT SUS INNER JOIN 
		[COADCORP].[dbo].[CLIENTES] CLI ON CLI.CLI_ID = SUS.CLI_ID INNER JOIN 
		COADCORP.dbo.ASSINATURA ASS1 ON ASS1.CLI_ID = CLI.CLI_ID
	WHERE 
		SUS.CLI_ID IS NOT NULL AND
		SUS.Phone2 IS NOT NULL AND
		SUS.Phone2 <> '' AND
		SUS.Phone2 <> '( )            ' AND
		ASS1.UEN_ID = 1	
		
		GROUP BY Phone2 
	 
	HAVING  NOT EXISTS (
	   
	   SELECT 1 FROM COADCORP.dbo.ASSINATURA_TELEFONE TEL
	   WHERE 
			TEL.ASN_NUM_ASSINATURA = MIN(ASS1.ASN_NUM_ASSINATURA) AND
			TEL.ATE_TELEFONE = Phone2
	)
	