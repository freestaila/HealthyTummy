using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthyTummy.Models;

namespace HealthyTummy.Services
{
    public interface IMealProductService
    {
        public List<Product> GetProductList();
        public List<Product> GetProductsListWithAssignedProducts(int mealId);
        public List<Product> GetMealProductsList(int mealId);
        public IEnumerable<Meal> GetMealsProductsDetails(IEnumerable<Meal> mealsList);
        public int CalculateMealCalories(string[] productsList);
        public void AddMealProductsToDatabase(int mealId, string[] productsIdsList);
        public void RemoveMealProductsFromDb(int mealId);
    }
}
