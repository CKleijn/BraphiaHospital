CREATE TABLE hospital (
    id UUID PRIMARY KEY,
    hospital VARCHAR(100) NOT NULL,
    street VARCHAR(100) NOT NULL,
    number VARCHAR(20) NOT NULL,
    postalCode VARCHAR(20) NOT NULL,
    city VARCHAR(100) NOT NULL,
    country VARCHAR(100) NOT NULL,
    stores INTEGER NOT NULL,
    squares INTEGER NOT NULL,
    phoneNumber VARCHAR(20) NOT NULL,
    email VARCHAR(100) NOT NULL,
    website VARCHAR(100),
    totalBeds INTEGER NOT NULL,
    builtYear INTEGER NOT NULL
);

INSERT INTO hospital (id, hospital, street, number, postalCode, city, country, stores, squares, phoneNumber, email, website, totalBeds, builtYear) VALUES 
('123e4567-e89b-12d3-a456-426614174000', 'General Hospital', 'Main Street', '123', '12345', 'Springfield', 'USA', 5, 100, '+1234567890', 'generalhospital@example.com', 'https://general-hospital.com', 300, 1990),
('123e4567-e89b-12d3-a456-426614174001', 'City Hospital', 'Oak Avenue', '456', '54321', 'Springfield', 'USA', 3, 80, '+1987654321', 'cityhospital@example.com', 'https://city-hospital.com', 200, 1985),
('123e4567-e89b-12d3-a456-426614174002', 'County Hospital', 'Elm Street', '789', '98765', 'Springfield', 'USA', 4, 120, '+1122334455', 'countyhospital@example.com', 'https://county-hospital.com', 400, 2000);
