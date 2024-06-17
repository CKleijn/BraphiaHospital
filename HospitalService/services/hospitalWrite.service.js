require('dotenv').config();
const { sendMessageToExchange } = require('../connections/connectRabbitMQ.js');
const { writePool } = require('../connections/connectPostgreDB.js'); 
const { v4: uuidv4 } = require('uuid');

const postEventQuery = `INSERT INTO eventStore (type, payload) VALUES ($1, $2) RETURNING *;`;

const handleDatabaseError = (res, err) => { 
    console.error(err);
    res.status(500).json({ error: 'Internal server error' });
};

const handleValidationErrors = (res, message) => { 
    res.status(400).json({ error: message });
};

const createEvent = async (req, res) => {
    const { name, specialization, street, city , state, zip, phone_number, email, employment_date, hospital_id} = req.body;

    // Construct the payload object to be inserted
    const payloadToInsert = {
        id: uuidv4(),
        name,
        specialization,
        street,
        city,
        state,
        zip,
        phone_number,
        email,
        employment_date,
        hospital_id
    };

    try {
        // Convert updatedPayload to a string for storage and message sending
        const payloadString = JSON.stringify(payloadToInsert);

        // Execute the query to store the event
        await writePool.query(postEventQuery, [process.env.STAFF_CREATED_RMC_KEY,payloadString]);

        // Send message to exchange with the complete payload
        sendMessageToExchange(process.env.EXCHANGE_RMC, process.env.STAFF_CREATED_RMC_KEY, payloadString);

        res.status(201).json(newEvent);
    } catch (err) {
        handleDatabaseError(res, err);
    }
};


const updateEvent = async (req, res) => { 
    try {
        await writePool.query(postEventQuery, [process.env.STAFF_UPDATED_RMC_KEY, req.body]);

        // send message to exchange based on event type
        sendMessageToExchange(process.env.EXCHANGE_RMC, process.env.STAFF_UPDATED_RMC_KEY, req.body);

        res.status(201).json(newEvent);
    } catch (err) {
        handleDatabaseError(res, err);
        return;
    }
};

module.exports = { createEvent, updateEvent };