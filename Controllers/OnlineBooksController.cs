using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ELibrary_Web_Service.Controllers
{
    public class OnlineBooksController : ApiController
    {
        // GET: api/OnlineBooks
        public object Get(string query, string location)
        {
            var mapApi = "https://maps.googleapis.com/maps/api/place/textsearch/json?";
            var radius = "&radius=10000";
            var key = "&key=AIzaSyCzH1X2hQEtT19vvxqTfsL69D0Gu532wC8";
            query = query + "+Book";

            var Url = mapApi + "query=" + query.Replace(' ','+') + "&location=" + location + radius + key;

            var request = WebRequest.Create(Url);
            request.Method = "GET";
            var response = (HttpWebResponse)request.GetResponse();

            string result = null;

            using (Stream stream = response.GetResponseStream())
            {
                var reader = new StreamReader(stream);
                result = reader.ReadToEnd();
                reader.Close();
            }

            return JObject.Parse(result);
        }
    }
}
