using ELibrary_Web_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ELibrary_Web_Service.Controllers
{
    public class NewsFeedController : ApiController
    {
        // GET: api/NewsFeed
        public IEnumerable<Feed> Get()
        {
            return new LibraryContext()?.NewsFeeds.ToList();
        }

        // GET: api/NewsFeed/5
        public Feed Get(int id)
        {
            return new LibraryContext()?.NewsFeeds?.Where(x => x.FeedId == id).FirstOrDefault();
        }

        // POST: api/NewsFeed
        public HttpResponseMessage Post([FromBody]string value)
        {
            return  new HttpResponseMessage(HttpStatusCode.NotImplemented);
        }

        // PUT: api/NewsFeed/5
        public HttpResponseMessage Put(int id, [FromBody]string value)
        {
            return new HttpResponseMessage(HttpStatusCode.NotImplemented);
        }

        // DELETE: api/NewsFeed/5
        public HttpResponseMessage Delete(int id)
        {
            return new HttpResponseMessage(HttpStatusCode.NotImplemented);
        }
    }
}
