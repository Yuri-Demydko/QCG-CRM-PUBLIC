using Newtonsoft.Json;

namespace CRM.DAL.Models.ResponseModels.Sia.TransactionResponse
{
    public partial class Transactionsignature
    {
        [JsonProperty("parentid")]
        public string Parentid { get; set; }

        [JsonProperty("publickeyindex")]
        public long Publickeyindex { get; set; }

        [JsonProperty("timelock")]
        public long Timelock { get; set; }

        [JsonProperty("coveredfields")]
        public Transaction Coveredfields { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }
    }
}