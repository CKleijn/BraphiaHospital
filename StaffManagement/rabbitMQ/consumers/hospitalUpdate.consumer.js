const { getChannel, setupConsumerByTopic } = require('../../connections/connectRabbitMQ');
const { updateHospital } = require('../../services/hospitalRead.service');

const startHospitalUpdateConsumer = async () => {
    await setupConsumerByTopic(process.env.HOSPITAL_QUEUE_RMC,process.env.HOSPITAL_UPDATED_RMC_KEY, hospitalUpdateHandler);
};

const hospitalUpdateHandler = async (msg) => {
    const messageContent = msg.content.toString();
    console.log(`Received message from hospital queue with routing key hospital.created:`, messageContent);
    
    try {
        const hospital = JSON.parse(messageContent);
        const result = await updateHospital(hospital); // Ensure updateHospital returns a promise
        console.log('Hospital updated:', result);

        const channel = getChannel();
        channel.ack(msg); // Acknowledge message after successful processing
    } catch (error) {
        console.error('Error processing message:', error);
        const channel = getChannel();
        channel.nack(msg); // Requeue message for processing if there's an error
    }
};



module.exports = startHospitalUpdateConsumer;
