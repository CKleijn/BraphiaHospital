using MediatR;
using Consultancy.Common.Entities;

namespace Consultancy.Features.ConsultFeature.GetConsult.Query
{
    public sealed record GetConsultQuery(Guid id)
        : IRequest<Consult>;
}
