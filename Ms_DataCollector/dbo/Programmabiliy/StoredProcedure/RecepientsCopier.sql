
CREATE PROCEDURE [dbo].[RecepientsCopier]

AS

BEGIN

    MERGE INTO [Ms_DataCollector].[dbo].[Ms_Recepients]

    USING 
    (
        (SELECT  *
        FROM    (SELECT 
						[M_RecepientName], 
                        [M_RecepientPhoneNo], 
                        [M_RecepientAccNo], 
                        [M_UniqueID],
                        ROW_NUMBER() OVER (PARTITION BY [M_UniqueID] ORDER BY [M_UniqueID]) AS RowNumber
                FROM   Ms_Collection) AS a
        WHERE  RowNumber = '1')         

    )
    X   
        (							
		     [M_RecepientName]		
		    ,[M_RecepientPhoneNo]	
		    ,[M_RecepientAccNo]	
            ,[M_UniqueID]
		    ,[RowNo]
        )

    ON
        (
		    [Ms_Recepients].[M_RecepientName]		= X.[M_RecepientName]		
	    AND [Ms_Recepients].[M_RecepientAccNo]      = X.[M_RecepientAccNo]
		AND [Ms_Recepients].[M_UniqueID]            = X.[M_UniqueID]
        )											  
	 
    WHEN NOT MATCHED BY TARGET THEN

    INSERT
        (
             [M_RecepientName]
            ,[M_RecepientPhoneNo]
            ,[M_RecepientAccNo]
            ,[M_UniqueID]
		    ,[RowNo]
        )

     VALUES
        (
             X.[M_RecepientName]
            ,X.[M_RecepientPhoneNo]
            ,X.[M_RecepientAccNo]
            ,X.[M_UniqueID]
		    ,X.[RowNo]
        );

END;