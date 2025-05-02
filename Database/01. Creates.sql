IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE [Name] = 'MoneyTracker')
BEGIN
    CREATE DATABASE MoneyTracker
END
GO

USE MoneyTracker

GO
    IF SCHEMA_ID('Catalogue') IS NULL EXECUTE('CREATE SCHEMA [Catalogue]')
GO
    IF SCHEMA_ID('Analytics') IS NULL EXECUTE('CREATE SCHEMA [Analytics]')
GO

-- 

-- Catalogue

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Catalogue' AND TABLE_NAME = 'TransactionCategory')
BEGIN
    CREATE TABLE [Catalogue].[TransactionCategory]
    (
        TransactionCategoryId INT IDENTITY(1,1) NOT NULL,
        TransactionCategoryName VARCHAR(100) NOT NULL,
        TransactionCategoryDescription VARCHAR(100) NOT NULL,
        TransactionCategoryIcon VARCHAR(100) NOT NULL,
        TransactionCategoryColor VARCHAR(6) NOT NULL,
        Created DATETIME NOT NULL DEFAULT GETDATE(),
        Modified DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT PK_TransactionCategory PRIMARY KEY (TransactionCategoryId),
        CONSTRAINT UK_TransactionCategoryName UNIQUE (TransactionCategoryName),
        CONSTRAINT UK_TransactionCategoryIcon UNIQUE (TransactionCategoryIcon),
        CONSTRAINT UK_TransactionCategoryColor UNIQUE (TransactionCategoryColor)
    )
END
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Catalogue' AND TABLE_NAME = 'TransactionType')
BEGIN
    CREATE TABLE [Catalogue].[TransactionType]
    (
        TransactionTypeId INT IDENTITY(1,1) NOT NULL,
        TransactionTypeName VARCHAR(100) NOT NULL,
        TransactionTypeDescription VARCHAR(100) NOT NULL,
        Created DATETIME NOT NULL DEFAULT GETDATE(),
        Modified DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT PK_TransactionType PRIMARY KEY (TransactionTypeId),
        CONSTRAINT UK_TransactionTypeName UNIQUE (TransactionTypeName)
    )
END
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Catalogue' AND TABLE_NAME = 'Bank')
BEGIN
    CREATE TABLE [Catalogue].[Bank]
    (
        BankId INT IDENTITY(1,1) NOT NULL,
        BankName VARCHAR(100) NOT NULL,
        Created DATETIME NOT NULL DEFAULT GETDATE(),
        Modified DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT PK_Bank PRIMARY KEY (BankId),
        CONSTRAINT UK_BankName UNIQUE (BankName)
    )
END
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Catalogue' AND TABLE_NAME = 'BudgetType')
BEGIN
    CREATE TABLE [Catalogue].[BudgetType]
    (
        BudgetTypeId INT IDENTITY(1,1) NOT NULL,
        BudgetTypeName VARCHAR(100) NOT NULL,
        BudgetTypeDays INT NOT NULL,
        Created DATETIME NOT NULL DEFAULT GETDATE(),
        Modified DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT PK_BudgetType PRIMARY KEY (BudgetTypeId),
        CONSTRAINT UK_BudgetTypeName UNIQUE (BudgetTypeName)
    )
END
GO

--

-- Analytics

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Analytics' AND TABLE_NAME = 'Budget')
BEGIN
    CREATE TABLE [Analytics].[Budget]
    (
        BudgetId INT IDENTITY(1,1) NOT NULL,
        TransactionCategoryId INT NOT NULL,
        BudgetTypeId INT NOT NULL,
        BudgetAmount DECIMAL(18,2) NOT NULL,
        Created DATETIME NOT NULL DEFAULT GETDATE(),
        Modified DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT PK_Budget PRIMARY KEY (BudgetId),
        CONSTRAINT FK_Budget_TransactionCategoryId FOREIGN KEY (TransactionCategoryId) REFERENCES [Catalogue].[TransactionCategory](TransactionCategoryId),
        CONSTRAINT FK_Budget_BudgetType FOREIGN KEY (BudgetTypeId) REFERENCES [Catalogue].[BudgetType](BudgetTypeId)
    )
END
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Analytics' AND TABLE_NAME = 'Transaction')
BEGIN
    CREATE TABLE [Analytics].[Transaction]
    (
        TransactionId INT IDENTITY(1,1) NOT NULL,
        TransactionCategoryId INT NOT NULL,
        TransactionTypeId INT NOT NULL,
        BankId INT NOT NULL,
        TransactionAmount DECIMAL(18,2) NOT NULL,
        TransactionDate DATETIME NOT NULL,
        TransactionDescription VARCHAR(150) NOT NULL,
        Created DATETIME NOT NULL DEFAULT GETDATE(),
        Modified DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT PK_Transaction PRIMARY KEY (TransactionId),
        CONSTRAINT FK_Transaction_TransactionCategory FOREIGN KEY (TransactionCategoryId) REFERENCES [Catalogue].[TransactionCategory](TransactionCategoryId),
        CONSTRAINT FK_Transaction_TransactionType FOREIGN KEY (TransactionTypeId) REFERENCES [Catalogue].[TransactionType](TransactionTypeId),
        CONSTRAINT FK_Transaction_Bank FOREIGN KEY (BankId) REFERENCES [Catalogue].[Bank](BankId)
    )
END
GO