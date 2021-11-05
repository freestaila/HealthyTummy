using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthyTummy.Models
{
    public class DietPlan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<DietPlanDays> Days { get; set; }

        [NotMapped]
        public bool ActionType { get; set; }

    }
}
