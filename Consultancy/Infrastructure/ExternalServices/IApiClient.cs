public interface IApiClient
{
    Task<T?> GetAsync<T>(string uri, CancellationToken cancellationToken) where T : class;
}
