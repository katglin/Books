using System;
using System.IO;
using System.Threading.Tasks;
using ViewModels;

namespace Infrastructure.Business
{
    public interface IAttachmentDM: IDisposable
    {
        Task<Attachment> UploadAttachmentAsync(int bookId, string fileName, Stream file);

        Task DeleteAttachmentAsync(string fileKey);
    }
}
