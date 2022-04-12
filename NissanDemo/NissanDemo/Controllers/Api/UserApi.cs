using FranciscoPech;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using NissanDemo.Models.Attributes;
using NissanDemo.Models.Objects;

namespace NissanDemo.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/user")]
    [ApiController]
    public class UserApi : ControllerBase
    {
        [HttpPost("login")]
        public async Task<Status> Login()
        {
            try
            {
                var jsn = await JsonManager.GetJsonPost(Request);
                Request<User> lgn = await Models.Objects.User.Login(Convert.ToString(jsn.email), Convert.ToString(jsn.password));
                string susr = JsonManager.Serialize(lgn.Result);
                //HttpContext.Items["CurrentSession"] = lgn.u Models.Objects.User;
                HttpContext.Session.SetString("User", susr);
                return lgn.RequestStatus;
            }
            catch (Exception ex)
            {
                return Status.Error(ex.Message);
            }
        }

        [HttpPost("CreateUser")]
        public async Task<Status> CreateUser(string email, string password)
        {
            try
            {
                var jsn = await JsonManager.GetJsonPost(Request);
                Models.Objects.User usr = new User(-1, Convert.ToString(jsn.FirstName), Convert.ToString(jsn.LastName), 
                                                    Convert.ToString(jsn.Identification),Convert.ToString(jsn.Email))
                                                    { Password = Convert.ToString(jsn.Password) };
                return await usr.Save();
            }
            catch (Exception ex)
            {
                return Status.Error(ex.Message);
            }
        }
        [HttpPost("ErrorApiCredentials")]
        public Status ErrorApiCredentials(string email, string password)
        {
            return Status.Error("Requieres autenticarte");
        }
    }
}