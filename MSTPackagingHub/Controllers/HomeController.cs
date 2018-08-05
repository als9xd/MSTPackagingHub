using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MSTPackagingHub.Services;
using MSTPackagingHub.Interfaces;

namespace MSTPackagingHub.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Audit()
        {
            ViewBag.ActiveTab = "Audit";
            return View();
        }

        public ActionResult Regex()
        {
            ViewBag.ActiveTab = "Regex";
            return View();
        }

    }
}