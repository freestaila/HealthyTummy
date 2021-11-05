using HealthyTummy.Data;
using HealthyTummy.Models;
using HealthyTummy.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HealthyTummy.Controllers
{
    public class DayController : NotificationsController
    {
        private readonly ApplicationDbContext _db;
        private readonly IDayMealsService _dayMealsService;

        public DayController(ApplicationDbContext db, IDayMealsService dayMealsService)
        {
            _db = db;
            _dayMealsService = dayMealsService;
        }
        public IActionResult Index()
        {
            //get ienumerable
            IEnumerable<Day> daysList = _db.Days
                       .Include(m => m.Meals)
                       .ToList();

            _dayMealsService.GetDayMealsDetails(daysList);

            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            if (isAjax)
            {
                return PartialView("_DaysTable", daysList);
            }
            return View(daysList);
        }

        #region CreateAction
        [HttpGet]
        public IActionResult Create()
        {
            Day newDay = new();
            newDay.ActionType = true;
            ViewBag.MealsList = _dayMealsService.GetMealsList();
            return PartialView("_AddEditDay", newDay);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DayMealsVM dayVM)
        {
            Day newDay = new();
            newDay.ActionType = true;
            if (ModelState.IsValid)
            {
                newDay.Name = dayVM.Name;
                newDay.Calories = _dayMealsService.CalculateDayCalories(dayVM.Meals);
                _db.Days.Add(newDay);
                _db.SaveChanges();

                _dayMealsService.AddDayMealsToDatabase(newDay.Id, dayVM.Meals);

                CreateNotification("Day saved!");
            }
            ViewBag.MealsList = _dayMealsService.GetMealsList();
            return PartialView("_AddEditDay", newDay);
        }
        #endregion

        #region EditSection
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var dayToEdit = _db.Days.Where(x => x.Id == id).FirstOrDefault();
            if (dayToEdit == null)
            {
                return NotFound();
            }
            dayToEdit.ActionType = false;
            ViewBag.MealsList = _dayMealsService.GetMealsListWithDayMealsChecked(id);
            return PartialView("_AddEditDay", dayToEdit);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Day editedDay, string[] Meals)
        {
            if (ModelState.IsValid)
            {
                Day dayToEdit = _db.Days.Find(editedDay.Id);
                dayToEdit.Name = editedDay.Name;
                dayToEdit.Calories = _dayMealsService.CalculateDayCalories(Meals);
                _db.SaveChanges();
                _dayMealsService.AddDayMealsToDatabase(editedDay.Id, Meals);
                CreateNotification("Day changed!");
            }
            ViewBag.MealsList = _dayMealsService.GetMealsListWithDayMealsChecked(editedDay.Id);
            return PartialView("_AddEditDay", editedDay);
        }
        #endregion

        #region DeleteSection
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var dayToDelete = _db.Days.Where(x => x.Id == id).FirstOrDefault();
            if (dayToDelete == null)
            {
                return NotFound();
            }
            return PartialView("_DeleteDayPartial", dayToDelete);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Day dayToDelete)
        {
            _db.Days.Remove(dayToDelete);
            CreateNotification("Day deleted!");
            _db.SaveChanges();
            return PartialView("_DeleteDayPartial", dayToDelete);
        }
        #endregion
    }
}
