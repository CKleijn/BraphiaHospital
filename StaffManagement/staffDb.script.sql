-- DROP TABLE IF EXISTS staff;
-- DROP TABLE IF EXISTS hospital;

CREATE TABLE hospital (
    hospitalId UUID PRIMARY KEY,
    name VARCHAR(255) UNIQUE NOT NULL
);

CREATE TABLE staff (
    id UUID PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    specialization VARCHAR(255),
    street VARCHAR(255),
    city VARCHAR(100),
    state VARCHAR(100),
    zip VARCHAR(20),
    phoneNumber VARCHAR(20) NOT NULL,
    email VARCHAR(255) NOT NULL,
    employmentDate DATE NOT NULL,
    hospitalId UUID NOT NULL,
    FOREIGN KEY (hospitalId) REFERENCES hospital(hospitalId)
);


INSERT INTO hospital (hospitalId, name) VALUES 
('123e4567-e89b-12d3-a456-426614174000', 'General Hospital'),
('123e4567-e89b-12d3-a456-426614174001', 'City Hospital'),
('123e4567-e89b-12d3-a456-426614174002', 'County Hospital');


INSERT INTO staff (id, name, specialization, street, city, state, zip, phoneNumber, email, employmentDate, hospitalId) VALUES 
('123e4567-e89b-12d3-a456-556614174000', 'John Doe', 'Cardiology', '123 Elm St', 'Springfield', 'IL', '62701', '555-1234', 'johndoe@example.com', '2020-01-01', '123e4567-e89b-12d3-a456-426614174000'),
('123e4567-e89b-12d3-a456-556614174001', 'Jane Smith', 'Neurology', '456 Oak St', 'Springfield', 'IL', '62702', '555-5678', 'janesmith@example.com', '2021-06-15', '123e4567-e89b-12d3-a456-426614174001'),
('123e4567-e89b-12d3-a456-556614174002', 'Alice Johnson', 'Pediatrics', '789 Pine St', 'Springfield', 'IL', '62703', '555-9101', 'alicejohnson@example.com', '2019-03-23', '123e4567-e89b-12d3-a456-426614174002');
