CREATE SEQUENCE version_seq START 1 INCREMENT 1;
CREATE TABLE eventStore (
	id SERIAL PRIMARY KEY,
	type VARCHAR(50) NOT NULL,
	payload TEXT NOT NULL,
	version INT NOT NULL DEFAULT nextval('version_seq'),
	createdAT TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

INSERT INTO eventStore (type, payload, version) VALUES 
('staff_insert', '{"id":"123e4567-e89b-12d3-a456-556614174000","name":"John Doe","specialization":"Cardiology","street":"Elm St","houseNumber":"123","city":"Springfield","zip":"62701","phoneNumber":"555-1234","email":"johndoe@example.com","employmentDate":"2020-01-01","hospitalId":"123e4567-e89b-12d3-a456-426614174000"}', nextval('version_seq')),
('staff_insert', '{"id":"123e4567-e89b-12d3-a456-556614174001","name":"Jane Smith","specialization":"Neurology","street":"Oak St","houseNumber":"456","city":"Springfield","zip":"62702","phoneNumber":"555-5678","email":"janesmith@example.com","employmentDate":"2021-06-15","hospitalId":"123e4567-e89b-12d3-a456-426614174001"}', nextval('version_seq')),
('staff_insert', '{"id":"123e4567-e89b-12d3-a456-556614174002","name":"Alice Johnson","specialization":"Pediatrics","street":"Pine St","houseNumber":"789","city":"Springfield","zip":"62703","phoneNumber":"555-9101","email":"alicejohnson@example.com","employmentDate":"2019-03-23","hospitalId":"123e4567-e89b-12d3-a456-426614174002"}', nextval('version_seq'));