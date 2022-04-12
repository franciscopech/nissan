using FranciscoPech;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using NissanDemo.Models.Attributes;
using NissanDemo.Models.Objects;

namespace NissanDemo.Controllers
{
    [Route("account")]
    public class AccountPages : Controller
    {
        [Route("create")]
        [MethodRestriction(RestrictionType.LoginRequired, "/")]
        public IActionResult CreateAcccount()
        {
            return View();
        }

        [Route("details")]
        [MethodRestriction(RestrictionType.LoginRequired, "/")]
        async public Task<ActionResult> Details(int account)
        {
            Models.Objects.User usr = JsonManager.GetCurrentUser(Request);
            if (usr != null)
            {
                Request<Account> acc = await Account.GetAccount(account, usr.Id);
                if(acc.Result != null)
                    return View(acc.Result);
                else
                    return Redirect("/");
            }
            else
                return Redirect("/");
        }
    }
}
