
CREATE PROCEDURE [dbo].[TransactionsCopier]

AS

SET IDENTITY_INSERT [DB_MSota].[dbo].[Ms_Transactions] ON

BEGIN

    MERGE INTO [DB_MSota].[dbo].[Ms_Transactions]

    USING 
    (
        (SELECT  

             [ID]                
            ,[Code]              
            ,[Code_ID]           
            ,[M_Date]            
            ,[M_RecepientPhoneNo]
            ,[M_CashAmount]      
            ,[M_Balance]         
            ,[M_PayBill_TillNo]  
            ,[M_TransactionCost] 
            ,[M_Quota]           
            ,[M_FulizaLimit]     
            ,[M_FulizaBorrowed]  
            ,[M_FulizaCharge]    
            ,[M_FulizaAmount]    

        FROM    [Ms_Collection]   )  

    )
    X   
        (							
		     X_ID                
            ,X_Code              
            ,X_Code_ID           
            ,X_Date            
            ,X_RecepientPhoneNo
            ,X_CashAmount      
            ,X_Balance         
            ,X_PayBill_TillNo  
            ,X_TransactionCost 
            ,X_Quota           
            ,X_FulizaLimit     
            ,X_FulizaBorrowed  
            ,X_FulizaCharge    
            ,X_FulizaAmount   
        )

    ON
        (
		    [Ms_Transactions].[Code_ID]	= X.X_Code_ID		
	    AND [Ms_Transactions].[M_Date]  = X.X_Date
        )											  
	 
    WHEN NOT MATCHED BY TARGET THEN

    INSERT
        (
             [ID]                
            ,[Code]              
            ,[Code_ID]           
            ,[M_Date]            
            ,[M_RecepientPhoneNo]
            ,[M_CashAmount]      
            ,[M_Balance]         
            ,[M_PayBill_TillNo]  
            ,[M_TransactionCost] 
            ,[M_Quota]           
            ,[M_FulizaLimit]     
            ,[M_FulizaBorrowed]  
            ,[M_FulizaCharge]    
            ,[M_FulizaAmount]  
        )

     VALUES
        (
             X.X_ID                
            ,X.X_Code              
            ,X.X_Code_ID           
            ,X.X_Date            
            ,X.X_RecepientPhoneNo
            ,X.X_CashAmount      
            ,X.X_Balance         
            ,X.X_PayBill_TillNo  
            ,X.X_TransactionCost 
            ,X.X_Quota           
            ,X.X_FulizaLimit     
            ,X.X_FulizaBorrowed  
            ,X.X_FulizaCharge    
            ,X.X_FulizaAmount  
        );

END;

SET IDENTITY_INSERT [DB_MSota].[dbo].[Ms_Transactions] OFF
