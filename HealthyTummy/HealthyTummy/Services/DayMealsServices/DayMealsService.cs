using HealthyTummy.Data;
using HealthyTummy.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthyTummy.Services
{
    public class DayMealsService : IDayMealsService
    {
        private readonly ApplicationDbContext _db;

        public DayMealsService(ApplicationDbContext db)
        {
            _db = db;
        }
        public List<Meal> GetMealsList()
        {
            var meals = _db.Meals.ToList();
            return meals;
        }

        public List<Meal> GetDayMealsList(int dayId)
        {
            var mealsList = (from meal in _db.Meals
                                join dayMeals in _db.DayMeals on meal.Id equals dayMeals.MealId
                                where dayMeals.DayId == dayId
                                select meal).ToList();
            return mealsList;
        }
        public List<Meal> GetMealsListWithDayMealsChecked(int dayId)
        { 
            List<Meal> mealsList = GetMealsList();
            List<Meal> assignedMealsList = GetDayMealsList(dayId);

            foreach(var meal in mealsList)
            {
                if(assignedMealsList.Any(ap=>ap.Id == meal.Id))
                {
                    meal.Assigned = true;
                }
                else
                {
                    meal.Assigned = false;
                }
            }
            return mealsList;
        }

        public IEnumerable<Day> GetDayMealsDetails(IEnumerable<Day> daysList)
        {

            foreach (var day in daysList)
            {
                List<Meal> dayMeals = GetDayMealsList(day.Id);
                foreach (var meal in day.Meals)
                {
                    Meal mealData = dayMeals.Find(id => id.Id == meal.MealId);
                    meal.Meal.Id = mealData.Id;
                    meal.Meal.Name = mealData.Name;
                    meal.Meal.Calories = mealData.Calories;
                    meal.Meal.Products = mealData.Products;
                    day.Calories+= mealData.Calories;
                }
            }
            return daysList;
        }

        public int CalculateDayCalories(string[] mealsList)
        {
            int dayCalories = 0;
            if (dayCalories!=0) {
                foreach (var mealForDay in mealsList)
                {
                    Meal newMeal = _db.Meals.First(x => x.Id == int.Parse(mealForDay));

                    dayCalories += newMeal.Calories;
                }
            }
            return dayCalories;
        }

        public void AddDayMealsToDatabase(int dayId, string[] mealsIdsList)
        {
            //checl if day was created befor or have any meals
            if (_db.DayMeals.Any(id => id.DayId == dayId))
            {
                //get day meals list
                var dayMeals = _db.DayMeals.Where(id => id.DayId == dayId);

                //day was created or have meals assigned OR new meals list need to be assigned
                if (mealsIdsList != null || mealsIdsList.Length != 0)
                {
                    //remove records not on list but they are on db
                    foreach (var dayMeal in dayMeals)
                    {
                        //if new list dont have meal in db remove it
                        if (!mealsIdsList.Contains(dayMeal.MealId.ToString()))
                        {
                            _db.DayMeals.Remove(dayMeal);
                        }
                        //remove meals from new list if they already are in db
                        else
                        {
                            mealsIdsList = mealsIdsList.Where(e => e != dayMeal.MealId.ToString()).ToArray();
                        }
                    }
                    //add new meals to db
                    AddDayMealsToDb(dayId, mealsIdsList);
                }
                //remove all meals because list is empty
                else
                {
                    foreach (var dayMeal in dayMeals)
                    {
                        _db.DayMeals.Remove(dayMeal);
                    }
                }
                _db.SaveChanges();
            }
            //new meal without any products yet
            else
            {
                AddDayMealsToDb(dayId, mealsIdsList);
            }
        }
        private void AddDayMealsToDb(int dayId, string[] mealsIdsList)
        {
            if (mealsIdsList != null)
            {
                foreach (var meal in mealsIdsList)
                {
                    DayMeals dayMeals = new();
                    dayMeals.DayId = dayId;
                    dayMeals.MealId = int.Parse(meal);
                    _db.DayMeals.Add(dayMeals);
                }
                _db.SaveChanges();
            }
        }
    }
}
