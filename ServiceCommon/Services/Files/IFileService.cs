using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CRM.ServiceCommon.Services.Files
{
    public interface IFileService
    {
        Task PutFileAsync(Stream stream, string folder, string fileName);
        
        Task PutFileAsync(Stream stream, string path);

        Task<MemoryStream> GetFileAsync(string path);

        Task<Stream> GetFileStreamTask(string path);
    }
}