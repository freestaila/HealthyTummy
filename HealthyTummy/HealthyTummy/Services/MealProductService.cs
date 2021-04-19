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
    }
}
