using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using Ahlam.Models;
using Ahlam.Bindings;

namespace Ahlam.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using Ahlam.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Dream>("Dreams");
    builder.EntitySet<ApplicationUser>("ApplicationUsers"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */

    [AllowAnonymous]
    public class DreamsController : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private CoreController core = new CoreController();

        // GET: odata/Dreams
        [EnableQuery]
        public IQueryable<Dream> GetDreams()
        {
            return db.Dreams;
        }

        // GET: odata/Dreams(5)
        [EnableQuery]
        public SingleResult<Dream> GetDream([FromODataUri] int key)
        {
            return SingleResult.Create(db.Dreams.Where(Dream => Dream.id == key));
        }

        // PUT: odata/Dreams(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Dream> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Dream Dream = db.Dreams.Find(key);
            if (Dream == null)
            {
                return NotFound();
            }

            patch.Put(Dream);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DreamExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(Dream);
        }

        // POST: odata/Dreams
        public IHttpActionResult Post(DreamBinding model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Dream dream = new Dream();
            dream.PhoneNumber = model.PhoneNumber;
            dream.ServicePathId = model.ServicePathId;
            dream.CreationDate = DateTime.Now;
            dream.LastModificationDate = DateTime.Now;
            dream.CreatorId = core.GetCurrentUserAsync(model.PhoneNumber).Result.Id;
            dream.Status = "Active";
            //Dream.Creator = core.getCurrentUser();
            //Dream.Modifier = core.getCurrentUser();
            dream.Payment=AddPayment(model, dream.CreatorId);
            db.Dreams.Add(dream);
            db.SaveChanges();

            return Created(dream);
        }
        private Payment AddPayment(DreamBinding temp, string creatorId)
        {
            Payment payment = new Payment();
            payment.Method = temp.Method;
            payment.Status = "Done";
            payment.Currency = temp.Currency;
            payment.Amount = temp.Amount;
            //payment.ServiceProviderId = service.ServiceProviderId;
            payment.CreatorId = creatorId;
            payment.ModifierId = creatorId;
            payment.CreationDate = DateTime.Now;
            payment.LastModificationDate = DateTime.Now;
            db.Payments.Add(payment);
            return payment;
        }

        // PATCH: odata/Dreams(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Dream> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Dream Dream = db.Dreams.Find(key);
            if (Dream == null)
            {
                return NotFound();
            }

            patch.Patch(Dream);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DreamExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(Dream);
        }

        // DELETE: odata/Dreams(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Dream Dream = db.Dreams.Find(key);
            if (Dream == null)
            {
                return NotFound();
            }

            db.Dreams.Remove(Dream);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        //// GET: odata/Dreams(5)/Creator
        //[EnableQuery]
        //public SingleResult<ApplicationUser> GetCreator([FromODataUri] int key)
        //{
        //    return SingleResult.Create(db.Dreams.Where(m => m.id == key).Select(m => m.Creator));
        //}

        //// GET: odata/Dreams(5)/Modifier
        //[EnableQuery]
        //public SingleResult<ApplicationUser> GetModifier([FromODataUri] int key)
        //{
        //    return SingleResult.Create(db.Dreams.Where(m => m.id == key).Select(m => m.Modifier));
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DreamExists(int key)
        {
            return db.Dreams.Count(e => e.id == key) > 0;
        }
    }
}
