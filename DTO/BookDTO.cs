using System;
using System.Collections.Generic;

namespace DTO
{
    public class BookDTO: BaseDTO
    {
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public IEnumerable<AuthorDTO> Authors { get; set; }
        public int Rate { get; set; }
        public int PageNumber { get; set; }
    }
}
