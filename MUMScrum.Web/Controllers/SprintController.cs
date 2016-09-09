using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MUMScrum.Model;
using MUMScrum.BusinessService;
using MUMScrum.DataAccess;
using MUMScrum.Web.Authorization;
using MUMScrum.Web.Helpers;

namespace MUMScrum.Web.Controllers
{
    [ScrumAuthorization(Roles = "productowner, scrummaster")]
    public class SprintController : Controller
    {
        //  private MUMScrumDbContext db = new MUMScrumDbContext();
        ISprintService service = new SprintService();
        IReleaseBacklogService releaseBacklog = new ReleaseBacklogService();

        // GET: /Sprint/
        public ActionResult Index()
        {
            //var sprints = db.Sprints.Include(s => s.releaseBacklog);
            return View(service.GetAll());
        }

        // GET: /Sprint/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sprint sprint = service.GetById(id.Value);
            if (sprint == null)
            {
                return HttpNotFound();
            }
            return View(sprint);
        }
        [ScrumAuthorization(Roles = "scrummaster")]
        // GET: /Sprint/Create
        public ActionResult Create(int? releaseId)
        {
            Employee ScrumM = Utility.GetEmployeeSession(Session);
            ViewBag.ReleaseBacklogId = new SelectList(releaseBacklog.GetAllReleasesByScrumMaster(ScrumM.Id), "Id", "ReleaseName" , releaseId);
            return View();
        }
        [ScrumAuthorization(Roles = "scrummaster")]
        // POST: /Sprint/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SprintName,Description,SprintRunning,StartDate,EndDate,ReleaseBacklogId,")] Sprint sprint)
        {
            if (ModelState.IsValid)
            {
                service.CreateSprint(sprint);
                //db.Sprints.Add(sprint);
                //db.SaveChanges();
                return RedirectToAction("Details", new { Id = sprint.Id });
            }
            return View("Details" ,"ReleaseBacklog", new { Id = sprint.ReleaseBacklogId });
        }
        [ScrumAuthorization(Roles = "scrummaster")]
        // GET: /Sprint/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sprint sprint = service.GetById(id.Value);
            if (sprint == null)
            {
                return HttpNotFound();
            }
            Employee ScrumM = Utility.GetEmployeeSession(Session);
            ViewBag.ReleaseBacklogId = new SelectList(releaseBacklog.GetAllReleasesByScrumMaster(ScrumM.Id), "Id", "ReleaseName", sprint.ReleaseBacklogId);
            return View(sprint);
        }
        [ScrumAuthorization(Roles = "scrummaster")]
        // POST: /Sprint/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SprintName,Description,SprintRunning,StartDate,EndDate,ReleaseBacklogId")] Sprint sprint)
        {
            if (ModelState.IsValid)
            {
                service.UpdateSprint(sprint);
                //db.Entry(sprint).State = EntityState.Modified;
                //db.SaveChanges();
                //return RedirectToAction("Details", new { Id = sprint.Id });
            }
            Employee ScrumM = Utility.GetEmployeeSession(Session);
            ViewBag.ReleaseBacklogId = new SelectList(releaseBacklog.GetAllReleasesByScrumMaster(ScrumM.Id), "Id", "ReleaseName", sprint.ReleaseBacklogId);
            return View(sprint);
        }
        [ScrumAuthorization(Roles = "scrummaster")]
        // GET: /Sprint/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sprint sprint = service.GetById(id.Value);
            if (sprint == null)
            {
                return HttpNotFound();
            }
            return View(sprint);
        }
        [ScrumAuthorization(Roles = "scrummaster")]
        // POST: /Sprint/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var releaseId = service.GetById(id).ReleaseBacklogId;
            service.DeleteSprint(id);
            // db.Sprints.Remove(sprint);
            //db.SaveChanges();
            return RedirectToAction("Details", "ReleaseBacklog", new { id = releaseId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
