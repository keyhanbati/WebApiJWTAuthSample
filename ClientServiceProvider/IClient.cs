using System.Threading.Tasks;

namespace ClientServiceProvider
{
    public interface IClient
    {
        Task<U> GetAsync<U>(string ApiPath);
        Task<U> GetAsync<T, U>(string ApiPath, T Param);
        Task<U> PostAsync<T, U>(string ApiPath, T Param);
    }
}