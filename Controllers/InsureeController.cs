using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using CarInsurance.Models;

namespace CarInsurance.Controllers
{
    public class InsureeController : Controller
    {
        private InsuranceEntities1 db = new InsuranceEntities1();

        // GET: Insuree
        public ActionResult Index()
        {
            return View(db.Insurees.ToList());
        }

         // GET: Admin
         public ActionResult Admin()
         {
            return View(db.Insurees.ToList());
         }

      // GET: Insuree/Details/5
      public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // GET: Insuree/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Insuree/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
               int age = DateTime.Now.Year - insuree.DateOfBirth.Year;
               insuree.Quote = CalculateQuote(age, insuree.CarYear, insuree.CarMake, insuree.CarModel, insuree.DUI, insuree.SpeedingTickets, insuree.CoverageType); 

               db.Insurees.Add(insuree);
               db.SaveChanges();
               return RedirectToAction("Index");
            }
            return View(insuree);
        }

        // GET: Insuree/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
               db.Entry(insuree).State = EntityState.Modified;
               int age = DateTime.Now.Year - insuree.DateOfBirth.Year;
               insuree.Quote = CalculateQuote(age, insuree.CarYear, insuree.CarMake, insuree.CarModel, insuree.DUI, insuree.SpeedingTickets, insuree.CoverageType);
               db.SaveChanges();
               return RedirectToAction("Index");
            }
            return View(insuree);
        }

        // GET: Insuree/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Insuree insuree = db.Insurees.Find(id);
            db.Insurees.Remove(insuree);
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
         private decimal CalculateQuote(int xage, int xyear, string xmake, string xmodel, bool xdui, int xticket, bool xcoverage)
         {
            decimal xquote = 50.00m;
            // age logic
            if (xage <= 18)
            {
               xquote += 100.00m;
            }
            else if (xage >= 19 && xage <= 25)
            {
               xquote += 50.00m;
            }
            else
            {
               xquote += 25.00m;
            }

            // car year, make, and model logic
            if (xyear < 2000 || xyear> 2015)
            {
               xquote += 25.00m;
            }
            if (xmake.ToLower() == "porsche")
            {
               xquote += 25.00m;
               if (xmodel.ToLower() == "911 carrera")
               {
                  xquote += 25.00m;
               }
            }
            // speeding tickets and DUI logic
            xquote += xticket * 10.00m;
            xquote += xdui ? xquote * 0.25m : 0.00m;
            xquote += xcoverage ? xquote * 0.50m : 0.00m;
            return xquote;
         }
    }
}
