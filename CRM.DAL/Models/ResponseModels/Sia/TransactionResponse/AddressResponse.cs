using Newtonsoft.Json;

namespace CRM.DAL.Models.ResponseModels.Sia.TransactionResponse
{
    public class AddressResponse
    {
        [JsonProperty("address")]
        public string Address { get; set; }
    }
}