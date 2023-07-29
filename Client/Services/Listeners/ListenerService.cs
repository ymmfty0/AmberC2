using Client.Data.Listeners;
using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;
using Client.Services.Api;

namespace Client.Services.Listeners
{
    public class ListenerService
    {
        public async Task<List<Listener>> GetListeners(string apiHost)
        {
			string apiUrl = $"http://{apiHost}{ApiList.GET_LISTENERS}";
			using (HttpClient httpClient = new HttpClient())
            {
				return await httpClient.GetFromJsonAsync<List<Listener>>(apiUrl);
			}
		} 
		
		public async Task CreateListener(string apiHost, Listener listener)
        {
            string apiUrl = $"http://{apiHost}{ApiList.CREATE_LISTENER}"; 
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, new StringContent(JsonConvert.SerializeObject(listener), Encoding.UTF8, "application/json"));
                if (!response.IsSuccessStatusCode) { }
               
            }
        }

        public async Task DeleteListener(string id,string apiHost)
        {
            string apiUrl = $"http://{apiHost}{ApiList.DELETE_LISTENER_BY_ID}{id}";
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.DeleteAsync(apiUrl);
                if (!response.IsSuccessStatusCode) { }
            }
        }

    }
}
