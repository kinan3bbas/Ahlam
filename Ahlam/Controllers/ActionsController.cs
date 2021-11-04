using Ahlam.Bindings;
using Ahlam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ahlam.Controllers
{
    [RoutePrefix("api/actions")]
    public class ActionsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private CoreController core = new CoreController();


        [Route("AddDream")]
        [HttpPost]
        public Dream AddDream(DreamBinding model)
        {
            Dream dream = new Dream();
            dream.PhoneNumber = model.PhoneNumber;
            dream.ServicePathId = model.ServicePathId;
            dream.CreationDate = DateTime.Now;
            dream.LastModificationDate = DateTime.Now;
            ApplicationUser userTemp = db.Users.Where(a => a.PhoneNumber.Equals(model.PhoneNumber)).FirstOrDefault();
            if (userTemp == null)
            {
                dream.Creator = core.GetCurrentUserAsync(model.PhoneNumber).Result;
            }
            else
            {
                dream.CreatorId = userTemp.Id;
            }
            

             

            //dream.CreatorId = core.GetCurrentUserAsync(model.PhoneNumber).Result.Id;
            dream.Status = "Active";
            dream.Description = model.Description;
            //Dream.Creator = core.getCurrentUser();
            //Dream.Modifier = core.getCurrentUser();
            dream.Payment = AddPayment(model);
            db.Dreams.Add(dream);
            db.SaveChanges();

            return dream;
        }
        private Payment AddPayment(DreamBinding temp)
        {
            Payment payment = new Payment();
            payment.Method = temp.Method;
            payment.Status = "Done";
            payment.Currency = temp.Currency;
            payment.Amount = temp.Amount;
            //payment.ServiceProviderId = service.ServiceProviderId;
            //payment.CreatorId = creatorId;
            ///payment.ModifierId = creatorId;
            payment.CreationDate = DateTime.Now;
            payment.LastModificationDate = DateTime.Now;
            db.Payments.Add(payment);
            return payment;
        }


        [AllowAnonymous]
        [Route("UserInfo")]
        [HttpGet]
        public UserInfoViewModel GetUserInfo(string phoneNumber)
        {
            ApplicationUser user = core.GetCurrentUserAsync(phoneNumber).Result;
            //ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                id = user.Id,
                PhoneNumber = user.PhoneNumber

            };
        }

    }
}
