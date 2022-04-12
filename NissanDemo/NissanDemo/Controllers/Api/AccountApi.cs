using FranciscoPech;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NissanDemo.Models.Objects;

namespace NissanDemo.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/account")]
    [ApiController]
    public class AccountApi : ControllerBase
    {
        [HttpPost("create-account")]
        public async Task<Status> CreateAccount()
        {
            try
            {
                var jsn = await JsonManager.GetJsonPost(Request);
                Models.Objects.User user = JsonManager.GetCurrentUser(Request);

                Account acc = new Account(0,Convert.ToInt32(user.Id),Convert.ToString( jsn.Name),Convert.ToDecimal(jsn.Balance));
                Status st = await acc.Save();
                return st;
            }
            catch (Exception ex)
            {
                return Status.Error(ex.Message);
            }
        }
        [HttpPost("get-accounts")]
        public async Task<Request<List<Account>>> GetUserCounts()
        {
            try
            {
                var jsn = await JsonManager.GetJsonPost(Request);
                Models.Objects.User user = JsonManager.GetCurrentUser(Request);
                return await Account.GetUserAccounts(Convert.ToInt32(user.Id));
            }
            catch (Exception ex)
            {
                return new Request<List<Account>>(Status.Error(ex.Message), null);
            }
        }
    }
}
