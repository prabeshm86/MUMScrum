using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MUMScrum.Model;
using MUMScrum.DataAccess;
using MUMScrum.BusinessService;
using MUMScrum.HR;
using MUMScrum.Web.Authorization;
using MUMScrum.Web.Helpers;

namespace MUMScrum.Web.Controllers
{
    [ScrumAuthorization]
    public class ReleaseBacklogController : Controller
    {
        // private MUMScrumDbContext db = new MUMScrumDbContext();

        private IReleaseBacklogService services = new ReleaseBacklogService();
        private IProductBacklogService productServices = new ProductBacklogService();
        IHRManager hr = new HRManager();

        // GET: /ReleaseBacklog/
        public ActionResult Index(int? ScrumId, int? productId)
        {
            if (ScrumId.HasValue)
                return View(services.GetAllReleasesByScrumMaster(ScrumId.Value));

            //var releasebacklogs = db.ReleaseBacklogs.Include(r => r.PbackLog);
            return View(services.GetAll());
        }

        // GET: /ReleaseBacklog/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReleaseBacklog releasebacklog = services.GetById(id.Value);
            if (releasebacklog == null)
            {
                return HttpNotFound();
            }
            return View(releasebacklog);
        }
        [ScrumAuthorization(Roles = "productowner")]
        // GET: /ReleaseBacklog/Create
        public ActionResult Create(int? productId)
        {
            Employee User = Utility.GetEmployeeSession(Session);
            ViewBag.ScrumMasterId = new SelectList(hr.GetScrumMaster(), "id", "FirstName");
            ViewBag.ProductBacklogId = new SelectList(productServices.GetAllProductBackLogsByOwner(User.Id), "Id", "Name", productId);
            return View();
        }

        // POST: /ReleaseBacklog/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ScrumAuthorization(Roles = "productowner")]
        public ActionResult Create([Bind(Include = "Id,ReleaseName,Description,ReleaseDate,ProductBacklogId,ScrumMasterId")] ReleaseBacklog releasebacklog)
        {
            if (ModelState.IsValid)
            {
                services.CreateRelease(releasebacklog);
                // db.SaveChanges();
                return RedirectToAction("Details", new { Id = releasebacklog.Id });
            }
            Employee User = Utility.GetEmployeeSession(Session);
            ViewBag.ProductBacklogId = new SelectList(productServices.GetAllProductBackLogsByOwner(User.Id), "Id", "Name", releasebacklog.ProductBacklogId);
            ViewBag.ScrumMasterId = new SelectList(hr.GetScrumMaster(), "Id", "FirstName", releasebacklog.ScrumMasterId);
            return View(releasebacklog);
        }

        // GET: /ReleaseBacklog/Edit/5
        [ScrumAuthorization(Roles = "productowner")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReleaseBacklog releasebacklog = services.GetById(id.Value);
            if (releasebacklog == null)
            {
                return HttpNotFound();
            }
            Employee User = Utility.GetEmployeeSession(Session);
            ViewBag.ScrumMasterId = new SelectList(hr.GetScrumMaster(), "Id", "FirstName", releasebacklog.ScrumMasterId);
            ViewBag.ProductBacklogId = new SelectList(productServices.GetAllProductBackLogsByOwner(User.Id), "Id", "Name", releasebacklog.ProductBacklogId);
            return View(releasebacklog);
        }

        // POST: /ReleaseBacklog/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ScrumAuthorization(Roles = "productowner")]
        public ActionResult Edit([Bind(Include = "Id,ReleaseName,Description,ReleaseDate,ProductBacklogId,ScrumMasterId")] ReleaseBacklog releasebacklog)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(releasebacklog).State = EntityState.Modified;
                //db.SaveChanges();

                services.UpdateRelease(releasebacklog);
                return RedirectToAction("Details", new { Id = releasebacklog.Id });
                // return RedirectToAction("Index");
            }
            Employee User = Utility.GetEmployeeSession(Session);
            ViewBag.ScrumMasterId = new SelectList(hr.GetScrumMaster(), "Id", "FirstName", releasebacklog.ScrumMasterId);
            ViewBag.ProductBacklogId = new SelectList(productServices.GetAllProductBackLogsByOwner(User.Id), "Id", "Name", releasebacklog.ProductBacklogId);
            return View(releasebacklog);
        }

        // GET: /ReleaseBacklog/Delete/5
        [ScrumAuthorization(Roles = "productowner")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReleaseBacklog releasebacklog = services.GetById(id.Value);
            if (releasebacklog == null)
            {
                return HttpNotFound();
            }
            return View(releasebacklog);
        }

        // POST: /ReleaseBacklog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [ScrumAuthorization(Roles = "productowner")]
        public ActionResult DeleteConfirmed(int id)
        {
            //ReleaseBacklog releasebacklog = db.ReleaseBacklogs.Find(id);
            try
            {
                services.DeleteRelease(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                var CurrentRelease = services.GetById(id);
                ViewBag.Message = ex.Message;
                return View(CurrentRelease);
            }

            //db.ReleaseBacklogs.Remove(releasebacklog);
            //db.SaveChanges();
            // return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
