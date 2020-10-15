using Dapper;
using Data.Extensions;
using DTO;
using Infrastructure.Data;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Data.Repositories
{
    public class AttachmentRepository: RepositoryDapper<BookDTO, long>, IAttachmentRepository
    {
        public void AddAttachment(long bookId, string fileName, string fileS3Key)
        {
            using (var connection = ConnectionProvider())
            {
                string sql = $@"INSERT INTO BookAttachment (BookId, FileS3Key, FileName) VALUES ({bookId}, '{fileS3Key}', '{fileName}')";
                connection.Query(sql);
            }
        }

        public void DeleteAttachment(string fileS3Key)
        {
            using (var connection = ConnectionProvider())
            {
                string sql = $@"DELETE BookAttachment WHERE FileS3Key='{fileS3Key}'";
                connection.Query(sql);
            }
        }
    }
}
