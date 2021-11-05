namespace HealthyTummy.Models
{
    public class DayMeals
    {
        public int DayId { get; set; }
        public virtual Day Day { get; set; }

        public int MealId { get; set; }
        public virtual Meal Meal { get; set; }

        public int Hour { get; set; }
        public int Minutes { get; set; }
    }
}
