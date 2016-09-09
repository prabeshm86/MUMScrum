using MUMScrum.Model;
using MUMScrum.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MUMScrum.Web.Helpers
{
    public class Utility
    {
        public static Employee GetEmployeeSession(HttpSessionStateBase session)
        {
            if (session["Employee"] == null) return null;
            else
            {
                Employee emp = (Employee)session["Employee"];
                return emp;
            }
        }

        public static void CreateEmployeeSession(HttpSessionStateBase session, Employee emp)
        {
            session["Employee"] = emp;
        }

        public static string GetLoggedInEmployeeName(HttpSessionStateBase session)
        {

            if (session == null)
            {

                return string.Empty;
            }

            var emp = GetEmployeeSession(session);
            if (emp == null) return string.Empty;
            return emp.FirstName + " " + emp.LastName + " [ " + emp.Role.RoleName + " ]";
        }

        public static void DestroySession(HttpSessionStateBase session)
        {
            session["Employee"] = null;
        }

        public static bool UserInRole(RoleEnum role, HttpSessionStateBase session)
        {
            if (session == null) return false;
            var emp = GetEmployeeSession(session);
            if (emp == null) return false;
            if (emp.RoleId == (int)role)
                return true;
            return false;
        }

        public static List<ProductSummaryViewModel> ConvertToProductSummary(IEnumerable<ProductBacklog> products)
        {
            var summaryList = new List<ProductSummaryViewModel>();
            foreach (var product in products)
            {
                var newSummary = new ProductSummaryViewModel
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    BacklogUserStories = product.UserStories.Count(i => i.ReleaseId == null),
                    ScrumMasters = product.ReleaseBacklogs.Select(i => i.ScrumMasterId).Distinct().Count(),
                    Releases = product.ReleaseBacklogs.Count(),
                    AssignedUserStories = product.UserStories.Count(i => i.DevelopedId != null || i.TesterId != null),
                    Developers = product.UserStories.Where(i => i.DevelopedId != null).Select(i => i.DevelopedId).Distinct().Count(),
                    Sprints = product.ReleaseBacklogs.Select(i => i.Sprints).Count(),
                    Testers = product.UserStories.Where(i => i.TesterId != null).Select(i => i.TesterId).Distinct().Count(),
                    PercentCompleted = product.UserStories.Any() ? product.UserStories.Select(i => i.PercentageCompleted()).Sum() / product.UserStories.Count() : 0

                };
                summaryList.Add(newSummary);
            }
            return summaryList;
        }
        public static List<SprintSummaryViewModel> ConvertToSprintSummary(IEnumerable<Sprint> sprints)
        {
            var summaryList = new List<SprintSummaryViewModel>();
            foreach (var sprint in sprints)
            {
                var newSummary = new SprintSummaryViewModel
                {
                    SprintId = sprint.Id,
                    SprintName = sprint.SprintName,
                    StartDate = sprint.StartDate,
                    EndDate = sprint.EndDate,
                    Developers = sprint.UserStories.Where(i => i.DevelopedId != null).Select(i => i.DevelopedId).Distinct().Count(),
                    UserStories = sprint.UserStories.Count(),
                    Testers = sprint.UserStories.Where(i => i.TesterId != null).Select(i => i.TesterId).Distinct().Count(),
                    PercentageCompleted = sprint.UserStories.Any() ? sprint.UserStories.Select(i => i.PercentageCompleted()).Sum() / sprint.UserStories.Count() : 0,
                    ProductName = sprint.ReleaseBacklog.ProductBacklog.Name,
                    ReleaseName = sprint.ReleaseBacklog.ReleaseName

                };
                summaryList.Add(newSummary);
            }
            return summaryList;
        }

        public static string GetRandomColor(int num)
        {
            var random = new Random().Next();

            switch ((random + num) % 7)
            {
                case 0:
                    return "yellow";
                case 1:
                    return "blue";
                case 3:
                    return "green";
                case 4:
                    return "black";
                case 5:
                    return "greenLight";
                case 6:
                    return "greenDark";
                default:
                    return "pink";


            }
        }


    }
}