CREATE OR ALTER PROC CreateBook
@title NVARCHAR(500), @authorID INT,
@picturePath NVARCHAR(MAX), @price DECIMAL,
@loanPrice DECIMAL, @ISBN NVARCHAR(50),
@statusID INT, @quantity INT, @desc NVARCHAR(MAX)
AS
INSERT INTO 
	Book([Title], [IDAuthor], 
	[PicturePath], [Price], 
	[LoanPrice], [ISBN], 
	[IDStatus], [Quantity], 
	[Description])
VALUES
	(@title,  @authorID, @picturePath,  @price, @loanPrice, @ISBN, @statusID, @quantity, @desc)
GO

CREATE OR ALTER PROC GetBook
@id INT
AS
SELECT * FROM Book
WHERE IDBook = @id
GO

CREATE OR ALTER PROC GetByTitle
@title NVARCHAR(1000)
AS
SELECT IDBook, IDStatus, Title, ISBN, IDAuthor, Price, LoanPrice, PicturePath, [Description], (
	SELECT SUM(Quantity)
	FROM Book
	WHERE Title = @title
) AS Quantity  FROM Book
WHERE Title = @title
GO

CREATE OR ALTER PROC GetAllBook
AS
SELECT DISTINCT Title, IDAuthor, Price, PicturePath FROM Book
GROUP BY Title, IDAuthor, Price, PicturePath
GO

CREATE OR ALTER PROC GetAllBooksByReading
@option NVARCHAR(5)
AS
IF (@option = 'MOST')
	BEGIN
		SELECT DISTINCT Title, IDAuthor, Price, PicturePath, Demand
		FROM Book AS b
		ORDER BY Demand DESC, Title
	END
ELSE
	BEGIN
		SELECT DISTINCT Title, IDAuthor, Price, PicturePath, Demand
		FROM Book AS b
		ORDER BY Demand ASC	, Title
END
GO

CREATE OR ALTER PROC UpdateBook
@id INT,
@title NVARCHAR(500), @authorID INT,
@picturePath NVARCHAR(MAX), @price DECIMAL,
@loanPrice DECIMAL, @ISBN NVARCHAR(50),
@statusID INT, @quantity INT, @desc NVARCHAR(MAX)
AS
UPDATE Book
SET Title = @title, IDAuthor = @authorID, PicturePath = @picturePath,  Price = @price,
	LoanPrice = @loanPrice, ISBN = @ISBN, IDStatus = @statusID, Quantity = @quantity, 
	Description = @desc
WHERE IDBook = @id
GO

CREATE OR ALTER PROC UpdateToUsedBook
@id INT,
@title NVARCHAR(500), @authorID INT,
@picturePath NVARCHAR(MAX), @price DECIMAL,
@loanPrice DECIMAL, @ISBN NVARCHAR(50)
, @quantity INT, @desc NVARCHAR(MAX)
AS
IF NOT EXISTS(SELECT * FROM Book WHERE Title = @title AND IDStatus = 2)
	BEGIN
		INSERT INTO
			Book([Title], [IDAuthor], 
				[PicturePath], [Price], 
				[LoanPrice], [ISBN], 
				[IDStatus], [Quantity], 
				[Description])
		VALUES
			(@title,  @authorID, @picturePath,  @price, @loanPrice, @ISBN, 2, @quantity, @desc)
	END
ELSE
	BEGIN
		UPDATE Book
			SET Title = @title, @authorID = @authorID, PicturePath = @picturePath,  Price = @price,
				LoanPrice = @loanPrice, ISBN = @ISBN, Quantity = @quantity, Description = @desc
			WHERE Title = @title
	END
GO

CREATE OR ALTER PROC RemoveBook
@id INT
AS
UPDATE Book
SET Quantity = Quantity - 1
WHERE IDBook = @id
GO

CREATE OR ALTER PROC DeleteBook
@id INT
AS
UPDATE Book
SET Quantity = 0
WHERE IDBook = @id
GO

CREATE OR ALTER PROC GetQntOfBook
@title nvarchar(1000), @statusID INT
AS
SELECT Quantity FROM Book
WHERE Title = @title AND IDStatus = @statusID
GO


