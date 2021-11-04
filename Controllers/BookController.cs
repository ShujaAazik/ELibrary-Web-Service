using ELibrary_Web_Service.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ELibrary_Web_Service.Controllers
{
    public class BookController : ApiController
    {
        // GET: api/Book
        public IEnumerable<object> Get()
        {
            return new LibraryContext()?.Books?.Where(x=>x.Shared == true)?.Select(y=> new { BookId = y.BookId, Title = y.Title, Author = y.Author });
        }

        // GET: api/Book/5
        public HttpResponseMessage Get(int id)
        {
            string filename = null;

            using (var db = new LibraryContext())
            {
                filename = db.Books.Where(x => x.BookId == id).Select(y => y.BookName).FirstOrDefault();
            }

            if(filename == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            var path = HttpContext.Current.Server.MapPath($"~/App_Data/{filename}");

            var FBytes = File.ReadAllBytes(path);

            var fMemStream = new MemoryStream(FBytes);

            var response = new HttpResponseMessage(HttpStatusCode.OK);

            response.Content = new StreamContent(fMemStream);

            var header = response.Content.Headers;

            header.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            header.ContentDisposition.FileName = filename;
            header.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            header.ContentLength = fMemStream.Length;

            return response;
        }
        
        // GET: api/Book/title
        public object Get(string title)
        {
            return new LibraryContext().Books.Where(x => x.Title == title).Select(y => new { BookId = y.BookId, Title = y.Title, Author = y.Author }).FirstOrDefault();
        }

        public IEnumerable<Book> Post([FromBody]string userId)
        {
            var id = Int32.Parse(userId);

            var context = new LibraryContext();

            var user = context.Users.Where(x => x.UserId == id).FirstOrDefault();

            return new LibraryContext().Books.Where(y => y.User == user).ToList();
        }

        // POST: api/Book
        public async Task<HttpResponseMessage> Post([FromBody]object bookObject)
        {
            if(bookObject == null || !typeof(Book).Equals(bookObject.GetType()))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            
            return await UploadFile((Book)bookObject);
        }

        // PUT: api/Book/5
        public HttpResponseMessage Put(int id, [FromBody]string value)
        {
            var shared = Convert.ToBoolean(value);

            using (var db = new LibraryContext())
            {
                var bookContext = db.Books?.Where(x => x.BookId == id).FirstOrDefault();

                if (bookContext != null)
                {
                    bookContext.Shared = shared;
                    db.SaveChanges();
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }
            }
        }

        // DELETE: api/Book/5
        public HttpResponseMessage Delete(int id)
        {
            using (var db = new LibraryContext())
            {
                var book = db?.Books?.Where(x => x.BookId == id).FirstOrDefault();

                try
                {
                    if (book != null)
                    {
                        db.Books.Remove(book);
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

        private async Task<HttpResponseMessage> UploadFile(Book book)
        {
            var context = HttpContext.Current.Server.MapPath("~/App_Data");
            var StreamProvider = new MultipartFormDataStreamProvider(context);

            try
            {
                await Request.Content
                    .ReadAsMultipartAsync(StreamProvider);

                foreach (var file in StreamProvider.FileData)
                {
                    var fileName = file.Headers
                        .ContentDisposition
                        .FileName;

                    // remove double quotes from string.
                    fileName = fileName.Trim('"');

                    var localFileName = file.LocalFileName;
                    var filePath = Path.Combine(context, fileName);

                    File.Move(localFileName, filePath);

                    book.BookName = fileName;

                    UploadBooktoDB(book);
                }
            }
            catch (Exception e)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        private void UploadBooktoDB(Book book)
        {

            try
            {
                using (var db = new LibraryContext())
                {
                    db.Books.Add(book);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
