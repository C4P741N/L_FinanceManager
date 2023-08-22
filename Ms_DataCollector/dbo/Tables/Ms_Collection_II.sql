CREATE TABLE Ms_Collection_II
(
		DocNum				INT NOT NULL IDENTITY (1, 1),
        DocEntry			VARCHAR(255) NOT NULL,
		TranId			    VARCHAR(255) NULL,
	    LongDate			    INT NOT NULL,
        DocDateTime		    DATETIME NOT NULL,
		Recepient		    VARCHAR(255) NOT NULL,
        AccNo	            varchar(255),
		TranAmount	        FLOAT(53) NOT NULL,
		Balance			    FLOAT(53) NOT NULL,
        Charges		        FLOAT(53) NOT NULL,
        DocType		        VARCHAR(255),
        Service_center	    VARCHAR(255),
        IsRead				INT,
        Quota				VARCHAR(255) NOT NULL,
        Body				varchar(MAX) NOT NULL
);