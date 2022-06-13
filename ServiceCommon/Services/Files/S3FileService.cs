using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace CRM.ServiceCommon.Services.Files
{
    public class S3FileService : IFileService
    {
        private readonly string bucket;
        
        private readonly IAmazonS3 s3Client;

        public S3FileService(IAmazonS3 s3Client, string bucket)
        {
            this.s3Client = s3Client;
            this.bucket = bucket;
        }

        public async Task PutFileAsync(Stream stream, string folder, string fileName)
        {
            await PutFileAsync(stream, folder + "/" + fileName);
        }

        // public async Task<MemoryStream> PartialDownload(string path)
        // {
        //     var result = await s3Client.Get
        // }

        public async Task PutFileAsync(Stream stream, string path)
        {
            var fileTransferUtility =
                new TransferUtility(s3Client);

            await fileTransferUtility.UploadAsync(stream,
                bucket, path);
        }
        
        

        public async Task<MemoryStream> GetFileAsync(string path)
        {
            var result = await s3Client.GetObjectAsync(bucket, path);

            var stream = new MemoryStream();

            await result.ResponseStream.CopyToAsync(stream);

            return stream;
        }
        

        public Task<Stream> GetFileStreamTask(string path)
        {
            return s3Client.GetObjectStreamAsync(bucket, path,null);
        }
    }
}