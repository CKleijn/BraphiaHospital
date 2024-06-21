const { getChannel, setupConsumerByTopic } = require('../../connections/connectRabbitMQ');
const { createHospital } = require('../../services/hospitalRead.service');

const startHospitalQueueConsumer = async () => {
        await setupConsumerByTopic(process.env.HOSPITAL_QUEUE_RMC, process.env.HOSPITAL_RMC_KEY, hospitalCreateHandler);
};

const hospitalCreateHandler = async (msg) => {
    const messageContent = msg.content.toString();
    console.log(`Received message from hospital queue with routing key hospital.created:`, messageContent);

    try {
        const hospital = JSON.parse(messageContent);
        const result = await createHospital(hospital); // Ensure createHospital returns a promise
        console.log('Hospital created:', result);

        const channel = getChannel();
        channel.ack(msg); // Acknowledge message after successful processing
    } catch (error) {
        console.error('Error processing message:', error);
        const channel = getChannel();
        channel.nack(msg); // Requeue message for processing if there's an error
    }

};

module.exports = startHospitalQueueConsumer;
