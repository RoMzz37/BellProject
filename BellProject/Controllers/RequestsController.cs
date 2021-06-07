using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BellProject.Models;

namespace BellProject.Controllers
{
    public class RequestsController : Controller
    {
        private BellSupportEntities db = new BellSupportEntities();

        // GET: Requests
        public ActionResult Index()
        {
            var requests = db.Requests.Include(r => r.Department).Include(r => r.Employee).Include(r => r.Project);
            return View(requests.ToList());
        }

        // GET: Requests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // GET: Requests/Create
        public ActionResult Create()
        {
            ViewBag.department_id = new SelectList(db.Departments, "id", "name");
            ViewBag.employee_id = new SelectList(db.Employees, "id", "firstName");
            ViewBag.project_id = new SelectList(db.Projects, "id", "name");

            ViewBag.employeeFK = new SelectList(db.Employees, "department_id", "firstName");
            ViewBag.departmentFK = new SelectList(db.Departments, "project_id", "name");

            return View();
        }

        // POST: Requests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,project_id,department_id,employee_id,time,description")] Request request)
        {
            if (ModelState.IsValid)
            {
                db.Requests.Add(request);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.department_id = new SelectList(db.Departments, "id", "name", request.department_id);
            ViewBag.employee_id = new SelectList(db.Employees, "id", "firstName", request.employee_id);
            ViewBag.project_id = new SelectList(db.Projects, "id", "name", request.project_id);
            return View(request);
        }

        public ActionResult Search()
        {
            ViewBag.department_id = new SelectList(db.Departments, "id", "name");
            ViewBag.employee_id = new SelectList(db.Employees, "id", "firstName");
            ViewBag.project_id = new SelectList(db.Projects, "id", "name");

            ViewBag.employeeFK = new SelectList(db.Employees, "department_id", "firstName");
            ViewBag.departmentFK = new SelectList(db.Departments, "project_id", "name");

            return View();
        }

        public ActionResult SearchResults(string employee_id, string project_id, string description, string department_id)
        {
            int empId = (employee_id == "") ? 0: Convert.ToInt32(employee_id);
            int projectId = (project_id == "") ? 0 : Convert.ToInt32(project_id); ;
            int departmentId = (department_id == "") ? 0 : Convert.ToInt32(department_id); ;

            var data = db.Requests.Include(r => r.Department).Include(r => r.Employee).Include(r => r.Project);

            if(empId != 0)
            {
                data = db.Requests.Where(j => j.employee_id == empId && j.description.Contains(description));
            }
            else if(departmentId != 0)
            {
                data = db.Requests.Where(j => j.department_id == departmentId && j.description.Contains(description));
            }
            else if(projectId != 0)
            {
                data = db.Requests.Where(j => j.project_id == projectId && j.description.Contains(description));
            }

            
            
            return View("Index", data.ToList());
        }

        // GET: Requests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            ViewBag.department_id = new SelectList(db.Departments, "id", "name", request.department_id);
            ViewBag.employee_id = new SelectList(db.Employees, "id", "firstName", request.employee_id);
            ViewBag.project_id = new SelectList(db.Projects, "id", "name", request.project_id);
            return View(request);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,project_id,department_id,employee_id,time,description")] Request request)
        {
            if (ModelState.IsValid)
            {
                db.Entry(request).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.department_id = new SelectList(db.Departments, "id", "name", request.department_id);
            ViewBag.employee_id = new SelectList(db.Employees, "id", "firstName", request.employee_id);
            ViewBag.project_id = new SelectList(db.Projects, "id", "name", request.project_id);
            return View(request);
        }

        // GET: Requests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Request request = db.Requests.Find(id);
            db.Requests.Remove(request);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
