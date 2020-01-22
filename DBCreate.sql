CREATE DATABASE RFID_DB
GO

USE RFID_DB
GO

CREATE TABLE [RfIdCard] (
	[IDCard]		int CONSTRAINT PK_RFIDCard PRIMARY KEY IDENTITY,
	[SerialNumber]	nvarchar(20) UNIQUE not null
)

CREATE TABLE [User] (
	[IDUser]		int CONSTRAINT PK_PERSON PRIMARY KEY IDENTITY,
	[Name]			nvarchar(30) not null,
	[Surname]		nvarchar(30) not null,
	[OIB]			nvarchar(13) not null,
	[Address]		nvarchar(40)
)

CREATE TABLE [RfIdReader] (
	[IDReader]		int CONSTRAINT PK_Reader PRIMARY KEY IDENTITY,
	[IPAddress]		nvarchar(15) not null,
	[Location]		nvarchar(50)
)

CREATE TABLE [RfIdUser] (
	[IDRfIdUser]	int CONSTRAINT PK_RFID_USER PRIMARY KEY IDENTITY,
	[UserId]		int CONSTRAINT FK_RFID_USER_USER FOREIGN KEY REFERENCES [User](IDUser),
	[CardId]		int CONSTRAINT FK_RFID_USER_CARD FOREIGN KEY REFERENCES [RFIDCard](IDCard)
)

CREATE TABLE [EntryLog] (
	[IDEntry]		int CONSTRAINT PK_ENTRY PRIMARY KEY IDENTITY,
	[RfIdUserId]	int CONSTRAINT FK_ENTRY_RFID_USER FOREIGN KEY REFERENCES [RfIdUser](IDRfIdUser),
	[RfIdReaderId]	int CONSTRAINT FK_ENTRY_RFID_READER FOREIGN KEY REFERENCES [RfIdReader](IDReader),
	[EntryTime]		datetime not null
)

--DROP TABLE [EntryLog]
--DROP TABLE [RfIdUser]
--DROP TABLE [RfIdReader]
--DROP TABLE [User]
--DROP TABLE [RfIdCard]

INSERT INTO [RfIdCard] (SerialNumber) VALUES ('12345678')
INSERT INTO [RfIdCard] (SerialNumber) VALUES ('11223344')
INSERT INTO [RfIdCard] (SerialNumber) VALUES ('88776655')
INSERT INTO [RfIdCard] (SerialNumber) VALUES ('87654321')

SELECT * FROM [RfIdReader]

INSERT INTO [RfIdReader] ([IPAddress], [Location]) VALUES ('172.19.124.111', 'Location A1')
INSERT INTO [RfIdReader] ([IPAddress], [Location]) VALUES ('172.19.124.112', 'Location B1')
INSERT INTO [RfIdReader] ([IPAddress], [Location]) VALUES ('172.19.124.113', 'Location C1')
INSERT INTO [RfIdReader] ([IPAddress], [Location]) VALUES ('172.19.124.114', 'Location D1')

SELECT * FROM [User]
SELECT * FROM [RfIdCard]

INSERT INTO [User] ([Name], [Surname], [OIB], [Address]) VALUES ('Pero', 'Peric', '1234567890123', 'Perina 22, 10000 Zagreb')
INSERT INTO [User] ([Name], [Surname], [OIB], [Address]) VALUES ('Mali', 'Mujo',  '1231231231231', 'Mujina 23, 10000 Zagreb')
INSERT INTO [User] ([Name], [Surname], [OIB], [Address]) VALUES ('Dino', 'Raða',  '1122334455667', 'Žnjan 22, 40000 Split')

SELECT * FROM [RfIdUser]

INSERT INTO [RfIdUser] ([UserId], [CardId]) VALUES (1, 1)
INSERT INTO [RfIdUser] ([UserId], [CardId]) VALUES (2, 2)


SELECT * FROM [EntryLog]

SELECT * FROM RfIdReader

