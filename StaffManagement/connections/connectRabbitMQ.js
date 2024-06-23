const amqp = require('amqplib');
require('dotenv').config();

let channel, connection;

const connectRabbitMQ = async () => {
    try {
        const connectionString = `amqp://${process.env.USER_RMC}:${process.env.PWD_RMC}@${process.env.HOST_RMC}:${process.env.PORT_RMC}${process.env.VHOST_RMC}`;
        connection = await amqp.connect(connectionString);
        channel = await connection.createChannel();
        console.log('Connected to RabbitMQ');
    } catch (err) {
        console.error('Error connecting to RabbitMQ:', err);
    }
};

const getChannel = () => channel;

const setupConsumer = async (queueName, routingKeyPattern, onMessage) => {
    try {
        if (!channel) {
            console.error('Channel is not initialized');
            return;
        }
        await channel.assertExchange(process.env.EXCHANGE_RMC, 'topic', { durable: true });
        await channel.assertQueue(queueName, { durable: true });
        await channel.bindQueue(queueName, process.env.EXCHANGE_RMC, routingKeyPattern + '#');
        channel.consume(queueName, (msg) => {
            if (msg) {
                const routingKey = msg.fields.routingKey;
                onMessage(msg, routingKey);
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
        await channel.assertExchange(exchangeName, 'topic', { durable: true });
        channel.publish(exchangeName, routingKey, Buffer.from(message));
        console.log(`Message sent to exchange ${exchangeName} with routing key ${routingKey}!`);
    } catch (error) {
        console.error('Error publishing message:', error);
    }
};


module.exports = { connectRabbitMQ, getChannel, setupConsumer, sendMessageToExchange};
