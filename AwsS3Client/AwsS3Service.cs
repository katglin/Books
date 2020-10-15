using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Business;
using Infrastructure.Business;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace AwsS3Client
{
    public class AwsS3Service: BaseDM, IAwsS3Service
    {
        private readonly string _accessKeyId = ConfigurationManager.AppSettings["AWSAccessKey"];
        private readonly string _secretAccessKey = ConfigurationManager.AppSettings["AWSSecretKey"];
        private static readonly RegionEndpoint _bucketRegion = RegionEndpoint.USEast2;
        private static readonly double _minutesToLive = int.Parse(ConfigurationManager.AppSettings["TemporaryS3LinkExpirationMinutes"]);
        private static IAmazonS3 _s3Client;

        public AwsS3Service(Infrastructure.IServiceProvider service) : base(service)
        {
            _s3Client = new AmazonS3Client(_accessKeyId, _secretAccessKey, _bucketRegion);
        }

        public string GetStaticImage(string fileKey)
        {
            string bucketName = this.StaticImagesBucket;
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
            };
            await _s3Client.PutObjectAsync(fileRequest);
            return fileKey;
        }

        public string GeneratePreSignedURL(string fileKey, string bucketName)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = fileKey,
                Protocol = Protocol.HTTPS,
                Expires = DateTime.UtcNow.AddMinutes(_minutesToLive)
            };
            string url = _s3Client.GetPreSignedURL(request);
            return url;
        }

        public async Task DeleteFileAsync(string fileKey, string bucketName)
        {
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = fileKey
            };
            await _s3Client.DeleteObjectAsync(deleteObjectRequest);
        }

        private string GetFileKey(string fileName)
        {
            return Guid.NewGuid() + Path.GetExtension(fileName);
        }
    }
}
