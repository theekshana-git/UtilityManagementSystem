-- Insert sample ingredients
INSERT INTO Ingredients (Name, Cost, Availability, NutritionalInfo, Taste, Texture)
VALUES 
('Tomato', 0.50, 1, 'Rich in Vitamin C', 'Sweet', 'Juicy'),
('Red Pepper', 0.70, 1, 'High in Vitamin A', 'Sweet', 'Crunchy'),
('Potato', 0.30, 1, 'Starchy and filling', 'Mild', 'Soft'),
('Cucumber', 0.40, 1, 'Hydrating and low-calorie', 'Mild', 'Crunchy');

-- Insert substitution types
INSERT INTO SubstitutionTypes (TypeName)
VALUES ('Manual');

-- Insert a manual substitution (valid foreign keys)
INSERT INTO IngredientSubstitutions (IngredientID, SubstituteID, SimilarityScore, DietaryRestrictions, SubstitutionTypeID)
VALUES (1, 2, 0.6, 'None', 1);

-- Insert inventory data
INSERT INTO Inventory (IngredientID, Quantity, ExpirationDate)
VALUES 
(1, 50, '2025-12-01'),
(2, 30, '2025-11-15'),
(3, 100, '2025-12-20'),
(4, 20, '2025-10-10');
