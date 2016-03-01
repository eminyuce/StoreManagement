using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DotLiquid.NamingConventions;
using NLog;
using Ninject;
using StoreManagement.Controllers;
using StoreManagement.Data;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Models;
using StoreManagement.Service.Repositories;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
         //   DotLiquid.Template.NamingConvention = new CSharpNamingConvention();

        }
        public override string GetVaryByCustomString(HttpContext context, string custom)
        {
            if ("User".Equals(custom, StringComparison.InvariantCultureIgnoreCase))
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    return context.User.Identity.Name;
                }
                else
                {
                    return base.GetVaryByCustomString(context, custom);
                }
            }
            return base.GetVaryByCustomString(context, custom);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            Redirect301();
        }
        private void Redirect301()
        {

            if (Request.Url.Host.Contains("login.seatechnologyjobs.com"))
            {
                return;
            }

            if (!Request.Url.Host.StartsWith("www") && !Request.Url.IsLoopback)
            {
                UriBuilder builder = new UriBuilder(Request.Url);
                builder.Host = "www." + Request.Url.Host;
                Response.StatusCode = 301;
                Response.AddHeader("Location", builder.ToString());
                Response.End();
            }
        }


        protected void Application_Error(object sender, EventArgs e)
        {
            var httpContext = ((MvcApplication)sender).Context;
            var currentController = " ";
            var currentAction = " ";
            var currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));

            if (currentRouteData != null)
            {
                if (currentRouteData.Values["controller"] != null && !String.IsNullOrEmpty(currentRouteData.Values["controller"].ToString()))
                {
                    currentController = currentRouteData.Values["controller"].ToString();
                }

                if (currentRouteData.Values["action"] != null && !String.IsNullOrEmpty(currentRouteData.Values["action"].ToString()))
                {
                    currentAction = currentRouteData.Values["action"].ToString();
                }
            }

            Exception exception = Server.GetLastError();

            var requestUrl = httpContext.Request.Url.ToStr();

            String requestUrlReferrer = "";
            if (httpContext.Request.UrlReferrer != null)
            {
                requestUrlReferrer = httpContext.Request.UrlReferrer.ToStr();
            }

            //We check if we have an AJAX request and return JSON in this case
            if (IsAjaxRequest())
            {
                Logger.Error("requestUrlReferrer: " + requestUrlReferrer + " requestUrl: " + requestUrl + " Controller:" + currentController + " currentAction:" + currentAction + " Ajax Request Error:" + exception.StackTrace, exception);
            }
            else
            {
                Logger.Error("requestUrlReferrer: " + requestUrlReferrer + " requestUrl: " + requestUrl + "  Controller:" + currentController + " Action:" + currentAction + " Application Error:" + exception.StackTrace, exception);

                var controller = new ErrorController();
                var routeData = new RouteData();
                var action = "Index";


                httpContext.ClearError();
                httpContext.Response.Clear();
                httpContext.Response.StatusCode = exception is HttpException ? ((HttpException)exception).GetHttpCode() : 500;
                httpContext.Response.TrySkipIisCustomErrors = true;

                routeData.Values["controller"] = "Error";
                routeData.Values["action"] = action;

                controller.ViewData.Model = new HandleErrorInfo(exception, currentController, currentAction);
                ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
            }

        }

        //This method checks if we have an AJAX request or not
        private bool IsAjaxRequest()
        {
            //The easy way
            bool isAjaxRequest = (Request["X-Requested-With"] == "XMLHttpRequest")
            || ((Request.Headers != null)
            && (Request.Headers["X-Requested-With"] == "XMLHttpRequest"));

            //If we are not sure that we have an AJAX request or that we have to return JSON 
            //we fall back to Reflection
            if (!isAjaxRequest)
            {
                try
                {
                    //The controller and action
                    string controllerName = Request.RequestContext.
                                            RouteData.Values["controller"].ToString();
                    string actionName = Request.RequestContext.
                                        RouteData.Values["action"].ToString();

                    //We create a controller instance
                    DefaultControllerFactory controllerFactory = new DefaultControllerFactory();
                    Controller controller = controllerFactory.CreateController(
                    Request.RequestContext, controllerName) as Controller;

                    //We get the controller actions
                    ReflectedControllerDescriptor controllerDescriptor =
                    new ReflectedControllerDescriptor(controller.GetType());
                    ActionDescriptor[] controllerActions =
                    controllerDescriptor.GetCanonicalActions();

                    //We search for our action
                    foreach (ReflectedActionDescriptor actionDescriptor in controllerActions)
                    {
                        if (actionDescriptor.ActionName.ToUpper().Equals(actionName.ToUpper()))
                        {
                            //If the action returns JsonResult then we have an AJAX request
                            if (actionDescriptor.MethodInfo.ReturnType == typeof(JsonResult))
                                return true;
                        }
                    }
                }
                catch
                {

                }
            }

            return isAjaxRequest;
        }

    }
}