using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Ahlam.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Data.Entity;

namespace Ahlam.Controllers
{
    [Authorize]
    [RoutePrefix("api/Core")]
    public class CoreController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public CoreController() { }

        [AllowAnonymous]
        //POST /api/Core/Upload 
        [Route("Upload")]
        public async Task<IHttpActionResult> SaveFile()
        {
            var path = Path.GetTempPath();

            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
            }

            MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(path);

            await Request.Content.ReadAsMultipartAsync(streamProvider);

            foreach (MultipartFileData fileData in streamProvider.FileData)
            {
                string fileName = "";
                if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
                {
                    fileName = Guid.NewGuid().ToString();
                }
                fileName = fileData.Headers.ContentDisposition.FileName;
                if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                {
                    fileName = fileName.Trim('"');
                }
                if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                {
                    fileName = Path.GetFileName(fileName);
                }

                var newFileName = Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data/"), fileName);
                var fileInfo = new FileInfo(newFileName);
                if (fileInfo.Exists)
                {
                    fileName = fileInfo.Name.Replace(fileInfo.Extension, "");
                    fileName = fileName + (new Random().Next(0, 10000)) + fileInfo.Extension;

                    newFileName = Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data/"), fileName);
                }

                if (!Directory.Exists(fileInfo.Directory.FullName))
                {
                    Directory.CreateDirectory(fileInfo.Directory.FullName);
                }

                Attachment attach = new Attachment();
                attach.CreationDate = DateTime.Now;
                attach.LastModificationDate = DateTime.Now;
                attach.FileName = fileInfo.Name;
                attach.extension = fileInfo.Extension;
                attach.Link = newFileName;
                //attach.Creator = getCurrentUser().UserName;
                //attach.Modifier = getCurrentUser().UserName;

                db.Attachments.Add(attach);
                db.SaveChanges();


                File.Move(fileData.LocalFileName, newFileName);

                return Json(attach);
            }

            return BadRequest();
        }



        [AllowAnonymous]
        //GET /api/Core/Download?id=4
        [Route("Download")]
        public HttpResponseMessage GetFile(int id)
        {
            Attachment attach = db.Attachments.Where(a => a.id.Equals(id)).FirstOrDefault();
            if (attach == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            String fileName = attach.FileName;

            //Create HTTP Response.
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            //Set the File Path.
            string filePath = HttpContext.Current.Server.MapPath("~/App_Data/") + fileName;

            //Check whether File exists.
            if (!File.Exists(filePath))
            {
                //Throw 404 (Not Found) exception if File not found.
                response.StatusCode = HttpStatusCode.NotFound;
                response.ReasonPhrase = string.Format("File not found: {0} .", fileName);
                throw new HttpResponseException(response);
            }

            //Read the File into a Byte Array.
            byte[] bytes = File.ReadAllBytes(filePath);

            //Set the Response Content.
            response.Content = new ByteArrayContent(bytes);

            //Set the Response Content Length.
            response.Content.Headers.ContentLength = bytes.LongLength;

            //Set the Content Disposition Header Value and FileName.
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = fileName;

            //Set the File Content Type.
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(fileName));
            return response;
        }

        public async Task<ApplicationUser> GetCurrentUserAsync (String phoneNumber)
        {
            //var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            //return  userManager.FindById(User.Identity.GetUserId());
            
            
                RegisterBindingModel model = new RegisterBindingModel();
                model.Name = phoneNumber;
                model.Type = "Client";
                model.Status = "Active";
                model.Username = "FireBase" + DateTime.Now.ToString();
                model.Email = "Firebase" + DateTime.Now.ToString() + "@gmail.com";
                model.Password = "1234567";
                model.ConfirmPassword = "1234567";
                model.PictureId = "1";
                model.PhoneNumber = phoneNumber;
                return await createNewUserAsync(model);          
            
        }
        public async Task<ApplicationUser> createNewUserAsync(RegisterBindingModel model)
        {

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = new ApplicationUser() { UserName = model.Username, Email = model.Email };
            user.Name = model.Name;
            user.Type = model.Type;
            user.Status = "Active";
            user.CreationDate = DateTime.Now;
            user.LastModificationDate = DateTime.Now;
            user.PictureId = model.PictureId;
            if (model.PhoneNumber != null)
                user.PhoneNumber = model.PhoneNumber;


            IdentityResult result = await userManager.CreateAsync(user, model.Password);
            db.SaveChanges();

            return user;
        }

        public Task<IHttpActionResult> throwExcetpion(String message)
        {

            var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(message),
                ReasonPhrase = message
            };
            throw new HttpResponseException(resp);
        }

        //[AllowAnonymous]
        ////GET /api/Core/GetParameter?id=4
        //[Route("GetParameter")]
        //public async Task<IHttpActionResult> GetParameter(String code)
        //{
        //    SystemParameter parm = db.SystemParameters.Where(a => a.Code.Equals(code)).FirstOrDefault();
        //    if (parm == null)
        //        throwExcetpion("No matching Parameter!");
        //    return Ok(parm);
        //}

        //public void ChangeOnlineStatus(ApplicationUser user, bool online)
        //{
        //    user.Online = online;
        //    //db.Entry(user).State = EntityState.Modified;
        //   // db.SaveChanges();
        //}
    }
}
