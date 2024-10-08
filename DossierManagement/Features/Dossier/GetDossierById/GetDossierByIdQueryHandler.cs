﻿using DossierManagement.Infrastructure.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DossierManagement.Features.Dossier.GetDossierById
{
    public sealed class GetDossierByIdQueryHandler(ApplicationDbContext context)
        : IRequestHandler<GetDossierByIdQuery, Dossier>
    {
        public async Task<Dossier> Handle(
            GetDossierByIdQuery request,
            CancellationToken cancellationToken)
        {
            var dossier = await context
                .Set<Dossier>()
                .Include(d => d.Patient)
                .Include(d => d.Consults)
                .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken)
                ?? throw new ArgumentNullException($"Dossier #{request.Id} doesn't exist");

            return dossier;
        }
    }
}
