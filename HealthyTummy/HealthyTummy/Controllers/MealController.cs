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
            IEnumerable<Meal> mealsList = _db.Meals
                       .Include(p => p.Products)
                       .ToList();
            //dla kazdego posilku
            foreach(var meal in mealsList)
            {
                //pobierz liste produktow dla wybranego posilku
                List<Product> mealProducts = _mealProductService.GetMealProductsList(meal.Id);
                //dla kazdego produktu przypisz wartosci pobrane z listy produktow
                foreach(var product in meal.Products)
                {
                    //wybieram product z listy produktow wedlug jego id
                    Product productData = mealProducts.Find(id=> id.Id == product.ProdutcId);
                    //przypisuje wszystkie pobrane dane produktu do naszego obiektu w kolekcji produktow
                    product.Product.Id = productData.Id;
                    product.Product.Name = productData.Name;
                    product.Product.CaloriesPerUnit = productData.CaloriesPerUnit;
                    product.Product.UnitType = productData.UnitType;
                }
            }
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
            Meal newMeal = new Meal();
            if (ModelState.IsValid)
            {
                newMeal.Name = mealVM.Name;
                _db.Meals.Add(newMeal);
                _db.SaveChanges();

                foreach(var product in mealVM.Products)
                {
                    MealProducts mealProducts = new MealProducts();
                    mealProducts.MealId = newMeal.Id;
                    mealProducts.ProdutcId =int.Parse(product);
                    _db.MealProducts.Add(mealProducts);
                }
                _db.SaveChanges();
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
