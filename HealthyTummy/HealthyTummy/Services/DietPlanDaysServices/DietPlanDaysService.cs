using HealthyTummy.Data;
using HealthyTummy.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthyTummy.Services
{
    public class DietPlanDaysService : IDietPlanDaysService

    {
        private readonly ApplicationDbContext _db;

        public DietPlanDaysService(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<Day> GetDaysList()
        {
            var days = _db.Days.ToList();
            return days;
        }

        public IEnumerable<DietPlan> GetDietPlanDaysDetails(IEnumerable<DietPlan> dietPlansList)
        {
            foreach (var dietPlan in dietPlansList)
            {
                List<Day> dietPlanDays = GetDietPlanDaysList(dietPlan.Id);
                foreach (var day in dietPlan.Days)
                {
                    Day dayData = dietPlanDays.Find(id => id.Id == day.DayId);
                    day.Day.Id = dayData.Id;
                    day.Day.Name = dayData.Name;
                    day.Day.Calories = dayData.Calories;
                    day.Day.Meals = dayData.Meals;
                }
            }
            return dietPlansList;
        }

        public List<Day> GetDietPlanDaysList(int dietPlanId)
        {
            var daysList = (from day in _db.Days
                             join dietPlanDays in _db.DietPlanDays on day.Id equals dietPlanDays.DayId
                             where dietPlanDays.DietPlanId == dietPlanId
                             select day).ToList();
            return daysList;
        }

        public List<Day> GetDietPlansListWithDietPlanDaysChecked(int dietPlanId)
        {
            List<Day> daysList = GetDaysList();
            List<Day> assignedDaysList = GetDietPlanDaysList(dietPlanId);

            foreach (var day in daysList)
            {
                if (assignedDaysList.Any(ap => ap.Id == day.Id))
                {
                    day.Assigned = true;
                }
                else
                {
                    day.Assigned = false;
                }
            }
            return daysList;
        }
        public void AddDietPlanDaysToDatabase(int dietPlanId, string[] daysIdsList)
        {
            //checl if dietplan was created befor or have any day
            if (_db.DietPlanDays.Any(id => id.DietPlanId == dietPlanId))
            {
                //get dietplan day list
                var dietPlanDays = _db.DietPlanDays.Where(id => id.DietPlanId == dietPlanId);

                //dietplan was created or have day assigned OR new day list need to be assigned
                if (daysIdsList != null || daysIdsList.Length != 0)
                {
                    //remove records not on list but they are on db
                    foreach (var dietPlanDay in dietPlanDays)
                    {
                        //if new list dont have day in db remove it
                        if (!daysIdsList.Contains(dietPlanDay.DayId.ToString()))
                        {
                            _db.DietPlanDays.Remove(dietPlanDay);
                        }
                        //remove day from new list if they already are in db
                        else
                        {
                            daysIdsList = daysIdsList.Where(e => e != dietPlanDay.DayId.ToString()).ToArray();
                        }
                    }
                    //add new day to db
                    AddDietPlanDaysToDb(dietPlanId, daysIdsList);
                }
                //remove all day because list is empty
                else
                {
                    foreach (var dietPlanDay in dietPlanDays)
                    {
                        _db.DietPlanDays.Remove(dietPlanDay);
                    }
                }
                _db.SaveChanges();
            }
            //new dietplan without any day yet
            else
            {
                AddDietPlanDaysToDb(dietPlanId, daysIdsList);
            }
        }
        private void AddDietPlanDaysToDb(int dietPlanId, string[] daysIdsList)
        {
            if (daysIdsList != null)
            {
                foreach (var day in daysIdsList)
                {
                    DietPlanDays dietPlanDays = new();
                    dietPlanDays.DietPlanId = dietPlanId;
                    dietPlanDays.DayId = int.Parse(day);
                    _db.DietPlanDays.Add(dietPlanDays);
                }
                _db.SaveChanges();
            }
        }
    }
}
