namespace JobInterviewProject.MVC.Services
{
    public interface IHttpClientService
    {
        Task<T> GetAsync<T>(string url, string authorizationToken = null);

        Task<T> PostAsync<T>(string url, object data, string authorizationToken = null);

        Task<T> PutAsync<T>(string url, object data, string authorizationToken = null);

        Task DeleteAsync(string url, string authorizationToken = null);
    }
}
