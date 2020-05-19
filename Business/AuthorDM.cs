using Infrastructure.Business;
using Infrastructure.Data;
using System.Collections.Generic;
using ViewModels;
using AutoMapper;
using DTO;

namespace Business
{
    public class AuthorDM: BaseDM, IAuthorDM
    {
        public AuthorDM(Infrastructure.IServiceProvider service) : base(service)
        {

        }

        public Author GetAuthor(long id)
        {
            using (var authRepo = ServiceProvider.GetService<IAuthorRepository>())
            {
                var author = authRepo.GetAuthor(id);
                return Mapper.Map<Author>(author);
            }
        }

        public IEnumerable<Author> GetAuthors()
        {
            using (var authRepo = ServiceProvider.GetService<IAuthorRepository>())
            {
                var authors = authRepo.GetAuthors();
                return Mapper.Map<IEnumerable<Author>>(authors);
            }
        }

        public void CreateAuthor(Author author)
        {
            using (var authRepo = ServiceProvider.GetService<IAuthorRepository>())
            {
                var entity = Mapper.Map<AuthorDTO>(author);
                authRepo.CreateAuthor(entity);
            }
        }

        public void UpdateAuthor(Author author)
        {
            using (var authRepo = ServiceProvider.GetService<IAuthorRepository>())
            {
                var entity = Mapper.Map<AuthorDTO>(author);
                authRepo.UpdateAuthor(entity);
            }
        }

        public void DeleteAuthor(long id)
        {
            using (var authRepo = ServiceProvider.GetService<IAuthorRepository>())
            {
                authRepo.DeleteAuthor(id);
            }
        }
    }
}
