using Microsoft.AspNetCore.Mvc;
using NissanDemo.Models.Attributes;

namespace NissanDemo.Controllers
{
    [Route("transaction")]
    public class TransactionsPages : Controller
    {
        [Route("Deposit")]
        [MethodRestriction(RestrictionType.LoginRequired, "/")]
        public ActionResult Deposit()
        {
            return View();
        }
        [Route("Withdrawal")]
        [MethodRestriction(RestrictionType.LoginRequired, "/")]
        public ActionResult Withdrawal()
        {
            return View();
        }
    }
}
