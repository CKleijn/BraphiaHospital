var express = require('express');
var router = express.Router();
const { getHospital, getHospitalById } = require('../services/hospitalRead.service');
const { createEvent, updateEvent } = require('../services/hospitalWrite.service');

/* GET hospitals listing. */
router.get('/', getHospital);
/* GET hospital. */
router.get('/:id', getHospitalById);


/* POST hospital. */
router.post('/', createEvent);
/* POST hospital. */
router.put('/', updateEvent);

module.exports = router;
