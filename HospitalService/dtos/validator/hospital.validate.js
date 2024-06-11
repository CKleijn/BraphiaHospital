// validation.js
const validateCreateHospitalDTO = (body) => {
    const requiredFields = ['hospital', 'street', 'number', 'postalCode', 'city', 'country', 'stores', 'squares', 'phoneNumber', 'email', 'website', 'totalBeds', 'builtYear'];
    // Define expected types for each field
    const fieldTypes = {
        hospital: 'string',
        street: 'string',
        number: 'number',
        postalCode: 'string',
        city: 'string',
        country: 'string',
        stores: 'number',
        squares: 'number',
        phoneNumber: 'string',
        email: 'string',
        website: 'string',
        totalBeds: 'number',
        builtYear: 'number'
    };

    // required validation
    for (const field of requiredFields) {
        if (!body[field]) {
            return { error: `Missing field: ${field}` };
        }
        // Check if the type of the field matches the expected type
        if (typeof body[field] !== fieldTypes[field]) {
            return { error: `Invalid type for field ${field}. Expected ${fieldTypes[field]}, received ${typeof body[field]}` };
        }
    }    

    return null; // no validation errors
}

module.exports = {
    validateCreateHospitalDTO
};
