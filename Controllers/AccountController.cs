using ELibrary_Web_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ELibrary_Web_Service.Controllers
{
    public class AccountController : ApiController
    {
        // GET: api/Account
        public IEnumerable<Person> Get()
        {
            return new LibraryContext().Users.ToList();
        }

        // GET: api/Account/5
        public Person Get(int id)
        {
            return new LibraryContext()?.Users?.Where(x => x.UserId == id).FirstOrDefault();
        }

        // POST: api/Account
        public HttpResponseMessage Post([FromBody]Person person)
        {
            if (person== null || !typeof(Person).Equals(person.GetType()))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            try
            {
                using (var db = new LibraryContext())
                {
                    db.Users.Add(person);
                    db.SaveChanges();
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
            }
            catch (Exception)
            {

                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // PUT: api/Account/5
        public HttpResponseMessage Put(int id, [FromBody]object person)
        {
            if (person == null || !typeof(Person).Equals(person.GetType()))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            var personData = (Person)person;

            using (var db = new LibraryContext())
            {
                var personContext = db.Users?.Where(x => x.UserId == id).FirstOrDefault();

                if (personContext != null)
                {
                    personContext.UserName = personData.UserName;
                    personContext.Email = personData.Email;
                    personContext.Password = personData.Password;
                    db.SaveChanges();
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }
            }
        }

        // DELETE: api/Account/5
        public HttpResponseMessage Delete(int id)
        {
            using (var db = new LibraryContext())
            {
                var person = db?.Users?.Where(x => x.UserId == id).FirstOrDefault();

                try
                {
                    if (person != null)
                    {
                        db.Users.Remove(person);
                        db.SaveChanges();
                        return new HttpResponseMessage(HttpStatusCode.OK);
                    }
                    else
                    {
                        return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    }
                }
                catch (Exception)
                {

                    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }
            }
        }
    }
}
