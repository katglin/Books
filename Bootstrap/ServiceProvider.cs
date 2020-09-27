using Business;
using Data.Repositories;
using Infrastructure;
using Infrastructure.Business;
using Infrastructure.Data;
using System;
using Unity;
using Unity.Injection;

namespace Bootstrap
{
    public class ServiceProvider: Infrastructure.IServiceProvider
    {
        private IUnityContainer _unityContainer;

        public ServiceProvider()
        {
            _unityContainer = new UnityContainer();
            RegisterTypes();
        }

        public TEntity GetService<TEntity>()
        {
            return (TEntity)_unityContainer.Resolve(typeof(TEntity));
        }

        private void RegisterTypes()
        {
            RegisterDataModels();
            RegisterBusinessModels();
            RegisterServices();
        }

        private void RegisterServices()
        {
            _unityContainer.RegisterType<IAwsS3Service, AwsS3Client.AwsS3Service>(new InjectionConstructor(this));
        }

        private void RegisterBusinessModels()
        {
            _unityContainer.RegisterType<IBookDM, BookDM>(new InjectionConstructor(this));
            _unityContainer.RegisterType<IAuthorDM, AuthorDM>(new InjectionConstructor(this));
        }

        private void RegisterDataModels()
        {
            _unityContainer.RegisterType<IBookRepository, BookRepository>();
            _unityContainer.RegisterType<IAuthorRepository, AuthorRepository>();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
