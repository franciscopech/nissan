using FranciscoPech;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NissanDemo.Models.Objects;

namespace FranciscoPech
{
    public class JsonManager
    {
        async static public Task<dynamic> GetJsonPost(HttpRequest request)
        {
            using (var Reader = new StreamReader(request.Body, Encoding.UTF8))
            {
                string str = await Reader.ReadToEndAsync();
                return Newtonsoft.Json.JsonConvert.DeserializeObject(str);
            }
        }
        static public User GetCurrentUser(HttpRequest request)
        {
            string SUser = request.HttpContext.Session.GetString("User");
            if(SUser !=null)
                return Newtonsoft.Json.JsonConvert.DeserializeObject<User>(SUser);
            else
                return null;
        }
        static public Request<T> GetObject<T>(Request<dynamic> obj)
        {
            T ret = default;
            if (obj.OK)
            {
                var aux = Newtonsoft.Json.JsonConvert.SerializeObject(obj.Result);
                ret = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(aux);
            }
            return new Request<T>(obj.RequestStatus, ret);
        }
        static public T GetObject<T>(dynamic obj)
        {
            T ret = default(T);
            if (obj != null)
            {
                var aux = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                ret = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(aux);
            }
            return ret;
        }
        static public T ParseObject<T>(object obj)
        {
            T ret = default(T);
            if (obj != null)
            {
                var aux = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                ret = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(aux);
            }
            return ret;
        }
        static public string Serialize(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
    }
}
