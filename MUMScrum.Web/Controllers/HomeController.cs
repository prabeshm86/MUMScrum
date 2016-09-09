using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MUMScrum.Web.Authorization;
using MUMScrum.DataAccess;
using MUMScrum.BusinessService;
using MUMScrum.Web.Helpers;
using MUMScrum.Model;

namespace MUMScrum.Web.Controllers
{
    [ScrumAuthorization]
    public class HomeController : Controller
    {
        IProductBacklogService productService = new ProductBacklogService();
        ISprintService sprintService = new SprintService();
        IUserStoryService usService = new UserStoryService();
        public ActionResult Index()
        {
            var emp = Utility.GetEmployeeSession(Session);
            switch ((RoleEnum)emp.RoleId)
            {
                case RoleEnum.ProductOwner:
                    ViewBag.Data = Utility.ConvertToProductSummary(productService.GetAllProductBackLogsByOwner(emp.Id));
                    break;
                case RoleEnum.ScrumMaster:
                    ViewBag.Data = Utility.ConvertToSprintSummary(sprintService.GetAll().Where(i => i.ReleaseBacklog.ScrumMasterId == emp.Id));
                    break;
                case RoleEnum.HR:
                    break;
                case RoleEnum.Developer:         
                    ViewBag.Data = usService.GetAllByDeveloperId(emp.Id);
                    break;
                case RoleEnum.Tester:
                    ViewBag.Data = usService.GetAllByTesterId(emp.Id);
                    break;

            }
            return View();
        }

        [ScrumAuthorization(Roles = "HR")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}