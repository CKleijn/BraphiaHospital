using AppointmentManagement.Common.Entities;
using AppointmentManagement.Features.AppointmentFeature.RescheduleAppointment.Command;
using AppointmentManagement.Features.AppointmentFeature.ScheduleAppointment.Command;
using AppointmentManagement.Features.AppointmentFeature.ScheduleAppointment.Event;
using AppointmentManagement.Features.AppointmentFeature.UpdatePatientArrival.Command;
using AppointmentManagement.Features.AppointmentFeature.UpdatePatientArrival.Event;

namespace AppointmentManagement.Common.Interfaces
{
    public interface IAppointmentMapper
    {
        Appointment ScheduleAppointmentCommandToAppointment(ScheduleAppointmentCommand command);
        AppointmentScheduledEvent AppointmentToAppointmentScheduledEvent(Appointment appointment);

        Appointment RescheduleAppointmentCommandToAppointment(RescheduleAppointmentCommand command);
        AppointmentRescheduledEvent AppointmentToAppointmentRescheduledEvent(Appointment appointment);

        Appointment UpdatePatientArrivalCommandToAppointment(UpdatePatientArrivalCommand command);
        PatientArrivalUpdatedEvent AppointmentToPatientArrivalUpdatedEvent(Appointment appointment);
    }
}