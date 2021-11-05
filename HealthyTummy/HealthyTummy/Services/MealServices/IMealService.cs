using HealthyTummy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthyTummy.Services
{
    public interface IMealService
    {
        public IEnumerable<Meal> GetMealsListFromDB();
    }
}
