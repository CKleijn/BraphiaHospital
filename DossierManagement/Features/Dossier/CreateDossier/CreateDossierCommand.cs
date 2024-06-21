﻿using MediatR;

namespace DossierManagement.Features.Dossier.CreateDossier
{
    public sealed record CreateDossierCommand(Guid PatientId)
        : IRequest;
}
