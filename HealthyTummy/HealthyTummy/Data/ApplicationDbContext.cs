using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthyTummy.Dal.Configurations;
using HealthyTummy.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthyTummy.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<DietPlan> DietPlans { get; set; }
        public DbSet<MealProducts> MealProducts { get; set; }
        public DbSet<DayMeals> DayMeals { get; set; }
        public DbSet<DietPlanDays> DietPlanDays { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new MealProductConfiguration());
            builder.ApplyConfiguration(new DayMealsConfiguration());
            builder.ApplyConfiguration(new DietPlanDaysConfiguration());
        }
    }
}
