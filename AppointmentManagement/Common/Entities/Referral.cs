using AppointmentManagement.Common.Abstractions;
using AppointmentManagement.Common.Aggregates;
using AppointmentManagement.Features.ReferralFeature.CreateReferral.Event;

namespace AppointmentManagement.Common.Entities
{
    public class Referral
        : AggregateRoot
    {
        public Guid HospitalFacilityId { get; set; }
        public string ReferralCode { get; set; } = Guid.NewGuid().ToString();
        public string BSN { get; set; } = string.Empty;
        public string Diagnosis { get; set; } = string.Empty;

        public void Apply(ReferralCreatedEvent @event)
        {
            HospitalFacilityId = @event.Referral.HospitalFacilityId;
            ReferralCode = @event.Referral.ReferralCode;
            BSN = @event.Referral.BSN;
            Diagnosis = @event.Referral.Diagnosis;
        }
    }
}