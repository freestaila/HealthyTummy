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
        public List<Product> GetMealProductsList(int mealId); 
    }
}
