
CREATE PROCEDURE [dbo].[RecepientsCopier]

AS

SET IDENTITY_INSERT [DB_MSota].[dbo].[Ms_Recepients] ON

BEGIN

    MERGE INTO [DB_MSota].[dbo].[Ms_Recepients]

    USING 
    (
        (SELECT  *
        FROM    (SELECT M_RecepientName, M_RecepientPhoneNo, M_RecepientAccNo,
                        ROW_NUMBER() OVER (PARTITION BY M_RecepientPhoneNo ORDER BY M_RecepientPhoneNo) 
                        AS RowNumber
                FROM   Ms_Collection) AS a
        WHERE   a.RowNumber = 1)         

    )
    X   
        (							
		     [M_RecepientName]		
		    ,[M_RecepientPhoneNo]	
		    ,[M_RecepientAccNo]		
		    ,[RowNo]
        )

    ON
        (
		    [Ms_Recepients].[M_RecepientName]		  = X.[M_RecepientName]		
	    AND [Ms_Recepients].[M_RecepientPhoneNo]      = X.[M_RecepientPhoneNo]
        )											  
	 
    WHEN NOT MATCHED BY TARGET THEN

    INSERT
        (
             [M_RecepientName]
            ,[M_RecepientPhoneNo]
            ,[M_RecepientAccNo]
		    ,[RowNo]
        )

     VALUES
        (
             X.[M_RecepientName]
            ,X.[M_RecepientPhoneNo]
            ,X.[M_RecepientAccNo]
		    ,X.[RowNo]
        );

END;

SET IDENTITY_INSERT [DB_MSota].[dbo].[Ms_Recepients] OFF