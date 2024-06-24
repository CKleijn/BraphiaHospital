const { getChannel, setupConsumer } = require('../../connections/connectRabbitMQ');
const { createHospital,updateHospital } = require('../../services/hospitalRead.service');

const startHospitalQueueConsumer = async () => {
        await setupConsumer(process.env.HOSPITAL_QUEUE_RMC, process.env.HOSPITAL_RMC_KEY, hospitalEventHandler);
};

const hospitalEventHandler = async (msg, key) => {
    try {
        var result;

        switch (key) {
            case process.env.HOSPITAL_CREATED_RMC_KEY:
                console.log('Handling hospital created event');
                await createHospital(JsonParse(msg,key));
                break;
            case process.env.HOSPITAL_UPDATED_RMC_KEY:
                console.log('Handling hospital updated event');
                await updateHospital(JsonParse(msg,key)); 
                break;
            default:
                console.log('Unknown routing key');
                break;
        }
        
        console.log(`${key} succesfully handled!`);
        const channel = getChannel();
        channel.ack(msg);
    } catch (error) {
        console.error('Error processing message:', error);
        const channel = getChannel();
        channel.nack(msg);
    }
};

const JsonParse = (msg, key) => {  
    const messageContent = msg.content.toString();
    console.log(`Received message from hospital queue with routing key ${key}!`);
    return JSON.parse(messageContent);
}

module.exports = startHospitalQueueConsumer ;
