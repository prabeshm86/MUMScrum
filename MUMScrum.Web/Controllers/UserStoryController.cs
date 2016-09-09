using System.Web.Mvc;
using MUMScrum.Model;
using MUMScrum.BusinessService;
using MUMScrum.HR;
using System.Collections.Generic;
using MUMScrum.Web.Helpers;
using System.Net;
using MUMScrum.Web.Authorization;

namespace MUMScrum.Web.Controllers
{
    [ScrumAuthorization]
    public class UserStoryController : Controller
    {
        private IUserStoryService service = new UserStoryService();
        private IHRManager HrService = new HRManager();
        private ISprintService SprintService = new SprintService();
        private IReleaseBacklogService ReleaseSevice = new ReleaseBacklogService();
        private IProductBacklogService ProductService = new ProductBacklogService();

        // GET: UserStory
        public ActionResult Index()
        {
            return View(service.GetAll());
        }

        public ActionResult GetUserStoriesByUId(int UserId)
        {
            if (Utility.UserInRole(RoleEnum.Developer, Session))
                return View("Index", service.GetAllByDeveloperId(UserId));
            else
                return View("Index", service.GetAllByTesterId(UserId));
        }

        // GET: UserStory/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserStory userStory = service.GetUserStoryById(id.Value);
            if (userStory == null)
            {
                return HttpNotFound();
            }
            return View(userStory);
        }

        //// GET: UserStory/Create
        [ScrumAuthorization(Roles = "ProductOwner")]
        public ActionResult Create(int? ProductId, int? ReleaseId, int? SprintId)
        {
            ViewBag.DevelopedId = new SelectList(HrService.GetAllDevelopers(), "Id", "FirstName");
            ViewBag.ProductBackLogId = new SelectList(ProductService.GetAllProductBackLogsByOwner(Utility.GetEmployeeSession(this.Session).Id), "Id", "Name", ProductId);
            ViewBag.ReleaseId = new SelectList(new List<ReleaseBacklog>());
            ViewBag.SprintId = new SelectList(new List<Sprint>(), "Id", "SprintName");
            ViewBag.TesterId = new SelectList(HrService.GetAllTesters(), "Id", "FirstName");
            return View(new UserStory
            {
                SprintId = SprintId,
                ReleaseId = ReleaseId
            });
        }

        // POST: UserStory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ScrumAuthorization(Roles = "ProductOwner")]
        public ActionResult Create([Bind(Include = "Id,Title,Description,DevelopedId,TesterId,ProductBackLogId,ReleaseId,SprintId,DevEstimate , TestEstiamte , DevActual ,TestActual ")] UserStory userStory)
        {
            if (ModelState.IsValid)
            {
                service.CreateUserStory(userStory);
                if (!string.IsNullOrEmpty(Request["sprintId"]))
                {
                    return RedirectToAction("Details", "Sprint", new { id = userStory.SprintId });
                }
                else if (!string.IsNullOrEmpty(Request["releaseId"]))
                {
                    return RedirectToAction("Details", "ReleaseBacklog", new { id = userStory.ReleaseId });
                }
                else if (!string.IsNullOrEmpty(Request["productId"]))
                {
                    return RedirectToAction("Details", "ProductBacklogs", new { id = userStory.ProductBackLogId });
                }

            }

            ViewBag.DevelopedId = new SelectList(HrService.GetAllDevelopers(), "Id", "FirstName");
            ViewBag.ProductBackLogId = new SelectList(ProductService.GetAllProductBackLogsByOwner(Utility.GetEmployeeSession(this.Session).Id), "Id", "Name");
            ViewBag.ReleaseId = new SelectList(ReleaseSevice.GetAll(), "Id", "ReleaseName");
            ViewBag.SprintId = new SelectList(SprintService.GetAll(), "Id", "SprintName");
            ViewBag.TesterId = new SelectList(HrService.GetAllTesters(), "Id", "FirstName");
            return View(userStory);
        }

        // GET: UserStory/Edit/5

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserStory userStory = service.GetUserStoryById(id.Value);
            if (userStory == null)
            {
                return HttpNotFound();
            }
            Employee User = Utility.GetEmployeeSession(this.Session);
            ViewBag.ProductBackLogId = new SelectList(ProductService.GetAllProductBackLogsByOwner(User.Id), "Id", "Name", userStory.ProductBackLogId);
            ViewBag.ReleaseId = new SelectList(new List<ReleaseBacklog>());
            ViewBag.SprintId = userStory.ReleaseId == null ? new SelectList(new List<Sprint>()) : new SelectList(SprintService.GetAllSprintsByRelId(userStory.ReleaseId.Value), "Id", "SprintName");

