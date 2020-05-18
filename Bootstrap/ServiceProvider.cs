using Data;
using Infrastructure.Data;
using System.Web.Http;
using Unity;

namespace Bootstrap
{
    public static class ServiceProvider
    {
        private static IUnityContainer _unityContainer;

        static ServiceProvider()
        {
            _unityContainer = new UnityContainer();
            RegisterTypes();
        }

        public static TEntity GetService<TEntity>()
        {
            return (TEntity)_unityContainer.Resolve(typeof(TEntity));
        }

        private static void RegisterTypes()
        {
            RegisterDataModels();
            RegisterBusinessModels();
        }

        private static void RegisterBusinessModels()
        {
            //_unityContainer.RegisterType<IBookDM, BookDM>();
            //_unityContainer.RegisterType<IAuthorDM, AuthorDM>();
        }

        private static void RegisterDataModels()
        {
            //_unityContainer.RegisterType<IBookRepository, BookRepository>();
            //_unityContainer.RegisterType<IAuthorRepository, AuthorRepository>();
        }
    }
}
