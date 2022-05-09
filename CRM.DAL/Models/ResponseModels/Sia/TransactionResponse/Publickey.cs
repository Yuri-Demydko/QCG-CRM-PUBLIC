using Newtonsoft.Json;

namespace CRM.DAL.Models.ResponseModels.Sia.TransactionResponse
{
    public partial class Publickey
    {
        [JsonProperty("algorithm")]
        public string Algorithm { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }
    }
}