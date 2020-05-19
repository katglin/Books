using Dapper;
using DTO;
using Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;

namespace Data.Repositories
{
    public class AuthorRepository: RepositoryDapper<AuthorDTO, long>, IAuthorRepository
    {
        public AuthorDTO GetAuthor(long id)
        {
            using (var connection = ConnectionProvider())
            {
                string sql = $@"SELECT a.Id, FirstName, LastName, COUNT(ba.Id) AS BookNumber
                               FROM Author a 
                               LEFT JOIN BookAuthor ba ON a.Id = ba.AuthorId
                               WHERE a.Id = {id}
                               GROUP BY a.Id, FirstName, LastName";

                var author = connection.Query<AuthorDTO>(sql).FirstOrDefault();
                return author;
            }
        }

        public IEnumerable<AuthorDTO> GetAuthors()
        {
            using (var connection = ConnectionProvider())
            {
                string sql = @"SELECT a.Id, FirstName, LastName, COUNT(ba.Id) AS BookNumber
                               FROM Author a 
                               LEFT JOIN BookAuthor ba ON a.Id = ba.AuthorId
                               GROUP BY a.Id, FirstName, LastName";

                var authors = connection.Query<AuthorDTO>(sql).ToList();
                return authors;
            }
        }

        public IEnumerable<ListItemDTO> GetAuthorsShort()
        {
            using (var connection = ConnectionProvider())
            {
                string sql = @"SELECT Id, CONCAT(FirstName, ' ', LastName) AS Text
                               FROM Author";

                var authors = connection.Query<ListItemDTO>(sql).ToList();
                return authors;
            }
        }

        public void CreateAuthor(AuthorDTO author)
        {
            using (var connection = ConnectionProvider())
            {
                string sql = $@"INSERT INTO Author (FirstName, LastName)
                               VALUES ('{author.FirstName}', '{author.LastName}')";
                connection.Query(sql);
            }
        }

        public void UpdateAuthor(AuthorDTO author)
        {
            using (var connection = ConnectionProvider())
            {
                string sql = $@"UPDATE Author
                                SET FirstName = '{author.FirstName}',
                                    LastName = '{author.LastName}'
                                WHERE Id = {author.Id}";
                connection.Query(sql);
            }
        }

        public void DeleteAuthor(long id)
        {
            using (var connection = ConnectionProvider())
            {
                string sql = $@"DELETE Author 
                                WHERE Id = {id} AND NOT EXISTS (SELECT 1 FROM BookAuthor WHERE AuthorId = {id})";
                connection.Query(sql);
            }
        }
    }
}
