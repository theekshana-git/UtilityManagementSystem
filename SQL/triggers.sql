CREATE TRIGGER TR_UpdateOutstandingBalance
ON Payment
AFTER INSERT
AS
BEGIN
    UPDATE Bill
    SET TotalAmount = TotalAmount - i.AmountPaid
    FROM Bill b
    INNER JOIN inserted i ON b.BillID = i.BillID;
END;

CREATE TRIGGER TR_SetPreviousReading
ON MeterReading
AFTER INSERT
AS
BEGIN
    UPDATE mr
    SET PreviousReading = ISNULL((
        SELECT TOP 1 CurrentReading
        FROM MeterReading
        WHERE MeterID = mr.MeterID
          AND ReadingID < mr.ReadingID
        ORDER BY ReadingDate DESC
    ), 0)
    FROM MeterReading mr
    INNER JOIN inserted i ON mr.ReadingID = i.ReadingID;
END;

CREATE TRIGGER TR_CloseTariff
ON Tariff
AFTER INSERT
AS
BEGIN
    UPDATE Tariff
    SET EffectiveTo = DATEADD(DAY, -1, i.EffectiveFrom)
    FROM Tariff t
    INNER JOIN inserted i ON t.UtilityID = i.UtilityID
    WHERE t.TariffID <> i.TariffID AND t.EffectiveTo IS NULL;
END;

CREATE TRIGGER TR_TariffHistory
ON Tariff
AFTER INSERT
AS
BEGIN
    INSERT INTO UtilityTariffHistory (UtilityID, TariffID, ChangedBy)
    SELECT UtilityID, TariffID, 1   -- default admin ID
    FROM inserted;
END;


