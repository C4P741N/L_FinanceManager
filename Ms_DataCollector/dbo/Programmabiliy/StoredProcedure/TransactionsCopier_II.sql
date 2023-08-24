CREATE PROCEDURE [dbo].[TransactionsCopier_II]

AS

BEGIN

    MERGE INTO [Ms_DataCollector].[dbo].[Ms_Transactions_II]

    USING 
    (
        (SELECT
			 R.DocNum		
			,C.DocEntry	
			,C.TranId		
			,C.DocDateTime	
			,C.TranAmount	
			,C.Balance		
			,C.Charges		
			,C.DocType		
			,C.Quota
			,C.TranType
    
		FROM [Ms_Collection_II] C
		JOIN [Ms_Recepients_II] R ON R.[Recepient] = C.[Recepient]  )  

    )
    X   
        (							
		      X_DocNum		
			 ,X_DocEntry	
			 ,X_TranId		
			 ,X_DocDateTime	
			 ,X_TranAmount	
			 ,X_Balance		
			 ,X_Charges		
			 ,X_DocType		
			 ,X_Quota	
			 ,X_TranType
        )

    ON
        (
		    [Ms_Transactions_II].DocEntry	= X.X_DocEntry		
	    AND [Ms_Transactions_II].TranId		= X.X_TranId
        )											  
	 
    WHEN NOT MATCHED BY TARGET THEN

    INSERT
        (
             DocNum		
			,DocEntry	
			,TranId		
			,DocDateTime	
			,TranAmount	
			,Balance		
			,Charges		
			,DocType		
			,Quota	
			,TranType
        )

     VALUES
        (
              X.X_DocNum		
			 ,X.X_DocEntry	
			 ,X.X_TranId		
			 ,X.X_DocDateTime	
			 ,X.X_TranAmount	
			 ,X.X_Balance		
			 ,X.X_Charges		
			 ,X.X_DocType		
			 ,X.X_Quota 
			 ,X.X_TranType 
        );

END;
