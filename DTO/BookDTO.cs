using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace DTO
{
    public class BookDTO: BaseDTO
    {
        public BookDTO()
        {
            Authors = new List<AuthorDTO>();
            Attachments = new List<AttachmentDTO>();
            AuthorIds = new List<long>();
        }

        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<AuthorDTO> Authors { get; set; }
        public IEnumerable<long> AuthorIds { get; set; }
        public int Rate { get; set; }
        public int PageNumber { get; set; }

        public string ImageS3Key { get; set; }

        public List<AttachmentDTO> Attachments { get; set; }
    }
}
