using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthyTummy.Models
{
    public class Day 
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int Calories { get; set; }

        public virtual ICollection<DayMeals> Meals { get; set; }

        public virtual ICollection<DietPlanDays> DietPlans { get; set; }

        [NotMapped]
        public string DayDetails => $"{Name}{" "}{Calories}";

        [NotMapped]
        public bool Assigned { get; set; }

        [NotMapped]
        public bool ActionType { get; set; }

    }
}
