INSERT INTO COADCORP_TEST.dbo.CLIENTES 
(	CLI_NOME, 
	CLI_A_C, 
	CLI_CPF_CNPJ,
	CLA_CLI_ID,
	DATA_CADASTRO,
	TIPO_CLI_ID)
SELECT TOP 3
		CardName AS CLI_NOME,
		CntctPrsn AS CONTATO,
		U_TAXID CLI_CPF_CNPJ,
		(CASE 
			WHEN CardType = 'S' THEN 1
			WHEN CardType = 'P' THEN 2
			WHEN CardType = 'C' THEN 3 
		END) AS CLA_CLI_ID,
		CreateDate AS CLI_DATA_INCLUSAO,
		2 AS TIPO_CLI_ID
	FROM
	COADSAT_HOMOL.dbo.SAT_SUSPECT SUS 
	WHERE NOT EXISTS
	(SELECT 1
		FROM
			COADCORP_TEST.dbo.CLIENTES_VW CLI 
		WHERE (SUS.CardName = CLI.CLI_NOME) AND 
			((SUS.U_TAXID IS NULL AND CLI.CLI_CPF_CNPJ IS NULL) OR 
			 (SUS.U_TAXID = CLI.CLI_CPF_CNPJ))
	)

	
	