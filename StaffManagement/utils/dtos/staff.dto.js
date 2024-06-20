class CreateStaffDTO {
    constructor(
        name,
        specialazation,
        hospitalId,
        hospital,
        street,
        city,
        state,
        zip,
        phoneNumber,
        email,
        employmentDate,
    ) {
        this.name = name;
        this.specialazation = specialazation;
        this.hospitalId = hospitalId;
        this.hospital = hospital;
        this.address = { street, city, state, zip };
        this.phoneNumber = phoneNumber;
        this.email = email;
        this.employmentDate = employmentDate;
    }
}

module.exports = StaffDTO;