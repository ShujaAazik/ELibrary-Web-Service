using ELibrary_Web_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ELibrary_Web_Service.Controllers
{
    public class LoginController : ApiController
    {

        // POST: api/Login
        public Person Post([FromBody]string value)
        {
            var valuePairs = value.Split(':');

            if(valuePairs != null && valuePairs.Length > 1)
            {
                var userName = valuePairs[0];
                var password = valuePairs[1];

                return new LibraryContext()?.Users?.Where(x => x.UserName == userName && x.Password == password).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }
    }
}
