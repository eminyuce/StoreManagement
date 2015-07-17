using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotLiquid.ViewEngine;

[assembly: WebActivator.PostApplicationStartMethod(typeof(StoreManagement.Data.App_Start.DotLiquidViewEngineStart), "Start")]
namespace StoreManagement.Data.App_Start
{
    public static class DotLiquidViewEngineStart
    {
        public static void Start()
        {
            ViewEngines.Engines.Add(new DotLiquidViewEngine());
        }
    }
}