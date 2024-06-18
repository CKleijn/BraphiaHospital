var express = require('express');
var router = express.Router();
const {getStaff, getStaffById } = require('../services/staffRead.service')
const { createEvent, updateEvent } = require('../services/staffWrite.service')

/* GET staffs listing. */
router.get('/', getStaff);
/* GET staff. */
router.get('/:id', getStaffById);


/* POST staff. */
router.post('/', createEvent);
/* PUT staff. */
router.put('/:id', updateEvent); 

module.exports = router;
