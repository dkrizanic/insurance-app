-- Create database if it doesn't exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'InsuranceApp')
BEGIN
    CREATE DATABASE InsuranceApp;
END
GO

USE InsuranceApp;
GO

-- Create Partners table
CREATE TABLE Partners (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(255) NOT NULL CHECK (LEN(FirstName) >= 2),
    LastName NVARCHAR(255) NOT NULL CHECK (LEN(LastName) >= 2),
    Address NVARCHAR(MAX),
    PartnerNumber NVARCHAR(20) NOT NULL CHECK (LEN(PartnerNumber) = 20),
    CroatianPIN NVARCHAR(11) NULL,
    PartnerTypeId INT NOT NULL CHECK (PartnerTypeId IN (1, 2)),
    CreatedAtUtc DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreateByUser NVARCHAR(255) NOT NULL CHECK (CreateByUser LIKE '%@%'),
    IsForeign BIT NOT NULL,
    ExternalCode NVARCHAR(20) NOT NULL UNIQUE CHECK (LEN(ExternalCode) >= 10 AND LEN(ExternalCode) <= 20),
    Gender CHAR(1) NOT NULL CHECK (Gender IN ('M', 'F', 'N'))
);

-- Create Policies table
CREATE TABLE Policies (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PartnerId INT NOT NULL,
    PolicyNumber NVARCHAR(15) NOT NULL CHECK (LEN(PolicyNumber) >= 10 AND LEN(PolicyNumber) <= 15),
    Amount DECIMAL(18,2) NOT NULL,
    CreatedAtUtc DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT FK_Policies_Partners FOREIGN KEY (PartnerId) REFERENCES Partners(Id),
    CONSTRAINT UQ_PolicyNumber UNIQUE (PolicyNumber)
);

-- Create indexes for better performance
CREATE INDEX IX_Partners_CreatedAtUtc ON Partners(CreatedAtUtc DESC);
CREATE INDEX IX_Partners_PartnerNumber ON Partners(PartnerNumber);
CREATE INDEX IX_Policies_PartnerId ON Policies(PartnerId);