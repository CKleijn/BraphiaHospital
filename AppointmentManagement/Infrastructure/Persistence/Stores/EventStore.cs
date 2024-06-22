using AppointmentManagement.Common.Abstractions;
using AppointmentManagement.Common.Annotations;
using AppointmentManagement.Common.Entities;
using AppointmentManagement.Features.AppointmentFeature.ScheduleAppointment.Event;
using AppointmentManagement.Features.AppointmentFeature.UpdatePatientArrival.Event;
using AppointmentManagement.Features.HospitalFacilityFeature.CreateHospitalFacility.Event;
using AppointmentManagement.Features.HospitalFacilityFeature.UpdateHospitalFacility.Event;
using AppointmentManagement.Features.ReferralFeature.CreateReferral.Event;
using AppointmentManagement.Features.StaffMemberFeature.CreateStaffMember.Event;
using AppointmentManagement.Features.StaffMemberFeature.UpdateStaffMember.Event;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace AppointmentManagement.Infrastructure.Persistence.Stores
{
    public class EventStore
        : IEventStore
    {
        public async Task<IEnumerable<NotificationEvent>> GetAllEventsByAggregateId(
                   Guid aggregateId,
                   int? fromVersion,
                   CancellationToken cancellationToken)
        {
            try
            {
                var events = new List<NotificationEvent>();

                using SqlConnection connection = new(ConfigurationHelper.GetConnectionString());

                await connection.OpenAsync(cancellationToken);

                // Base query
                string query = "SELECT Type, Payload, Version FROM Events WHERE AggregateId = @AggregateId";

                if (fromVersion.HasValue)
                    query += " AND Version > @FromVersion";

                query += " ORDER BY Version ASC";

                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@AggregateId", aggregateId);

                if (fromVersion.HasValue)
                    command.Parameters.AddWithValue("@FromVersion", fromVersion.Value);

                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        var type = reader.GetString(0);
                        var payload = reader.GetString(1);
                        var version = reader.GetInt32(2);

                        switch (type)
                        {
                            case nameof(ReferralCreatedEvent):
                                events.Add(new ReferralCreatedEvent(JsonConvert.DeserializeObject<Referral>(payload)!)
                                {
                                    AggregateId = aggregateId,
                                    Type = type,
                                    Payload = payload,
                                    Version = version,
                                });
                                break;
                            case nameof(HospitalFacilityCreatedEvent):
                                var createdHospitalFacility = JsonConvert.DeserializeObject<HospitalFacility>(payload);
                                if (createdHospitalFacility != null)
                                {
                                    events.Add(new HospitalFacilityCreatedEvent(createdHospitalFacility)
                                    {
                                        AggregateId = aggregateId,
                                        Type = type,
                                        Payload = payload,
                                        Version = version,
                                    });
                                }
                                break;
                            case nameof(HospitalFacilityUpdatedEvent):
                                var updatedHospitalFacility = JsonConvert.DeserializeObject<HospitalFacility>(payload);
                                if (updatedHospitalFacility != null)
                                {
                                    events.Add(new HospitalFacilityUpdatedEvent(
                                        updatedHospitalFacility.Id,
                                        updatedHospitalFacility.Hospital,
                                        updatedHospitalFacility.Street,
                                        updatedHospitalFacility.Number,
                                        updatedHospitalFacility.PostalCode,
                                        updatedHospitalFacility.City,
                                        updatedHospitalFacility.Country)
                                    {
                                        AggregateId = aggregateId,
                                        Type = type,
                                        Payload = payload,
                                        Version = version,
                                    });
                                }
                                break;
                            case nameof(StaffCreatedEvent):
                                var createdStaffMember = JsonConvert.DeserializeObject<StaffMember>(payload);
                                if (createdStaffMember != null)
                                {
                                    events.Add(new StaffCreatedEvent(createdStaffMember)
                                    {
                                        AggregateId = aggregateId,
                                        Type = type,
                                        Payload = payload,
                                        Version = version,
                                    });
                                }
                                break;
                            case nameof(StaffUpdatedEvent):
                                var updatedStaffMember = JsonConvert.DeserializeObject<StaffMember>(payload);
                                if (updatedStaffMember != null)
                                {
                                    events.Add(new StaffUpdatedEvent(
                                        updatedStaffMember.Id,
                                        updatedStaffMember.HospitalId,
                                        updatedStaffMember.Name,
                                        updatedStaffMember.Specialization)
                                    {
                                        AggregateId = aggregateId,
                                        Type = type,
                                        Payload = payload,
                                        Version = version,
                                    });
                                }
                                break; 
                            case nameof(AppointmentScheduledEvent):
                                var appointmentScheduledEvent = JsonConvert.DeserializeObject<AppointmentScheduledEvent>(payload)!;
                                events.Add(new AppointmentScheduledEvent(
                                   appointmentScheduledEvent.Id,
                                   appointmentScheduledEvent.PatientId,
                                   appointmentScheduledEvent.ReferralId,
                                   appointmentScheduledEvent.PhysicianId,
                                   appointmentScheduledEvent.HospitalFacilityId,
                                   appointmentScheduledEvent.ScheduledDateTime,
                                   appointmentScheduledEvent.Status)
                                {
                                    AggregateId = aggregateId,
                                    Type = type,
                                    Payload = payload,
                                    Version = version,
                                });
                                break;

                            case nameof(AppointmentRescheduledEvent):
                                var appointmentRescheduledEvent = JsonConvert.DeserializeObject<Appointment>(payload);
                                if (appointmentRescheduledEvent != null)
                                {
                                    events.Add(new AppointmentRescheduledEvent(
                                        appointmentRescheduledEvent.Id,
                                        appointmentRescheduledEvent.ScheduledDateTime)
                                    {
                                        AggregateId = aggregateId,
                                        Type = type,
                                        Payload = payload,
                                        Version = version,
                                    });
                                }
                                break;

                            case nameof(AppointmentArrivalUpdatedEvent):
                                var updatedPatientArrivalEvent = JsonConvert.DeserializeObject<Appointment>(payload);
                                if (updatedPatientArrivalEvent != null)
                                {
                                    events.Add(new AppointmentArrivalUpdatedEvent(
                                        updatedPatientArrivalEvent.Id,
                                        updatedPatientArrivalEvent.Status)
                                    {
                                        AggregateId = aggregateId,
                                        Type = type,
                                        Payload = payload,
                                        Version = version,
                                    });
                                }
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

        public async Task<bool> PatientBSNMatchesReferral(
        string bsn,
        Guid aggregateId,
        CancellationToken cancellationToken)
        {
            try
            {
                using SqlConnection connection = new(ConfigurationHelper.GetConnectionString());

                await connection.OpenAsync(cancellationToken);

                string query = "SELECT COUNT(1) FROM Events WHERE AggregateId = @AggregateId AND JSON_VALUE(Payload, '$.BSN') = @BSN;";

                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@BSN", bsn);
                command.Parameters.AddWithValue("@AggregateId", aggregateId);

                return ((int)await command.ExecuteScalarAsync(cancellationToken) > 0);
            }
            catch (Exception)
            {
                throw new Exception(Errors.SQL_READ_ERROR);
            }
        }

        public async Task<bool> AddEvent(
            NotificationEvent @event,
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