-- AUTHORS -- 

CREATE OR ALTER PROC CreateAuthor
@fname NVARCHAR(1000), 
@desc NVARCHAR(1000), @picturePath NVARCHAR(MAX)
AS
INSERT INTO Author([FullName], [Description], [PicturePath])
VALUES (@fname, @desc, @picturePath)
GO

CREATE OR ALTER PROC GetAuthor
@id INT
AS
SELECT * FROM Author
WHERE IDAuthor = @id
GO

CREATE OR ALTER PROC GetAllAuthor
AS
SELECT * FROM Author
GO

CREATE OR ALTER PROC UpdateAuthor
@id INT, @fname NVARCHAR(1000), 
@desc NVARCHAR(1000), @picturePath NVARCHAR(MAX)
AS
UPDATE Author
SET FullName = @fname, Description = @desc, PicturePath = @picturePath
WHERE IDAuthor = @id
GO

CREATE OR ALTER PROC DeleteAuthor
@id INT
AS
UPDATE Author
SET IsDeleted = 1
WHERE IDAuthor = @id
GO

CREATE OR ALTER PROC GetUserPassword
@id INT
AS
SELECT PasswordHash FROM [User]
WHERE IDUser = @id
GO

CREATE OR ALTER PROC ChangePassword
@ID INT, @pass NVARCHAR(MAX)
AS
UPDATE [User]
SET PasswordHash = @pass
WHERE IDUser = @ID
GO

GO


-- STATUS

CREATE OR ALTER PROC GetStatus
@id	INT
AS
SELECT * FROM Status
WHERE IDStatus = @id
GO

CREATE OR ALTER PROC GetAllStatus
AS
SELECT * FROM Status
GO

-- USER --

CREATE OR ALTER PROCEDURE CreateUser
@fname NVARCHAR(100), @lname NVARCHAR(100),
@birth DATE, @email NVARCHAR(1000),
@pass NVARCHAR(MAX), @city NVARCHAR(100),
@zip INT, @address NVARCHAR(100)
AS
DECLARE @n INT
SELECT @n = COUNT(*) FROM [User] WHERE DAY(GETDATE()) = DAY(CreatedAt)
INSERT INTO [User]
	([FirstName], [LastName], [DateOfBirth], 
	[Email], [PasswordHash], [CityName], 
	[ZipCode], [Address], [CreatedAt] ,[UserCode])
VALUES
	(@fname, @lname, @birth,
	@email, @pass, @city, @zip, @address, GETDATE(),
	CASE  
		WHEN LEN(@n) = 1 THEN CONCAT('U', DAY(GETDATE())+MONTH(GETDATE()), RIGHT(YEAR(GETDATE()), 2), '000' , @n)
		WHEN LEN(@n) = 1 THEN CONCAT('U', DAY(GETDATE())+MONTH(GETDATE()), RIGHT(YEAR(GETDATE()), 2),  '00' , @n)
		WHEN LEN(@n) = 1 THEN CONCAT('U', DAY(GETDATE())+MONTH(GETDATE()), RIGHT(YEAR(GETDATE()), 2),   '0' , @n)
		WHEN LEN(@n) = 1 THEN CONCAT('U', DAY(GETDATE())+MONTH(GETDATE()), RIGHT(YEAR(GETDATE()), 2),    '' , @n)
	END)
GO

USE [DB_PRA]
GO

