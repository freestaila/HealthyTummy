using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthyTummy.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthyTummy.Dal.Configurations
{
    public class MealProductConfiguration : IEntityTypeConfiguration<MealProducts>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<MealProducts> builder)
        {
            builder
                .HasKey(mp => new { mp.MealId, mp.ProdutcId });
            builder
                .HasOne(mp => mp.Meal)
                .WithMany(mp => mp.Products)
                .HasForeignKey(mp => mp.MealId);
            builder
                .HasOne(mp => mp.Product)
                .WithMany(mp => mp.Meals)
                .HasForeignKey(mp => mp.ProdutcId);
        }
    }
}
