using Client.Data.Listeners;
using Client.Services.Api;
using System.Net.Http.Json;

namespace Client.Services.Listeners
{
    public class ListenerTypeService
	{

        public ListenerTypeService()
        {
        }

		public async Task<List<ListenerType>> GetListenerTypes(string apiHost)
		{
            string apiUrl = $"http://{apiHost}{ApiList.GET_LISTENER_TYPES}";

            using (HttpClient httpClient = new HttpClient())
			{
                return await httpClient.GetFromJsonAsync<List<ListenerType>>(apiUrl);
            }
		}

		public async Task<ListenerType> GetListenerTypeById(string id,string apiHost)
        {
            string apiUrl = $"http://{apiHost}{ApiList.GET_LISTENE_TYPE_BY_ID}{id}";

            using (HttpClient httpClient = new HttpClient())
            {
                return await httpClient.GetFromJsonAsync<ListenerType>(apiUrl);
            }
        }
		public async Task DeleteListenerType(string id, string apiHost)
		{
			string apiUrl = $"http://{apiHost}{ApiList.DELETE_LISTENER_TYPE_BY_ID}{id}";
			using (HttpClient httpClient = new HttpClient())
			{
				await httpClient.DeleteAsync(apiUrl);
			}
		}
	}
	
}
