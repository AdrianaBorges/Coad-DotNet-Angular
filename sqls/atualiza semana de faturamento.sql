
INSERT INTO corporativo2_HOMOL.dbo.datas_fat (DATA_FAT, PERIODO, SEMANA)
(
SELECT   
DATA_FAT,
	PERIODO, 
	SEMANA
FROM 
	corporativo2.dbo.datas_fat d1
WHERE 
	NOT EXISTS (
		SELECT 
			1 
		FROM 
			corporativo2_HOMOL.dbo.datas_fat d2
		WHERE 
		d1.DATA_FAT = d2.DATA_FAT
	)
	)
