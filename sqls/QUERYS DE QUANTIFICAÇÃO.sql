
SELECT  [CardCode]
      ,[CardName]
      ,[CardType]      
      ,[U_TAXID]
      ,[PARA_REGIAO]
      ,[IMPORTADO]
      ,[CLI_ID]
      ,[FLAG_VALIDO]
       FROM COADSAT_HOMOL.dbo.SAT_SUSPECT SUS
WHERE FLAG_VALIDO = 1 
AND CardCode NOT IN 

(SELECT * from
((
SELECT SUS.CardCode
FROM
	COADSAT_HOMOL.dbo.SAT_SUSPECT SUS 
	INNER JOIN COADCORP_TEST.dbo.CLIENTES CLI 
		ON (CLI.CLI_FLAG_VALIDO = 1 
					AND
					(SUS.CardName = CLI.CLI_NOME) AND 
					(
						(SUS.U_TAXID IS NULL AND CLI.CLI_CPF_CNPJ IS NULL) OR 
						(SUS.U_TAXID = CLI.CLI_CPF_CNPJ)))
			 
	WHERE SUS.FLAG_VALIDO = 1
				AND SUS.CLI_ID IS NULL
				AND CLI.SUS_ID_REF IS NULL)
				
				
				
UNION

(SELECT SUS.CardCode
FROM
	COADSAT_HOMOL.dbo.SAT_SUSPECT SUS 
	INNER JOIN COADCORP_TEST.dbo.CLIENTES CLI 
		ON CLI.SUS_ID_REF = SUS.CARDCODE
WHERE SUS.FLAG_VALIDO = 1 AND CLI.CLI_FLAG_VALIDO = 1)) as t)
---------------------------------------------------------------------------------------------------------------

SELECT
(SELECT COUNT(*)
FROM
	COADSAT_HOMOL.dbo.SAT_SUSPECT SUS 
	INNER JOIN COADCORP_TEST.dbo.CLIENTES CLI 
		ON (CLI.CLI_FLAG_VALIDO = 1 
					AND
					(SUS.CardName = CLI.CLI_NOME) AND 
					(
						(SUS.U_TAXID IS NULL AND CLI.CLI_CPF_CNPJ IS NULL) OR 
						(SUS.U_TAXID = CLI.CLI_CPF_CNPJ)))
			 
	WHERE SUS.FLAG_VALIDO = 1
				AND CLI.SUS_ID_REF IS NULL)
				
				
				
+
(SELECT COUNT(*)
FROM
	COADSAT_HOMOL.dbo.SAT_SUSPECT SUS 
	INNER JOIN COADCORP_TEST.dbo.CLIENTES CLI 
		ON CLI.SUS_ID_REF = SUS.CARDCODE
WHERE SUS.FLAG_VALIDO = 1 AND CLI.CLI_FLAG_VALIDO = 1)

-------------------------------------------------------------------- QUANTIDADE DE HISTORICO
/****** Script do comando SelectTopNRows de SSMS  ******/
SELECT COUNT(*)
  FROM [COADSAT_HOMOL].[dbo].[SAT_SUSPATIVID] SAT_SUS
  INNER JOIN [COADSAT_HOMOL].[dbo].[SAT_SUSPECT] SUSP ON SAT_SUS.CardCode = SUSP.CardCode
  WHERE SUSP.FLAG_VALIDO = 1
  
  -------------------------------------------------------------------------------QUANTIDADE DE EMAILS
/****** Script do comando SelectTopNRows de SSMS  ******/
SELECT COUNT(*) FROM [COADSAT_HOMOL].[dbo].[SAT_SUSPECT] SUS
  WHERE SUS.FLAG_VALIDO = 1 AND 
  SUS.E_Mail IS NOT NULL AND
			SUS.E_Mail <> '' AND
			SUS.E_Mail <> '(null)'
---------------------------------------------------------------------------------- CARTEIRAMENTOS
SELECT COUNT(*) FROM
(
SELECT DISTINCT SUS.CardCode, OP.SlpCode
FROM
	COADSAT_HOMOL.dbo.SAT_CARTEIRAMENTO CAR 
		INNER JOIN COADSAT_HOMOL.dbo.SAT_SUSPECT SUS ON CAR.CARDCODE = SUS.CardCode
	INNER JOIN COADSAT_HOMOL.dbo.SAT_OPERADOR OP ON OP.SLPCode = CAR.SLPCODE
WHERE SUS.FLAG_VALIDO = 1 AND CAR.STATUS = 'A'
AND OP.REP_ID IS NOT NULL) AS T
  ---------------------------------------------------------------------------------------
  select COUNT(*) FROM
  (
  SELECT DISTINCT
		CLI.CLI_ID,
		CART02.CAR_ID
		FROM COADSAT_HOMOL.dbo.SAT_SUSPECT SUS
			INNER JOIN COADCORP_TEST.dbo.CLIENTES CLI ON SUS.CLI_ID = CLI.CLI_ID		
			INNER JOIN COADSAT_HOMOL.dbo.SAT_CARTEIRAMENTO CART ON SUS.CardCode = CART.CARDCODE
			INNER JOIN COADSAT_HOMOL.dbo.SAT_OPERADOR OP ON CART.SLPCODE = OP.SlpCode
			INNER JOIN COADCORP_TEST.dbo.REPRESENTANTE REP ON REP.REP_ID = OP.REP_ID
			INNER JOIN COADCORP_TEST.dbo.CARTEIRA_REPRESENTANTE CAR_REP ON REP.REP_ID = CAR_REP.REP_ID
			INNER JOIN COADCORP_TEST.dbo.CARTEIRA CART02 ON CART02.CAR_ID = CAR_REP.CAR_ID
	WHERE 
		SUS.FLAG_VALIDO = 1 AND CLI.CLI_FLAG_VALIDO = 1 AND
		CART.[STATUS] = 'A' 
		AND 
		CAR_REP.CAR_ID = (
			
			SELECT MIN(CART02_SUB.CAR_ID) FROM COADCORP_TEST.dbo.CARTEIRA_REPRESENTANTE CAR_REP_SUB INNER JOIN 
			COADCORP_TEST.dbo.CARTEIRA CART02_SUB ON CART02_SUB.CAR_ID = CAR_REP_SUB.CAR_ID
			WHERE CAR_REP_SUB.REP_ID = REP.REP_ID AND CART02_SUB.UEN_ID = 1
		)
	AND CART02.UEN_ID = 1
	AND NOT EXISTS (SELECT 1 FROM COADCORP_TEST.dbo.CARTEIRA_CLIENTE CCLI WHERE CCLI.CAR_ID = CART02.CAR_ID AND CCLI.CLI_ID = CLI.CLI_ID)
) AS T

---------------------------------------------- QUANTIDADE DE AGENDAMENTOS ---------------------------------------------
SELECT COUNT(*) 
FROM
	COADSAT_HOMOL.dbo.SAT_SUSPECT SUS 
WHERE SUS.FLAG_VALIDO = 1 AND SUS.ToDate IS NOT NULL
AND SUS.ToDate >= GETDATE() 