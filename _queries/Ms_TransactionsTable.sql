CREATE TABLE [dbo].[Ms_Transactions] (
    [ID]                 INT            IDENTITY (1, 1) NOT NULL,
    [Code]               VARCHAR (255)  NOT NULL,
    [Code_ID]            VARCHAR (255)  NOT NULL,
    [M_Date]             VARCHAR (255)  NOT NULL,
    [M_RecepientPhoneNo] VARCHAR (255)  NOT NULL,
    [M_CashAmount]       FLOAT (53)     NULL,
    [M_Balance]          FLOAT (53)     NULL,
    [M_PayBill_TillNo]   VARCHAR (255)  NULL,
    [M_TransactionCost]  FLOAT (53)     NULL,
    [M_Quota]            VARCHAR (255)  NOT NULL,
    [M_FulizaLimit]      FLOAT (53)     NULL,
    [M_FulizaBorrowed]   FLOAT (53)     NULL,
    [M_FulizaCharge]     FLOAT (53)     NULL,
    [M_FulizaAmount]     FLOAT (53)     NULL
);

