using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthyTummy.Data;
using HealthyTummy.Models;
using HealthyTummy.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            IEnumerable<Meal> mealsList = _db.Meals;    
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
            //var mealProducts = _mealProductService.GetProductList();
            //newMeal.Products = new List<MealProducts>();
            //foreach (var product in mealProducts)
            //{
            //   newMeal.Products.Add(new MealProducts
            //   {
            //      Product = product
            //   });
            //}
            return PartialView("_AddEditMeal", newMeal);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Meal newMeal)
        {
            if (ModelState.IsValid)
            {
                newMeal.Calories = CalculateMealCalories(newMeal);
                if (newMeal.Products.Count != 0)
                {
                    foreach (var productId in newMeal.Products)
                    {
                        MealProducts newMealProducts = new ();
                        newMealProducts.MealId = newMeal.Id;
                        newMealProducts.ProdutcId = productId.ProdutcId;
                    }
                }
                _db.Meals.Add(newMeal);
                CreateNotification("Meal saved!");
            }
            _db.SaveChanges();
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
            return PartialView("_AddEditMeal", mealToEdit);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Meal editedMeal)
        {
            Meal mealToEdit = _db.Meals.Find(editedMeal.Id);
            mealToEdit.Name = editedMeal.Name;
            mealToEdit.Products = editedMeal.Products;
            mealToEdit.Calories = CalculateMealCalories(editedMeal);
            _db.SaveChanges();
            CreateNotification("Meal changed!");
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
        [NonAction]
        private static int CalculateMealCalories(Meal meal)
        {
            foreach (var productForMeal in meal.Products)
            {
                meal.Calories += productForMeal.Product.CaloriesPerUnit;
            }
            return meal.Calories;
        }
    }

}
