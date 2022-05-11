using Newtonsoft.Json;

namespace CRM.DAL.Models.ResponseModels.Sia.TransactionResponse
{
    public partial class Unlockconditions
    {
        [JsonProperty("timelock")]
        public string Timelock { get; set; }

        [JsonProperty("publickeys")]
        public Publickey[] Publickeys { get; set; }

        [JsonProperty("signaturesrequired")]
        public string Signaturesrequired { get; set; }
    }
}