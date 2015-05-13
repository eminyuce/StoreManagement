using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace StoreManagement.Admin.Controllers
{
    public class TestController : Controller
    {

        public ActionResult TestJquery()
        {
            return View();
        }
            
            //
        // GET: /Test/
        public ActionResult Index()
        {
            var connectionString = WebConfigurationManager.ConnectionStrings["Stores"].ConnectionString;
            ViewBag.ConnectionString = connectionString;
            try
            {
                deleteCategories();
            }
            catch (Exception ex)
            {

                throw new Exception("ConnectionString:" + connectionString, ex);
            }



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