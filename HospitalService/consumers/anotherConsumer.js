const { getChannel, setupConsumer } = require('../connections/connectRabbitMQ');

const anotherQueueHandler = (msg) => {
    const messageContent = msg.content.toString();
    console.log(`Received message from another queue:`, messageContent);
    const channel = getChannel();
    channel.ack(msg);
};

const startAnotherConsumer = async () => {
    await setupConsumer(process.env.RABBITMQ_QUEUE_PAYMENT, anotherQueueHandler);
};

module.exports = startAnotherConsumer;
