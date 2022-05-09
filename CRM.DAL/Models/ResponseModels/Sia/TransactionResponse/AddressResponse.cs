using Newtonsoft.Json;

namespace CRM.DAL.Models.ResponseModels.Sia.TransactionResponse
{
    public class AddressResponse
    {
        //{"address":"dbd2d0654a243b415007e7b4a23c2dd6811cfa5c1b6369fee4be64996a49ee08cb446471d4c3"}
        [JsonProperty("address")]
        private string Address { get; set; }
    }
}