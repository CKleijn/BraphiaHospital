const axios = require('axios');
const csv = require('csv-parser');
const stream = require('stream');
const { promisify } = require('util');
const {adHocHandler} = require('./adHocHandler');
const e = require('express');
const pipeline = promisify(stream.pipeline);
const URL = 'https://marcavans.blob.core.windows.net/solarch/fake_customer_data_export.csv?sv=2023-01-03&st=2024-06-14T10%3A31%3A07Z&se=2032-06-15T10%3A31%3A00Z&sr=b&sp=r&sig=q4Ie3kKpguMakW6sbcKl0KAWutzpMi747O4yIr8lQLI%3D';

const getStaffAdHoc = async () => {
    console.log('(ADHOC) => Fetching staff members data...')
    try {
        const response = await axios.get(URL, { responseType: 'stream' });
        const staffMembers = [];
    
        await pipeline(
            response.data,
            csv(),
            new stream.Writable({
              objectMode: true,
              write(chunk, encoding, callback) {
              // Split address into street, number, zip, city
              const [streetNumber, zipCity] = chunk.Address.split(', ');
              const [zip, city] = zipCity.split(' ');
              const streetMatch = streetNumber.match(/^(.+)\s(\d+)$/);
              const street = streetMatch ? streetMatch[1] : '';
              const number = streetMatch ? streetMatch[2] : '';
              
                const staff = {
                    name: `${chunk['First Name']} ${chunk['Last Name']}`,
                    street,
                    houseNumber: number,
                    city,
                    zip,
                    phoneNumber: chunk['Phone Number'],
                }
                staffMembers.push(staff);
                callback();
              }
            })
          );
          console.log('Staff members data successfully retrieved. count: ' + staffMembers.length);
        await adHocHandler(staffMembers);
    
        console.log('Staff members data successfully retrieved and processed.');
    } catch (error) {
    console.error('Error fetching and processing staff members:', error);
    }
};

module.exports = { getStaffAdHoc };