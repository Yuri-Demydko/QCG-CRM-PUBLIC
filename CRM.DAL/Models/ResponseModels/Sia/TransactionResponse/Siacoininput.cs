using Newtonsoft.Json;

namespace CRM.DAL.Models.ResponseModels.Sia.TransactionResponse
{
    public partial class Siacoininput
    {
        [JsonProperty("parentid")]
        public string Parentid { get; set; }

        [JsonProperty("unlockconditions")]
        public Unlockconditions Unlockconditions { get; set; }
    }
}