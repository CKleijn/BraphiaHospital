const { getChannel, setupConsumer } = require('../../connections/connectRabbitMQ');
const { createHospital, updateHospital } = require('../../services/hospitalRead.service');

const startHospitalQueueConsumer = async () => {
        await setupConsumer(process.env.HOSPITAL_QUEUE_RMC, process.env.HOSPITAL_CREATED_RMC_KEY, hospitalEventHandler);
};

const hospitalEventHandler = async (msg, key) => {
    try {
        var result;

        switch (key) { // Switch statement to handle different routing keys
            case process.env.HOSPITAL_CREATED_RMC_KEY:
                result = await createHospital(JsonParse(msg,key));
                console.log('Handling hospital created event');
                break;
            case process.env.HOSPITAL_UPDATED_RMC_KEY:
                result = await updateHospital(JsonParse(msg,key)); 
                console.log('Handling hospital updated event');
                break;
            default:
                console.log('Unknown routing key');
                break;
        }
        console.log(`${key} succesfully handled:`, result);

        const channel = getChannel();
        channel.ack(msg); // Acknowledge message after successful processing
    } catch (error) {
        console.error('Error processing message:', error);
        const channel = getChannel();
        channel.nack(msg); // Requeue message for processing if there's an error
    }
};



const JsonParse = (msg, key) => {  
    const messageContent = msg.content.toString();
    console.log(`Received message from hospital queue with routing key ${key}:`, messageContent);
    return JSON.parse(messageContent);
}


module.exports = startHospitalCreateConsumer;
