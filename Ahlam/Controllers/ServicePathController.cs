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

namespace Ahlam.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using Ahlam.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<ServicePath>("ServicePaths");
    builder.EntitySet<ApplicationUser>("ApplicationUsers"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */

        [AllowAnonymous]
    public class ServicePathController : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private CoreController core = new CoreController();

        // GET: odata/ServicePaths
        [EnableQuery]
        public IQueryable<ServicePath> GetServicePaths()
        {
            return db.ServicePaths;
        }

        // GET: odata/ServicePaths(5)
        [EnableQuery]
        public SingleResult<ServicePath> GetServicePath([FromODataUri] int key)
        {
            return SingleResult.Create(db.ServicePaths.Where(ServicePath => ServicePath.id == key));
        }

        // PUT: odata/ServicePaths(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<ServicePath> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ServicePath ServicePath = db.ServicePaths.Find(key);
            if (ServicePath == null)
            {
                return NotFound();
            }

            patch.Put(ServicePath);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServicePathExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(ServicePath);
        }

        // POST: odata/ServicePaths
        public IHttpActionResult Post(ServicePath ServicePath)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ServicePath.CreationDate = DateTime.Now;
            ServicePath.LastModificationDate = DateTime.Now;
            //ServicePath.Creator = core.getCurrentUser();
            //ServicePath.Modifier = core.getCurrentUser();
            db.ServicePaths.Add(ServicePath);
            db.SaveChanges();

            return Created(ServicePath);
        }

        // PATCH: odata/ServicePaths(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<ServicePath> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ServicePath ServicePath = db.ServicePaths.Find(key);
            if (ServicePath == null)
            {
                return NotFound();
            }

            patch.Patch(ServicePath);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServicePathExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(ServicePath);
        }

        // DELETE: odata/ServicePaths(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            ServicePath ServicePath = db.ServicePaths.Find(key);
            if (ServicePath == null)
            {
                return NotFound();
            }

            db.ServicePaths.Remove(ServicePath);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        //// GET: odata/ServicePaths(5)/Creator
        //[EnableQuery]
        //public SingleResult<ApplicationUser> GetCreator([FromODataUri] int key)
        //{
        //    return SingleResult.Create(db.ServicePaths.Where(m => m.id == key).Select(m => m.Creator));
        //}

        //// GET: odata/ServicePaths(5)/Modifier
        //[EnableQuery]
        //public SingleResult<ApplicationUser> GetModifier([FromODataUri] int key)
        //{
        //    return SingleResult.Create(db.ServicePaths.Where(m => m.id == key).Select(m => m.Modifier));
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ServicePathExists(int key)
        {
            return db.ServicePaths.Count(e => e.id == key) > 0;
        }
    }
}
