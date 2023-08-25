CREATE TABLE [dbo].[Ms_Recepients_II]
(
		ID				INT NOT NULL IDENTITY (1, 1),
		DocNum AS 'C' + CAST(ID AS NVARCHAR(10)) PERSISTED PRIMARY KEY,
		Recepient			varchar(255) NOT NULL,
		Relation			INT
    
)
