using Newtonsoft.Json;

namespace CRM.DAL.Models.ResponseModels.Sia.TransactionResponse
{
    public class TransactionsResponse
    {
        [JsonProperty("confirmedtransactions")]
        public TransactionResponse[] ConfirmedTransactions { get; set; }
        
        [JsonProperty("unconfirmedtransactions")]
        public TransactionResponse[] UnconfirmedTransactions { get; set; }
    }
}