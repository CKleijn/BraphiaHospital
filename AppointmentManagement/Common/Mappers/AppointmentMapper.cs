using Riok.Mapperly.Abstractions;
using AppointmentManagement.Common.Entities;
using AppointmentManagement.Common.Interfaces;
using AppointmentManagement.Features.AppointmentFeature.ScheduleAppointment.Event;
using AppointmentManagement.Features.AppointmentFeature.RescheduleAppointment.Command;
using AppointmentManagement.Features.AppointmentFeature.ScheduleAppointment.Command;
using AppointmentManagement.Features.AppointmentFeature.UpdatePatientArrival.Command;
using AppointmentManagement.Features.AppointmentFeature.UpdatePatientArrival.Event;

namespace AppointmentManagement.Common.Mappers
{
    [Mapper]
    public partial class AppointmentMapper
        : IAppointmentMapper
    {
        public partial Appointment RescheduleAppointmentCommandToAppointment(RescheduleAppointmentCommand command);
        public partial AppointmentRescheduledEvent AppointmentToAppointmentRescheduledEvent(Appointment appointment);

        public partial Appointment UpdatePatientArrivalCommandToAppointment(UpdateAppointmentArrivalCommand command);
        public partial AppointmentArrivalUpdatedEvent AppointmentToPatientArrivalUpdatedEvent(Appointment appointment);
    }
}
