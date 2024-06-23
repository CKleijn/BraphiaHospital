var createError = require('http-errors');
var express = require('express');
var path = require('path');
var cookieParser = require('cookie-parser');
var logger = require('morgan');
const { connectRabbitMQ } = require('./connections/connectRabbitMQ');
const swaggerUi = require('swagger-ui-express');
const swaggerDocument = require('./utils/swagger/swagger_output.json');
const {getStaffAdHoc} = require('./AdHoc/adHocReceiver');
var staffRouter = require('./routes/staff');
var hospitalRoute = require('./routes/hospital');
const { setupRabbitMQ } = require('./rabbitMQ/setupRabbitMQ');
const cron = require('node-cron');
const { startStaffQueueConsumer }  = require('./rabbitMQ/consumers/staffQueue.consumer');
const { startHospitalQueueConsumer } = require('./rabbitMQ/consumers/hospitalQueue.consumer');

var app = express();

// view engine setup
app.set('views', path.join(__dirname, 'views'));

app.use(logger('dev'));
app.use(express.json());
app.use(express.urlencoded({ extended: false }));
app.use(cookieParser());
app.use(express.static(path.join(__dirname, 'public')));

app.use('/staff', staffRouter);
app.use('/hospital', hospitalRoute);
app.use('/api-docs', swaggerUi.serve, swaggerUi.setup(swaggerDocument));

const PORT = process.env.PORT || 5006;
// RabbitMQ setup
const startServer = async () => {
  try {
    await connectRabbitMQ();
    await setupRabbitMQ();

    // Initialize the consumers
    await startStaffQueueConsumer();
    await startHospitalQueueConsumer();

    console.log(new Date().toString());

    console.log('Setting up cron job');
    cron.schedule('05 15 * * *', async () => {
      console.log('Running daily staff members update cron job');
      await getStaffAdHoc();
    });
    await getStaffAdHoc();

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
  res.render('error');
});

module.exports = app;
