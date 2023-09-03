CREATE TABLE Ms_Collection_II
(
		DocNum				INT NOT NULL IDENTITY (1, 1),
        DocEntry			VARCHAR(255) NOT NULL,
		TranId			    VARCHAR(255) NULL,
	    LongDate			BIGINT NOT NULL,
        DocDateTime		    DATETIME NOT NULL,
		Recepient		    VARCHAR(255) NOT NULL,
        AccNo	            varchar(255),
		TranAmount	        DECIMAL(20,10) NOT NULL,
		Balance			    DECIMAL(20,10) NOT NULL,
        Charges		        DECIMAL(20,10) NOT NULL,
        DocType		        VARCHAR(255),
        Service_center	    VARCHAR(255),
        IsRead				INT,
        Quota				VARCHAR(255) NOT NULL,
        TranType 		    CHAR NOT NULL,
        Body				varchar(MAX) NOT NULL
);