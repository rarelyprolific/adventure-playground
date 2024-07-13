CREATE DATABASE SimpleDatabase
GO

SELECT Name from sys.Databases
GO

CREATE TABLE SimpleTodos (
    Id INT NOT NULL IDENTITY,
    Name VARCHAR(50) NOT NULL,
    Description VARCHAR(100) NOT NULL,
    PRIMARY KEY (Id)
);
GO
