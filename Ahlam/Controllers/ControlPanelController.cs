using Ahlam.Models;
using Ahlam.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ahlam.Controllers
{
    [RoutePrefix("api/controlPanel")]
    public class ControlPanelController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private CoreController core = new CoreController();

        [Route("GetEmployeesList")]
        [HttpGet]
        public List<InterpreterViewModel> GetInterpreters()
        {

            List<ApplicationUser> users = db.Users.Where(a => a.Type.Equals("Service_Provider")).ToList();
            List<InterpreterViewModel> result = new List<InterpreterViewModel>();
            foreach (var item in users)
            {
                InterpreterViewModel temp = new InterpreterViewModel();
                temp.name = item.Name;
                temp.username = item.UserName;
                temp.password = item.password==null?item.PasswordHash:item.password;
                temp.numberOfActiveDreams = db.DreamExplanations.Where(a => a.InterpreterId.Equals(item.Id) && a.Status.Equals("Active")).Count();
                temp.numberOfDoneDreams= db.DreamExplanations.Where(a => a.InterpreterId.Equals(item.Id) && a.Status.Equals("Done")).Count();
                temp.availableBalance = temp.numberOfDoneDreams;
                temp.sentBalance = 0.0;
                result.Add(temp);
            }
            return result;
        }


        [Route("GetStatistics")]
        [HttpGet]
        public StatisticsViewModel getStatistics()
        {
            StatisticsViewModel result = new StatisticsViewModel();
            result.numberOfActiveDreams = db.Dreams.Where(a => a.Status.Equals("Active")).Count();
            result.numberOfDoneDreams = db.Dreams.Where(a => a.Status.Equals("Done")).Count();
            result.PurchasesBalance = db.Payments.Sum(a => a.Amount);

            return result;

        }

        [Route("GetDreamsList")]
        [HttpGet]
        public List<DreamsViewModel> getDreams()
        {
            List<DreamExplanation> doneDreams = db.DreamExplanations.Where(a => a.Status.Equals("Done")).Include(a=>a.Dream).Include(a=>a.Interpreter).ToList();
            List<DreamsViewModel> resutl = new List<DreamsViewModel>();
            foreach (var item in doneDreams)
            {
                DreamsViewModel temp = new DreamsViewModel();

                temp.createtionDate = item.Dream.CreationDate;
                temp.interpretationTime = item.ExplanationDate;
                temp.dream = item.Dream.Description;
                temp.interpretationPeriod = temp.interpretationTime.Subtract(temp.createtionDate).TotalMinutes;
                temp.interpreterName = item.Interpreter.Name;
                resutl.Add(temp);
            }
            return resutl;
        }

    }
}
