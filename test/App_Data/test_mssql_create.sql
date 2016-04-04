CREATE DATABASE MiniBlogDB
GO

USE MiniBlogDB
GO

CREATE TABLE Users (
	Id integer NOT NULL PRIMARY KEY,
	Name NVARCHAR(50) NOT NULL
)
GO

CREATE TABLE Messages (
	Id INT NOT NULL,
	IdUser INT NOT NULL,
	Text NVARCHAR(MAX) NOT NULL,
	MessageDate date NOT NULL,
	LikeCount integer NOT NULL,
	PRIMARY KEY CLUSTERED (Id ASC), 
	CONSTRAINT FK_Messages_ToUser FOREIGN KEY (IdUser) REFERENCES Users(Id)
)
GO

CREATE TABLE Attachments (
    Id INT NOT NULL,
    Attachment NVARCHAR (MAX) NOT NULL,
    IdMessage INT NOT NULL,
    PRIMARY KEY CLUSTERED (Id ASC), 
    CONSTRAINT FK_Attachments_ToMessage FOREIGN KEY (IdMessage) REFERENCES Messages(Id)
);
GO


INSERT INTO Users (Id, Name)
VALUES (1, N'Вася');
GO
INSERT INTO Users (Id, Name)
VALUES (2, N'Петя');
GO
INSERT INTO Users (Id, Name)
VALUES (3, N'Игорь');
GO

INSERT INTO Messages (Id, IdUser, Text, MessageDate, LikeCount)
VALUES (1, 1, N'новостной ресурс',  GETDATE(), 0);
GO
INSERT INTO Messages (Id, IdUser, Text, MessageDate, LikeCount)
VALUES (2, 2, N'Хабраха́бр (он же Хабр) — многофункциональный сайт, представляющий
 собой смешение новостного сайта и коллективного блога (специализированная пресса), 
 созданный для публикации новостей, аналитических статей, мыслей, связанных с информационными 
 технологиями, бизнесом и Интернетом',  GETDATE(), 0);
GO
INSERT INTO Messages (Id, IdUser, Text, MessageDate, LikeCount)
VALUES (3, 3, N'система управления версиями',  GETDATE(), 0);
GO


INSERT INTO Attachments (Id, Attachment, IdMessage)
VALUES (1, 'https://www.ukr.net/', 1);
GO
INSERT INTO Attachments (Id, Attachment, IdMessage)
VALUES (2, 'https://habrahabr.ru/', 2);
GO
INSERT INTO Attachments (Id, Attachment, IdMessage)
VALUES (3, 'https://github.com/', 3);
GO
INSERT INTO Attachments (Id, Attachment, IdMessage)
VALUES (4, 'https://bitbucket.org/', 3);
GO
