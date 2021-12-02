using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookShop.Data.Models
{
    public class Author
    {
        public Author()
        {
            AuthorsBooks = new HashSet<AuthorBook>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        public virtual ICollection<AuthorBook> AuthorsBooks { get; set; }
    }
}
