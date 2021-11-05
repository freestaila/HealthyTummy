namespace HealthyTummy.Models
{
    public class DayMealsVM
    {
        public string Name { get; set; }
        public virtual string[] Meals { get; set; }
    }
}
