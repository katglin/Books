using System;
using System.Collections.Generic;

namespace DTO
{
    public class BookDTO: BaseDTO
    {
        public BookDTO()
        {
            Authors = new List<AuthorDTO>();
        }

        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<AuthorDTO> Authors { get; set; }
        public IEnumerable<long> AuthorIds { get; set; }
        public int Rate { get; set; }
        public int PageNumber { get; set; }
    }
}
