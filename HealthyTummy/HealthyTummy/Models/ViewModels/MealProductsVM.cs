using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthyTummy.Models
{
    public class MealProductsVM
    {
        public string Name { get; set; }
        public virtual string[] Products { get; set; }
    }
}
