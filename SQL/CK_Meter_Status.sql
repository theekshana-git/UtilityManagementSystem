SELECT name, definition
FROM sys.check_constraints
WHERE name = 'CK_Meter_Status';

ALTER TABLE Meter
DROP CONSTRAINT CK_Meter_Status;

ALTER TABLE Meter
ADD CONSTRAINT CK_Meter_Status
CHECK (Status IN ('Active','Inactive','Faulty'));

