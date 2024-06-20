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
    // Construct the payload object to be inserted
    const payloadToInsert = {
        id: uuidv4(),
        ...req.body
    };

    try {
        // Convert updatedPayload to a string for storage and message sending
        const payloadString = JSON.stringify(payloadToInsert);

        // Execute the query to store the event
        const newEvent = await writePool.query(postEventQuery, [process.env.HOSPITAL_CREATED_RMC_KEY,payloadString]);

        // Send message to exchange with the complete payload
        sendMessageToExchange(process.env.EXCHANGE_RMC, process.env.HOSPITAL_CREATED_RMC_KEY, payloadString);

        res.status(201).json(newEvent);
    } catch (err) {
        handleDatabaseError(res, err);
        return;
    }
};


const updateEvent = async (req, res) => { 
    const id = req.params.id;
    const payload = { id, ...req.body };

    try {
        const payloadString = JSON.stringify(payload);

        await writePool.query(postEventQuery, [process.env.HOSPITAL_UPDATED_RMC_KEY, payloadString]);

        // send message to exchange based on event type
        sendMessageToExchange(process.env.EXCHANGE_RMC, process.env.HOSPITAL_UPDATED_RMC_KEY, payloadString);

        res.status(201).json(newEvent);
    } catch (err) {
        handleDatabaseError(res, err);
        return;
    }
};

module.exports = { createEvent, updateEvent };