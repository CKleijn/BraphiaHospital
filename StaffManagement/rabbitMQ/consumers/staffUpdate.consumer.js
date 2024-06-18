const { getChannel, setupConsumerByTopic } = require('../../connections/connectRabbitMQ');

const startStaffUpdateConsumer = async () => {
    await setupConsumerByTopic(process.env.STAFF_QUEUE_RMC, process.env.STAFF_UPDATED_RMC_KEY, staffUpdateHandler);
};

const staffUpdateHandler = async (msg) => {
    const messageContent = msg.content.toString();
    console.log(`Received message from staff queue with routing key staff.updated:`, messageContent);

    try {
        const staff = JSON.parse(messageContent);
        const result = await updateStaff(staff); // Ensure updateStaff returns a promise
        console.log('Staff updated:', result);

        const channel = getChannel();
        channel.ack(msg); // Acknowledge message after successful processing  
    } catch (errir) {
        console.error('Error processing message:', error);
        const channel = getChannel();
        channel.nack(msg); // Requeue message for processing if there's an error
    }
};



module.exports = startStaffUpdateConsumer;
