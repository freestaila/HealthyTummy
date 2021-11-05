using System.ComponentModel.DataAnnotations;

namespace HealthyTummy.Models
{
    public class MealProductsVM
    {
        [Required]
        public string Name { get; set; }
        public virtual string[] Products { get; set; }
    }
}
