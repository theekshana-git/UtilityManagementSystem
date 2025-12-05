CREATE VIEW vw_UnpaidBills AS
SELECT b.BillID, c.FirstName, c.LastName, b.TotalAmount, b.DueDate
FROM Bill b
INNER JOIN Customer c ON b.CustomerID = c.CustomerID
WHERE b.Status <> 'Paid';


CREATE VIEW vw_MonthlyRevenue AS
SELECT 
    YEAR(PaymentDate) AS [Year],
    MONTH(PaymentDate) AS [Month],
    SUM(AmountPaid) AS TotalRevenue
FROM Payment
GROUP BY YEAR(PaymentDate), MONTH(PaymentDate);


CREATE VIEW vw_TopConsumers AS
SELECT TOP 10 
    c.CustomerID,
    c.FirstName + ' ' + c.LastName AS CustomerName,
    SUM(mr.Consumption) AS TotalConsumption
FROM MeterReading mr
INNER JOIN Meter m ON mr.MeterID = m.MeterID
INNER JOIN Customer c ON m.CustomerID = c.CustomerID
GROUP BY c.CustomerID, c.FirstName, c.LastName
ORDER BY TotalConsumption DESC;


CREATE VIEW vw_ComplaintsSummary AS
SELECT 
    ComplaintID,
    c.FirstName + ' ' + c.LastName AS CustomerName,
    UtilityID,
    c.Status,
    ComplaintDate
FROM Complaint cp
INNER JOIN Customer c ON cp.CustomerID = c.CustomerID;




