using Newtonsoft.Json;

namespace CRM.DAL.Models.ResponseModels.Sia.TransactionResponse
{
    public partial class Unlockconditions
    {
        [JsonProperty("timelock")]
        public long Timelock { get; set; }

        [JsonProperty("publickeys")]
        public Publickey[] Publickeys { get; set; }

        [JsonProperty("signaturesrequired")]
        public long Signaturesrequired { get; set; }
    }
}