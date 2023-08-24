CREATE PROCEDURE [dbo].[RecepientsCopier_II]

AS

BEGIN

    MERGE INTO [Ms_DataCollector].[dbo].[Ms_Recepients_II]

    USING 
    (
        (SELECT  *
        FROM    (SELECT 
						[Recepient], 
                        ROW_NUMBER() OVER (PARTITION BY [Recepient] ORDER BY [Recepient]) AS RowNumber
                FROM   Ms_Collection_II) AS a
        WHERE  RowNumber = '1')         

    )
    X   
        (							
		     [Recepient]
		    ,[RowNo]
        )

    ON
        (
		    [Ms_Recepients_II].[Recepient]		= X.[Recepient]	
        )											  
	 
    WHEN NOT MATCHED BY TARGET THEN

    INSERT
        (
             [Recepient]
        )

     VALUES
        (
             X.[Recepient]
        );

END;