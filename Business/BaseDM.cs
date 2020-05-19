
using Infrastructure;
using System;

namespace Business
{
    public class BaseDM: IDisposable
    {
        protected Infrastructure.IServiceProvider ServiceProvider;

        public BaseDM(Infrastructure.IServiceProvider ServiceProvider)
        {
            this.ServiceProvider = ServiceProvider;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
