using DTO;

namespace Infrastructure.Data
{
    public interface IAttachmentRepository: IRepository<BookDTO, long>
    {
        void AddAttachment(long bookId, string fileName, string fileS3Key);

        void DeleteAttachment(string fileS3Key);
    }
}
