const { getChannel, setupConsumerByTopic } = require('../../connections/connectRabbitMQ');

const startHospitalUpdateConsumer = async () => {
    await setupConsumerByTopic(process.env.HOSPITAL_QUEUE_RMC,process.env.HOSPITAL_UPDATED_RMC_KEY, hospitalUpdateHandler);
};

const hospitalUpdateHandler = (msg) => {
    const messageContent = msg.content.toString();
    console.log(`Received message from another queue hospital with *.created:`, messageContent);
    const channel = getChannel();
    channel.ack(msg);
};



module.exports = startHospitalUpdateConsumer;
