
using Infrastructure;
using System;
using System.Configuration;

namespace Business
{
    public class BaseDM: IDisposable
    {
        protected Infrastructure.IServiceProvider ServiceProvider;
        protected string BookTitlesBucket { get; set; }
        protected string BookAttachmentsBucket { get; set; }
        protected string StaticImagesBucket { get; set; }

        public BaseDM(Infrastructure.IServiceProvider ServiceProvider)
        {
            this.ServiceProvider = ServiceProvider;
            this.BookTitlesBucket = ConfigurationManager.AppSettings["BookTitlesBucket"];
            this.BookAttachmentsBucket = ConfigurationManager.AppSettings["BookAttachmentsBucket"];
            this.StaticImagesBucket = ConfigurationManager.AppSettings["StaticFilesBucket"];
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
