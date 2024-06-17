const amqp = require('amqplib');
require('dotenv').config();

let channel, connection;

const connectRabbitMQ = async () => {
    try {
        // remote connection string
        const connectionString = `amqps://${process.env.USER_RMC}:${process.env.PWD_RMC}@${process.env.HOST_RMC}/${process.env.VHOST_RMC}`;
        // local connection string
        // const connectionString = `amqp://${process.env.USER_RMC}:${process.env.PWD_RMC}@${process.env.HOST_RMC}:${process.env.PORT_RMC}${process.env.VHOST_RMC}`;
        connection = await amqp.connect(connectionString);
        channel = await connection.createChannel();
        console.log('Connected to RabbitMQ');
    } catch (err) {
        console.error('Error connecting to RabbitMQ:', err);
    }
};

const getChannel = () => channel;

// Listens to all messages in the queue
const setupConsumer = async (queueName, onMessage) => {
    try {
        if (!channel) {
            console.error('Channel is not initialized');
            return;
        }
        await channel.assertQueue(queueName, { durable: true });
        channel.consume(queueName, onMessage, { noAck: false });
        console.log(`Consuming messages from ${queueName}`);
    } catch (err) {
        console.error(`Error consuming messages from ${queueName}:`, err);
    }
};

// Listens to messages in the queue with the specific routing key pattern
const setupConsumerByTopic = async (queueName, routingKeyPattern, onMessage) => {
    try {
        if (!channel) {
            console.error('Channel is not initialized');
            return;
        }
        // Assert the queue to ensure it exists
        await channel.assertQueue(queueName, { durable: true });

        // Consume messages from the queue with the specific routing key pattern
        channel.consume(queueName, (msg) => {
            if (msg.fields.routingKey === routingKeyPattern) {
                onMessage(msg);
            }
        }, { noAck: false });

        console.log(`Consuming messages from ${queueName} with topic '${routingKeyPattern}'`);
    } catch (err) {
        console.error(`Error consuming messages from ${queueName}:`, err);
    }
};

const sendMessageToExchange = async (exchangeName, routingKey, message) => {
    try {
        const channel = getChannel();
        if (!channel) {
            console.error('Channel is not initialized');
            return;
        }

        // Assert the exchange (create if not exists)
        await channel.assertExchange(exchangeName, 'topic', { durable: true });

        // Publish the message to the exchange with the specified routing key
        channel.publish(exchangeName, routingKey, Buffer.from(message));
        console.log(`Message sent to exchange ${exchangeName} with routing key ${routingKey}: ${message}`);

    } catch (error) {
        console.error('Error publishing message:', error);
    }
};

module.exports = { connectRabbitMQ, getChannel, setupConsumer, setupConsumerByTopic, sendMessageToExchange};
