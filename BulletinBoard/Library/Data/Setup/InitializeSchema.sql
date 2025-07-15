-- Create Categories table
IF
NOT EXISTS (SELECT * FROM sysobjects WHERE name='Categories' AND xtype='U')
BEGIN
CREATE TABLE Categories
(
    Id   UNIQUEIDENTIFIER PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL UNIQUE
);
END

-- Create Subcategories table
IF
NOT EXISTS (SELECT * FROM sysobjects WHERE name='Subcategories' AND xtype='U')
BEGIN
CREATE TABLE Subcategories
(
    Id   UNIQUEIDENTIFIER PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    ParentId UNIQUEIDENTIFIER NOT NULL,

    FOREIGN KEY (ParentId) REFERENCES Categories (Id)
);
END

-- Create Announcements table
IF
NOT EXISTS (SELECT * FROM sysobjects WHERE name='Announcements' AND xtype='U')
BEGIN
CREATE TABLE Announcements
(
    Id            UNIQUEIDENTIFIER PRIMARY KEY,
    Title         NVARCHAR(200) NOT NULL,
    Description   NVARCHAR(MAX),
    CreatedDate   DATETIME         NOT NULL DEFAULT GETDATE(),
    Status        INT              NOT NULL,
    SubcategoryId UNIQUEIDENTIFIER NOT NULL,

    FOREIGN KEY (SubcategoryId) REFERENCES Subcategories (Id)
);
END

-- Create Users table
IF
NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
BEGIN
CREATE TABLE Users
(
    Id       UNIQUEIDENTIFIER PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email    NVARCHAR(50) UNIQUE NOT NULL,
    Password NVARCHAR(100) NOT NULL
);
END
