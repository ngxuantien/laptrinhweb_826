using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using Mercuryshop.Models;
using System.Data.Entity;
using System.Net.Http;
using System.Net;

namespace Mercuryshop.Controllers
{
    public class MercuryshopController : Controller
    {
        MecuryshopEntities db = new MecuryshopEntities();
        // GET: Mercuryshop
        public ActionResult Index(string searchString)
        {
            var ls = from s in db.Products select s;
            var link = from l in db.Products
                       select l;
            if (!String.IsNullOrEmpty(searchString))
            {
                link = link.Where(s => s.Product_Name.Contains(searchString));
            }
            return View(link);
        }
        public ActionResult Product(int? page, int? Loai, string search,string sortOrder)
        {
            var Lsp = from s in db.Products select s;
            if (Loai==1)
            {
                Lsp = Lsp.Where(n => n.Product_Filter == 1).OrderBy(b => b.Product_Price);
            }
            else if 
            (Loai == 0)
            {
                Lsp = Lsp.Where(n => n.Product_Filter == 0).OrderBy(b => b.Product_Price); 
            }
            else
            {
                Lsp = Lsp.OrderBy(b => b.Product_Price);
            }

            // sap xep
            
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Sắp xếp theo tên", Value = "ten_desc" });
            items.Add(new SelectListItem { Text = "Sắp xếp giá tăng", Value = "price" });
            items.Add(new SelectListItem { Text = "Sắp xếp giá giảm", Value = "price_desc" });
            ViewBag.SortValue = items;
            switch (sortOrder)
            {
                case "ten_desc":
                    Lsp = Lsp.OrderByDescending(s => s.Product_Name);
                    ViewBag.SortOrder = "ten_desc";
                    break;
                case "price_desc":
                    Lsp = Lsp.OrderByDescending(s => s.Product_Promotional_Price);
                    ViewBag.SortOrder = "price_desc";
                    break;
                case "price":
                    Lsp = Lsp.OrderBy(s => s.Product_Promotional_Price);
                    ViewBag.SortOrder = "price";
                    break;
                default:
                    Lsp = Lsp.OrderBy(s => s.Product_Name);
                    break;
            }

            if (page == null) page = 1;
           
            int pageSize = 8;
            int PageNumber = (page ?? 1);
            return View(Lsp.ToPagedList(PageNumber, pageSize));

            //tìm kiếm
            if (!String.IsNullOrEmpty(search))
            {
                Lsp = Lsp.Where(s => s.Product_Name.Contains(search));
            }
            return View(Lsp);
        }
        //public ActionResult DetailProduct(int id)
        //{
        //    var sp = from s in db.Products
        //               where s.Product_Id == id
        //               select s;
        //    return View(sp.Single());
        //}
        public ActionResult DetailProduct(int? id)
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
    }
}