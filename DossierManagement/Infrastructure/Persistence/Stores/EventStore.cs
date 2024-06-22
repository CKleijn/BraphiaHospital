using DossierManagement.Common.Abstractions;
using DossierManagement.Common.Annotations;
using DossierManagement.Events.ConsultAppended;
using DossierManagement.Events.DossierCreated;
using DossierManagement.Events.MedicationPrescribed;
using DossierManagement.Events.PatientRegistered;
using DossierManagement.Features.Dossier;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace DossierManagement.Infrastructure.Persistence.Stores
{
    public class EventStore
        : IEventStore
    {
        public async Task<IEnumerable<Event>> GetAllEventsByAggregateId(
            Guid aggregateId,
            CancellationToken cancellationToken)
        {
            try
            {
                var events = new List<Event>();

                using SqlConnection connection = new(ConfigurationHelper.GetConnectionString());

                await connection.OpenAsync(cancellationToken);

                string query = "SELECT Type, Payload, Version FROM Events WHERE AggregateId = @AggregateId ORDER BY Version ASC";

                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@AggregateId", aggregateId);

                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        var type = reader.GetString(0);
                        var payload = reader.GetString(1);
                        var version = reader.GetInt32(2);

                        switch (type)
                        {
                            case nameof(DossierCreatedEvent):
                                events.Add(new DossierCreatedEvent(JsonConvert.DeserializeObject<Dossier>(payload)!)
                                {
                                    AggregateId = aggregateId,
                                    Type = type,
                                    Payload = payload,
                                    Version = version,
                                });
                                break;
                            case nameof(PatientRegisteredEvent):
                                events.Add(new PatientRegisteredEvent(JsonConvert.DeserializeObject<Patient>(payload)!)
                                {
                                    AggregateId = aggregateId,
                                    Type = type,
                                    Payload = payload,
                                    Version = version,
                                });
                                break;
                            case nameof(DossierConsultAppendedEvent):
                                events.Add(new DossierConsultAppendedEvent(JsonConvert.DeserializeObject<Consult>(payload)!)
                                {
                                    AggregateId = aggregateId,
                                    Type = type,
                                    Payload = payload,
                                    Version = version,
                                });
                                break;
                            case nameof(DossierMedicationPrescribedEvent):
                                var dossier = JsonConvert.DeserializeObject<Dossier>(payload)!;
                                events.Add(new DossierMedicationPrescribedEvent(dossier.PatientId, dossier.Medications?.ToList())
                                {
                                    AggregateId = aggregateId,
                                    Type = type,
                                    Payload = payload,
                                    Version = version,
                                });
                                break;
                        }
                    }
                }

                return events;
            }
            catch (Exception)
            {
                throw new Exception(Errors.SQL_READ_ERROR);
            }
        }

        public async Task<Guid> GetDossierAggregateIdByPatientId(
            Guid patientId,
            CancellationToken cancellationToken)
        {
            try
            {
                using SqlConnection connection = new(ConfigurationHelper.GetConnectionString());

                await connection.OpenAsync(cancellationToken);

                string query = "SELECT TOP 1 AggregateId FROM Events WHERE JSON_VALUE(Payload, '$.PatientId') = @PatientId AND Type = @Type";

                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@PatientId", patientId);
                command.Parameters.AddWithValue("@Type", nameof(DossierCreatedEvent));

                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        var aggregateId = reader.GetGuid(0);
                        return aggregateId;
                    }
                }

                return Guid.Empty;
            }
            catch (Exception)
            {
                throw new Exception(Errors.SQL_READ_ERROR);
            }
        }

        public async Task<int> GetLatestVersionOfEventByAggregateId(
            Guid aggregateId, 
            string @event, 
            CancellationToken cancellationToken)
        {
            try
            {
                using SqlConnection connection = new(ConfigurationHelper.GetConnectionString());

                await connection.OpenAsync(cancellationToken);

                string query = "SELECT TOP 1 Version FROM Events WHERE AggregateId = @AggregateId AND Type = @Type ORDER BY Version DESC";

                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@AggregateId", aggregateId);
                command.Parameters.AddWithValue("@Type", @event);

                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        var latestVersion = reader.GetInt32(0);
                        return latestVersion;
                    }
                }

                return -1;
            }
            catch (Exception)
            {
                throw new Exception(Errors.SQL_READ_ERROR);
            }
        }

        public async Task<bool> EventByAggregateIdExists(
            Guid aggregateId,
            CancellationToken cancellationToken)
        {
            try
            {
                using SqlConnection connection = new(ConfigurationHelper.GetConnectionString());

                await connection.OpenAsync(cancellationToken);

                string query = "SELECT COUNT(1) FROM Events WHERE AggregateId = @AggregateId";

                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@AggregateId", aggregateId);

                return ((int)await command.ExecuteScalarAsync(cancellationToken) > 0);
            }
            catch (Exception)
            {
                throw new Exception(Errors.SQL_READ_ERROR);
            }
        }

        public async Task<bool> DossierWithPatientExists(
            Guid patientId,
            CancellationToken cancellationToken)
        {
            try
            {
                using SqlConnection connection = new(ConfigurationHelper.GetConnectionString());

                await connection.OpenAsync(cancellationToken);

                string query = "SELECT COUNT(1) FROM Events WHERE JSON_VALUE(Payload, '$.PatientId') = @PatientId AND Type = @Type";

                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@PatientId", patientId);
                command.Parameters.AddWithValue("@Type", nameof(DossierCreatedEvent));

                return ((int)await command.ExecuteScalarAsync(cancellationToken) > 0);
            }
            catch (Exception)
            {
                throw new Exception(Errors.SQL_READ_ERROR);
            }
        }

        public async Task<bool> PatientExists(
            Guid patientId,
            CancellationToken cancellationToken)
        {
            try
            {
                using SqlConnection connection = new(ConfigurationHelper.GetConnectionString());

                await connection.OpenAsync(cancellationToken);

                string query = "SELECT COUNT(1) FROM Events WHERE JSON_VALUE(Payload, '$.Id') = @PatientId AND Type = @Type";

                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@PatientId", patientId);
                command.Parameters.AddWithValue("@Type", nameof(PatientRegisteredEvent));

                return ((int)await command.ExecuteScalarAsync(cancellationToken) > 0);
            }
            catch (Exception)
            {
                throw new Exception(Errors.SQL_READ_ERROR);
            }
        }

        public async Task<bool> AddEvent(
            Event @event,
            CancellationToken cancellationToken)
        {
            try
            {
                using SqlConnection connection = new(ConfigurationHelper.GetConnectionString());

                await connection.OpenAsync(cancellationToken);

                string validationQuery = "SELECT COUNT(1) FROM Events WHERE AggregateId = @AggregateId AND Type = @Type AND Version = @Version";

                using SqlCommand validationCommand = new(validationQuery, connection);
                validationCommand.Parameters.AddWithValue("@AggregateId", @event.AggregateId);
                validationCommand.Parameters.AddWithValue("@Type", @event.Type);
                validationCommand.Parameters.AddWithValue("@Version", @event.Version);

                if ((int)await validationCommand.ExecuteScalarAsync(cancellationToken) > 0) return false;

                string query = "INSERT INTO Events (AggregateId, Type, Payload, Version) VALUES (@AggregateId, @Type, @Payload, @Version)";

                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@AggregateId", @event.AggregateId);
                command.Parameters.AddWithValue("@Type", @event.Type);
                command.Parameters.AddWithValue("@Payload", @event.Payload);
                command.Parameters.AddWithValue("@Version", @event.Version);

                await command.ExecuteNonQueryAsync(cancellationToken);

                return true;
            }
            catch (Exception)
            {
                throw new Exception(Errors.SQL_INSERT_ERROR);
            }
        }
    }
}
