using BellProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
//using System.Data;
//using System.Data.EntityClient.EntityClientConnection;
using System.Data.Entity.Core.EntityClient;
using System.Data;

namespace BellProject.Controllers
{
    public class HomeController : Controller
    {
        private BellSupportEntities db = new BellSupportEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult DashboardCount()
        {
            var connectionString = @"data source = (LocalDB)\MSSQLLocalDB; attachdbfilename = C:\Users\Home\BellSupport.mdf; integrated security = True";

            try   
            {
                string[] DashBoardcount = new string[2];
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("Select count(project_id) as Internet, (Select count(project_id) from Request where project_id=2 ) as Technology from Request where project_id=1", con);
                DataTable dt = new DataTable();
                SqlDataAdapter cmd1 = new SqlDataAdapter(cmd);

                cmd1.Fill(dt);
                if(dt.Rows.Count == 0)
                {
                    DashBoardcount[0] = "0";
                    DashBoardcount[1] = "0";
                }
                else
                {
                    DashBoardcount[0] = dt.Rows[0]["Internet"].ToString();
                    DashBoardcount[1] = dt.Rows[0]["Technology"].ToString();

                }

                return Json(new { DashBoardcount }, JsonRequestBehavior.AllowGet);

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
