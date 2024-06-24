require('dotenv').config();
const { readPool } = require('../connections/connectPostgreDB');

const createStaffQuery = `INSERT INTO staff (id, name, specialization, street, houseNumber, city, zip, phoneNumber, email, employmentDate, hospitalId)
                            VALUES ($1, $2, $3, $4, $5, $6, $7, $8, $9, $10, $11) RETURNING *;`;
const getStaffQuery = `SELECT * FROM staff`;
const getStaffByIdQuery = `SELECT * FROM staff WHERE id = $1`;
const getStaffByNameQuery = `SELECT * FROM staff WHERE name = $1;`;
const updateStaffQuery = `UPDATE staff SET name = $1, specialization = $2, street = $3, houseNumber = $4, city = $5, 
                            zip = $6, phoneNumber = $7, email= $8, employmentDate = $9, hospitalId = $10 WHERE id = $11 RETURNING *;`;


const handleDatabaseError = (err) => { 
    console.error('Database Error: ', err);
};

const createStaff = async (data) => {
    const { id, name, specialization, street, houseNumber, city, zip, phoneNumber, email, employmentDate, hospitalId } = data;
    try {
        const result = await readPool.query(createStaffQuery, 
            [id, name, specialization, street, houseNumber, city, zip, phoneNumber, email, employmentDate, hospitalId]
        );    
        if (result.rowCount === 0) {
            throw new Error('Failed to create staff');
        }
        console.log(`Staff record created successfully!`);
        return result.rows[0];
    } catch (error) {
        handleDatabaseError(error);
        return;
    }
};

const updateStaff = async (data) => { 
    const { id, name, specialization, street, houseNumber, city, zip, phoneNumber, email, employmentDate, hospitalId } = data;    
    try {
        const result = await readPool.query(updateStaffQuery, 
            [name, specialization, street, houseNumber, city, zip, phoneNumber, email, employmentDate, hospitalId, id]
        );
        if(result.rowCount === 0) { 
            console.log(`Staff not found! ${id}`);
            return;
        }
        console.log(`Staff record updated successfully! ${id}`);
        return result.rows[0];
    } catch (error) {
        handleDatabaseError(error);
        return;
    }
};

const getStaff = async (req,res) => {
    try {
        const result = await readPool.query(getStaffQuery);
        res.status(200).json({count: result.rows.length, result: result.rows});
    } catch (error) {
        handleDatabaseError(error);
        return;
    }
};

const getStaffById = async (req,res) => {
    const { id } = req.params;
    try {
        const result = await readPool.query(getStaffByIdQuery, [id]);
        const staff = result.rows[0];
        if (!staff) {
            res.status(404).json({error: `Staff not found! ${id}`});
            return;
        }
        res.status(200).json(staff);
    } catch (error) {
        handleDatabaseError(res, error);
        return;
    }
};

const getStaffByName = async (name) => { 
    try {
        const result = await readPool.query(getStaffByNameQuery, [name]);
        return result.rows[0] > 0;
    } catch (error) {
        handleDatabaseError(error);
        return;
    }
}

module.exports = { createStaff, getStaff, getStaffById, updateStaff, getStaffByName};