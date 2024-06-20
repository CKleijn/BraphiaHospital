require('dotenv').config();
const { readPool } = require('../connections/connectPostgreDB');

const createStaffQuery = `INSERT INTO staff (id, name, specialization, hospitalId, street, city, state, zip, phoneNumber, email, employmentDate)
                            VALUES ($1, $2, $3, $4, $5, $6, $7, $8, $9, $10, $11) RETURNING *;`;
const getStaffQuery = `SELECT * FROM staff`;
const getStaffByIdQuery = `SELECT * FROM staff WHERE id = $1`;
const updateStaffQuery = `UPDATE staff SET name = $1, specialization = $2, hospitalId = $3, street = $4, city = $5, 
                            state = $6, zip = $7, phoneNumber = $8, email = $9, employmentDate = $10 WHERE id = $11 RETURNING *;`;


const handleDatabaseError = (res, err) => { 
    console.error(err);
    res.status(500).json({error: 'Internal server error'});
};

const createStaff = async (data) => {
    const { id, name, specialization, hospitalId, street, city, state, zip, phoneNumber, email, employmentDate } = data;
    
    try {
        const result = await readPool.query(createStaffQuery, 
            [id, name, specialization, hospitalId, street, city, state, zip, phoneNumber, email, employmentDate]
        );    
        if (result.rowCount === 0) {
            throw new Error('Failed to create staff');
        }
        console.log('Staff created successfully:', result.rows[0]);
        return result.rows[0];
    } catch (error) {
        handleDatabaseError(res, error);
        return;
    }
};

const getStaff = async (req,res) => {
    try {
        const result = await readPool.query(getStaffQuery);
        res.status(200).json({count: result.rows.length, result: result.rows});
    } catch (error) {
        handleDatabaseError(res, error);
        return;
    }
};

const getStaffById = async (req,res) => {
    const { id } = req.params;
    try {
        const result = await readPool.query(getStaffByIdQuery, [id]);
        const staff = result.rows[0];
        if (!staff) {
            res.status(404).json({error: 'Staff not found'});
            return;
        }
        res.status(200).json(staff);
    } catch (error) {
        handleDatabaseError(res, error);
        return;
    }
};

const updateStaff = async (data) => { 
    const { id, name, specialization, hospitalId, street, city, state, zip, phoneNumber, email, employmentDate } = data;    
    try {
        const result = await readPool.query(updateStaffQuery, 
            [name, specialization, hospitalId, street, city, state, zip, phoneNumber, email, employmentDate, id]
        );
        if(result.rowCount === 0) { 
            res.status(404).json({error: 'Staff not found'});
            return;
        }
        res.status(200).json(result.rows[0]);
    } catch (error) {
        handleDatabaseError(res, error);
        return;
    }
};

// ----------------- Hospital -----------------
const createHospitalQuery = `INSERT INTO hospital (hospital_id, name) VALUES ($1, $2);`;
const updateHospitalQuery = `UPDATE hospital SET name = $1 WHERE hospitalId = $2;`;

// Hospital
const createHospital = async (data) => {
    const { hopitalId, name } = data;

    try {
        const result = await readPool.query(createHospitalQuery, [hopitalId, name]);
        if (result.rowCount === 0) {
            res.status(400).json({ error: 'Failed to create hospital' });
            return;
        }
        res.status(201).json({ message: 'Hospital created successfully' });
    } catch (error) {
        handleDatabaseError(res, error);
        return;
    }
 };


const updateHospital = async (data) => {
    const { hopitalId, name } = data;
    try {
        const result = await readPool.query(updateHospitalQuery, [name, hopitalId]);
        if (result.rowCount === 0) {
            res.status(404).json({ error: 'Hospital not found' });
            return;
        }
        res.status(201).json({ message: 'Hospital updated successfully' });
    } catch (error) {
        handleDatabaseError(res, error);
        return;
    }
};


module.exports = { createStaff, getStaff, getStaffById, updateStaff, createHospital, updateHospital };