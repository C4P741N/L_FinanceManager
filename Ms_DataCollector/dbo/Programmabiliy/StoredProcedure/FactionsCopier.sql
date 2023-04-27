CREATE PROCEDURE [dbo].[FactionsCopier]
AS
BEGIN

	IF NOT EXISTS (SELECT 1 FROM [Ms_DataCollector].[dbo].[Ms_Factions])
	  BEGIN 

		INSERT INTO [Ms_DataCollector].[dbo].[Ms_Factions]
				SELECT DISTINCT M_Quota FROM Ms_Collection;

		UPDATE Ms_Recepients
			SET  Ms_Recepients.M_Relation = [Ms_Factions].ID
			FROM Ms_Transactions
			INNER JOIN [Ms_Factions] ON Ms_Transactions.M_Quota = [Ms_Factions].M_Relation
			WHERE Ms_Recepients.M_UniqueID = Ms_Transactions.M_UniqueID;
		
	  END 

	--ELSE
	  --BEGIN
		--Do Another Thing
	  --END

END
GO
