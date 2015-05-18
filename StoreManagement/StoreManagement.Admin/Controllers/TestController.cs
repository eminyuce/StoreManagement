using RazorEngine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.IO;
using System.Web.Mvc;
using StoreManagement.Data.Entities;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;
using StoreManagement.Data.GeneralHelper;
using Ninject;

namespace StoreManagement.Admin.Controllers
{
    public class TestController : BaseController
    {
        [Inject]
        public IContentRepository ProductRepository { set; get; }
         //
        // GET: /Setting/
        public TestController(IStoreContext dbContext, ISettingRepository settingRepository)
            : base(dbContext, settingRepository)
        {

        }

        public ActionResult TestJquery()
        {
            return View();
        }
        public ActionResult DetailRazorPage()
        {
            var pr = ProductRepository.GetSingle(1);
            var template = System.IO.File.ReadAllText(Server.MapPath("~/Views/Products/Details2.cshtml"));
            String body = Razor.Parse(template, pr);
            ViewBag.BodyHtml = body;


            return View();
        }
             
            //
        // GET: /Test/
        public ActionResult Index()
        {
            //var connectionString = WebConfigurationManager.ConnectionStrings["Stores"].ConnectionString;
            //ViewBag.ConnectionString = connectionString;
            //try
            //{
            //    deleteCategories();
            //}
            //catch (Exception ex)
            //{

            //    throw new Exception("ConnectionString:" + connectionString, ex);
            //}



            return View();
        }
        public Boolean deleteCategories()
        {
            using (SqlConnection connect = new SqlConnection(WebConfigurationManager.ConnectionStrings["Stores"].ConnectionString))
            {
                SqlCommand command = new SqlCommand();

                connect.Open();
                command.CommandType = CommandType.Text;
                command.CommandText = @"select * From Categories";
                command.Connection = connect;

                int affectedRowNumber = command.ExecuteNonQuery();
                connect.Close();
                return true;
            }
        }
	}
}