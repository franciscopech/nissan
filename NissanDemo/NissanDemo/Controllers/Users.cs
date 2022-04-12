using FranciscoPech;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using NissanDemo.Models.Attributes;

namespace NissanDemo.Controllers
{
    public class Users : Controller
    {
        [MethodRestriction(RestrictionType.LoginRequired, "/")]
        public IActionResult Index()
        {
            return View();
        }

        [MethodRestriction(RestrictionType.IsLoginPage, "/Users/Index")]
        public ActionResult LoginPage()
        {
            return View();
        }

        public ActionResult CreateUser()
        {
            return View();
        }
    }
}
