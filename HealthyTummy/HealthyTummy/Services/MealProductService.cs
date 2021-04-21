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
                                join mealProducts in _db.MealProducts on product.Id equals mealProducts.ProdutcId
                                where mealProducts.MealId == mealId
                                select product).ToList();
            return productsList;
        }
        public List<Product> GetProductsListWithAssignedProducts(int mealId)
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
                    Product productData = mealProducts.Find(id => id.Id == product.ProdutcId);
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

            foreach (var productForMeal in productsList)
            {
                Product newProduct = _db.Products.First(x => x.Id ==  int.Parse(productForMeal));

                mealCalories += newProduct.CaloriesPerUnit;
            }
            return mealCalories;
        }

        public void AddMealProductsToDatabase(int mealId, string[] productsIdsList)
        {
            //Najpierw sprawdzam czy nasz posilek byl juz wczesniej stworzony lub czy posiadał jakies produkty
            if (_db.MealProducts.Any(id => id.MealId == mealId))
            {
                //pobieram liste produktow dla danego posilku
                
                var mealProducts = _db.MealProducts.Where(id => id.MealId == mealId);

                //mamy posilek ktory edytujemy lub mial jakies produkty przypisane
                //mam nowa liste produktow
                if (productsIdsList.Count() != 0)
                {
                    //teraz musze usunac wszystkie rekordy ktore nie sa na naszej nowej liscie a sa w bazie danych
                    foreach(var mealProduct in mealProducts)
                    {
                        //jezeli nasza nowa lista nie zawiera obecnego w bazie id produktu to usuwam ten rekord
                        if (!productsIdsList.Contains(mealProduct.ProdutcId.ToString()))
                        {
                            _db.MealProducts.Remove(mealProduct);
                        }
                    }
                    //musimy dodac nowe produkty do bazy jezeli nie bylo ich wczesniej
                    foreach(var id in productsIdsList)
                    {
                        if(!mealProducts.Any(mp=> mp.ProdutcId == int.Parse(id)))
                        {
                            MealProducts newMealProducts = new();
                            newMealProducts.MealId = mealId;
                            newMealProducts.ProdutcId = int.Parse(id);
                            _db.MealProducts.Add(newMealProducts);
                        }
                    }
                }
                //usuwam wszystkie rekordy dla danego posilku bo wszystkei odznaczylem
                else
                {
                    foreach(var mealProduct in mealProducts)
                    {
                        _db.MealProducts.Remove(mealProduct);
                    }
                }
                _db.SaveChanges();
            }
            //mam albo nowy posilek, albo nie mial wczesniej zadnych produktow
            else
            {
                //oczywiscie sprawdzam czy mamy jakies produkty do dodania 
                if (productsIdsList.Count() != 0)
                {
                    foreach (var product in productsIdsList)
                    {
                        MealProducts mealProducts = new();
                        mealProducts.MealId = mealId;
                        mealProducts.ProdutcId = int.Parse(product);
                        _db.MealProducts.Add(mealProducts);
                    }
                    _db.SaveChanges();
                }
            }
        }

        public void RemoveMealProductsFromDb(int mealId)
        {
            
        }
    }
}
