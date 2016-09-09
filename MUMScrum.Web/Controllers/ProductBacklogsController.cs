using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MUMScrum.BusinessService;
using MUMScrum.Model;
using MUMScrum.Web.Authorization;
using MUMScrum.Web.Helpers;

namespace MUMScrum.Web.Controllers
{
    [ScrumAuthorization]
    public class ProductBacklogsController : Controller
    {
        //private MUMScrumDbContext db = new MUMScrumDbContext();
        private IProductBacklogService service = new ProductBacklogService();

        [ScrumAuthorization(Roles = "ProductOwner")]
        // GET: ProductBacklogs
        public ActionResult Index()
        {
            var empId = Utility.GetEmployeeSession(Session).Id;
            return View(service.GetAllProductBackLogsByOwner(empId));
        }

        // GET: ProductBacklogs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductBacklog productBacklog = service.GetById(id.Value);
            if (productBacklog == null)
            {
                return HttpNotFound();
            }
            return View(productBacklog);
        }

        // GET: ProductBacklogs/Create
        [ScrumAuthorization(Roles = "productowner")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductBacklogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ScrumAuthorization(Roles = "productowner")]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] ProductBacklog productBacklog)
        {
            if (ModelState.IsValid)
            {
                productBacklog.OwnerId = Utility.GetEmployeeSession(Session).Id;
                service.CreateProduct(productBacklog);
                return RedirectToAction("Details", new { Id = productBacklog.Id });
            }

            return View(productBacklog);
        }

        // GET: ProductBacklogs/Edit/5
        [ScrumAuthorization(Roles = "productowner")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductBacklog productBacklog = service.GetById(id.Value);
            if (productBacklog == null)
            {
                return HttpNotFound();
            }
            return View(productBacklog);
        }

        // POST: ProductBacklogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ScrumAuthorization(Roles = "productowner")]
        public ActionResult Edit([Bind(Include = "Id,Name,Description")] ProductBacklog productBacklog)
        {
            if (ModelState.IsValid)
            {
                productBacklog.OwnerId = Utility.GetEmployeeSession(Session).Id;
                service.UpdateProduct(productBacklog);
                return RedirectToAction("Details", new { Id = productBacklog.Id });
            }
            return View(productBacklog);
        }

        // GET: ProductBacklogs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductBacklog productBacklog = service.GetById(id.Value);
            if (productBacklog == null)
            {
                return HttpNotFound();
            }
            return View(productBacklog);
        }

        // POST: ProductBacklogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                service.DeleteProduct(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ProductBacklog productBacklog = service.GetById(id);
                ViewBag.Message = ex.Message;
                return View(productBacklog);
            }
            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
               // unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
