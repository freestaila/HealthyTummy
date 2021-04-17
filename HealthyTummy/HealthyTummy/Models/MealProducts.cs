using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthyTummy.Models
{
    public class MealProducts
    {
        public int MealId { get; set; }
        public virtual Meal Meal { get; set; }

        public int ProdutcId { get; set; }
        public virtual Product Product { get; set; }

    }
}
