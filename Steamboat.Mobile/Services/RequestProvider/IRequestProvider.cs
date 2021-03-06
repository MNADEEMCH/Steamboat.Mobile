using System;
using System.Threading.Tasks;

namespace Steamboat.Mobile.Services.RequestProvider
{
	public interface IRequestProvider
	{
		Task<TResult> GetAsync<TResult>(string uri, string sessionID = "");
		Task<TResult> PostAsync<TResult, TData>(string uri, TData data, string sessionID = "");
		Task<TResult> PostAsync<TResult>(string uri, TResult data, string sessionID = "");
		Task<TResult> PostAsync<TResult>(string uri, string sessionID = "");
		Task PostAsync(string uri, string sessionID = "");
        Task<TResult> PostImageAsync<TResult>(string uri, byte[] mediaFile, string comment, string opinion, string sessionID = "");
        Task<TResult> PutAsync<TResult>(string uri, TResult data, string sessionID = "");
		Task DeleteAsync(string uri, string sessionID = "");
	}
}
