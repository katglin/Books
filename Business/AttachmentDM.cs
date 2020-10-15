using Infrastructure.Business;
using Infrastructure.Data;
using ViewModels;
using System.Threading.Tasks;
using System.IO;

namespace Business
{
    public class AttachmentDM: BaseDM, IAttachmentDM
    {
        public AttachmentDM(Infrastructure.IServiceProvider service) : base(service)
        {
        }

        public async Task<Attachment> UploadAttachmentAsync(int bookId, string fileName, Stream file)
        {
            using (var attachmentRepo = ServiceProvider.GetService<IAttachmentRepository>())
            using (var s3Service = ServiceProvider.GetService<IAwsS3Service>())
            {
                string bucketName = this.BookAttachmentsBucket;
                var attachment = new Attachment();
                attachment.FileS3Key = await s3Service.UploadFileAsync(fileName, bucketName, file);
                attachmentRepo.AddAttachment(bookId, fileName, attachment.FileS3Key);
                attachment.FileUrl = s3Service.GeneratePreSignedURL(attachment.FileS3Key, bucketName);
                return attachment;
            }
        }

        public async Task DeleteAttachmentAsync(string fileKey)
        {
            using (var attachmentRepo = ServiceProvider.GetService<IAttachmentRepository>())
            using (var s3Service = ServiceProvider.GetService<IAwsS3Service>())
            {
                string bucketName = this.BookAttachmentsBucket;
                attachmentRepo.DeleteAttachment(fileKey);
                await s3Service.DeleteFileAsync(fileKey, bucketName);
            }
        }
    }
}
