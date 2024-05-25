using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Library.WebApi.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public string? Description { get; set; }
        public byte[]? Image { get; set; }

        [Range(0,5)]
        public double Rating { get; set; }
        [Required]
        public DateTime PublishingDate { get; set; }
        [Required]
        public bool IsCheckedOut { get; set; }
        [JsonIgnore]
        public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();

    }
}
