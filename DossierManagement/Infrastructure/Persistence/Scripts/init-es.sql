IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'BraphiaHospitalDossierManagementEventStore')
BEGIN
  CREATE DATABASE BraphiaHospitalDossierManagementEventStore;
END;
GO

USE BraphiaHospitalDossierManagementEventStore
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Events')
BEGIN
  CREATE TABLE Events (
    ID INT PRIMARY KEY identity(1,1),
    AggregateId UNIQUEIDENTIFIER NOT NULL,
    Type NVARCHAR(100) NOT NULL,
    Payload NVARCHAR(MAX) NULL,
    Version INT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
  )
END