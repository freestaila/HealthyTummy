using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public virtual ICollection<DayMeals> Days { get; set; }

        [NotMapped]
        public int Hours { get; set; }
        [NotMapped]
        public int Minutes { get; set; }

        [NotMapped]
        public string MealDetails => $"{Name}{" "}{Calories}";
        [NotMapped]
        public bool Assigned { get; set; }
        [NotMapped]
        public bool ActionType { get; set; }
    }
}
