using Newtonsoft.Json;

namespace CRM.DAL.Models.ResponseModels.Sia.Files
{
    public class FilesResponse
    {
        [JsonProperty("files")]
        public FileResponse[] Files { get; set; }
    }
}