using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CRM.DAL.Models.ResponseModels.Sia;
using Newtonsoft.Json;

namespace CRM.ServiceCommon.Clients
{
    public class SiaApiClient
    {
        private readonly HttpClient client;
        private readonly string siadAddress;
        private readonly string apiPassword;
        private readonly string encPassword;
        public SiaApiClient(HttpClient client, string siadAddress, string apiPassword, string encPassword)
        {
            this.client = client;
            this.siadAddress = siadAddress;
            this.apiPassword = apiPassword;
            this.encPassword = encPassword;
            client.DefaultRequestHeaders.Add("User-Agent", "Sia-Agent");
        }

        public async Task<WalletResponse> GetWalletAsync()
        {
            var response = await client.GetAsync($"http://{siadAddress}/wallet");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<WalletResponse>(await response.Content.ReadAsStringAsync());
            }

            throw new Exception(response.StatusCode.ToString());
        }
    }
}