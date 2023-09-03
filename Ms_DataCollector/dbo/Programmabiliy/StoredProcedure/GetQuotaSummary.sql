CREATE PROCEDURE [dbo].[GetQuotaSummary]

AS
BEGIN

	SELECT 

        SUM([TranAmount]) AS Amount,
        [Quota] AS Quota

    FROM [Ms_DataCollector].[dbo].[Ms_Transactions_II]

    WHERE [Quota] NOT IN ('None', 'InvalidTransaction')

    GROUP BY [Quota]

END;