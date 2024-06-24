const { getChannel, setupConsumer } = require('../../connections/connectRabbitMQ');
const { createStaff,updateStaff } = require('../../services/staffRead.service');

const startStaffQueueConsumer = async () => {
        await setupConsumer(process.env.STAFF_QUEUE_RMC, process.env.STAFF_RMC_KEY, staffEventHandler);
};

const staffEventHandler = async (msg, key) => {
    try {
        var result; 

        switch (key) { // Switch statement to handle different routing keys 
            case process.env.STAFF_CREATED_RMC_KEY:
                console.log('Handling staff created event');
                await createStaff(JsonParse(msg,key));
                break;
            case process.env.STAFF_UPDATED_RMC_KEY:
                console.log('Handling staff updated event');
                await updateStaff(JsonParse(msg,key)); 
                break;
            default:
                console.log('Unknown routing key');
                break;  
        }

        console.log(`${key} succesfully handled:`);
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
    console.log(`Received message from hospital queue with routing key ${key}!`);
    return JSON.parse(messageContent);
}


module.exports = startStaffQueueConsumer ;
