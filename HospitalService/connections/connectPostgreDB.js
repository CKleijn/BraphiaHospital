require('dotenv').config();
const { Pool } = require('pg');

const readPool = new Pool({
    user: process.env.USER_DB,
    host: process.env.HOST_DB,
    database: process.env.READ_DB,
    password: process.env.PWD_DB || 'admin',
    port: 5432, // default port
});

const writePool = new Pool({
    user: process.env.USER_DB,
    host: process.env.HOST_DB,
    database: process.env.WRITE_DB,
    password: process.env.PWD_DB || 'admin',
    port: 5432, // default port
});

module.exports = { readPool, writePool};