
CREATE PROCEDURE "FactionsList" 

	@faction nvarchar(50)

AS
BEGIN

	SELECT

		r.[M_RecepientName]		AS FullName,
		SUM(t.M_CashAmount)		AS Amount,
		r.[M_RecepientAccNo]	AS AccNumber

	FROM [Ms_DataCollector].[dbo].[Ms_Recepients] r

	INNER JOIN [Ms_DataCollector].[dbo].[Ms_Transactions] t ON r.[M_UniqueID] = t.M_UniqueID
	LEFT JOIN [Ms_DataCollector].[dbo].[Ms_Factions] f ON r.[M_Relation] = f.ID

	WHERE f.[M_Relation] = @faction
	 AND t.[M_Quota] = @faction

	GROUP BY r.[M_RecepientName],r.[M_RecepientAccNo]

	ORDER BY r.[M_RecepientName]

END
GO
