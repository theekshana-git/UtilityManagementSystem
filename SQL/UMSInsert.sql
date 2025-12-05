INSERT INTO Customer (FirstName, LastName, CustomerType, Address, City, ContactNumber, Email)
VALUES
('Kamal', 'Perera', 'Household', 'No 25, Temple Road, Biyagama', 'Gampaha', '0771234567', 'kamalp@gmail.com'),
('Nimali', 'Fernando', 'Household', '45/3, Lake View, Katugastota', 'Kandy', '0712244567', 'nimali.f@gmail.com'),
('Sunil', 'Jayasinghe', 'Business', '120 Station Road, Maradana', 'Colombo', '0778854433', 'sunilj@biz.lk'),
('Dilani', 'Wickramasinghe', 'Household', '33/1, Main Street, Rajanganaya', 'Kurunegala', '0759988776', 'dilaniw@gmail.com'),
('Ruwan', 'Silva', 'Government', 'Public Quarters, Block A, Kollupitiya', 'Colombo', '0719934556', 'ruwan.s@min.gov.lk'),
('Shanaka', 'Karunaratne', 'Household', 'No 78, Aluthgama Village, Ahangama', 'Galle', '0773459876', 'shanaka.k@gmail.com'),
('Thisara', 'Weerasinghe', 'Business', '62/5, Industrial Zone, Rathmalana', 'Colombo', '0763459981', 'tweerasinghe@company.lk'),
('Harsha', 'Rathnayake', 'Household', '99/12 Lakeside, Peradeniya', 'Kandy', '0716677889', 'harsha.r@gmail.com'),
('Ayodya', 'Gunasekara', 'Household', '40/8, Hill View, Tangalle', 'Matara', '0771239876', 'ayodyag@yahoo.com'),
('Lakshan', 'Dias', 'Household', '12/4, Palm Grove, Katukurunda', 'Kalutara', '0768899001', 'lakshand@gmail.com');

INSERT INTO UtilityType (UtilityName, Unit, Description)
VALUES
('Electricity', 'kWh', 'General domestic and business electricity service'),
('Water', 'm³', 'National Water Supply and Drainage Board consumption'),
('Gas', 'kg', 'Household LP Gas supply');

INSERT INTO Employee (FirstName, LastName, Role, Username, PasswordHash, ContactNumber, Email)
VALUES
('Suranga', 'Ranasinghe', 'Admin', 'admin1', 'HASHEDPW1', '0771000001', 'suranga@umc.lk'),
('Sanjaya', 'Perera', 'Field Officer', 'field01', 'HASHEDPW2', '0772000033', 'sanjaya@umc.lk'),
('Tharushi', 'Silva', 'Cashier', 'cash01', 'HASHEDPW3', '0712334455', 'tharushi@umc.lk'),
('Kasun', 'Jayawardena', 'Manager', 'mgr01', 'HASHEDPW4', '0772998844', 'kasun@umc.lk'),
('Nadeesha', 'Fernando', 'Field Officer', 'field02', 'HASHEDPW5', '0754432112', 'nadeesha@umc.lk'),
('Ishara', 'Gunawardena', 'Cashier', 'cash02', 'HASHEDPW6', '0778923451', 'ishara@umc.lk'),
('Amal', 'Hettiarachchi', 'Field Officer', 'field03', 'HASHEDPW7', '0715551122', 'amalh@umc.lk'),
('Diluka', 'Karunarathne', 'Manager', 'mgr02', 'HASHEDPW8', '0774455667', 'diluka@umc.lk'),
('Rashmi', 'Rajapaksa', 'Cashier', 'cash03', 'HASHEDPW9', '0713344552', 'rashmir@umc.lk'),
('Chathura', 'Weerasekara', 'Field Officer', 'field04', 'HASHEDPW10', '0776699884', 'chathura@umc.lk');

INSERT INTO Meter (CustomerID, UtilityID, InstallationDate, Location)
VALUES
(1,1,'2023-01-01','Front Wall'),
(2,1,'2023-02-10','Gate Side'),
(3,1,'2022-11-05','Office Entrance'),
(4,2,'2023-03-12','Bathroom Line'),
(5,1,'2022-05-20','Back Yard'),
(6,2,'2023-04-22','Meter Box Room'),
(7,3,'2023-05-11','Front Gate'),
(8,1,'2022-06-01','Left Wall'),
(9,2,'2022-09-14','Kitchen'),
(10,1,'2023-07-09','Side Fence');