/****** Object:  StoredProcedure [dbo].[CreateEmployee]    Script Date: 17.6.2022. 10:02:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER   PROCEDURE [dbo].[CreateEmployee]
@fname NVARCHAR(100), @lname NVARCHAR(100),
@birth DATETIME, @email NVARCHAR(1000),
@pass NVARCHAR(MAX), @oib NVARCHAR(11),
@workPosition NVARCHAR(100)
AS
DECLARE @n INT
SELECT @n = COUNT(*) FROM [User] WHERE DAY(GETDATE()) = DAY(CreatedAt)
INSERT INTO [User]
	([FirstName], [LastName], [DateOfBirth], [IsAdmin], 
	[Email], [PasswordHash],[OIB],[CreatedAt], [WorkPosition] , [UserCode])
VALUES
	(@fname, @lname, @birth, 1,
	@email, @pass, @oib, GETDATE(), @workPosition,
	CASE  
		WHEN LEN(@n) = 1 THEN CONCAT('A', DAY(GETDATE())+MONTH(GETDATE()), RIGHT(YEAR(GETDATE()), 2), '000' , @n)
		WHEN LEN(@n) = 1 THEN CONCAT('A', DAY(GETDATE())+MONTH(GETDATE()), RIGHT(YEAR(GETDATE()), 2),  '00' , @n)
		WHEN LEN(@n) = 1 THEN CONCAT('A', DAY(GETDATE())+MONTH(GETDATE()), RIGHT(YEAR(GETDATE()), 2),   '0' , @n)
		WHEN LEN(@n) = 1 THEN CONCAT('A', DAY(GETDATE())+MONTH(GETDATE()), RIGHT(YEAR(GETDATE()), 2),    '' , @n)
	END)
GO

CREATE OR ALTER PROC GetUser
@id INT
AS
SELECT * FROM [User]
WHERE IDUser = @id AND DeletedAt IS NULL
GO

CREATE OR ALTER PROC GetUserByEmail
@email NVARCHAR(100)
AS
SELECT * FROM [User]
WHERE Email = @email
GO

CREATE OR ALTER PROC GetAllUser
AS
SELECT * FROM [User] WHERE DeletedAt IS NULL
GO

CREATE OR ALTER PROCEDURE UpdateUser
@id INT,
@fname NVARCHAR(100), @lname NVARCHAR(100),
@birth DATE, @email NVARCHAR(1000),
@pass NVARCHAR(MAX), @city NVARCHAR(100),
@zip INT, @address NVARCHAR(100)
AS
UPDATE [User]
SET FirstName = @fname, LastName = @lname,
	DateOfBirth = @birth, Email = @email,
	PasswordHash = @pass, CityName = @city,
	ZipCode = @zip, Address = @address
WHERE IDUser = @id
GO

CREATE OR ALTER PROC DeleteUser
@id INT
AS
UPDATE [User] 
SET DeletedAt = GETDATE(), FirstName = 'XXX', LastName = 'XXX'
WHERE IDUser = @id
GO

CREATE OR ALTER PROC ConfirmEmail
@id INT
AS
UPDATE [User]
SET EmailConfirmed = 1
WHERE IDUser = @id
GO

CREATE OR ALTER PROC ConfirmPassword
@id INT
AS
UPDATE [User]
SET PasswordConfirmed = 1
WHERE IDUser = @id
GO

CREATE OR ALTER PROC [dbo].[AuthUser]
	@email NVARCHAR(100),
	@password NVARCHAR(128)
AS
BEGIN
	SELECT * FROM [User] WHERE Email = @email AND PasswordHash=@password AND DeletedAt IS NULL
END
GO


CREATE OR ALTER PROC CheckEmail
@ID INT
AS
SELECT EmailConfirmed FROM [User] WHERE IDUser = @ID
GO
-- PURCHASE --

CREATE OR ALTER PROCEDURE PurchaseBook
@userid INT, @bookID INT
AS
INSERT INTO Purchase([IDUser], [IDBook], [PurchaseDate])
VALUES	(@userid, @bookID, GETDATE())

DECLARE @title NVARCHAR(1000)
SELECT @title = Title FROM Book WHERE IDBook = @bookID 

DECLARE @qnt INT
SELECT @qnt = Quantity FROM Book WHERE IDBook = @bookID 

UPDATE Book
SET Quantity = @qnt - 1
WHERE IDBook = @bookID 

DECLARE @dmnd INT
SELECT @dmnd = Demand from Book WHERE Title = @title
ORDER BY Demand ASC

IF EXISTS (SELECT * FROM Book WHERE Title = @title AND IDStatus = 2)
	BEGIN
		UPDATE Book
		SET Demand = @dmnd + 1
		WHERE Title = @title
	END
ELSE 
	BEGIN 
		INSERT INTO Book([IDAuthor], [Description], [IDStatus], [ISBN], [LoanPrice], [PicturePath], [Price], [Quantity], [Title])
		SELECT IDAuthor, Description, 2, ISBN, LoanPrice, PicturePath, Price, Quantity, Title FROM Book
		WHERE Title = @title

		UPDATE Book
		SET Demand = @dmnd + 1
		WHERE Title = @title
	END
GO

CREATE OR ALTER PROCEDURE GetAllPurchase
@userid INT
AS
SELECT * FROM Purchase
WHERE IDUser = @userid
GO

-- LOAN --

CREATE OR ALTER PROC LoanBook
@userid INT, @title NVARCHAR(1000), @delayPrice DECIMAL,
@beginDATE DATETIME, @endDATE DATETIME
AS
IF EXISTS (SELECT * FROM Book WHERE Title = @title AND IDStatus = 2 AND Quantity > 0 )
	BEGIN
		DECLARE @bookid INT
		SELECT @bookid = IDBook FROM Book WHERE Title = @title AND IDStatus = 2
		
		INSERT INTO Loan([IDUser], [IDBook], [DelayPrice] , [LoanBeginDate], [LoanEndDate])
		VALUES (@userid, @bookid, @delayPrice, @beginDATE, @endDATE)

		UPDATE Book
		SET Quantity = Quantity - 1
		WHERE Title = @title AND IDStatus = 2
	END
ELSE IF EXISTS (SELECT * FROM Book WHERE Title = @title)
	BEGIN
		DECLARE @book INT
		SELECT @book = IDBook FROM Book WHERE Title = @title AND IDStatus = 1
		
		INSERT INTO Loan([IDUser], [IDBook], [DelayPrice] , [LoanBeginDate], [LoanEndDate])
		VALUES (@userid, @book, @delayPrice, @beginDATE, @endDATE)

		UPDATE Book
		SET Quantity = Quantity - 1
		WHERE Title = @title AND IDStatus = 1

		INSERT INTO 
			Book([Title], [IDAuthor], 
				[PicturePath], [Price], 
				[LoanPrice], [ISBN],
				[Quantity],[Description]
				,[IDStatus])
			SELECT Title, IDAuthor, PicturePath, Price, LoanPrice, ISBN, 0, Description, 2 FROM Book
			WHERE Title = @title
	END

UPDATE Book
SET Demand = Demand + 1
WHERE Title = @title
GO




CREATE OR ALTER PROC RefreshLoan
@userID INT
AS
IF EXISTS (SELECT * FROM Loan WHERE IDUser = @userID  AND GETDATE() > LoanEndDate)
	BEGIN
		UPDATE Loan
		SET TotalDelayAmount = (DATEDIFF(DAY, LoanEndDate, GETDATE()) * DelayPrice)
		WHERE IDUser = @userID 
	END
GO

CREATE OR ALTER PROCEDURE GetAllLoan
@userid INT
AS
SELECT * FROM Loan
WHERE IDUser = @userid
GO

CREATE OR ALTER PROC GettOngoingLoans
AS
SELECT * FROM Loan
WHERE IsSettled = 0
GO

CREATE OR ALTER PROC DeleteLoan
@loanID INT
AS
UPDATE Loan
SET IsSettled = 1
WHERE IDLoan = @loanID

DECLARE @bookID INT, @title NVARCHAR(1000)
SELECT @bookID = IDBook FROM Loan
WHERE IDLoan = @loanID

SELECT @title = Title FROM Book WHERE IDBook = @bookID

IF EXISTS (SELECT * FROM Book WHERE Title = @title AND IDStatus = 2)
	BEGIN
		UPDATE Book
		SET Quantity = Quantity + 1
		WHERE Title = @title AND IDStatus = 2
	END
ELSE
	BEGIN
	INSERT INTO 
			Book([Title], [IDAuthor], 
				[PicturePath], [Price], 
				[LoanPrice], [ISBN],
				[Quantity],[Description]
				,[IDStatus])
			SELECT Title, IDAuthor, PicturePath, Price, LoanPrice, ISBN, 1, Description, 2 FROM Book
			WHERE Title = @title
	END
GO












