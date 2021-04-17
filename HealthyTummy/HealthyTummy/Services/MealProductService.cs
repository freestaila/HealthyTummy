using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var mealProducts = (from product in _db.Products
                                where product.Meals.Any(m => m.MealId == mealId)
                                select product).ToList();
            return mealProducts;
        }
    }
}
