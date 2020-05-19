using System;
using System.Collections.Generic;
using ViewModels;

namespace Infrastructure.Business
{
    public interface IAuthorDM: IDisposable
    {
        Author GetAuthor(long id);

        IEnumerable<Author> GetAuthors();

        IEnumerable<ListItem> GetAuthorsShort();

        void CreateAuthor(Author author);

        void UpdateAuthor(Author author);

        void DeleteAuthor(long id);
    }
}
