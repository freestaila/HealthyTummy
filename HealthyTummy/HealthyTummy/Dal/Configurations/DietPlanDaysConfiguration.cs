using HealthyTummy.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthyTummy.Dal.Configurations
{
    public class DietPlanDaysConfiguration : IEntityTypeConfiguration<DietPlanDays>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<DietPlanDays> builder)
        {
            builder
                .HasKey(dm => new { dm.DietPlanId, dm.DayId });
            builder
                .HasOne(dm => dm.DietPlan)
                .WithMany(dm => dm.Days)
                .HasForeignKey(dm => dm.DietPlanId);
            builder
                .HasOne(dm => dm.Day)
                .WithMany(dm => dm.DietPlans)
                .HasForeignKey(dm => dm.DayId);
        }
    }
}
