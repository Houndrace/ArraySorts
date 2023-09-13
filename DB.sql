create database ArraySorting
go
use ArraySorting
go

CREATE TABLE [Arrays] (
    Id INT IDENTITY(1,1) NOT NULL,
    [Data] NVARCHAR(MAX) NOT NULL,
    ItemQuantity INT NOT NULL,

	CONSTRAINT Arrays_pk
        PRIMARY KEY (Id)
);

CREATE TABLE SortingTypes (
    Id INT IDENTITY(1,1) NOT NULL,
    [Type_name] NVARCHAR(MAX) NOT NULL,

	CONSTRAINT SortingTypes_pk
        PRIMARY KEY (Id)
);

CREATE TABLE Sortings (
    Id INT IDENTITY(1,1) NOT NULL,
	TypeId INT NOT NULL,
    [StartDate] DATE NOT NULL,  
    [StartTime] TIME NOT NULL,
	TimeResult INT NOT NULL,
    OriginalArrayId INT NOT NULL,

	CONSTRAINT Sortings_pk
        PRIMARY KEY (Id),
	CONSTRAINT Sortings_SortingType_id_fk
		FOREIGN KEY (TypeId) REFERENCES SortingTypes
	    ON UPDATE CASCADE ON DELETE CASCADE,
	CONSTRAINT Sortings_Array_id_fk
		FOREIGN KEY (OriginalArrayId) REFERENCES [Arrays]
	    ON UPDATE CASCADE ON DELETE CASCADE
);

-- —оздание таблицы дл€ хранени€ данных массивов
