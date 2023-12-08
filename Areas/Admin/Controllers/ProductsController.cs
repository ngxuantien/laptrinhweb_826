using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Mercuryshop.Models;

namespace Mercuryshop.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private MecuryshopEntities db = new MecuryshopEntities();

        // GET: Admin/Products
        public ActionResult Index(string Search, string sortOrder, string sortProperty)
        {
            var products = db.Products.Include(p => p.DatailProduct);
            var link = from l in db.Products select l;
            if (!String.IsNullOrEmpty(Search))
            {
                link = link.Where(s => s.Product_Name.Contains(Search));
            }
            ViewBag.SortOrder = String.IsNullOrEmpty(sortOrder) ? "desc" : "";
            if (sortOrder == "desc")
                link = link.OrderBy(s=>s.Product_Price);
            else
                link = link.OrderBy(s=>s.Product_Name);
            return View(link.ToList());

            //var ls = from s in db.Products select s;
            //var link = from l in db.Products
            //           select l;
            
            //return View(link);
        }

        // GET: Admin/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            ViewBag.Product_Id = new SelectList(db.DatailProducts, "Product_Id", "Name");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Product_Id,Product_Name,Product_Size,Product_Price,Product_Img,Product_Status,Product_Filter,Product_Promotional_Price,Product_Describe")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Product_Id = new SelectList(db.DatailProducts, "Product_Id", "Name", product.Product_Id);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.Product_Id = new SelectList(db.DatailProducts, "Product_Id", "Name", product.Product_Id);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Product_Id,Product_Name,Product_Size,Product_Price,Product_Img,Product_Status,Product_Filter,Product_Promotional_Price,Product_Describe")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Product_Id = new SelectList(db.DatailProducts, "Product_Id", "Name", product.Product_Id);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
