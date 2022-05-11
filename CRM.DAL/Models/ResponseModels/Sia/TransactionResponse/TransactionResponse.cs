using Newtonsoft.Json;

namespace CRM.DAL.Models.ResponseModels.Sia.TransactionResponse
{
    public class TransactionResponse
    {
        [JsonProperty("transaction")]
        public Transaction Transaction { get; set; }

        [JsonProperty("transactionid")]
        public string Transactionid { get; set; }

        [JsonProperty("confirmationheight")]
        public long Confirmationheight { get; set; }

        [JsonProperty("confirmationtimestamp")]
        public string Confirmationtimestamp { get; set; }

        [JsonProperty("inputs")]
        public Input[] Inputs { get; set; }

        [JsonProperty("outputs")]
        public Output[] Outputs { get; set; }
    }
}