using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestRESTAPI.Data.Models
{
    public class Item
    {
        [Required]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public int price { get; set; }
        public string Notes { get; set; }
        public byte[]? Image { get; set; }
        [ForeignKey("category")]
        public int CategoryId { get; set; }
        public Category category { get; set; }
    }
}
