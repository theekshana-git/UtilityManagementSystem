CREATE PROCEDURE sp_GenerateBill
    @ReadingID INT,
    @GeneratedBy INT
AS
BEGIN
    DECLARE @CustomerID INT, @DueDate DATE, @Amount DECIMAL(10,2);

    SELECT @CustomerID = m.CustomerID FROM MeterReading mr
    INNER JOIN Meter m ON mr.MeterID = m.MeterID
    WHERE mr.ReadingID = @ReadingID;

    SET @DueDate = DATEADD(DAY, 14, GETDATE());

    SET @Amount = dbo.udf_CalculateBill(@ReadingID);

    INSERT INTO Bill (CustomerID, ReadingID, DueDate, TotalAmount, GeneratedBy)
    VALUES (@CustomerID, @ReadingID, @DueDate, @Amount, @GeneratedBy);
END;


CREATE PROCEDURE sp_ListDefaulters
AS
BEGIN
    SELECT c.CustomerID, c.FirstName, c.LastName, SUM(b.TotalAmount) AS Outstanding
    FROM Bill b
    INNER JOIN Customer c ON b.CustomerID = c.CustomerID
    WHERE b.Status <> 'Paid'
    GROUP BY c.CustomerID, c.FirstName, c.LastName
    HAVING SUM(b.TotalAmount) > 0;
END;


CREATE PROCEDURE sp_AddMeterReading
    @MeterID INT,
    @Current DECIMAL(10,2),
    @RecordedBy INT
AS
BEGIN
    INSERT INTO MeterReading (MeterID, CurrentReading, ReadingDate, RecordedBy)
    VALUES (@MeterID, @Current, GETDATE(), @RecordedBy);
END;


CREATE PROCEDURE sp_RecordPayment
    @BillID INT,
    @CustomerID INT,
    @Amount DECIMAL(10,2),
    @ProcessedBy INT
AS
BEGIN
    INSERT INTO Payment (BillID, CustomerID, AmountPaid, ProcessedBy, PaymentMethod, ReferenceNumber)
    VALUES (@BillID, @CustomerID, @Amount, @ProcessedBy, 'Cash', NEWID());
END;


CREATE PROCEDURE sp_MonthlyRevenue
    @Year INT,
    @Month INT
AS
BEGIN
    SELECT SUM(AmountPaid) AS Revenue
    FROM Payment
    WHERE YEAR(PaymentDate) = @Year AND MONTH(PaymentDate) = @Month;
END;


CREATE PROCEDURE sp_CustomerUsageSummary
    @CustomerID INT
AS
BEGIN
    SELECT 
        c.FirstName, c.LastName,
        SUM(mr.Consumption) AS TotalConsumption,
        SUM(b.TotalAmount) AS TotalBilled,
        dbo.udf_GetOutstanding(@CustomerID) AS OutstandingBalance
    FROM Customer c
    LEFT JOIN Meter m ON c.CustomerID = m.CustomerID
    LEFT JOIN MeterReading mr ON m.MeterID = mr.MeterID
    LEFT JOIN Bill b ON b.CustomerID = c.CustomerID
    WHERE c.CustomerID = @CustomerID
    GROUP BY c.FirstName, c.LastName;
END;


