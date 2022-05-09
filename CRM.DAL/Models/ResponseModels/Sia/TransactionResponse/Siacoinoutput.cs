using Newtonsoft.Json;

namespace CRM.DAL.Models.ResponseModels.Sia.TransactionResponse
{
    public partial class Siacoinoutput
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("unlockhash")]
        public string Unlockhash { get; set; }
    }
}