using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthyTummy.Models;

namespace HealthyTummy.Services
{
    public interface IDietPlanDaysService
    {
        public List<Day> GetDaysList();
        public List<Day> GetDietPlanDaysList(int dietPlanId);
        public List<Day> GetDietPlansListWithDietPlanDaysChecked(int dietPlanId);
        public IEnumerable<DietPlan> GetDietPlanDaysDetails(IEnumerable<DietPlan> dietPlansList);
        public void AddDietPlanDaysToDatabase(int dietPlanId, string[] daysIdsList);
    }
}
