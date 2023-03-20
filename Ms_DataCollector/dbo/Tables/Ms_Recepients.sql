CREATE TABLE Ms_Recepients
(
		ID					INT NOT NULL IDENTITY (1, 1),
		M_UniqueID			varchar(255) NOT NULL,
		M_RecepientName		varchar(255) NOT NULL,
        M_RecepientPhoneNo	varchar(255) NOT NULL,
        M_RecepientAccNo	varchar(255),
		RowNo				INT
);