using HealthyTummy.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthyTummy.Dal.Configurations
{
    public class DayMealsConfiguration : IEntityTypeConfiguration<DayMeals>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<DayMeals> builder)
        {
            builder
                .HasKey(dm => new { dm.DayId, dm.MealId });
            builder
                .HasOne(dm => dm.Day)
                .WithMany(dm => dm.Meals)
                .HasForeignKey(dm => dm.DayId);
            builder
                .HasOne(dm => dm.Meal)
                .WithMany(dm => dm.Days)
                .HasForeignKey(dm => dm.MealId);
        }
    }
}
