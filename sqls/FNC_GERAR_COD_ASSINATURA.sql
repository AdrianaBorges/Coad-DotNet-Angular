-- ================================================
-- Template generated from Template Explorer using:
-- Create Scalar Function (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the function.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
ALTER FUNCTION dbo.GERAR_COD_ASSI
(
	@PRO_ID INT,
	@MES INT
)
RETURNS INT
AS
BEGIN
	-- Declare the return variable here
	DECLARE @COD_ASSI CHAR(8), @LETRA CHAR(1), @SEQ INT
	
	SET @LETRA = dbo.RETORNA_LETRA(@MES);
	SET @SEQ = (SELECT 
				SEQUENCIA 
				FROM [corporativo2].[dbo].[SEQ_PROD_COPIA] 
				WHERE 
					LETRA = @LETRA AND COD_PROD = @PRO_ID);
					
	SET @SEQ+= 1;				
	
	/*
	UPDATE 
		[corporativo2].[dbo].[SEQ_PROD_COPIA]
		SET SEQUENCIA = @SEQ 
	WHERE LETRA = @LETRA AND COD_PROD = @PROD_ID;
	*/
	
	SET @SEQ = REPLICATE('0',5-LEN(@SEQ)) + @SEQ;
	SET @COD_ASSI = CAST(@PRO_ID AS CHAR) + CAST(@LETRA AS CHAR) + CAST(@SEQ AS CHAR);
	
	-- Return the result of the function
	RETURN @COD_ASSI

END
GO

