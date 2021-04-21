using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthyTummy.Data;
using HealthyTummy.Models;
using HealthyTummy.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HealthyTummy.Controllers
{
    public class MealController : NotificationsController
    {
        private readonly ApplicationDbContext _db;
        private readonly IMealProductService _mealProductService;

        public MealController(ApplicationDbContext db, IMealProductService mealProductService)
        {
            _db = db;
            _mealProductService = mealProductService;
        }
        public IActionResult Index()
        {
            //pobieram ienumerable
            IEnumerable<Meal> mealsList = _db.Meals
                       .Include(p => p.Products)
                       .ToList();

            _mealProductService.GetMealsProductsDetails(mealsList);

            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            if (isAjax)
            {
                return PartialView("_MealsTable", mealsList);
            }
            return View(mealsList);
        }

        #region CreateAction
        [HttpGet]
        public IActionResult Create()
        {
            Meal newMeal = new();
            ViewBag.ProductsList = _mealProductService.GetProductList();
            return PartialView("_AddEditMeal", newMeal);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MealProductsVM mealVM)
        {
            Meal newMeal = new ();

            if (ModelState.IsValid)
            {
                newMeal.Name = mealVM.Name;
                newMeal.Calories= _mealProductService.CalculateMealCalories(mealVM.Products);
                _db.Meals.Add(newMeal);
                _db.SaveChanges();

                _mealProductService.AddMealProductsToDatabase(newMeal.Id, mealVM.Products);

                CreateNotification("Meal saved!");
            }
            ViewBag.ProductsList = _mealProductService.GetMealProductsList(newMeal.Id);
            return PartialView("_AddEditMeal", newMeal);
        }
        #endregion

        #region EditSection
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var mealToEdit = _db.Meals.Where(x => x.Id == id).FirstOrDefault();
            if (mealToEdit == null)
            {
                return NotFound();
            }
            ViewBag.ProductsList = _mealProductService.GetProductsListWithAssignedProducts(id);
            return PartialView("_AddEditMeal", mealToEdit);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Meal editedMeal, string[] Products)
        {
            Meal mealToEdit = _db.Meals.Find(editedMeal.Id);

            if (ModelState.IsValid)
            {
                mealToEdit.Name = editedMeal.Name;
                mealToEdit.Calories = _mealProductService.CalculateMealCalories(Products);

                _db.SaveChanges();

                _mealProductService.AddMealProductsToDatabase(editedMeal.Id, Products);

                CreateNotification("Meal changed!");
            }

            ViewBag.ProductsList = _mealProductService.GetMealProductsList(editedMeal.Id);
            return PartialView("_AddEditMeal", editedMeal);
        }
        #endregion

        #region DeleteSection
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var mealToDelete = _db.Meals.Where(x => x.Id == id).FirstOrDefault();
            if (mealToDelete == null)
            {
                return NotFound();
            }
            return PartialView("_DeleteMealPartial", mealToDelete);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Meal mealToDelete)
        {
            _mealProductService.RemoveMealProductsFromDb(mealToDelete.Id);
            _db.Meals.Remove(mealToDelete);
            CreateNotification("Meal deleted!");
            _db.SaveChanges();
            return PartialView("_DeleteMealPartial", mealToDelete);
        }
        #endregion
    }
}
