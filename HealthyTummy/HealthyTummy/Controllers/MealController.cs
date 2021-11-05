using HealthyTummy.Data;
using HealthyTummy.Models;
using HealthyTummy.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthyTummy.Controllers
{
    public class MealController : NotificationsController
    {
        private readonly ApplicationDbContext _db;
        private readonly IMealProductService _mealProductService;
        private readonly IMealService _mealService;

        public MealController(ApplicationDbContext db, IMealProductService mealProductService, IMealService mealService)
        {
            _db = db;
            _mealProductService= mealProductService;
            _mealService = mealService;
        }
        public IActionResult Index()
        {
            IEnumerable<Meal> mealsList = _mealProductService.GetMealsProductsDetails(_mealService.GetMealsListFromDB());
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
            Meal newMeal= new();
            newMeal.ActionType = true;
            ViewBag.ProductsList = _mealProductService.GetProductList();
            return PartialView("_AddEditMeal", newMeal);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MealProductsVM mealVM)
        {
            Meal newMeal = new();
            newMeal.ActionType = true;
            if (ModelState.IsValid)
            {
                newMeal.Name = mealVM.Name;
                newMeal.Calories = _mealProductService.CalculateMealCalories(mealVM.Products);
                _db.Meals.Add(newMeal);
                _db.SaveChanges();
                _mealProductService.AddMealProductsToDatabase(newMeal.Id, mealVM.Products);
                CreateNotification("Meal saved!");
            }
            //now we need to refresh our site - with valid or invalid form
            ViewBag.ProductsList = _mealProductService.GetProductList();
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
            mealToEdit.ActionType = false;
            ViewBag.ProductsList = _mealProductService.GetProductsListWithMealProductsChecked(id);
            return PartialView("_AddEditMeal", mealToEdit);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Meal editedMeal, string[] Products)
        {
            if (ModelState.IsValid)
            {
                Meal mealToEdit = _db.Meals.Find(editedMeal.Id);
                mealToEdit.Name = editedMeal.Name;
                mealToEdit.Calories = _mealProductService.CalculateMealCalories(Products);
                _db.SaveChanges();
                _mealProductService.AddMealProductsToDatabase(editedMeal.Id, Products);
                CreateNotification("Meal changed!");
            }
            ViewBag.ProductsList = _mealProductService.GetProductsListWithMealProductsChecked(editedMeal.Id);
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
            _db.Meals.Remove(mealToDelete);
            CreateNotification("Meal deleted!");
            _db.SaveChanges();
            return PartialView("_DeleteMealPartial", mealToDelete);
        }
        #endregion
    }
}
