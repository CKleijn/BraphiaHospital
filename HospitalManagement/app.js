var createError = require('http-errors');
var express = require('express');
var path = require('path');
var cookieParser = require('cookie-parser');
var logger = require('morgan');
const { connectRabbitMQ} = require('./connections/connectRabbitMQ');
const startHospitalQueueConsumer = require('./rabbitMQ/consumers/hospitalQueue.consumer');
const swaggerUi = require('swagger-ui-express');
const swaggerDocument = require('./utils/swagger/swagger_output.json');
var hospitalRouter = require('./routes/hospital');
const { setupRabbitMQ } = require('./rabbitMQ/setupRabbitMQ');

var app = express();

// view engine setup
app.set('views', path.join(__dirname, 'views'));

app.use(logger('dev'));
app.use(express.json());
app.use(express.urlencoded({ extended: false }));
app.use(cookieParser());
app.use(express.static(path.join(__dirname, 'public')));

app.use('/hospital', hospitalRouter);
app.use('/api-docs', swaggerUi.serve, swaggerUi.setup(swaggerDocument));

const PORT = process.env.PORT || 5008;

// RabbitMQ setup
const startServer = async () => {
  try {
    await connectRabbitMQ();
    
    await setupRabbitMQ();

    // // Initialize the consumers
    // await startHospitalQueueConsumer();

    app.listen(PORT, () => {
      console.log(`Server listening on port ${PORT}`);
    });
  } catch (error) {
    console.error('Error starting server:', error);
    process.exit(1); // Exit the process if an error occurs during startup
  }
};
startServer();

// catch 404 and forward to error handler
app.use(function(req, res, next) {
  next(createError(404));
});

// error handler
app.use(function(err, req, res, next) {
  // set locals, only providing error in development
  res.locals.message = err.message;
  res.locals.error = req.app.get('env') === 'development' ? err : {};

  // render the error page
  res.status(err.status || 500);
  res.json({
    error: err.message || 'Internal server error'
  }); 
});

module.exports = app;
