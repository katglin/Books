using Infrastructure.Business;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using ViewModels;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

namespace Web.Controllers
{
    public class AttachmentController : BaseController
    {
        [HttpPost]
        public async Task<JsonResult> UploadAttachmentAsync()
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                var fileName = Path.GetFileName(file.FileName);
                Stream fileStream = file.InputStream;
                var bookId = Int32.Parse(Request.Params["BookId"]);
                using (var attachmentDM = ServiceProvider.GetService<IAttachmentDM>())
                {
                    var attachment = await attachmentDM.UploadAttachmentAsync(bookId, fileName, fileStream);
                    return Json(attachment);
                }
            }
            return Json(null);
        }

        [HttpPost]
        public async Task DeleteAttachmentAsync(string fileKey)
        {
            using (var attachmentDM = ServiceProvider.GetService<IAttachmentDM>())
            {
                await attachmentDM.DeleteAttachmentAsync(fileKey);
            }
        }
    }
}
