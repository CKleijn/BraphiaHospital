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
        public partial Appointment ScheduleAppointmentCommandToAppointment(ScheduleAppointmentCommand command);
        public partial AppointmentScheduledEvent AppointmentToAppointmentScheduledEvent(Appointment appointment);

        public partial Appointment RescheduleAppointmentCommandToAppointment(RescheduleAppointmentCommand command);
        public partial AppointmentRescheduledEvent AppointmentToAppointmentRescheduledEvent(Appointment appointment);

        public partial Appointment UpdatePatientArrivalCommandToAppointment(UpdatePatientArrivalCommand command);
        public partial PatientArrivalUpdatedEvent AppointmentToPatientArrivalUpdatedEvent(Appointment appointment);
    }
}
