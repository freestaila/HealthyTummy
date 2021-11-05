using HealthyTummy.Data;
using HealthyTummy.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthyTummy.Services
{
    public class MealService: IMealService
    {
        private readonly ApplicationDbContext _db;

        public MealService(ApplicationDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Meal> GetMealsListFromDB()
        {
            return _db.Meals.Include(p => p.Products).ToList();
        }
    }
}
