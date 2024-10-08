require('dotenv').config();
const { readPool } = require('../connections/connectPostgreDB');

const createHospitalQuery = `INSERT INTO hospital (hospitalId, name) VALUES ($1, $2);`;
const updateHospitalQuery = `UPDATE hospital SET name = $1 WHERE hospitalId = $2;`;
const getHospitalQuery = `SELECT * FROM hospital;`;
const getHospitalByIdQuery = 'SELECT * FROM hospital WHERE hospitalId = $1;';

const handleDatabaseError = (err) => { 
    console.error('Database Error: ', err);
};

const createHospital = async (data,) => {
    const queryValues = [ data.id,data.hospital ];

    try {
        await readPool.query(createHospitalQuery, queryValues);
        console.log(`Hospital record inserted successfully: ${data.hospital.id}`);
    } catch (error) {
        handleDatabaseError( error);
        return;
    }
};

const updateHospital = async (data) => {
    const queryValues = [ data.hospital, data.id];
    try {
        await readPool.query(updateHospitalQuery, queryValues);
        console.log(`Hospital record updated successfully! ${data.id}`);
    } catch (error) {
        handleDatabaseError( error);
        return;
    }
};

const getHospital = async (req,res) => {   
    try {
        const result = await readPool.query(getHospitalQuery);
        res.status(200).json({count: result.rows.length, result: result.rows});
    } catch (err) {
        handleDatabaseError(err);
        return;
    }   
};

const getHospitalById = async (req, res) => {
    const { id } = req.params;
    try {
        const result = await readPool.query(getHospitalByIdQuery, [id]);
        const hospital = result.rows[0];
        if (!hospital) {
            res.status(404).json({error: `Hospital not found! ${id}`});
            return;
        }
        res.status(200).json(hospital);
    } catch (err) {
        handleDatabaseError(res, err);
        return;
    }
}; 

module.exports = { createHospital,updateHospital, getHospital, getHospitalById};