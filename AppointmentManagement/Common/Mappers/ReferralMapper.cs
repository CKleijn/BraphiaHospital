using AppointmentManagement.Common.Entities;
using AppointmentManagement.Common.Interfaces;
using AppointmentManagement.Features.ReferralFeature.CreateReferral.Command;
using AppointmentManagement.Features.ReferralFeature.CreateReferral.Event;
using Riok.Mapperly.Abstractions;

namespace AppointmentManagement.Common.Mappers
{
    [Mapper]
    public partial class ReferralMapper
        : IReferralMapper
    {
        public partial Referral CreatedReferralCommandToReferral(CreateReferralCommand command);
        public partial ReferralCreatedEvent ReferralToReferralCreatedEvent(Referral referral);
    }
}
