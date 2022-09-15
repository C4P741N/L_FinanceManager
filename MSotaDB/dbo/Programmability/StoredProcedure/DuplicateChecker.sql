CREATE PROCEDURE [dbo].[Ms_DuplicateChecker]
(
@szCode						nvarchar(250),
@szDate						nvarchar(250),
@szRecepientName			nvarchar(250),
@szRecepientPhoneNo			nvarchar(250),
@szRecepientDate			nvarchar(250),
@szRecepientAccNo			nvarchar(250),
@szCashAmount				nvarchar(250),
@szBalance					nvarchar(250),
@szszProtocol				nvarchar(250),
@szTransactionStatus		nvarchar(250),
@szTransactionCost			nvarchar(250),
@szAddress					nvarchar(250),
@szType						nvarchar(250),
@szSubject					nvarchar(250),
@szBody						nvarchar(2000),
@szToa						nvarchar(250),
@szSc_toa					nvarchar(250),
@szService_center			nvarchar(250),
@szRead						nvarchar(250),
@szLocked					nvarchar(250),
@szDate_sent				nvarchar(250),
@szStatus					nvarchar(250),
@szSub_id					nvarchar(250),
@szReadable_date			nvarchar(250),
@szszContact_name			nvarchar(250),
@szQuota					nvarchar(250),
@szFulizaLimit				nvarchar(250),
@szFulizaBorrowed			nvarchar(250),
@szFulizaCharge				nvarchar(250),
@szFulizaAmount				nvarchar(250)	
)

AS

BEGIN
 
	DECLARE @dCode					NVARCHAR(253)
	DECLARE @dM_TransactionStatus	NVARCHAR(253)

MERGE INTO [DB_MSota].[dbo].[Ms_Collection]

USING
(values(
		  @szCode 
		 ,@szDate				
		 ,@szRecepientName	
		 ,@szRecepientPhoneNo	
		 ,@szRecepientDate	
		 ,@szRecepientAccNo	
		 ,@szCashAmount		
		 ,@szBalance			
		 ,@szszProtocol		
		 ,@szTransactionStatus
		 ,@szTransactionCost	
		 ,@szAddress			
		 ,@szType				
		 ,@szSubject			
		 ,@szBody				
		 ,@szToa				
		 ,@szSc_toa			
		 ,@szService_center	
		 ,@szRead				
		 ,@szLocked			
		 ,@szDate_sent		
		 ,@szStatus			
		 ,@szSub_id			
		 ,@szReadable_date	
		 ,@szszContact_name	
		 ,@szQuota			
		 ,@szFulizaLimit		
		 ,@szFulizaBorrowed	
		 ,@szFulizaCharge		
		 ,@szFulizaAmount		
		)
) 

X(
		 [Code]					
		,[M_Date]				
		,[M_RecepientName]		
		,[M_RecepientPhoneNo]	
		,[M_RecepientDate]		
		,[M_RecepientAccNo]		
		,[M_CashAmount]			
		,[M_Balance]				
		,[M_szProtocol]			
		,[M_TransactionStatus]	
		,[M_TransactionCost]		
		,[M_Address]				
		,[M_Type]				
		,[M_Subject]				
		,[M_Body]				
		,[M_Toa]					
		,[M_Sc_toa]				
		,[M_Service_center]		
		,[M_Read]				
		,[M_Locked]				
		,[M_Date_sent]			
		,[M_Status]				
		,[M_Sub_id]				
		,[M_Readable_date]		
		,[M_szContact_name]		
		,[M_Quota]				
		,[M_FulizaLimit]			
		,[M_FulizaBorrowed]		
		,[M_FulizaCharge]		
		,[M_FulizaAmount]		
)

	ON
  (
		[Ms_Collection].[Code]				  = @szCode 
	AND [Ms_Collection].[M_TransactionStatus] = @szTransactionStatus 
  )											  
	 
WHEN NOT MATCHED BY TARGET THEN

 INSERT
 (
		 [Code]
        ,[M_Date]
        ,[M_RecepientName]
        ,[M_RecepientPhoneNo]
        ,[M_RecepientDate]
        ,[M_RecepientAccNo]
        ,[M_CashAmount]
        ,[M_Balance]
        ,[M_szProtocol]
        ,[M_TransactionStatus]
        ,[M_TransactionCost]
        ,[M_Address]
        ,[M_Type]
        ,[M_Subject]
        ,[M_Body]
        ,[M_Toa]
        ,[M_Sc_toa]
        ,[M_Service_center]
        ,[M_Read]
        ,[M_Locked]
        ,[M_Date_sent]
        ,[M_Status]
        ,[M_Sub_id]
        ,[M_Readable_date]
        ,[M_szContact_name]
        ,[M_Quota]
        ,[M_FulizaLimit]
        ,[M_FulizaBorrowed]
        ,[M_FulizaCharge]
        ,[M_FulizaAmount]
 )
 --VALUES(X.[Code], X.[M_Date], X.[M_RecepientName], X.[M_TransactionStatus]);

	VALUES
	(
		 X.[Code]
        ,X.[M_Date]
        ,X.[M_RecepientName]
        ,X.[M_RecepientPhoneNo]
        ,X.[M_RecepientDate]
        ,X.[M_RecepientAccNo]
        ,X.[M_CashAmount]
        ,X.[M_Balance]
        ,X.[M_szProtocol]
        ,X.[M_TransactionStatus]
        ,X.[M_TransactionCost]
        ,X.[M_Address]
        ,X.[M_Type]
        ,X.[M_Subject]
        ,X.[M_Body]
        ,X.[M_Toa]
        ,X.[M_Sc_toa]
        ,X.[M_Service_center]
        ,X.[M_Read]
        ,X.[M_Locked]
        ,X.[M_Date_sent]
        ,X.[M_Status]
        ,X.[M_Sub_id]
        ,X.[M_Readable_date]
        ,X.[M_szContact_name]
        ,X.[M_Quota]
        ,X.[M_FulizaLimit]
        ,X.[M_FulizaBorrowed]
        ,X.[M_FulizaCharge]
        ,X.[M_FulizaAmount]
	);

 END;