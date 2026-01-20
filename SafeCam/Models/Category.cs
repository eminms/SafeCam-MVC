using SafeCam.Models.Common;

namespace SafeCam.Models
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }
        public List<Product>? Products { get; set; }
    }
}
