IF
NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'AnnouncementsDB')
BEGIN
    CREATE
DATABASE AnnouncementsDB;
END
GO

USE AnnouncementsDB;
GO

-- Create Categories table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Categories' AND xtype='U')
BEGIN
CREATE TABLE Categories
(
    Id   UNIQUEIDENTIFIER PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL
);
END
GO

-- Create Announcements table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Announcements' AND xtype='U')
BEGIN
CREATE TABLE Announcements
(
    Id            UNIQUEIDENTIFIER PRIMARY KEY,
    Title         NVARCHAR(200) NOT NULL,
    Description   NVARCHAR(MAX),
    CreatedDate   DATETIME NOT NULL DEFAULT GETDATE(),
    Status        INT      NOT NULL,
    CategoryId    UNIQUEIDENTIFIER NULL,
    SubcategoryId UNIQUEIDENTIFIER NULL,

    FOREIGN KEY (CategoryId) REFERENCES Categories (Id),
    FOREIGN KEY (SubcategoryId) REFERENCES Categories (Id)
);
END
GO

-- Create Users table
IF
NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
BEGIN
CREATE TABLE Users
(
    Id       UNIQUEIDENTIFIER PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email    UNIQUEIDENTIFIER NVARCHAR(50) NOT NULL,
    Password NVARCHAR(100) NOT NULL
);
END
