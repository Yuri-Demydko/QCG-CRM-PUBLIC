using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CRM.ServiceCommon.Services.DbEventHandlerService
{
    public enum DbOperation
    {
        [JsonProperty("INSERT")] Insert,
        [JsonProperty("UPDATE")] Update,
        [JsonProperty("DELETE")] Delete
    }

    public class DbEvent
    {
        public DateTime CreatedAt { get; set; }

        public DbOperation Operation { get; set; }

        public string TableName { get; set; }

        public string Discriminator { get; set; }

        public string EntryId { get; set; }

        public JRaw Old { get; set; }

        public T GetOld<T>() where T : class
        {
            return JsonConvert.DeserializeObject<T>(Old.Value<string>());
        }

        public JRaw New { get; set; }

        public T GetNew<T>() where T : class
        {
            return JsonConvert.DeserializeObject<T>(New.Value<string>());
        }
    }
}