using HealthyTummy.Data;
using HealthyTummy.Models;
using HealthyTummy.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HealthyTummy.Controllers
{
    public class DietPlanController : NotificationsController
    {
        private readonly ApplicationDbContext _db;
        private readonly IDietPlanDaysService _dietPlanDaysService;

        public DietPlanController(ApplicationDbContext db, IDietPlanDaysService dietPlanDaysService)
        {
            _db = db;
            _dietPlanDaysService = dietPlanDaysService;
        }
        public IActionResult Index()
        {
            //get ienumerable
            IEnumerable<DietPlan> dietPlansList = _db.DietPlans
                       .Include(m => m.Days)
                       .ToList();

            _dietPlanDaysService.GetDietPlanDaysDetails(dietPlansList);

            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            if (isAjax)
            {
                return PartialView("_DietPlansTable", dietPlansList);
            }
            return View(dietPlansList);
        }

        #region CreateAction
        [HttpGet]
        public IActionResult Create()
        {
            DietPlan newDietPlan = new();
            newDietPlan.ActionType = true;
            ViewBag.DaysList = _dietPlanDaysService.GetDaysList();
            return PartialView("_AddEditDietPlan", newDietPlan);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DietPlanDaysVM dietPlanVM)
        {
            DietPlan newDietPlan = new();
            newDietPlan.ActionType = true;

            if (ModelState.IsValid)
            {
                newDietPlan.Name = dietPlanVM.Name;
                _db.DietPlans.Add(newDietPlan);
                _db.SaveChanges();

                _dietPlanDaysService.AddDietPlanDaysToDatabase(newDietPlan.Id, dietPlanVM.Days);

                CreateNotification("DietPlan saved!");
            }
            ViewBag.DaysList = _dietPlanDaysService.GetDaysList();
            return PartialView("_AddEditDietPlan", newDietPlan);
        }
        #endregion

        #region EditSection
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var dietPlanToEdit = _db.DietPlans.Where(x => x.Id == id).FirstOrDefault();
            if (dietPlanToEdit == null)
            {
                return NotFound();
            }
            dietPlanToEdit.ActionType = false;
            ViewBag.DaysList = _dietPlanDaysService.GetDietPlansListWithDietPlanDaysChecked(id);
            return PartialView("_AddEditDietPlan", dietPlanToEdit);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(DietPlan editedDietPlan, string[] Days)
        {
            if (ModelState.IsValid)
            {
                DietPlan dietPlanToEdit = _db.DietPlans.Find(editedDietPlan.Id);
                dietPlanToEdit.Name = editedDietPlan.Name;
                _db.SaveChanges();
                _dietPlanDaysService.AddDietPlanDaysToDatabase(editedDietPlan.Id, Days);
                CreateNotification("DietPlan changed!");
            }
            ViewBag.DaysList = _dietPlanDaysService.GetDietPlansListWithDietPlanDaysChecked(editedDietPlan.Id);
            return PartialView("_AddEditDietPlan", editedDietPlan);
        }
        #endregion

        #region DeleteSection
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var dietPlanToDelete = _db.DietPlans.Where(x => x.Id == id).FirstOrDefault();
            if (dietPlanToDelete == null)
            {
                return NotFound();
            }
            return PartialView("_DeleteDietPlanPartial", dietPlanToDelete);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(DietPlan dietPlanToDelete)
        {
            _db.DietPlans.Remove(dietPlanToDelete);
            CreateNotification("Diet Plan deleted!");
            _db.SaveChanges();
            return PartialView("_DeleteDietPlanPartial", dietPlanToDelete);
        }
        #endregion
    }
}
