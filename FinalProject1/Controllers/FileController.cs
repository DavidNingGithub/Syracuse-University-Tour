using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using FinalProject1.Models;
namespace FinalProject1.Controllers
{
    public class FileController : ApiController
    {
        private FinalProject1Context db = new FinalProject1Context();
        private UserManager<ApplicationUser> check = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        // GET api/file
        public IEnumerable<string> Get()
        {
            // available files
            //string path = System.Web.HttpContext.Current.Server.MapPath("~\\App_Data");
            string path = System.Web.HttpContext.Current.Server.MapPath("~\\Content\\image");

            string[] files = Directory.GetFiles(path);
            for (int i = 0; i < files.Length; ++i)
                files[i] = Path.GetFileName(files[i]);
            return files;
        }

        // GET api/file/5
        public HttpResponseMessage Get(string fileName, string open)
        {
            string sessionId;
            var response = new HttpResponseMessage();
            Models.Session session = new Models.Session();

            CookieHeaderValue cookie = Request.Headers.GetCookies("session-id").FirstOrDefault();
            if (cookie == null)
            {
                sessionId = session.incrSessionId();
                cookie = new CookieHeaderValue("session-id", sessionId);
                cookie.Expires = DateTimeOffset.Now.AddDays(1);
                cookie.Domain = Request.RequestUri.Host;
                cookie.Path = "/";
            }
            else
            {
                sessionId = cookie["session-id"].Value;
            }
            try
            {
                string path = System.Web.HttpContext.Current.Server.MapPath("~\\Content\\image");
                string currentFileSpec = path + "\\" + fileName;
                FileStream down;
                if (open == "true")  // attempt to open requested fileName
                {
                    down = new FileStream(currentFileSpec, FileMode.Create);
                    session.saveStream(down, sessionId);
                }
                else  // close FileStream
                {
                    down = session.getStream(sessionId);
                    down.Close();
                }
                response.StatusCode = (HttpStatusCode)200;
            }
            catch
            {
                response.StatusCode = (HttpStatusCode)400;
            }
          finally  // return cookie to save current sessionId
            {
                response.Headers.AddCookies(new CookieHeaderValue[] { cookie });
            }
            return response;
        }

        // POST api/file
        public void Post(HttpRequestMessage request, string FileName, string description, string siteName,string AddOrNot)
        {
            Task<byte[]> taskb = request.Content.ReadAsByteArrayAsync();
            byte[] Block = taskb.Result;
            Models.Session session = new Models.Session();
            string sessionId = session.getSessionId();
            FileStream down = session.getStream(sessionId);//save it into the session,get it from the 
            int blockSize = 512;
            if (Block.Length == 0 || blockSize <= 0)
                return;
            down.Write(Block, 0, Block.Length);
            if(Block.Length<512&&AddOrNot=="true")
            {
                Models.Site NewData = new Models.Site();
                NewData.Description = description;
                NewData.Image = FileName;
                NewData.SiteName = siteName;
                db.Sites.Add(NewData);
                db.SaveChanges();         
            }

        }

        // PUT api/file/5
        public string Put(string UserName, string Password)
        {
            ApplicationUser user = check.Find(UserName, Password);
            var store = new UserStore<FinalProject1.Models.ApplicationUser>(new ApplicationDbContext());
            var userMananger = new UserManager<ApplicationUser>(store);
            if(user==null)
            {
                string fail="false";
                return fail;
            }
            else
            {               
                string success = "true";
                if(!userMananger.IsInRole(user.Id,"admin"))
                {
                    success = "false";
                }
                return success;
            }
            
        }

        // DELETE api/file/5
        public void Delete(string filePath, string SimplyAddOrNot)
        {
            string path = System.Web.HttpContext.Current.Server.MapPath("~\\Content\\image");
            string currentFileSpec = path + "\\" + filePath;
            File.Delete(currentFileSpec);
            if(SimplyAddOrNot=="false")
            {
                Models.Site test = db.Sites.First(Site => Site.Image == filePath);
                db.Sites.Remove(test);
                db.SaveChanges();               
            }
           
        }
    }
}
