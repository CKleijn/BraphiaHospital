using MediatR;
using Consultancy.Common.Entities;

namespace Consultancy.Features.ConsultFeature.GetConsults.Query
{
    public sealed record GetConsultsQuery()
        : IRequest<IEnumerable<Consult>>;
}
