﻿using AppointmentManagement.Common.Abstractions;
using AppointmentManagement.Common.Entities;

namespace AppointmentManagement.Infrastructure.Persistence.Stores
{
    public interface IEventStore
    {
        Task<IEnumerable<NotificationEvent>> GetAllEventsByAggregateId(Guid aggregateId, int? fromVersion, CancellationToken cancellationToken);
        Task<bool> EventByAggregateIdExists(Guid aggregateId, CancellationToken cancellationToken);
        Task<bool> PatientBSNMatchesReferral(string bsn, Guid aggregateId, CancellationToken cancellationToken);
        Task<bool> AddEvent(NotificationEvent @event, CancellationToken cancellationToken);
    }
}
