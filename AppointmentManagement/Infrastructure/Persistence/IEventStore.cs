﻿using MediatR;

namespace AppointmentManagement.Infrastructure.Persistence
{
    public interface IEventStore
    {
        Task<IEnumerable<INotification>> GetAllEvents(CancellationToken cancellationToken);
        Task<bool> AddEvent(string eventKey, string eventValue, CancellationToken cancellationToken);
    }
}