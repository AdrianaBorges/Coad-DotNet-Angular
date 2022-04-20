
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
BEGIN TRANSACTION CADASTRAR_NOVO_SUSPECT
BEGIN TRY

	DECLARE @KEY INT = (SELECT AutoKey
				FROM [COADSAT].[dbo].[SAT_TABSEQUENCES]
				WHERE ObjectCode = 90000001 + 1);
					
	INSERT INTO dbo.SAT_SUSPECT(
		
		CardCode,
		DocEntry,
		CardFName,
		CardName,
		CntctPrsn,
		Phone1,
		Cellular,
		E_Mail,
		U_TAXID,
		Block,
		City,
		State1,
		CardType,
		PARA_REGIAO,
		CreateDate)
		VALUES(
		@KEY,
		@KEY,
		 'Cadastro de teste',
		 'Cadastro de teste',
		 'Diego',
		 '21 99999999',
		 '21 999999999',
		 'dasilva@coad.com.br',
		 '333333333333',
		 'Bairro',
		 'RJ',
		 'S',
		 1,
		 GETDATE()	 	
		)
		
		
	UPDATE [COADSAT].[dbo].[SAT_TABSEQUENCES]
	SET AutoKey = @KEY
	WHERE ObjectCode = 90000001

	INSERT INTO [COADSAT].[dbo].SAT_CARTEIRAMENTO (CARDCODE, SLPCODE, DT_INICIO, STATUS,UEN)
	VALUES
	(@KEY,3,GETDATE(), 'A', '03' )
	
	INSERT INTO [COADSAT].[dbo].SAT_PRIORIDADE (CARDCODE, COMENTARIO, DEMANDANTE, ORIGEM, STATUS, SLPCODE, UEN, DATA_PRIORIDADE)
	VALUES(@KEY, 'Teste de insersão e execução do job', 'teste', 'R', 'A', 2, '03', GETDATE)

  COMMIT TRANSACTION CADASTRAR_NOVO_SUSPECT
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION CADASTRAR_NOVO_SUSPECT
	SELECT
	 ERROR_NUMBER() AS ErrorNumber
    ,ERROR_SEVERITY() AS ErrorSeverity
    ,ERROR_STATE() AS ErrorState
    ,ERROR_PROCEDURE() AS ErrorProcedure
    ,ERROR_LINE() AS ErrorLine
    ,ERROR_MESSAGE() AS ErrorMessage;
END CATCH
