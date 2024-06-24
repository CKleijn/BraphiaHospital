using CsvHelper;
using MediatR;
using PatientManagement.Features.Patient.RegisterPatient;
using PatientManagement.Infrastructure.ExternalServices;
using PatientManagement.Infrastructure.Persistence.Stores;
using System.Globalization;
using System.Text;

namespace PatientManagement.Features.Patient.SyncPatients
{
    public sealed class SyncPatientsCommandHandler(
        IApiClient apiClient,
        IEventStore eventStore,
        ISender sender)
        : IRequestHandler<SyncPatientsCommand>
    {
        public async Task Handle(
            SyncPatientsCommand command, 
            CancellationToken cancellationToken)
        {
            var csvData = await apiClient.GetAsync("https://marcavans.blob.core.windows.net/solarch/fake_customer_data_export.csv?sv=2023-01-03&st=2024-06-14T10%3A31%3A07Z&se=2032-06-15T10%3A31%3A00Z&sr=b&sp=r&sig=q4Ie3kKpguMakW6sbcKl0KAWutzpMi747O4yIr8lQLI%3D", cancellationToken);

            using var reader = new StringReader(csvData);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            
            csv.Context.RegisterClassMap<ExternalPatientMap>();

            var externalPatients = csv
                .GetRecords<ExternalPatient>()
                .ToList();

            foreach (var externalPatient in externalPatients)
            {
                var registerPatientCommand = new RegisterPatientCommand(
                    externalPatient.FirstName,
                    externalPatient.LastName,
                    GenerateRandomDate(),
                    await GenerateUniqueBSN(cancellationToken),
                    externalPatient.Address);

                await sender.Send(registerPatientCommand, cancellationToken);
            }
        }

        private DateTime GenerateRandomDate()
        {
            DateTime start = new(1950, 1, 1);
            var range = (DateTime.Today - start).Days;
            return start.AddDays(new Random().Next(range));
        }

        private async Task<string> GenerateUniqueBSN(CancellationToken cancellationToken)
        {
            Random random = new();
            StringBuilder sb = new(9);

            while (true)
            {
                for (int i = 0; i < 9; i++)
                {
                    sb.Append(random.Next(0, 10));
                }

                string bsn = sb.ToString();

                if (!await eventStore.PropertyExists("BSN", bsn, cancellationToken)) return bsn;

                sb.Clear();
            }
        }
    }
}
