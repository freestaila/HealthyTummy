using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthyTummy.Models;

namespace HealthyTummy.Services
{
    public interface IDayMealsService
    {
        public List<Meal> GetMealsList();
        public List<Meal> GetDayMealsList(int dayId);
        public List<Meal> GetMealsListWithDayMealsChecked(int dayId);
        public IEnumerable<Day> GetDayMealsDetails(IEnumerable<Day> daysList);
        public int CalculateDayCalories(string[] mealsList);
        public void AddDayMealsToDatabase(int dayId, string[] mealsIdsList);
    }
}
