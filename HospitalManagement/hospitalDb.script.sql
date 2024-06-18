-- DROP TABLE IF EXISTS hospital;
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
