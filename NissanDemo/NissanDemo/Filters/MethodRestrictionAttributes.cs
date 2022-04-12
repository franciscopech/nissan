using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace NissanDemo.Models.Attributes
{
    public enum RestrictionType
    {
        IsLoginPage = 0,
        LoginRequired = 1,
    }
    public class MethodRestrictionAttribute:Attribute, IActionFilter
    {
        public RestrictionType Restriction { get; set; }
        public string UrlRedirection { get; set; }

        public MethodRestrictionAttribute(RestrictionType restriction, string urlrediretion=null)
        {
            Restriction = restriction;
            UrlRedirection = urlrediretion;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string SUser = context.HttpContext.Session.GetString("User");
            if(SUser != null)
            {
                Models.Objects.User OUser = Newtonsoft.Json.JsonConvert.DeserializeObject< Models.Objects.User >(SUser);
                if (Restriction == RestrictionType.IsLoginPage && OUser != null)
                {
                    context.HttpContext.Response.Redirect(UrlRedirection);
                }
            }
            else
            {
                if (Restriction == RestrictionType.LoginRequired)
                {
                    context.HttpContext.Response.Redirect(UrlRedirection);
                }
            }
        }
    }
}
