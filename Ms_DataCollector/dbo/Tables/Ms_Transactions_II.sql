CREATE TABLE [dbo].[Ms_Transactions_II]
(
	DocNum				NVARCHAR(10) NOT NULL,
    DocEntry			VARCHAR(255) NOT NULL PRIMARY KEY,
	TranId			    VARCHAR(255) NOT NULL,
    DocDateTime		    DATETIME NOT NULL,
	TranAmount	        FLOAT(53) NOT NULL,
	Balance			    FLOAT(53) NOT NULL,
    Charges		        FLOAT(53) NOT NULL,
    DocType		        VARCHAR(255),
    Quota				VARCHAR(255) NOT NULL
)
