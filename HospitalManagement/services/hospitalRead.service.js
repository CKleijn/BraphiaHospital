require('dotenv').config();
const { validateCreateHospitalDTO } = require('../utils/dtos/validator/hospital.validate.js');
const { readPool }  = require('../connections/connectPostgreDB.js'); 

const getHospitalQuery = `SELECT * FROM hospital;`;

const getHospitalByIdQuery = 'SELECT * FROM hospital WHERE id = $1;';

const createHospitalQuery = `INSERT INTO hospital (id, hospital, street, number, postalCode, city, country, stores, squares, phoneNumber, email, website, totalBeds, builtYear) 
                             VALUES ($1, $2, $3, $4, $5, $6, $7, $8, $9, $10, $11, $12, $13, $14) RETURNING *;`;

const updateHospitalQuery = `UPDATE hospital SET hospital = $1, street = $2, number = $3, postalCode = $4, city = $5, country = $6, stores = $7, squares = $8, phoneNumber = $9, 
                             email = $10, website = $11, totalBeds = $12, builtYear = $13 WHERE id = $14 RETURNING *;`;

const handleDatabaseError = (res, err) => { 
    console.error(err);
    res.status(500).json({ error: 'Internal server error' });
};

const handleValidationErrors = (res, message) => { 
    res.status(400).json({ error: message });
};

const createHospital = async (data) => {
    console.log('Creating hospital:', data);

    const queryValues = [
        data.id,
        data.hospital,
        data.street,
        data.number,
        data.postalCode,
        data.city,
        data.country,
        data.stores,
        data.squares,
        data.phoneNumber,
        data.email,
        data.website,
        data.totalBeds,
        data.builtYear
    ];

    try {
        const result = await readPool.query(createHospitalQuery, queryValues);
        console.log(`Hospital record inserted successfully:`, result.rows[0]);
    } catch (err) {
        handleDatabaseError(res, err);
        return;
    }
};

const getHospital = async (req, res) => {   
    try {
        const result = await readPool.query(getHospitalQuery);
        res.status(200).json({count: result.rows.length, result: result.rows});
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

const updateHospital = async (data) => {
    const { id, hospital, street, number, postalCode, city, country, stores, squares, phoneNumber, email, website, totalBeds, builtYear } = data;
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