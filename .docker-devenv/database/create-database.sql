-- Create SimpleDatabase
CREATE DATABASE SimpleDatabase
GO

SELECT Name FROM sys.Databases
GO

-- Use SimpleDatabase
USE SimpleDatabase
GO

CREATE TABLE SimpleTodos (
    Id INT NOT NULL IDENTITY,
    Name VARCHAR(50) NOT NULL,
    Description VARCHAR(100) NOT NULL,
    PRIMARY KEY (Id)
);
GO

INSERT INTO SimpleTodos(Name, Description) VALUES("Shopping", "Go and do the shopping at the supermarket.")
GO

INSERT INTO SimpleTodos(Name, Description) VALUES("Tidying", "Tidy the house and put stuff away.")
GO
