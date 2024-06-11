const handleDatabaseError = (res, err) => { 
    console.error(err);
    res.status(500).json({error: 'Internal server error'});
};

const handleValidationErrors = (message) => { 
    res.status(400).json({error: message});
};

module.exports = { handleDatabaseError, handleValidationErrors };