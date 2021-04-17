using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HealthyTummy.Models
{
    public class Meal
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public int Calories { get; set; }

        public virtual ICollection<MealProducts> Products { get; set; }

    }
}
