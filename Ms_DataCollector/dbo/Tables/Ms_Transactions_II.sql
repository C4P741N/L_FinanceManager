CREATE TABLE [dbo].[Ms_Transactions_II]
(
	DocNum				NVARCHAR(10) NOT NULL,
    DocEntry			VARCHAR(255) NOT NULL PRIMARY KEY,
	TranId			    VARCHAR(255) NOT NULL,
    DocDateTime		    DATETIME NOT NULL,
	TranAmount	        DECIMAL(20,10) NOT NULL,
	Balance			    DECIMAL(20,10) NOT NULL,
    Charges		        DECIMAL(20,10) NOT NULL,
    DocType		        VARCHAR(255),
    TranType            CHAR NOT NULL,
    Quota				VARCHAR(255) NOT NULL
)
