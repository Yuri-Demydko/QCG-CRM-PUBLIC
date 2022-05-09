using Newtonsoft.Json;

namespace CRM.DAL.Models.ResponseModels.Sia
{
    public class ConsensusResponse
    {
        [JsonProperty("synced")]
        public bool Synced { get; set; }

        [JsonProperty("height")]
        public string Height { get; set; }

        [JsonProperty("currentblock")]
        public string Currentblock { get; set; }

        [JsonProperty("target")]
        public string[] Target { get; set; }

        [JsonProperty("difficulty")]
        public string Difficulty { get; set; }

        [JsonProperty("foundationprimaryunlockhash")]
        public string Foundationprimaryunlockhash { get; set; }

        [JsonProperty("foundationfailsafeunlockhash")]
        public string Foundationfailsafeunlockhash { get; set; }

        [JsonProperty("blockfrequency")]
        public string Blockfrequency { get; set; }

        [JsonProperty("blocksizelimit")]
        public string Blocksizelimit { get; set; }

        [JsonProperty("extremefuturethreshold")]
        public string Extremefuturethreshold { get; set; }

        [JsonProperty("futurethreshold")]
        public string Futurethreshold { get; set; }

        [JsonProperty("genesistimestamp")]
        public string Genesistimestamp { get; set; }

        [JsonProperty("maturitydelay")]
        public string Maturitydelay { get; set; }

        [JsonProperty("mediantimestampwindow")]
        public string Mediantimestampwindow { get; set; }

        [JsonProperty("siafundcount")]
        public string Siafundcount { get; set; }

        [JsonProperty("siafundportion")]
        public string Siafundportion { get; set; }

        [JsonProperty("initialcoinbase")]
        public string Initialcoinbase { get; set; }

        [JsonProperty("minimumcoinbase")]
        public string Minimumcoinbase { get; set; }

        [JsonProperty("roottarget")]
        public string[] Roottarget { get; set; }

        [JsonProperty("rootdepth")]
        public string[] Rootdepth { get; set; }

        [JsonProperty("siacoinprecision")]
        public string Siacoinprecision { get; set; }
    }
}