using DTO;
using System.Collections.Generic;

namespace Infrastructure.Data
{
    public interface IAuthorRepository: IRepository<AuthorDTO, long>
    {
        AuthorDTO GetAuthor(long id);

        IEnumerable<AuthorDTO> GetAuthors();

        void CreateAuthor(AuthorDTO author);

        void UpdateAuthor(AuthorDTO author);

        void DeleteAuthor(long id);
    }
}
