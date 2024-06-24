const {readPool, writePool} = require('../connections/connectPostgreDB');
const { sendMessageToExchange } = require('../connections/connectRabbitMQ');
const { v4: uuidv4 } = require('uuid');

const findStaff = `SELECT * FROM staff WHERE name = $1;`;
const postEventQuery = `INSERT INTO eventStore (type, payload) VALUES ($1, $2) RETURNING *;`;

const handleDatabaseError = (err) => { 
    console.error(err);
};

const adHocHandler = async (staffMembers) => {
    try {
        for (const staffMember of staffMembers) {
            const staffRecord = await readPool.query(findStaff, [staffMember.name]);
            if (staffRecord.rows.length > 0) {
                if (staffIsChanged(staffMember, staffRecord)) {
                    // staff is changed so update
                    const updatedStaffMember = addDefaultStaffProperties({ ...staffMember });
                    await adHocUpdateHandler({ id: staffRecord.rows[0].id, ...updatedStaffMember });
                }   
                // staff is not changed so do nothing
            } else {
                // staff does not exist yet so create
                const newStaffMember = addDefaultStaffProperties({ ...staffMember });
                await adHocCreateHandler({ id: uuidv4(), ...newStaffMember });
            }
        }
    } catch (error) {
        console.error('Error fetching and processing staff members:', error);
    }
};

const adHocCreateHandler = async (staffMember) => { 
    try {
        const payloadString = JSON.stringify(staffMember);
        await writePool.query(postEventQuery, [process.env.STAFF_CREATED_RMC_KEY,payloadString]);
        sendMessageToExchange(process.env.EXCHANGE_RMC, process.env.STAFF_CREATED_RMC_KEY, payloadString);
    } catch (err) {
        handleDatabaseError(err);
        return;
    }
}

const adHocUpdateHandler = async (staffMember) => {
    try {
        const payloadString = JSON.stringify(staffMember);
        await writePool.query(postEventQuery, [process.env.STAFF_UPDATED_RMC_KEY, payloadString]);
        sendMessageToExchange(process.env.EXCHANGE_RMC, process.env.STAFF_UPDATED_RMC_KEY, payloadString);
    } catch (err) {
        handleDatabaseError(err);
        return;
    }   
}

const staffIsChanged = (staffMember, staffRecord) => {
    for (const key in staffMember) {
        const keyL = key.toLowerCase(); 
        const valueM = staffMember[key];
        const valueE = staffRecord.rows[0][keyL];   
        if (valueM !== valueE) {
            return true;
        }
    }
    return false;
}

const addDefaultStaffProperties = (staffMember) => { 
    return { 
        ...staffMember,
        email: staffMember.name +`@example.com`,
        specialization: "Cardiology",
        employmentDate: new Date('2020-01-01T00:00:00.000Z'),
        hospitalId: "123e4567-e89b-12d3-a456-426614174000" };
}

module.exports = { adHocHandler };