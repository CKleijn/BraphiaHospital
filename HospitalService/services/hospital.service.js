require('dotenv').config();
const pool = require('../connections/connectPostgreDB');
const { getChannel, sendMessage } = require('../connections/connectRabbitMQ');
const { validateCreateHospitalDTO } = require('../dtos/validator/hospital.validate');

const createHospitalQuery = `INSERT INTO hospital (hospital, street, number, postal_code, city, country, stores, squares, phone_number, email, website, total_beds, built_year) 
                                VALUES ($1, $2, $3, $4, $5, $6, $7, $8, $9, $10, $11, $12, $13) RETURNING *;`;
const getHospitalQuery = `SELECT * FROM hospital;`;
const getHospitalByIdQuery = 'SELECT * FROM hospital WHERE id = $1;';
const updateHospitalQuery = `UPDATE hospitals SET hospital = $1, street = $2, number = $3, postal_code = $4, city = $5, country = $6 stores = $7, squares = $8, phone_number = $9, 
                                email = $10, website = $11, total_beds = $12, built_year = $13 WHERE id = $14 RETURNING *;`;

const handleDatabaseError = (res, err) => { 
    console.error(err);
    res.status(500).json({error: 'Internal server error'});
};

const handleValidationErrors = (res,message) => { 
    res.status(400).json({error: message});
};

const createHospital = async (req, res) => {
    const validationError = validateCreateHospitalDTO(req.body);
    if (validationError) {  
        handleValidationErrors(res,validationError);
        return;
    }

    const { hospital, street, number, postalCode, city, country, stores, squares, phoneNumber, email, website, totalBeds, builtYear } = req.body;
    try {
        const result = await pool.query(createHospitalQuery, 
            [hospital, street, number, postalCode, city, country, stores, squares, phoneNumber, email, website, totalBeds, builtYear]
        );
        const newHospital = result.rows[0];
        res.status(201).json(newHospital);
    } catch (err) {
        handleDatabaseError(res, err);
        return;
    }
};

const updateHospital = async (req, res) => {
    const { id } = req.params;
    const { hospital, street, number, postalCode, city, country, stores, squares, phoneNumber, email, website, totalBeds, builtYear } = req.body;
    try {
        const result = await pool.query(updateHospitalQuery, 
            [hospital, street, number, postalCode, city, country, stores, squares, phoneNumber, email, website, totalBeds, builtYear, id]
        );
        if (result.rows.length === 0) {
            return res.status(404).json({ error: `Hospital ${id} not found` });
        }
        //TO-DO: Send a message to the RabbitMQ exchange
        
        res.status(200).json(result.row[0]);
    } catch (err) {
        handleDatabaseError(res, err);
        return;
    }
}

const getHospital = async (req, res) => {   
    try {
        const result = await pool.query(getHospitalQuery);
        const hospitals = result.rows;
        res.status(200).json(hospitals);
    } catch (err) {
        handleDatabaseError(res, err);
        return;
    }   
};

const getHospitalById = async (req, res) => {
    const { id } = req.params;
    try {
        const result = await pool.query(getHospitalByIdQuery, [id]);
        const hospital = result.rows[0];
        if (!hospital) {
            res.status(404).json({error: 'Hospital not found'});
            return;
        }
        res.status(200).json(hospital);
    } catch (err) {
        handleDatabaseError(res, err);
        return;
    }
};  

module.exports = { createHospital, updateHospital, getHospital, getHospitalById };