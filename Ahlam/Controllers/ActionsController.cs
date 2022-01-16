using Ahlam.Bindings;
using Ahlam.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        [Route("GetUserToken")]
        [HttpGet]
        public UsersDeviceTokens GetUserToken(string phoneNumber)
        {
            //ApplicationUser user = core.GetCurrentUserAsync(phoneNumber).Result;
            //ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);
            return db.UsersDeviceTokens.Where(a => a.PhoneNumber.Equals(phoneNumber)).FirstOrDefault();

            //return new UserInfoViewModel
            //{
            //    id = user.Id,
            //    PhoneNumber = user.PhoneNumber

            //};
        }

        [Route("ReceiveDream")]
        [HttpGet]
        public DreamExplanation ReceiveDream(int dreamId, String interpreterId)
        {
            Dream dream = db.Dreams.Where(a => a.id.Equals(dreamId)).Include(a => a.ServicePath).FirstOrDefault();
            if (dream == null)
            {
                core.throwExcetpion("No matching dream!!!");
            }
            if (dream.ServicePath == null)
            {
                core.throwExcetpion("No plan related to the dream!!!");
            }
            int DreamsExplanationCount = db.DreamExplanations.Where(a => a.DreamId.Equals(dreamId)).Count();

            if (DreamsExplanationCount >= dream.ServicePath.NumberOfInterpreters)
            {
                core.throwExcetpion("You can't receive the dream, the plan reached to the limit!!!");
            }
            var tempInterpreter = db.DreamExplanations.Where(a => a.InterpreterId.Equals(interpreterId) && a.DreamId.Equals(dreamId)).FirstOrDefault();

            if (tempInterpreter != null)
            {
                core.throwExcetpion("You can't receive the dream, you received it before!!!");
            }

            if (DreamsExplanationCount == (dream.ServicePath.NumberOfInterpreters-1))
            {
                dream.Status = "Under_Interpretation";
            }


            DreamExplanation explanation = new DreamExplanation();
            explanation.InterpreterId = interpreterId;
            explanation.CreationDate = DateTime.Now;
            explanation.DreamId = dreamId;
            explanation.CreatorId = dream.CreatorId;
            explanation.LastModificationDate = DateTime.Now;
            explanation.Status = "Active";
            explanation.ExplanationDate = DateTime.Now;
            db.Entry(dream).State = EntityState.Modified;
            db.DreamExplanations.Add(explanation);
            db.SaveChanges();
            return explanation;

        }


        [Route("FinishDream")]
        [HttpGet]
        public DreamExplanation FinishDream(int dreamId, String interpreterId)
        {
            Dream dream = db.Dreams.Where(a => a.id.Equals(dreamId)).Include(a => a.ServicePath).FirstOrDefault();
            if (dream == null)
            {
                core.throwExcetpion("No matching dream!!!");
            }
            int DreamsExplanationCount = db.DreamExplanations.Where(a => a.DreamId.Equals(dreamId) && a.Status.Equals("Done")).Count();

            if (DreamsExplanationCount == dream.ServicePath.NumberOfInterpreters-1)
            {
                dream.Status = "Done";
                dream.LastModificationDate = DateTime.Now;
            }
            var temp = db.DreamExplanations.Where(a => a.InterpreterId.Equals(interpreterId) && a.DreamId.Equals(dreamId)).FirstOrDefault();

            if (temp == null)
            {
                core.throwExcetpion("The interpreter didn't receive the dream before!!!");

            }

            temp.LastModificationDate = DateTime.Now;
            temp.Status = "Done";
            temp.ExplanationDate = DateTime.Now;

            db.Entry(temp).State = EntityState.Modified;
            db.Entry(dream).State = EntityState.Modified;

            db.SaveChanges();

            return temp;

        }

        [Route("GetDreamInterpreters")]
        [HttpGet]
        public List<ApplicationUser> GetDreamInterpreters(int dreamId)
        {
            Dream dream = db.Dreams.Where(a => a.id.Equals(dreamId)).Include(a => a.ServicePath).FirstOrDefault();
            if (dream == null)
            {
                core.throwExcetpion("No matching dream!!!");
            }
            return db.DreamExplanations.Where(a => a.DreamId.Equals(dreamId)).Select(a => a.Interpreter).ToList() ;

        }

        [Route("TransferDream")]
        [HttpGet]
        public DreamExplanation TransferDream(int dreamId, String interpreterId)
        {
            Dream dream = db.Dreams.Where(a => a.id.Equals(dreamId)).Include(a => a.ServicePath).FirstOrDefault();
            if (dream == null)
            {
                core.throwExcetpion("No matching dream!!!");
            }
            int DreamsExplanationCount = db.DreamExplanations.Where(a => a.DreamId.Equals(dreamId) && a.Status.Equals("Done")).Count();
            var temp = db.DreamExplanations.Where(a => a.InterpreterId.Equals(interpreterId) && a.DreamId.Equals(dreamId)).FirstOrDefault();

            if (temp == null)
            {
                core.throwExcetpion("The interpreter didn't receive the dream before!!!");

            }


            db.Entry(temp).State = EntityState.Deleted;

            db.SaveChanges();

            return temp;

        }
    }
}
