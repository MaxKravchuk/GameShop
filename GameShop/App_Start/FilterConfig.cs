﻿using GameShop.Filters;
using System.Web;
using System.Web.Mvc;

namespace GameShop
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
