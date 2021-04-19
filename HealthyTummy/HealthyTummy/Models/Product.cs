using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HealthyTummy.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string UnitType { get; set; }
        [Required]
        public int CaloriesPerUnit { get; set; }
        public ICollection<MealProducts> Meals { get; set; }

        [NotMapped]
        public string ProductDetails => $"{Name}{" "}{CaloriesPerUnit}{"/"}{UnitType}";
    }
}
