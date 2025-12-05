CREATE TABLE Customer (
    CustomerID INT IDENTITY(1,1) CONSTRAINT PK_Customer PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    CustomerType VARCHAR(20) 
        CONSTRAINT CK_Customer_CustomerType CHECK (CustomerType IN ('Household','Business','Government')),
    Address VARCHAR(150) NOT NULL,
    City VARCHAR(50) NOT NULL,
    ContactNumber VARCHAR(15) NOT NULL CONSTRAINT UQ_Customer_ContactNumber UNIQUE,
    Email VARCHAR(100) CONSTRAINT UQ_Customer_Email UNIQUE,
    RegistrationDate DATE CONSTRAINT DF_Customer_RegistrationDate DEFAULT (GETDATE()),
    Status VARCHAR(10) CONSTRAINT CK_Customer_Status CHECK (Status IN ('Active','Inactive'))
        CONSTRAINT DF_Customer_Status DEFAULT 'Active'
);

CREATE TABLE UtilityType (
    UtilityID INT IDENTITY(1,1) CONSTRAINT PK_UtilityType PRIMARY KEY,
    UtilityName VARCHAR(30) NOT NULL CONSTRAINT UQ_UtilityType_UtilityName UNIQUE,
    Unit VARCHAR(10) NOT NULL,
    Description VARCHAR(100)
);

CREATE TABLE Employee (
    EmployeeID INT IDENTITY(1,1) CONSTRAINT PK_Employee PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Role VARCHAR(20) 
        CONSTRAINT CK_Employee_Role CHECK (Role IN ('Admin','Field Officer','Cashier','Manager')),
    Username VARCHAR(50) NOT NULL CONSTRAINT UQ_Employee_Username UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    ContactNumber VARCHAR(15) NOT NULL,
    Email VARCHAR(100) CONSTRAINT UQ_Employee_Email UNIQUE,
    HireDate DATE CONSTRAINT DF_Employee_HireDate DEFAULT (GETDATE()),
    Status VARCHAR(10) 
        CONSTRAINT CK_Employee_Status CHECK (Status IN ('Active','Inactive'))
        CONSTRAINT DF_Employee_Status DEFAULT 'Active'
);

CREATE TABLE Meter (
    MeterID INT IDENTITY(1,1) CONSTRAINT PK_Meter PRIMARY KEY,
    CustomerID INT NOT NULL,
    UtilityID INT NOT NULL,
    InstallationDate DATE NOT NULL,
    Location VARCHAR(100) NOT NULL,
    Status VARCHAR(15) 
        CONSTRAINT CK_Meter_Status CHECK (Status IN ('Active','Faulty','Disconnected'))
        CONSTRAINT DF_Meter_Status DEFAULT 'Active',
    
    CONSTRAINT FK_Meter_Customer FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID),
    CONSTRAINT FK_Meter_UtilityType FOREIGN KEY (UtilityID) REFERENCES UtilityType(UtilityID)
);

CREATE TABLE MeterReading (
    ReadingID INT IDENTITY(1,1) CONSTRAINT PK_MeterReading PRIMARY KEY,
    MeterID INT NOT NULL,
    ReadingDate DATE NOT NULL,
    PreviousReading DECIMAL(10,2) DEFAULT 0,
    CurrentReading DECIMAL(10,2) NOT NULL,
    Consumption AS (CurrentReading - PreviousReading) PERSISTED,
    RecordedBy INT NOT NULL,
    Notes VARCHAR(150),

    CONSTRAINT FK_MeterReading_Meter FOREIGN KEY (MeterID) REFERENCES Meter(MeterID),
    CONSTRAINT FK_MeterReading_Employee FOREIGN KEY (RecordedBy) REFERENCES Employee(EmployeeID)
);

CREATE TABLE Tariff (
    TariffID INT IDENTITY(1,1) CONSTRAINT PK_Tariff PRIMARY KEY,
    UtilityID INT NOT NULL,
    EffectiveFrom DATE NOT NULL,
    EffectiveTo DATE NULL,
    FixedCharge DECIMAL(10,2) DEFAULT 0,
    SlabStart DECIMAL(10,2) NOT NULL,
    SlabEnd DECIMAL(10,2) NULL,
    RatePerUnit DECIMAL(10,2) NOT NULL,

    CONSTRAINT FK_Tariff_UtilityType FOREIGN KEY (UtilityID) REFERENCES UtilityType(UtilityID)
);

