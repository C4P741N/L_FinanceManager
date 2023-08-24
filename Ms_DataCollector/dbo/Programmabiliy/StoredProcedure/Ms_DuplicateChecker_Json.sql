CREATE PROCEDURE [dbo].[Ms_DuplicateChecker_Json]
(
	@DocEntry			nvarchar(250),
	@TranId			    nvarchar(250),
	@LongDate			numeric(18, 0),
	@DocDateTime		datetime,
	@Recepient		    nvarchar(250),
	@AccNo	            nvarchar(250),
	@TranAmount	        float(53),
	@Balance			float(53),
	@Charges		    float(53),
	@DocType		    nvarchar(250),
	@Service_center	    nvarchar(250),
	@IsRead				int,
	@Quota				nvarchar(250),
    @TranType           char,
	@Body				nvarchar(MAX)		
)

AS

BEGIN

MERGE INTO [Ms_DataCollector].[dbo].[Ms_Collection_II]

USING
(values(
		  @DocEntry		
		  ,@TranId			
		  ,@LongDate		
		  ,@DocDateTime	
		  ,@Recepient		
		  ,@AccNo	        
		  ,@TranAmount	    
		  ,@Balance		
		  ,@Charges		
		  ,@DocType		
		  ,@Service_center	
		  ,@IsRead			
		  ,@Quota	
          ,@TranType
		  ,@Body			
		)
) 

X(
		 [DocEntry]
         ,[TranId]
         ,[LongDate]
         ,[DocDateTime]
         ,[Recepient]
         ,[AccNo]
         ,[TranAmount]
         ,[Balance]
         ,[Charges]
         ,[DocType]
         ,[Service_center]
         ,[IsRead]
         ,[Quota]
         ,[TranType]
         ,[Body]
)

	ON
  (
		[Ms_Collection_II].[TranId]		= @TranId 
	AND [Ms_Collection_II].[Body]		= @Body 
  )											  
	 
WHEN NOT MATCHED BY TARGET THEN

 INSERT
 (
		[DocEntry]
         ,[TranId]
         ,[LongDate]
         ,[DocDateTime]
         ,[Recepient]
         ,[AccNo]
         ,[TranAmount]
         ,[Balance]
         ,[Charges]
         ,[DocType]
         ,[Service_center]
         ,[IsRead]
         ,[Quota]
         ,[TranType]
         ,[Body]
 )
 --VALUES(X.[Code], X.[M_Date], X.[M_RecepientName], X.[M_PayBill_TillNo]);

	VALUES
	(
		  X.[DocEntry]
         ,X.[TranId]
         ,X.[LongDate]
         ,X.[DocDateTime]
         ,X.[Recepient]
         ,X.[AccNo]
         ,X.[TranAmount]
         ,X.[Balance]
         ,X.[Charges]
         ,X.[DocType]
         ,X.[Service_center]
         ,X.[IsRead]
         ,X.[Quota]
         ,X.[TranType]
         ,X.[Body]
	);

 END;

BEGIN
    EXECUTE [RecepientsCopier_II]
END;

BEGIN
    EXECUTE [TransactionsCopier_II]
END;