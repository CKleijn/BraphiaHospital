﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using AppointmentManagement.Infrastructure.Persistence.Contexts;
using AppointmentManagement.Common.Entities;

namespace AppointmentManagement.Features.AppointmentFeature.GetAppointment.Query
{
    public class GetAppointmentQueryHandler(
        ApplicationDbContext context, IApiClient apiClient)
        : IRequestHandler<GetAppointmentQuery, Appointment>
    {
        public async Task<Appointment> Handle(
            GetAppointmentQuery request,
            CancellationToken cancellationToken)
        {
            var result = await context.Set<Appointment>()
                .Include(a => a.Referral)
                .Include(a => a.Physician)
                .Include(a => a.HospitalFacility)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if(result == null)
                throw new ArgumentNullException($"Appointment #{request.Id} doesn't exist");

            var patientUrl = $"{ConfigurationHelper.GetPatientManagementServiceConnectionString()}/patient/{result.PatientId}";

            result.Patient =  (await apiClient
                .GetAsync<Patient>(patientUrl, cancellationToken))!;

            return result;
        }
    }
}
