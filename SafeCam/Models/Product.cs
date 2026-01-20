using SafeCam.Models.Common;

namespace SafeCam.Models
{
    public class Product:BaseEntity
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
