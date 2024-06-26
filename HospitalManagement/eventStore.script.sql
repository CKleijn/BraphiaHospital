CREATE SEQUENCE version_seq START 1 INCREMENT 1;
CREATE TABLE eventStore (
	id SERIAL PRIMARY KEY,
	type VARCHAR(50) NOT NULL,
	payload TEXT NOT NULL,
	version INT NOT NULL DEFAULT nextval('version_seq'),
	createdAT TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

INSERT INTO eventStore (type, payload, version) VALUES 
('hospital_insert', '{"id":"123e4567-e89b-12d3-a456-426614174000","hospital":"General Hospital","street":"Main Street","number":"123","postalCode":"12345","city":"Springfield","country":"USA","stores":5,"squares":100,"phoneNumber":"+1234567890","email":"generalhospital@example.com","website":"https://general-hospital.com","totalBeds":300,"builtYear":1990}', nextval('version_seq')),
('hospital_insert', '{"id":"123e4567-e89b-12d3-a456-426614174001","hospital":"City Hospital","street":"Oak Avenue","number":"456","postalCode":"54321","city":"Springfield","country":"USA","stores":3,"squares":80,"phoneNumber":"+1987654321","email":"cityhospital@example.com","website":"https://city-hospital.com","totalBeds":200,"builtYear":1985}', nextval('version_seq')),
('hospital_insert', '{"id":"123e4567-e89b-12d3-a456-426614174002","hospital":"County Hospital","street":"Elm Street","number":"789","postalCode":"98765","city":"Springfield","country":"USA","stores":4,"squares":120,"phoneNumber":"+1122334455","email":"countyhospital@example.com","website":"https://county-hospital.com","totalBeds":400,"builtYear":2000}', nextval('version_seq'));
