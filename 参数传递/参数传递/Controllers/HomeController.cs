using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace 参数传递.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
           

            return View();
        }

        public ActionResult Edit(string password,string username)
        {
            //string username = Request.QueryString["UserName"];
            //string username = Request.Form["UserName"];
            Response.Write("UserName:" + username+ password);

            return View();
        }
    }
}