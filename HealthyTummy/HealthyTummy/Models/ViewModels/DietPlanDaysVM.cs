namespace HealthyTummy.Models
{
    public class DietPlanDaysVM
    {
        public string Name { get; set; }
        public virtual string[] Days { get; set; }
    }
}
