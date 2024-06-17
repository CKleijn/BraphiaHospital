using MediatR;

namespace AppointmentManagement.Features.ReferralFeature.CreateReferral.Command
{
    public sealed record CreateReferralCommand(
        Guid HospitalFacilityId,
        string BSN,
        string Diagnosis
    ) : IRequest;
}
