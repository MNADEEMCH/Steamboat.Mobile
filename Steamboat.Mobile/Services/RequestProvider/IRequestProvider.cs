using System;
using System.Threading.Tasks;

namespace Steamboat.Mobile.Services.RequestProvider
{
    public interface IRequestProvider
    {
        Task<TResult> GetAsync<TResult>(string uri);

        Task<TResult> PostAsync<TResult,TData>(string uri, TData data, string sessionID = "");

        Task<TResult> PostAsync<TResult>(string uri, TResult data, string sessionID = "");

        Task<TResult> PostAsync<TResult>(string uri, string sessionID = "");

        Task<TResult> PutAsync<TResult>(string uri, TResult data, string sessionID = "");

        Task DeleteAsync(string uri, string sessionID = "");
    }
}
