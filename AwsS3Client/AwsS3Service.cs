using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Business;
using Infrastructure.Business;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AwsS3Client
{
    public class AwsS3Service: BaseDM, IAwsS3Service
    {
        //private const string bucketName = "book-title-images";
        private const string accessKeyId = "AKIAZUSFJVVJLP6YPIS4";
        private const string secretAccessKey = "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";
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast2;
        private static readonly double minutesToLive = 5; 

        private static IAmazonS3 s3Client;

        public AwsS3Service(Infrastructure.IServiceProvider service) : base(service)
        {
            s3Client = new AmazonS3Client(accessKeyId, secretAccessKey, bucketRegion);
        }

        public string GetStaticImage(string fileKey)
        {
            string bucketName = "static-bookstore-images";
            return this.GeneratePreSignedURL(fileKey, bucketName);
        }

        public async Task<string> UploadFileAsync(string fileName, string bucketName, Stream inputStream, string contentType = null)
        {
            var fileKey = GetFileKey(fileName);
            var fileRequest = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = fileKey,
                InputStream = inputStream
                //ContentType = contentType,
            };
            await s3Client.PutObjectAsync(fileRequest);
            return fileKey;
                //GeneratePreSignedURL(fileKey, bucketName);
        }

        public string GeneratePreSignedURL(string fileKey, string bucketName)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = fileKey,
                Protocol = Protocol.HTTPS,
                Expires = DateTime.UtcNow.AddMinutes(minutesToLive)
            };

            string url = s3Client.GetPreSignedURL(request);
            return url;
        }

        public async Task DeleteFileAsync(string fileKey, string bucketName)
        {
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = fileKey
            };

            await s3Client.DeleteObjectAsync(deleteObjectRequest);
        }

        private string GetFileKey(string fileName)
        {
            return Guid.NewGuid() + Path.GetExtension(fileName); ;
               // DateTime.UtcNow + fileName;
        }

        public void Dispose()
        {
        }
    }
}
