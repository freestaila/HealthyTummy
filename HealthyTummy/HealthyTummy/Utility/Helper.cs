using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace HealthyTummy.Utility
{
    public static class Helper 
    {
        public static string Monday = "Monday";
        public static string Thuesday = "Thuesday";
        public static string Wednesday = "Wednesday";
        public static string Thursday = "Thursday";
        public static string Friday = "Friday";
        public static string Saturday = "Saturday";
        public static string Sunday = "Sunday";

        public static List<SelectListItem> GetDaysOfWeek()
        {
            return new List<SelectListItem>
            {
                new SelectListItem{Value=Helper.Monday, Text=Helper.Monday },
                new SelectListItem{Value=Helper.Thuesday, Text=Helper.Thuesday },
                new SelectListItem{Value=Helper.Wednesday, Text=Helper.Wednesday },
                new SelectListItem{Value=Helper.Thursday, Text=Helper.Thursday },
                new SelectListItem{Value=Helper.Saturday, Text=Helper.Saturday },
                new SelectListItem{Value=Helper.Sunday, Text=Helper.Sunday }
            };
        }
    }
}
