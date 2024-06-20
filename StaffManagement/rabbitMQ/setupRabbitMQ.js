require('dotenv').config();
const { getChannel } = require("../connections/connectRabbitMQ");

async function setupRabbitMQ() {
    try {
        const exchangeName = process.env.EXCHANGE_RMC;
        const queueNameS = process.env.STAFF_QUEUE_RMC;
        const queueNameH = process.env.HOSPITAL_QUEUE_RMC;
        const routingKeyS = process.env.STAFF_RMC_KEY
        const routingKeyH = process.env.HOSPITAL_RMC_KEY

        const queueNameAS = process.env.APPOINTMENT_STAFF_QUEUE_RMC;
        
        // Setup queues and bindings
        await setupQueue(queueNameS, exchangeName, routingKeyS);
        await setupQueue(queueNameH, exchangeName, routingKeyH);
        await setupQueue(queueNameAS, exchangeName, routingKeyS);
        
        console.log('Queues and bindings setup completed');
    } catch (error) {
        console.error('Error setting up queues and bindings:', error);
    }
}

async function setupQueue(queueName, exchangeName, routingKeyPattern) {
    try {
        const channel = getChannel();

        // Assert the exchange (create if not exists)
        await channel.assertExchange(exchangeName, 'topic', { durable: true });

        // Assert the queue (create if not exists)
        await channel.assertQueue(queueName, { durable: true });

        // Bind the queue to the exchange with the routing key pattern
        await channel.bindQueue(queueName, exchangeName, routingKeyPattern + '#');

        console.log(`Queue ${queueName} is bound to exchange ${exchangeName} with pattern ${routingKeyPattern}#`);

    } catch (error) {
        console.error('Error setting up queue:', error);
    }
}

module.exports = { setupRabbitMQ };
