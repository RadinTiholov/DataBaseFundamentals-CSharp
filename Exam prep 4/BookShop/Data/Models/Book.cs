using BookShop.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookShop.Data.Models
{
    public class Book
    {
        public Book()
        {
            AuthorsBooks = new HashSet<AuthorBook>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [Range(1,3)]
        public Genre Genre { get; set; }

        [Range(typeof(Decimal), "0.01", "79228162514264337593543950335")]
        public decimal Price { get; set; }

        [Range(50, 5000)]
        public int Pages { get; set; }

        [Required]
        public DateTime PublishedOn { get; set; }

        public virtual ICollection<AuthorBook> AuthorsBooks { get; set; }
    }
}
