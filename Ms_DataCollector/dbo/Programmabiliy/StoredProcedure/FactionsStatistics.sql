CREATE PROCEDURE [dbo].[FactionsStatistics]
	@fromDate datetime,
	@toDate datetime

AS
BEGIN

	SELECT

	 [M_Quota] AS GroupName
	 , SUM([M_CashAmount]) AS GroupTotal

	FROM[Ms_DataCollector].[dbo].[Ms_Transactions]
	 
	WHERE [M_Date] BETWEEN @fromDate 
					AND @toDate

	GROUP BY[M_TransactionCost],[M_Quota]

END
