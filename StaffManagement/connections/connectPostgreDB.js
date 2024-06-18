require('dotenv').config();
const { Pool } = require('pg');

const readPool = new Pool({
    user: process.env.DATABASE_USER || 'admin',
    host: process.env.DATABASE_HOST_READ || 'staff.database',
    database: process.env.DATABASE_NAME_READ || 'StaffDb',
    password: process.env.DATABASE_PASSWORD || 'admin',
    port: process.env.DATABASE_PORT || 5432,
});

const writePool = new Pool({
    user: process.env.DATABASE_USER || 'admin',
    host: process.env.DATABASE_HOST_WRITE || 'staff.eventstore',
    database: process.env.DATABASE_NAME_WRITE || 'StaffEventStore',
    password: process.env.DATABASE_PASSWORD || 'admin',
    port: process.env.DATABASE_PORT || 5432,
});

module.exports = { readPool, writePool };