INSERT INTO MeterReading (MeterID, ReadingDate, PreviousReading, CurrentReading, RecordedBy, Notes)
VALUES
(1,'2024-01-01',120,150,2,'Normal'),
(2,'2024-01-02',80,110,5,'OK'),
(3,'2024-01-05',300,340,2,'Office high usage'),
(4,'2024-01-10',40,55,5,'Water leak suspected'),
(5,'2024-01-12',500,550,2,'OK'),
(6,'2024-01-15',20,26,5,'Normal'),
(7,'2024-01-18',10,15,2,'Gas refill'),
(8,'2024-01-20',90,125,5,'Usage spike'),
(9,'2024-01-22',30,42,2,'OK'),
(10,'2024-01-25',70,95,5,'Normal');

INSERT INTO Tariff (UtilityID, EffectiveFrom, SlabStart, SlabEnd, RatePerUnit, FixedCharge)
VALUES
(1,'2023-01-01',0,30,8.00,150),
(1,'2023-01-01',31,60,20.00,250),
(1,'2023-01-01',61,NULL,45.00,500),
(2,'2023-01-01',0,5,12.00,50),
(2,'2023-01-01',6,10,30.00,100),
(2,'2023-01-01',11,NULL,45.00,150),
(3,'2023-01-01',0,5,100.00,0),
(3,'2023-01-01',6,10,180.00,0),
(3,'2023-01-01',11,NULL,250.00,0),
(1,'2024-01-01',0,30,10.00,200);

INSERT INTO Bill (CustomerID, ReadingID, DueDate, TotalAmount, GeneratedBy)
VALUES
(1,1,'2024-02-01',3200,3),
(2,2,'2024-02-02',2500,3),
(3,3,'2024-02-05',7800,3),
(4,4,'2024-02-10',1400,3),
(5,5,'2024-02-12',5200,3),
(6,6,'2024-02-15',900,3),
(7,7,'2024-02-18',1500,3),
(8,8,'2024-02-20',4300,3),
(9,9,'2024-02-22',1100,3),
(10,10,'2024-02-25',2900,3);

INSERT INTO Payment (BillID, CustomerID, AmountPaid, PaymentMethod, ProcessedBy, ReferenceNumber)
VALUES
(1,1,3200,'Online',3,'REF001'),
(2,2,2500,'Cash',3,'REF002'),
(3,3,7800,'Card',3,'REF003'),
(4,4,1400,'Cash',3,'REF004'),
(5,5,5200,'Online',3,'REF005'),
(6,6,900,'Card',3,'REF006'),
(7,7,1500,'Online',3,'REF007'),
(8,8,4300,'Cash',3,'REF008'),
(9,9,1100,'Card',3,'REF009'),
(10,10,2900,'Online',3,'REF010');

INSERT INTO Complaint (CustomerID, UtilityID, Description, HandledBy, ResolutionNote, ResolutionDate)
VALUES
(1,1,'Power outage in area',4,'Resolved after repair','2024-01-10'),
(2,2,'Low water pressure',4,'Pipe fixed','2024-01-12'),
(3,1,'Meter not working',4,'Replaced meter','2024-01-15'),
(4,2,'Water leak',4,'Leak repaired','2024-01-18'),
(5,1,'High bill amount',4,'Explained usage spike','2024-01-20'),
(6,3,'Gas smell detected',4,'Checked, no leak','2024-01-22'),
(7,1,'Incorrect reading',4,'Corrected','2024-01-25'),
(8,1,'Power fluctuations',4,'Transformer fixed','2024-01-28'),
(9,2,'Dirty water',4,'Filter replaced','2024-01-30'),
(10,3,'Late gas delivery',4,'Resolved','2024-02-01');

INSERT INTO UtilityTariffHistory (UtilityID, TariffID, ChangedBy)
VALUES
(1,1,1),
(1,2,1),
(1,3,1),
(1,10,1),
(2,4,1),
(2,5,1),
(2,6,1),
(3,7,1),
(3,8,1),
(3,9,1);





