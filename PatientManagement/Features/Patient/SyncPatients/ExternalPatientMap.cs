using CsvHelper.Configuration;

namespace PatientManagement.Features.Patient.SyncPatients
{
    public sealed class ExternalPatientMap 
        : ClassMap<ExternalPatient>
    {
        public ExternalPatientMap()
        {
            Map(m => m.CompanyName).Name("Company Name");
            Map(m => m.FirstName).Name("First Name");
            Map(m => m.LastName).Name("Last Name");
            Map(m => m.PhoneNumber).Name("Phone Number");
            Map(m => m.Address).Name("Address");
        }
    }
}
