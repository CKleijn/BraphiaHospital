var express = require('express');
var router = express.Router();
const { getHospital, getHospitalById } = require('../services/hospitalRead.service')


/* GET staffs listing. */
router.get('/', getHospital);
/* GET staff. */
router.get('/:id', getHospitalById);

module.exports = router;
