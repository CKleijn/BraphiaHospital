require('dotenv').config();
const { readPool } = require('../connections/connectPostgreDB');

const createHospitalQuery = `INSERT INTO hospital (hospital_id, name) VALUES ($1, $2);`;
const updateHospitalQuery = `UPDATE hospital SET name = $1 WHERE hospital_id = $2;`;
const getHospitalQuery = `SELECT * FROM hospital;`;
const getHospitalByIdQuery = 'SELECT * FROM hospital WHERE hospitalId = $1;';

const handleDatabaseError = (res, err) => { 
    console.error(err);
    res.status(500).json({error: 'Internal server error'});
};

const createHospital = async (data) => {
    const queryValues = [ data.hospital, data.id];

    try {
        await readPool.query(createHospitalQuery, queryValues);
        
        console.log(`Hospital record inserted successfully: ${data.hospital}`);
    } catch (error) {
        handleDatabaseError(res, error);
        return;
    }
};

const updateHospital = async (data) => {
    const queryValues = [ data.hospital, data.id];
    try {
        await readPool.query(updateHospitalQuery, queryValues);
       
        console.log(`Hospital record updated successfully: ${data.hospital}`);
    } catch (error) {
        handleDatabaseError(res, error);
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

module.exports = { createHospital,updateHospital, getHospital, getHospitalById};