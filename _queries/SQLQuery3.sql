/****** Script for SelectTopNRows command from SSMS  ******/
INSERT INTO dbo.Students 
      (FirstName,LastName,DateOfBirth,NationalID,CourseID,DateOfAdmission)

 VALUES ('Vandame','Jerry','1997-02-13','1234567','BBIT','2022-02-13');
 -- FROM [testsqldb].[dbo].[Students]

 SELECT * FROM Students

 DELETE FROM Students WHERE FirstName ='marry'

EXEC sp_help Students

ALTER TABLE Students
DROP CONSTRAINT PK__Students__32C52A79782BBD41

ALTER TABLE Students 
 ALTER COLUMN StudentID AS 'SP02/'+ ID PERSISTED;

 select CAST(year(getdate()) AS VARCHAR(20))

 SELECT 'SP02/'+CAST(ID AS VARCHAR(20)) +'/' +CAST(year(getdate()) AS VARCHAR(20)) FROM Students

 ALTER TABLE Students
 ALTER COLUMN StudentID 

ALTER TABLE Students
ADD StudentID AS RIGHT('SP02/'+ CAST(ID AS VARCHAR(20)) +'/' +CAST(year(getdate()) AS VARCHAR(20)),20)

ALTER TABLE Students
DROP COLUMN StudentID
 ,
 FirstName VARCHAR NULL,
 LastName VARCHAR NULL,
 DateOfBirth DATE NULL,
 NationalID INT NULL,
 CourseID VARCHAR NULL,
 DateOfAdmission DATE NULL,
 PRIMARY KEY (StudentID)
);

