using Newtonsoft.Json;

namespace CRM.DAL.Models.ResponseModels.Sia.TransactionResponse
{
    public partial class Transaction
    {
        [JsonProperty("siacoininputs")]
        public Siacoininput[] Siacoininputs { get; set; }

        [JsonProperty("siacoinoutputs")]
        public Siacoinoutput[] Siacoinoutputs { get; set; }

        [JsonProperty("filecontracts")]
        public object[] Filecontracts { get; set; }

        [JsonProperty("filecontractrevisions")]
        public object[] Filecontractrevisions { get; set; }

        [JsonProperty("storageproofs")]
        public object[] Storageproofs { get; set; }

        [JsonProperty("siafundinputs")]
        public object[] Siafundinputs { get; set; }

        [JsonProperty("siafundoutputs")]
        public object[] Siafundoutputs { get; set; }

        [JsonProperty("minerfees")]
        public string[] Minerfees { get; set; }

        [JsonProperty("arbitrarydata")]
        public object[] Arbitrarydata { get; set; }

        [JsonProperty("transactionsignatures")]
        public Transactionsignature[] Transactionsignatures { get; set; }

        [JsonProperty("wholetransaction", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Wholetransaction { get; set; }
    }
}