            if (Utility.UserInRole(RoleEnum.ProductOwner, Session))
                return View("_POEdit", userStory);

            ViewBag.DevelopedId = new SelectList(HrService.GetAllDevelopers(), "Id", "FirstName", userStory.DevelopedId);
            ViewBag.TesterId = new SelectList(HrService.GetAllTesters(), "Id", "FirstName", userStory.TesterId);

            if (Utility.UserInRole(RoleEnum.ScrumMaster, Session))
                return View("_SMEdit", userStory);

            else if (Utility.UserInRole(RoleEnum.Developer, Session) || Utility.UserInRole(RoleEnum.Tester, Session))
                return View("_EmpEdit", userStory);

            return View(userStory);
        }

        // POST: UserStory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,DevelopedId,TesterId,ProductBackLogId,ReleaseId,SprintId,DevEstimate , TestEstiamte , DevActual ,TestActual ")] UserStory userStory)
        {
            Employee User = Utility.GetEmployeeSession(this.Session);
            if (ModelState.IsValid)
            {
                service.UpdateUserStory(userStory, User.RoleId);
                return RedirectUrl(service.GetUserStoryById(userStory.Id));
            }
            ViewBag.ProductBackLogId = new SelectList(ProductService.GetAllProductBackLogsByOwner(User.Id), "Id", "Name", userStory.ProductBackLogId);
            ViewBag.ReleaseId = new SelectList(new List<ReleaseBacklog>());
            ViewBag.SprintId = new SelectList(SprintService.GetAllSprintsByRelId(userStory.ReleaseId.Value), "Id", "SprintName");

            if (Utility.UserInRole(RoleEnum.ProductOwner, Session))
                return View("_POEdit", userStory);

            ViewBag.DevelopedId = new SelectList(HrService.GetAllDevelopers(), "Id", "FirstName", userStory.DevelopedId);
            ViewBag.TesterId = new SelectList(HrService.GetAllTesters(), "Id", "FirstName", userStory.TesterId);

            if (Utility.UserInRole(RoleEnum.ScrumMaster, Session))
                return View("_SMEdit", userStory);

            else if (Utility.UserInRole(RoleEnum.Developer, Session) || Utility.UserInRole(RoleEnum.Tester, Session))
                return View("_EmpEdit", userStory);
            return View(userStory);
        }

        private RedirectToRouteResult RedirectUrl(UserStory userStory)
        {
            var emp = Utility.GetEmployeeSession(Session);
            if ((RoleEnum)emp.RoleId == RoleEnum.Developer || (RoleEnum)emp.RoleId == RoleEnum.Tester)
                return RedirectToAction("GetUserStoriesByUId", new { UserId = emp.Id });

            if (Request["return"] == null) return RedirectToAction("Index", "Home");
            switch (Request["return"].ToLower())
            {

                case "sprint":
                    return RedirectToAction("Details", "Sprint", new { id = userStory.SprintId });
                case "product":
                    return RedirectToAction("Details", "ProductBacklogs", new { id = userStory.ProductBackLogId });
                case "release":
                default:
                    return RedirectToAction("Details", "ReleaseBacklog", new { id = userStory.ReleaseId  });
            }
        }

        //// GET: UserStory/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserStory userStory = service.GetUserStoryById(id.Value);
            if (userStory == null)
            {
                return HttpNotFound();
            }
            return View(userStory);
        }

        // POST: UserStory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var thisUs = service.GetUserStoryById(id);

            var userStory = new UserStory
            {
                SprintId = thisUs.SprintId,
                ProductBackLogId = thisUs.ProductBackLogId,
                ReleaseId = thisUs.ReleaseId,
            };

            try
            {
                if (service.DeleteUserStory(id))
                {
                    service.DeleteRelatedLogs(id);
                }
            }
            catch (System.Exception ex)
            {
                TempData.Add("Message", ex.Message);
            }
            
            return RedirectUrl(userStory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
            base.Dispose(disposing);
        }
    }
}
