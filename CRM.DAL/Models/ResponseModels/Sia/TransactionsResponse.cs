using Newtonsoft.Json;

namespace CRM.DAL.Models.ResponseModels.Sia
{
    public class TransactionsResponse
    {
        [JsonProperty("confirmedtransactions")]
        public TransactionResponse.TransactionResponse[] ConfirmedTransactions { get; set; }
        
        [JsonProperty("unconfirmedtransactions")]
        public TransactionResponse.TransactionResponse[] UnconfirmedTransactions { get; set; }
    }
}