using System;
using Newtonsoft.Json;

namespace CRM.DAL.Models.ResponseModels.Sia.Files
{
    public class FileResponse
    {
        [JsonProperty("accesstime")]
        public DateTimeOffset Accesstime { get; set; }

        [JsonProperty("available")]
        public bool Available { get; set; }

        [JsonProperty("changetime")]
        public DateTimeOffset Changetime { get; set; }

        [JsonProperty("ciphertype")]
        public string Ciphertype { get; set; }

        [JsonProperty("createtime")]
        public DateTimeOffset Createtime { get; set; }

        [JsonProperty("expiration")]
        public long Expiration { get; set; }

        [JsonProperty("filesize")]
        public long Filesize { get; set; }

        [JsonProperty("health")]
        public long Health { get; set; }

        [JsonProperty("localpath")]
        public string Localpath { get; set; }

        [JsonProperty("maxhealth")]
        public string Maxhealth { get; set; }

        [JsonProperty("maxhealthpercent")]
        public long Maxhealthpercent { get; set; }

        [JsonProperty("modtime")]
        public DateTimeOffset Modtime { get; set; }

        [JsonProperty("mode")]
        public long Mode { get; set; }

        [JsonProperty("numstuckchunks")]
        public long Numstuckchunks { get; set; }

        [JsonProperty("ondisk")]
        public bool Ondisk { get; set; }

        [JsonProperty("recoverable")]
        public bool Recoverable { get; set; }

        [JsonProperty("redundancy")]
        public long Redundancy { get; set; }

        [JsonProperty("renewing")]
        public bool Renewing { get; set; }

        [JsonProperty("repairbytes")]
        public long Repairbytes { get; set; }

        [JsonProperty("skylinks")]
        public object Skylinks { get; set; }

        [JsonProperty("siapath")]
        public string Siapath { get; set; }

        [JsonProperty("stuck")]
        public bool Stuck { get; set; }

        [JsonProperty("stuckbytes")]
        public long Stuckbytes { get; set; }

        [JsonProperty("stuckhealth")]
        public double Stuckhealth { get; set; }

        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("uploadedbytes")]
        public long Uploadedbytes { get; set; }

        [JsonProperty("uploadprogress")]
        public long Uploadprogress { get; set; }
    }
}