var createError = require('http-errors');
var express = require('express');
var path = require('path');
var cookieParser = require('cookie-parser');
var logger = require('morgan');
const { connectRabbitMQ} = require('./connections/connectRabbitMQ');
const startAnotherConsumer = require('./consumers/anotherConsumer');

var hospitalRouter = require('./routes/hospital');

var app = express();

// view engine setup
app.set('views', path.join(__dirname, 'views'));
app.set('view engine', 'jade');

app.use(logger('dev'));
app.use(express.json());
app.use(express.urlencoded({ extended: false }));
app.use(cookieParser());
app.use(express.static(path.join(__dirname, 'public')));

app.use('/hospital', hospitalRouter);
const PORT = process.env.PORT || 3001;

// RabbitMQ setup
const startServer = async () => {
  try {
    await connectRabbitMQ();
    
    // Initialize the consumers
    await startAnotherConsumer();

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
