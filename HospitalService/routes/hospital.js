var express = require('express');
var router = express.Router();
const { createHospital, updateHospital, getHospital, getHospitalById } = require('../services/hospital.service');

/* GET hospitals listing. */
router.get('/', getHospital);
/* GET hospital. */
router.get('/:id', getHospitalById);
/* PUT hospital. */
router.put('/:id', updateHospital); 
/* POST hospital. */
router.post('/', createHospital);

module.exports = router;
