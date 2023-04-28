CREATE PROCEDURE [dbo].[TransactionStatistics]
	@fromDate datetime,
	@toDate datetime

AS
BEGIN

	SELECT
		R.[M_RecepientName]               AS RecepientName
		,R.[M_UniqueID]                   AS RecepientID
		,SUM(T.M_CashAmount)              AS TransactionAmount
		,T.[M_Quota]                      As TranactionQuota
	FROM[Ms_DataCollector].[dbo].[Ms_Recepients] R
	
	JOIN[Ms_DataCollector].[dbo].[Ms_Transactions] T
		ON R.M_UniqueID = T.M_UniqueID
	 
	 WHERE [M_Date] BETWEEN @fromDate 
						AND @toDate

	GROUP BY R.[M_RecepientName], T.[M_Quota], R.[M_UniqueID]

END
