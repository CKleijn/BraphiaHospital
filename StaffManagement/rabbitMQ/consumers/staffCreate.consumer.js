const { getChannel, setupConsumerByTopic } = require('../../connections/connectRabbitMQ');
const { createStaff } = require('../../services/staffRead.service');

const startStaffCreateConsumer = async () => {
        await setupConsumerByTopic(process.env.STAFF_QUEUE_RMC, process.env.STAFF_CREATED_RMC_KEY, staffCreateHandler);
};

const staffCreateHandler = async (msg) => {
    const messageContent = msg.content.toString();
    console.log(`Received message from staff queue with routing key staff.created:`, messageContent);

    try {
        const staff = JSON.parse(messageContent);
        const result = await createStaff(staff); // Ensure createStaff returns a promise
        console.log('Staff created:', result);

        const channel = getChannel();
        channel.ack(msg); // Acknowledge message after successful processing
    } catch (error) {
        console.error('Error processing message:', error);
        const channel = getChannel();
        channel.nack(msg); // Requeue message for processing if there's an error
    }
};


module.exports = startStaffCreateConsumer;
