CREATE PROCEDURE [dbo].[GetLegerSummary]

AS
BEGIN

	SELECT 
		SUM(CASE 
				WHEN TranType = 'C' 
					THEN ([TranAmount] + [Charges] )
				ELSE 0 
			END) AS Sum_CreditAmount,

		SUM(CASE 
				WHEN TranType = 'D' 
					THEN [TranAmount] 
				ELSE 0 
			END) AS Sum_DepositAmount

	FROM [Ms_DataCollector].[dbo].[Ms_Transactions_II]
	WHERE TranType IN ('C', 'D')

END;