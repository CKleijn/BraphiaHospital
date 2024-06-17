require('dotenv').config();
const { Pool } = require('pg');

const pool = new Pool({
    user: process.env.USER_DB,
    host: process.env.HOST_DB,
    database: process.env.NAME_DB,
    password: process.env.PWD_DB || 'admin',
    port: 5432, // default port
});

pool.connect()
    .then(() => {
        console.log('Connected to PostgreSQL database.');
        return pool.query('SELECT version();');
    })
    .then(res => {
        console.log('PostgreSQL version:', res.rows[0].version);
    })
    .catch(err => {
        console.error('Error connecting to PostgreSQL database:', err);
    })
    .finally(() => {
        pool.end();
    });

module.exports = pool;