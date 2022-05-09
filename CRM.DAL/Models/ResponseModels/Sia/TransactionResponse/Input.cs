using Newtonsoft.Json;

namespace CRM.DAL.Models.ResponseModels.Sia.TransactionResponse
{
    public partial class Input
    {
        [JsonProperty("parentid")]
        public string Parentid { get; set; }

        [JsonProperty("fundtype")]
        public string Fundtype { get; set; }

        [JsonProperty("walletaddress")]
        public bool Walletaddress { get; set; }

        [JsonProperty("relatedaddress")]
        public string Relatedaddress { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}