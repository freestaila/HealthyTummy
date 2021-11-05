namespace HealthyTummy.Models
{
    public class MealProducts
    {
        public int MealId { get; set; }
        public virtual Meal Meal { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

    }
}
