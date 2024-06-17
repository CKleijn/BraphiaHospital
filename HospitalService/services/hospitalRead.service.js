require('dotenv').config();
const { validateCreateHospitalDTO } = require('../utils/dtos/validator/hospital.validate.js');
const { readPool }  = require('../connections/connectPostgreDB.js'); 

const getHospitalQuery = `SELECT * FROM hospital;`;

const getHospitalByIdQuery = 'SELECT * FROM hospital WHERE id = $1;';

const createHospitalQuery = `INSERT INTO hospital (id, hospital, street, number, postal_code, city, country, stores, squares, phone_number, email, website, total_beds, built_year) 
                             VALUES ($1, $2, $3, $4, $5, $6, $7, $8, $9, $10, $11, $12, $13, $14) RETURNING *;`;

const updateHospitalQuery = `UPDATE hospital SET hospital = $1, street = $2, number = $3, postal_code = $4, city = $5, country = $6, stores = $7, squares = $8, phone_number = $9, 
                             email = $10, website = $11, total_beds = $12, built_year = $13 WHERE id = $14 RETURNING *;`;

const handleDatabaseError = (res, err) => { 
    console.error(err);
    res.status(500).json({ error: 'Internal server error' });
};

const handleValidationErrors = (res, message) => { 
    res.status(400).json({ error: message });
};

const createHospital = async (data) => {
    const { id, hospital, street, number, postal_code, city, country, stores, squares, phone_number, email, website, total_beds, built_year } = data;

    try {
        await readPool.query(createHospitalQuery, 
            [id, hospital, street, number, postal_code, city, country, stores, squares, phone_number, email, website, total_beds, built_year]
        );

        console.log(`Hospital record inserted successfully: ${hospital}`);
    } catch (err) {
        console.error('Error creating hospital:', err);
        throw err;
    }
};

const getHospital = async (req, res) => {   
    try {
        const result = await readPool.query(getHospitalQuery);
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
        const result = await readPool.query(getHospitalByIdQuery, [id]);
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

const updateHospital = async (req, res) => {
    const { id } = req.params;
    const { hospital, street, number, postalCode, city, country, stores, squares, phoneNumber, email, website, totalBeds, builtYear } = req.body;
    try {
        const result = await readPool.query(updateHospitalQuery, 
            [hospital, street, number, postalCode, city, country, stores, squares, phoneNumber, email, website, totalBeds, builtYear, id]
        );
        if (result.rows.length === 0) {
            return res.status(404).json({ error: `Hospital ${id} not found` });
        }        
        res.status(200).json(result.row[0]);
    } catch (err) {
        handleDatabaseError(res, err);
        return;
    }
}

module.exports = { createHospital, getHospital, getHospitalById, updateHospital };