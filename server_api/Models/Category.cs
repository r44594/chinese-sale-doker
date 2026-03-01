using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace server_api.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }=string.Empty;
        [JsonIgnore]
        public ICollection<Gift> Gifts { get; set; } = new List<Gift>();
    }
}
