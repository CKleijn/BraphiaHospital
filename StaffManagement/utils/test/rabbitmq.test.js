// Importeer de connectiefunctionaliteiten vanuit het bestand
const { connectRabbitMQ } = require('../../connections/connectRabbitMQ');

// Roep de connectie aan
connectRabbitMQ()
  .then(() => {
    console.log('Connection to RabbitMQ successful');
  })
  .catch((err) => {
    console.error('Error connecting to RabbitMQ:', err);
  });
