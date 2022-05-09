using System;
using System.Buffers.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CRM.DAL.Models.ResponseModels.Sia;
using CRM.DAL.Models.ResponseModels.Sia.Exceptions;
using CRM.DAL.Models.ResponseModels.Sia.TransactionResponse;
using Newtonsoft.Json;

namespace CRM.ServiceCommon.Clients
{
    public class SiaApiClient
    {
        private readonly HttpClient client;
        private readonly string siadAddress;
        private readonly string apiPassword;
        private readonly string encPassword;
        public SiaApiClient(
            HttpClient client, 
            string siadAddress, 
            string apiPassword, 
            string encPassword)
        {
            this.client = client;
            this.siadAddress = siadAddress;
            this.apiPassword = apiPassword;
            this.encPassword = encPassword;
            client.DefaultRequestHeaders.Add("User-Agent", "Sia-Agent");
            client.DefaultRequestHeaders.Authorization = AuthHeader;
        }

        public async Task<AddressResponse> GetAddressAsync()
        {
            var response = await client.GetAsync($"http://{siadAddress}/wallet/address");
            
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<AddressResponse>(await response.Content.ReadAsStringAsync());
            }
            throw new SiaApiException(response);
        }

        public async Task<ConsensusResponse> GetConsensusAsync()
        {
            var response = await client.GetAsync($"http://{siadAddress}/consensus");
            
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<ConsensusResponse>(await response.Content.ReadAsStringAsync());
            }
            throw new SiaApiException(response);
        }

        public async Task<TransactionsResponse> GetTransactionsAsync(string address)
        {
            var response = await client.GetAsync($"http://{siadAddress}/wallet/transactions/{address}");
            
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<TransactionsResponse>(await response.Content.ReadAsStringAsync());
            }
            throw new SiaApiException(response);
        }
        
        public async Task<TransactionsResponse> GetTransactionsAsync(long startHeight, long endHeight)
        {
            var response = await client.GetAsync($"http://{siadAddress}/wallet/transactions?startheight={startHeight}&endheight={endHeight}");
            
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<TransactionsResponse>(await response.Content.ReadAsStringAsync());
            }
            throw new SiaApiException(response);
        }
        
        public async Task<WalletResponse> GetWalletAsync()
        {
            var response = await client.GetAsync($"http://{siadAddress}/wallet");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<WalletResponse>(await response.Content.ReadAsStringAsync());
            }

            throw new SiaApiException(response);
        }
        
        private AuthenticationHeaderValue AuthHeader
        {
            get
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(apiPassword);
                return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(plainTextBytes));
            }
        }
        
        
    }
}