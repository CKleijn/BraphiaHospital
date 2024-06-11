class CreateHospitalDTO {
    constructor(
        hospital,
        street,
        number,
        postalCode,
        city,
        country,
        stores,
        squares,
        phoneNumber,
        email,
        website,
        totalBeds,
        builtYear,
    ) {
        this.hospital = hospital;
        this.street = street;
        this.number = number;
        this.postalCode = postalCode;
        this.city = city;
        this.country = country;
        this.stores = stores;
        this.squares = squares;
        this.phoneNumber = phoneNumber;
        this.email = email;
        this.website = website;
        this.totalBeds = totalBeds;
        this.builtYear = builtYear;
    }
}

module.exports = HospitalDTO;