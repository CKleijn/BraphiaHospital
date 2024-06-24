namespace PatientManagement.Infrastructure.ExternalServices
{
    public interface IApiClient
    {
        Task<string> GetAsync(string uri, CancellationToken cancellationToken);
    }
}

