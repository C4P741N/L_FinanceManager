//using System.Text.RegularExpressions;

//Select
//     [Ms_Recepients].[M_RecepientName]      AS RecepientName
//    , SUM([Ms_Transactions].M_CashAmount)  AS Amount
//  FROM [DB_MSota].[dbo].[Ms_Recepients]
//JOIN[DB_MSota].[dbo].[Ms_Transactions] ON[Ms_Recepients].[M_RecepientPhoneNo] = [Ms_Transactions].[M_RecepientPhoneNo]

//  GROUP BY[Ms_Recepients].[M_RecepientName]