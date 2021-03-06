SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
BEGIN TRANSACTION TX_ASS
BEGIN TRY

DECLARE @Table TABLE(
	PRO_ID INT ,-- CRIO A VARIÁVEL DO TIPO TABELA
	ASN_ANO_COAD INT, 
	ASN_CORTESIA INT ,
	CLI_ID INT ,
	UEN_ID INT,
	COD_ASSINATURA CHAR(8));
-------------------------------------------------------------------------------------------------------------------------------------------------
INSERT INTO @Table -- JOGO O RESULTADO NA TABELA
	SELECT TOP 10
		40 AS PRO_ID,
		49 AS ASN_ANO_COAD,
		0 AS ASN_CORTESIA,
		CLI.CLI_ID AS CLI_ID,
		1 AS UEN_ID,
		[COADCORP_TEST].[dbo].GERAR_COD_ASSI(40, MONTH(CLI.DATA_CADASTRO), ((ROW_NUMBER() OVER(ORDER BY CLI.CLI_ID)) -1 )) AS COD_ASSINATURA
	FROM
			COADSAT_HOMOL.dbo.SAT_SUSPECT SUS 
			INNER JOIN COADCORP_TEST.dbo.CLIENTES_VW CLI ON SUS.CLI_ID = CLI.CLI_ID
	WHERE NOT EXISTS (
		SELECT  1
		FROM COADCORP_TEST.dbo.ASSINATURA ASS
		WHERE ASS.CLI_ID = CLI.CLI_ID  
		AND UEN_ID = 1
	)
	
-------------------------------------------------------------------------------------------------------------------------------------------------

INSERT INTO [COADCORP_TEST].dbo.ASSINATURA  ( -- INSIRO A ASSINATURA
	PRO_ID,
	ASN_ANO_COAD,
	ASN_CORTESIA,
	CLI_ID,
	UEN_ID,
	ASN_NUM_ASSINATURA
)
SELECT * FROM
@Table 
	ORDER BY
	SUBSTRING(COD_ASSINATURA,1,3);
-------------------------------------------------------------------------------------------------------------------------------------------------
 
UPDATE SEQ_PRO
 SET SEQ_PRO.SEQUENCIA = SUBSTRING(T1.SEQ,1,4)
FROM [corporativo2].[dbo].[SEQ_PROD_COPIA] AS SEQ_PRO
	INNER JOIN
	(SELECT	
			T.COD_CORTADO,
			T.SEQ,
			SUBSTRING(T.COD_CORTADO,1,2) AS PRO_ID,
			SUBSTRING(T.COD_CORTADO,3,3) AS LETRA
	FROM 		
		(
		SELECT 
			SUBSTRING(COD_ASSINATURA,1,3) AS COD_CORTADO,
			MAX(SUBSTRING(COD_ASSINATURA,4,8)) AS SEQ
		FROM @Table
		GROUP BY SUBSTRING(COD_ASSINATURA,1,3)
		) AS T) AS T1 
		
		ON SEQ_PRO.COD_PROD = T1.PRO_ID AND T1.LETRA = SEQ_PRO.LETRA;
-------------------------------------------------------------------------------------------------------------------------------------------------

	
COMMIT TRANSACTION TX_ASS

END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION TX_ASS
	SELECT
	 ERROR_NUMBER() AS ErrorNumber
    ,ERROR_SEVERITY() AS ErrorSeverity
    ,ERROR_STATE() AS ErrorState
    ,ERROR_PROCEDURE() AS ErrorProcedure
    ,ERROR_LINE() AS ErrorLine
    ,ERROR_MESSAGE() AS ErrorMessage;
END CATCH
