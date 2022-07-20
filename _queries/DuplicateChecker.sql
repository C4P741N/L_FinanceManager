DECLARE 
@Code				NVARCHAR(253),
@M_TransactionStatus	NVARCHAR(253)

MERGE INTO [MSota].[dbo].[Ms_Collection]

USING
(values(
	@Code, @M_TransactionStatus)) 

X(
	[Code], [M_TransactionStatus]
)

ON(
		[Ms_Collection].[Code]					= @Code 
	AND [Ms_Collection].[M_TransactionStatus]	= @M_TransactionStatus 
  )
	 
WHEN NOT MATCHED BY TARGET THEN

 INSERT([Code],[M_Date],[M_RecepientName])
 VALUES(X.[Code], X.[M_Date], X.[M_RecepientName]);
