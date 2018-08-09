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

        public ActionResult Analytics()
        {
            ViewBag.ActiveTab = "Analytics";
            return View();
        }

        private string[][] InstallMonkeyDocs =
        {
            new[] { "markdown", "Tutorial", System.IO.File.ReadAllText(@"C:\Users\t-als9xd\source\repos\MSTPackagingHub\MSTPackagingHub\InstallMonkeyDocs\InstallMonkeyTutorial.md") },
            new[] { "perl", "Template", System.IO.File.ReadAllText(@"C:\Users\t-als9xd\source\repos\MSTPackagingHub\MSTPackagingHub\InstallMonkeyDocs\InstallMonkeyTemplate.pl") },
            new[] { "markdown", "Documentation", System.IO.File.ReadAllText(@"C:\Users\t-als9xd\source\repos\MSTPackagingHub\MSTPackagingHub\InstallMonkeyDocs\InstallMonkey.md") }
        };

        public ActionResult InstallMonkey(int id = 0)
        {

            ViewBag.DocType = InstallMonkeyDocs[id][0];
            ViewBag.ActiveTab = InstallMonkeyDocs[id][1];
            ViewBag.Document = InstallMonkeyDocs[id][2];
            return View();
        }

        public ActionResult Tools(int id = 0)
        {
            ViewBag.ActiveTab = "Tools";
            ViewBag.SCCMAddPackageDoc = System.IO.File.ReadAllText(@"C:\Users\t-als9xd\source\repos\MSTPackagingHub\MSTPackagingHub\ToolsDocs\SCCMAddPackage.md");
            ViewBag.AutoITDoc = System.IO.File.ReadAllText(@"C:\Users\t-als9xd\source\repos\MSTPackagingHub\MSTPackagingHub\ToolsDocs\AutoIT.md");
            ViewBag.PSToolsDoc = System.IO.File.ReadAllText(@"C:\Users\t-als9xd\source\repos\MSTPackagingHub\MSTPackagingHub\ToolsDocs\PSTools.md");
            ViewBag.WTGDoc = System.IO.File.ReadAllText(@"C:\Users\t-als9xd\source\repos\MSTPackagingHub\MSTPackagingHub\ToolsDocs\WTG.md");
            return View();
        }

        public ActionResult Links()
        {
            ViewBag.ActiveTab = "Links";
            return View();
        }
    }
}