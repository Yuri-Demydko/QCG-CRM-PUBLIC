using System.IO;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CRM.DAL.Models.ResponseModels.Sia.Downloads
{
    public class SiaDownloadHttpResponse:SiaHttpResponse
    {
        public ConfiguredTaskAwaitable<Stream> ResponseStream { get; set; }
        
    }
}