﻿CREATE TABLE [dbo].[Author]
(
    [Id] BIGINT IDENTITY (1,1) NOT NULL, 
    [FirstName] NVARCHAR(50) NOT NULL, 
    [LastName] NVARCHAR(50) NOT NULL 

    CONSTRAINT PK_Author PRIMARY KEY (Id)
)
