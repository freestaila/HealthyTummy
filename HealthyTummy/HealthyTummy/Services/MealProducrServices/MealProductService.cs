using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthyTummy.Controllers;
using HealthyTummy.Data;
using HealthyTummy.Models;

namespace HealthyTummy.Services
{
    public class MealProductService : IMealProductService
    {
        private readonly ApplicationDbContext _db;

        public MealProductService(ApplicationDbContext db)
        {
            _db = db;
        }
        public List<Product> GetProductList()
        {
            var products = _db.Products.ToList();
            return products;
        }

        public List<Product> GetMealProductsList(int mealId)
        {
            var productsList = (from product in _db.Products
                                join mealProducts in _db.MealProducts on product.Id equals mealProducts.ProductId
                                where mealProducts.MealId == mealId
                                select product).ToList();
            return productsList;
        }
        public List<Product> GetProductsListWithMealProductsChecked(int mealId)
        { 
            List<Product> productsList = GetProductList();
            List<Product> assignedProductsList = GetMealProductsList(mealId);

            foreach(var product in productsList)
            {
                if(assignedProductsList.Any(ap=>ap.Id ==product.Id))
                {
                    product.Assigned = true;
                }
                else
                {
                    product.Assigned = false;
                }
            }
            return productsList;
        }
        public IEnumerable<Meal> GetMealsProductsDetails(IEnumerable<Meal> mealsList)
        {
            foreach (var meal in mealsList)
            {
                List<Product> mealProducts = GetMealProductsList(meal.Id);
                foreach (var product in meal.Products)
                {
                    Product productData = mealProducts.Find(id => id.Id == product.ProductId);
                    product.Product.Id = productData.Id;
                    product.Product.Name = productData.Name;
                    product.Product.CaloriesPerUnit = productData.CaloriesPerUnit;
                    product.Product.UnitType = productData.UnitType;
                }
            }
            return mealsList;
        }

        public int CalculateMealCalories(string[] productsList)
        {
            int mealCalories = 0;
            if (productsList != null)
            {
                foreach (var productForMeal in productsList)
                {
                    Product newProduct = _db.Products.First(x => x.Id == int.Parse(productForMeal));

                    mealCalories += newProduct.CaloriesPerUnit;
                }
            }
            return mealCalories;
        }

        public void AddMealProductsToDatabase(int mealId, string[] productsIdsList)
        {
            //checl if meal was created befor or have any products
            if (_db.MealProducts.Any(id => id.MealId == mealId))
            {
                //get meal products list
                var mealProducts = _db.MealProducts.Where(id => id.MealId == mealId);

                //meal was created or have products assigned OR new products list need to be assigned
                if (productsIdsList != null || productsIdsList.Length != 0)
                {
                    //remove records not on list but they are on db
                    foreach(var mealProduct in mealProducts)
                    {
                        //if new list dont have product in db remove it
                        if (!productsIdsList.Contains(mealProduct.ProductId.ToString()))
                        {
                            _db.MealProducts.Remove(mealProduct);
                        }
                        //remove products from new list if they already are in db
                        else
                        {
                            productsIdsList = productsIdsList.Where(e => e != mealProduct.ProductId.ToString()).ToArray();
                        }
                    }
                    //add new products to db
                    AddMealProductToDb(mealId, productsIdsList);
                }
                //remove all products because list is empty
                else
                {
                    foreach(var mealProduct in mealProducts)
                    {
                        _db.MealProducts.Remove(mealProduct);
                    }
                }
                _db.SaveChanges();
            }
            //new meal without any products yet
            else
            {
                AddMealProductToDb(mealId,productsIdsList);
            }
        }
        private void AddMealProductToDb(int mealId, string[] productsIdsList)
        {
            if (productsIdsList != null)
            {
                foreach (var product in productsIdsList)
                {
                    MealProducts mealProducts = new();
                    mealProducts.MealId = mealId;
                    mealProducts.ProductId = int.Parse(product);
                    _db.MealProducts.Add(mealProducts);
                }
                _db.SaveChanges();
            }
        }
    }
}
