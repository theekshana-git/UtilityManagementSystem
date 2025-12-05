CREATE FUNCTION udf_CalculateBill (@ReadingID INT)
RETURNS DECIMAL(10,2)
AS
BEGIN
    DECLARE @meterId INT, @con DECIMAL(10,2), @util INT, @total DECIMAL(10,2)=0;

    SELECT @meterId = MeterID, @con = Consumption
    FROM MeterReading WHERE ReadingID = @ReadingID;

    SELECT @util = UtilityID FROM Meter WHERE MeterID = @meterId;

    SELECT @total = SUM(RatePerUnit * 
           CASE 
               WHEN @con > SlabEnd AND SlabEnd IS NOT NULL THEN SlabEnd - SlabStart
               WHEN @con > SlabStart THEN @con - SlabStart
               ELSE 0 
           END
    ) + SUM(FixedCharge)
    FROM Tariff WHERE UtilityID = @util;

    RETURN @total;
END;


CREATE FUNCTION udf_LateFee (@DueDate DATE, @Amount DECIMAL(10,2))
RETURNS DECIMAL(10,2)
AS
BEGIN
    IF (GETDATE() > @DueDate)
        RETURN @Amount * 0.05;  -- 5% late fee
    RETURN 0;
END;


CREATE FUNCTION udf_GetOutstanding (@CustomerID INT)
RETURNS DECIMAL(10,2)
AS
BEGIN
    RETURN (
        SELECT SUM(TotalAmount)
        FROM Bill
        WHERE CustomerID = @CustomerID AND Status <> 'Paid'
    );
END;


CREATE FUNCTION udf_MonthlyConsumption (@CustomerID INT, @Start DATE, @End DATE)
RETURNS DECIMAL(10,2)
AS
BEGIN
    RETURN (
        SELECT SUM(Consumption)
        FROM MeterReading mr
        INNER JOIN Meter m ON mr.MeterID = m.MeterID
        WHERE m.CustomerID = @CustomerID
        AND mr.ReadingDate BETWEEN @Start AND @End
    );
END;


