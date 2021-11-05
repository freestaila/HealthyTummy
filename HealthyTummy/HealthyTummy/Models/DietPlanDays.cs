namespace HealthyTummy.Models
{
    public class DietPlanDays
    {
        public int DietPlanId { get; set; }
        public virtual DietPlan DietPlan { get; set; }

        public int DayId { get; set; }
        public virtual Day Day { get; set; }
    }
}
