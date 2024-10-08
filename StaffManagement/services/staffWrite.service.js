require('dotenv').config();
const { json } = require('express');
const { writePool } = require('../connections/connectPostgreDB');
const { sendMessageToExchange } = require('../connections/connectRabbitMQ');
const { v4: uuidv4 } = require('uuid');
const { getStaffByName } = require('./staffRead.service');

const postEventQuery = `INSERT INTO eventStore (type, payload) VALUES ($1, $2) RETURNING *;`;

const handleDatabaseError = (res, err) => { 
    console.error(err);
    res.status(500).json({ error: 'Internal server error' });
};

const createEvent = async (req, res) => {
    try {
        if (await getStaffByName(req.body.name)) {
            return res.status(400).json({ error: 'Staff member already exists with this name!' });
        }
    
        // Construct the payload object to be inserted
        const payloadToInsert = {
            id: uuidv4(),
            ...req.body
        };
        // Convert updatedPayload to a string for storage and message sending
        const payloadString = JSON.stringify(payloadToInsert);

        // Execute the query to store the event
        const newEvent = await writePool.query(postEventQuery, [process.env.STAFF_CREATED_RMC_KEY,payloadString]);

        // Send message to exchange with the complete payload
        sendMessageToExchange(process.env.EXCHANGE_RMC, process.env.STAFF_CREATED_RMC_KEY, payloadString);

        res.status(201).json(newEvent.rows[0]);
    } catch (err) {
        handleDatabaseError(res, err);
        return;
    } 
};

 const updateEvent = async (req, res) => { 
    const id = req.params.id;
    const payload = {
        id,
        ...req.body
    };

    try {
        // Convert updatedPayload to a string for storage and message sending
        const payloadString = JSON.stringify(payload);

        const newEvent = await writePool.query(postEventQuery, [process.env.STAFF_UPDATED_RMC_KEY, payloadString]);

        // send message to exchange based on event type
        sendMessageToExchange(process.env.EXCHANGE_RMC, process.env.STAFF_UPDATED_RMC_KEY, payloadString);

        res.status(201).json(newEvent.rows[0]);
    } catch (err) {
        handleDatabaseError(res, err);
        return;
    }
};

module.exports = { createEvent,updateEvent };