using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MUMScrum.DataAccess;
using MUMScrum.Model;
using MUMScrum.HR;
using MUMScrum.Web.Authorization;
using MUMScrum.Web.Helpers;

namespace MUMScrum.Web.Controllers
{
    [ScrumAuthorization]
    public class EmployeesController : Controller
    {
        private IHRManager hrService = new HRManager();

        [ScrumAuthorization(Roles ="hr")]
        // GET: Employees
        public ActionResult Index()
        {
            var employees = hrService.GetAllEmployees();
            return View(employees.ToList());
        }
        [ScrumAuthorization(Roles = "hr")]
        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = hrService.GetEmployeeById(id.Value);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }
        [ScrumAuthorization(Roles = "hr")]
        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.RoleId = new SelectList(hrService.GetAllRoles(), "Id", "RoleName");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ScrumAuthorization(Roles = "hr")]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,RoleId,UserName,Password,IsDeactivated")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                var result = hrService.CreateEmployee(employee);
                if (result == Model.Enum.CreateEmployeeStatus.DulplicateUsername)
                {
                    ModelState.AddModelError("", "Username already exists.");
                    ViewBag.RoleId = new SelectList(hrService.GetAllRoles(), "Id", "RoleName", employee.RoleId);
                    return View(employee);
                }

                return RedirectToAction("Index");
            }

            ViewBag.RoleId = new SelectList(hrService.GetAllRoles(), "Id", "RoleName", employee.RoleId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        [ScrumAuthorization(Roles = "hr")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = hrService.GetEmployeeById(id.Value);           
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleId = new SelectList(hrService.GetAllRoles(), "Id", "RoleName", employee.RoleId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ScrumAuthorization(Roles ="hr")]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,RoleId,UserName,Password,IsDeactivated")] Employee employee)
        {
            if (ModelState.IsValid)
            {               
                var result = hrService.UpdateEmployee(employee);
                if (result == Model.Enum.CreateEmployeeStatus.DulplicateUsername)
                {
                    ModelState.AddModelError("", "Username already exists.");
                    ViewBag.RoleId = new SelectList(hrService.GetAllRoles(), "Id", "RoleName", employee.RoleId);
                    return View(employee);
                }
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(hrService.GetAllRoles(), "Id", "RoleName", employee.RoleId);
            return View(employee);
        }


        // GET: Employees/Edit/5
        [ScrumAuthorization]
        public ActionResult UpdateProfile(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = hrService.GetEmployeeById(id.Value);
            if (employee.Id != Utility.GetEmployeeSession(Session).Id)
                return HttpNotFound();

            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleId = new SelectList(hrService.GetAllRoles(), "Id", "RoleName", employee.RoleId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ScrumAuthorization]
        public ActionResult UpdateProfile([Bind(Include = "Id,FirstName,LastName,UserName,Password")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (employee.Id != Utility.GetEmployeeSession(Session).Id)
                    return HttpNotFound();
                var result = hrService.UpdateProfile(employee);
                if (result == Model.Enum.CreateEmployeeStatus.DulplicateUsername)
                {
                    ModelState.AddModelError("", "Username already exists.");
                    ViewBag.RoleId = new SelectList(hrService.GetAllRoles(), "Id", "RoleName", employee.RoleId);
                    return View(employee);
                }
                return RedirectToAction("Index", "Home");
            }
            ViewBag.RoleId = new SelectList(hrService.GetAllRoles(), "Id", "RoleName", employee.RoleId);
            return View(employee);
        }


        // GET: Employees/Delete/5
        [ScrumAuthorization(Roles = "hr")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = hrService.GetEmployeeById(id.Value);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [ScrumAuthorization(Roles = "hr")]
        public ActionResult DeleteConfirmed(int id)
        {
            hrService.DeleteEmployee(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
               // hrService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
