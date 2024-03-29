﻿CREATE TABLE Ms_Collection
(
		ID					INT NOT NULL IDENTITY (1, 1),
		Code				varchar(255) NOT NULL,
		--Code_ID				AS RIGHT('000000000' + CAST(ID as varchar(10)), 6) + Code,
        [M_UniqueID] VARCHAR(255) NOT NULL,
		M_Date				VARCHAR(255),
        M_Readable_date		DATETIME,
		M_RecepientName		varchar(255),
        M_RecepientPhoneNo	varchar(255),
        --M_RecepientDate		varchar(255),
        M_RecepientAccNo	varchar(255),
		M_CashAmount		FLOAT(53),
		M_Balance			FLOAT(53),
		M_szProtocol		varchar(255),
		M_PayBill_TillNo	varchar(255),
		M_TransactionCost	FLOAT(53),
        M_Address			varchar(255),
        M_Type				varchar(255),
        M_Subject			varchar(255),
        M_Body				varchar(2000),
        M_Toa				varchar(255),
        M_Sc_toa			varchar(255),
        M_Service_center	varchar(255),
        M_Read				varchar(255),
        M_Locked			varchar(255),
        M_Date_sent			varchar(255),
		M_Status			varchar(255),
        M_Sub_id			varchar(255),
        
        M_Quota				varchar(255),
        M_FulizaLimit		FLOAT(53),
        M_FulizaBorrowed	FLOAT(53),
        M_FulizaCharge		FLOAT(53),
        M_FulizaAmount		FLOAT(53)
    
);