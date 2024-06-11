const amqp = require('amqplib');
require('dotenv').config();

let channel, connection;

const connectRabbitMQ = async () => {
    try {
        const connectionString = `amqps://${process.env.RABBITMQ_USER}:${process.env.RABBITMQ_PASSWORD}@${process.env.RABBITMQ_HOST}/${process.env.RABBITMQ_VHOST}`;
        connection = await amqp.connect(connectionString);
        channel = await connection.createChannel();
        console.log('Connected to RabbitMQ');
    } catch (err) {
        console.error('Error connecting to RabbitMQ:', err);
    }
};

const getChannel = () => channel;

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

const sendMessage = async (queueName, message) => {
    try {
        if (!channel) {
            console.error('Channel is not initialized');
            return;
        }
        await channel.assertQueue(queueName, { durable: true });
        channel.sendToQueue(queueName, Buffer.from(message));
        console.log(`Sent message to ${queueName}: ${message}`);
    } catch (error) {
        console.error('Failed to send message', error);
    }
};

module.exports = { connectRabbitMQ, getChannel, setupConsumer, sendMessage };
