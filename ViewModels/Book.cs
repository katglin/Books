using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ViewModels
{
    public class Book: Entity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [DisplayName("Released")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime ReleaseDate { get; set; }

        public IEnumerable<Author> Authors { get; set; }

        [Required]
        public IEnumerable<long> AuthorIds { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "The value must be between 1 and 10")]
        public int Rate { get; set; }

        [DisplayName("Pages")]
        [Required]
        [Range(1, 10000, ErrorMessage = "The value must be between 1 and 10000")]
        public int PageNumber { get; set; }

        public string ImageS3Key { get; set; }

        public string ImageUrl { get; set; }

        public IEnumerable<Attachment> Attachments { get; set; } = new List<Attachment>();
    }
}