CREATE TABLE Bill (
    BillID INT IDENTITY(1,1) CONSTRAINT PK_Bill PRIMARY KEY,
    CustomerID INT NOT NULL,
    ReadingID INT NOT NULL,
    BillDate DATE CONSTRAINT DF_Bill_BillDate DEFAULT(GETDATE()),
    DueDate DATE NOT NULL,
    TotalAmount DECIMAL(10,2) NOT NULL,
    Status VARCHAR(15) 
        CONSTRAINT CK_Bill_Status CHECK (Status IN ('Paid','Unpaid','Partially Paid'))
        CONSTRAINT DF_Bill_Status DEFAULT 'Unpaid',
    GeneratedBy INT NOT NULL,

    CONSTRAINT FK_Bill_Customer FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID),
    CONSTRAINT FK_Bill_MeterReading FOREIGN KEY (ReadingID) REFERENCES MeterReading(ReadingID),
    CONSTRAINT FK_Bill_Employee FOREIGN KEY (GeneratedBy) REFERENCES Employee(EmployeeID)
);

CREATE TABLE Payment (
    PaymentID INT IDENTITY(1,1) CONSTRAINT PK_Payment PRIMARY KEY,
    BillID INT NOT NULL,
    CustomerID INT NOT NULL,
    PaymentDate DATE CONSTRAINT DF_Payment_PaymentDate DEFAULT(GETDATE()),
    AmountPaid DECIMAL(10,2) NOT NULL,
    PaymentMethod VARCHAR(15) 
        CONSTRAINT CK_Payment_Method CHECK (PaymentMethod IN ('Cash','Card','Online')),
    ProcessedBy INT NOT NULL,
    ReferenceNumber VARCHAR(30) CONSTRAINT UQ_Payment_ReferenceNumber UNIQUE,

    CONSTRAINT FK_Payment_Bill FOREIGN KEY (BillID) REFERENCES Bill(BillID),
    CONSTRAINT FK_Payment_Customer FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID),
    CONSTRAINT FK_Payment_Employee FOREIGN KEY (ProcessedBy) REFERENCES Employee(EmployeeID)
);

CREATE TABLE Complaint (
    ComplaintID INT IDENTITY(1,1) CONSTRAINT PK_Complaint PRIMARY KEY,
    CustomerID INT NOT NULL,
    UtilityID INT NOT NULL,
    Description VARCHAR(200) NOT NULL,
    ComplaintDate DATE CONSTRAINT DF_Complaint_ComplaintDate DEFAULT(GETDATE()),
    Status VARCHAR(20) 
        CONSTRAINT CK_Complaint_Status CHECK (Status IN ('Pending','In Progress','Resolved'))
        CONSTRAINT DF_Complaint_Status DEFAULT 'Pending',
    HandledBy INT NULL,
    ResolutionNote VARCHAR(200),
    ResolutionDate DATE NULL,

    CONSTRAINT FK_Complaint_Customer FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID),
    CONSTRAINT FK_Complaint_UtilityType FOREIGN KEY (UtilityID) REFERENCES UtilityType(UtilityID),
    CONSTRAINT FK_Complaint_Employee FOREIGN KEY (HandledBy) REFERENCES Employee(EmployeeID)
);

CREATE TABLE Report (
    ReportID INT IDENTITY(1,1) CONSTRAINT PK_Report PRIMARY KEY,
    ReportType VARCHAR(30) NOT NULL,
    GeneratedBy INT NOT NULL,
    GeneratedDate DATE CONSTRAINT DF_Report_GeneratedDate DEFAULT(GETDATE()),
    ReportPeriodStart DATE NOT NULL,
    ReportPeriodEnd DATE NOT NULL,
    FilePath VARCHAR(150),
    Summary VARCHAR(250),

    CONSTRAINT FK_Report_Employee FOREIGN KEY (GeneratedBy) REFERENCES Employee(EmployeeID)
);

CREATE TABLE UtilityTariffHistory (
    RecordID INT IDENTITY(1,1) CONSTRAINT PK_UtilityTariffHistory PRIMARY KEY,
    UtilityID INT NOT NULL,
    TariffID INT NOT NULL,
    ChangedOn DATE CONSTRAINT DF_UtilityTariffHistory_ChangedOn DEFAULT(GETDATE()),
    ChangedBy INT NOT NULL,

    CONSTRAINT FK_UTH_UtilityType FOREIGN KEY (UtilityID) REFERENCES UtilityType(UtilityID),
    CONSTRAINT FK_UTH_Tariff FOREIGN KEY (TariffID) REFERENCES Tariff(TariffID),
    CONSTRAINT FK_UTH_Employee FOREIGN KEY (ChangedBy) REFERENCES Employee(EmployeeID)
);

