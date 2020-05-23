using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ViewModels
{
    public class Author: Entity
    {
        [Required]
        [DisplayName("First name")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Last name")]
        public string LastName { get; set; }

        [DisplayName("Books")]
        public int BookNumber { get; set; }
    }
}
