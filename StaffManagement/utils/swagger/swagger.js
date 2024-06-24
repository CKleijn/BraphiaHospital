const swaggerAutogen = require('swagger-autogen')();

const outputFile = './swagger_output.json';
const endpointsFiles = ['../routes/*.js']; // Path to the API routes

const doc = {
  info: {
    title: 'Express API with Swagger',
    description: 'A simple Express API application with Swagger documentation',
    version: '1.0.0',
  },
  host: 'localhost:5006', // Update with your host and port
  basePath: '/',
};

swaggerAutogen(outputFile, endpointsFiles, doc).then(() => {
  console.log('Swagger documentation generated successfully');
});
