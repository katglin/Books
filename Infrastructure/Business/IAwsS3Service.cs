using System;
using System.IO;
using System.Threading.Tasks;

namespace Infrastructure.Business
{
    public interface IAwsS3Service: IDisposable
    {
        string GetStaticImage(string fileKey);

        Task<string> UploadFileAsync(string fileName, string bucketName, Stream inputStream, string contentType = null);

        string GeneratePreSignedURL(string fileKey, string bucketName);

        Task DeleteFileAsync(string fileKey, string bucketName);

    }
}
