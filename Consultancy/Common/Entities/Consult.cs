using Consultancy.Common.Aggregates;
using Consultancy.Features.ConsultFeature.CreateConsult.RabbitEvent;
using Consultancy.Features.ConsultFeature.UpdateNotes.RabbitEvent;
using Consultancy.Features.ConsultFeature.UpdateQuestions.RabbitEvent;

namespace Consultancy.Common.Entities
{
    public sealed class Consult
        : AggregateRoot
    {
        public Guid PatientId { get; set; } = Guid.Empty;
        public Guid AppointmentId { get; set; } = Guid.Empty;
        public Survey? Survey { get; set; } = null;
        public string? Notes { get; set; } = string.Empty;

        public void Apply(ConsultCreatedEvent @event)
        {
            PatientId = @event.Consult.PatientId;
            AppointmentId = @event.Consult.AppointmentId;
            Survey = @event.Consult.Survey;
            Notes = @event.Consult.Notes;
        }

        public void Apply(DossierConsultAppendedEvent @event)
        {
            Notes = @event.Consult.Notes;
        }

        public void Apply(ConsultSurveyFilledInEvent @event)
        {
            if (Survey != null)
                Survey.Questions = @event.Questions;
        }
    }
}
