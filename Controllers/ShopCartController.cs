using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mercuryshop.Models;
using System.Data.Entity;

namespace Mercuryshop.Controllers
{
    public class ShopCartController : Controller
    {
        MecuryshopEntities db = new MecuryshopEntities();
        public List<Cart> LayGioHang()
        {
            List<Cart> lstGioHang = Session["Cart"] as List<Cart>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<Cart>();
                Session["Cart"] = lstGioHang;
            }
            return lstGioHang;
        }

        public ActionResult ThemGioHang(int ms, string url)
        {
            if(Session["TaiKhoan"]==null)
            {
                return RedirectToAction("DangNhap","Customer");
            }
            else
            {
                List<Cart> lstGioHang = LayGioHang();
                Cart sp = lstGioHang.Find(n => n.iMasp == ms);
                if (sp == null)
                {
                    sp = new Cart(ms);
                    lstGioHang.Add(sp);
                }
                else
                {
                    sp.iSoLuong++;
                }
                return Redirect(url);
            }    
           
        }
        public ActionResult Tanglen(int ms)
        {
            List<Cart> lstGioHang = LayGioHang();
            Cart sp = lstGioHang.Find(n => n.iMasp == ms);
            var Thanhtien = sp.dThanhTien;
            if (sp.iSoLuong < 100)
            {
                sp.iSoLuong++;
                Thanhtien = sp.dThanhTien; 
            }
            return Content(string.Format("{0:#,##0,0}", Thanhtien));
        }
        public ActionResult GiamXuong(int ms)
        {
            List<Cart> lstGioHang = LayGioHang();
            Cart sp = lstGioHang.Find(n => n.iMasp == ms);
            var Thanhtien = sp.dThanhTien;
            if(sp.iSoLuong > 1)
            {
                sp.iSoLuong--;
                Thanhtien = sp.dThanhTien;
            }
            return Content(string.Format("{0:#,##0,0}", Thanhtien));
        }

        
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<Cart> lstGioHang = Session["Cart"] as List<Cart>;
            if (lstGioHang != null)
            {
                iTongSoLuong = lstGioHang.Sum(n => n.iSoLuong);

            }
            return iTongSoLuong;
        }
        private double TongTien()
        {
            double dTongTien = 0;
            List<Cart> lstGioHang = Session["Cart"] as List<Cart>;
            if (lstGioHang != null)
            {
                dTongTien = lstGioHang.Sum(n => n.dThanhTien);
            }
            //ViewBag.TongSoLuong = TongSoLuong();
            //ViewBag.TongTien = TongTien();
            //ViewBag.TongTiencoship = TongTien() + 32000;
            return dTongTien;
        }
        
        public ActionResult hehe()
        {
            double dTongTien = 0;
            dTongTien = TongTien();
            return Content(string.Format("{0:#,##0,0}", dTongTien));
        }
        public ActionResult GioHang()
        {
            List<Cart> lstGioHang = LayGioHang();
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Product", "Mercuryshop");
                //return RedirectToAction("GioHang", "ShopCart");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }

        public ActionResult XoaSPKhoiGioHang(int iMasp)
        {
            List<Cart> lstGioHang = LayGioHang();
            Cart sp = lstGioHang.SingleOrDefault(n => n.iMasp == iMasp);
            if (sp != null)
            {
                lstGioHang.RemoveAll(n => n.iMasp == iMasp);
                if (lstGioHang.Count == 0)
                {
                    return RedirectToAction("Index", "Mercuryshop");
                }
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult CapNhatGioHang(int iMasp, FormCollection f)
        {
            List<Cart> lstGioHang = LayGioHang();
            Cart sp = lstGioHang.SingleOrDefault(n => n.iMasp == iMasp);
            if (sp != null)
            {
                sp.iSoLuong = int.Parse(f["txtSoLuong"].ToString());
            }
            return RedirectToAction("Cart");
        }
        public ActionResult XoaGioHang()
        {
            List<Cart> lstGioHang = LayGioHang();
            lstGioHang.Clear();
            return RedirectToAction("Index", "Mercuryshop");
        }

        public List<Cart> LayDsSpThanhToan()
        {
            List<Cart> lstSPThanhToan = Session["DSThanhToan"] as List<Cart>;
            if (lstSPThanhToan == null)
            {
                lstSPThanhToan = new List<Cart>();
                Session["DSThanhToan"] = lstSPThanhToan;
            }
            return lstSPThanhToan;
        }

        [HttpPost]
        public ActionResult Taodstt(List<int> selectedValues)
        {
            double dTongTien = 0;
            Session["DSThanhToan"] = null;
            List<Cart> lstSPThanhToan = LayDsSpThanhToan();
            List<Cart> lstGioHang = LayGioHang();
            if (selectedValues != null)
            {
                foreach (var id in selectedValues)
                {
                    Cart sp = lstGioHang.Find(n => n.iMasp == id);
                    lstSPThanhToan.Add(sp);
                }
            }
            dTongTien = TongTien();
            return Content(string.Format("{0:#,##0,0}", dTongTien));
        }

        private double TongTienDSTT()
        {
            
            double dTongTien = 0;
            List<Cart> lstSPThanhToan = Session["DSThanhToan"] as List<Cart>;
            if (lstSPThanhToan != null)
            {
                dTongTien = lstSPThanhToan.Sum(n => n.dThanhTien);
            }
            //ViewBag.TongSoLuong = TongSoLuong();
            //ViewBag.TongTien = TongTien();
            //ViewBag.TongTiencoship = TongTien() + 32000;
            return dTongTien;
        }

        [HttpPost]
        public ActionResult DatHang(List<int> selectedValues)
        {
            //if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            //{
            //    return RedirectToAction("DangNhap", "Customer");
            //}
            //if (Session["Cart"] == null)
            //{
            //    return RedirectToAction("Product", "Mercuryshop");
            //}
            Session["DSThanhToan"] = null;
            List<Cart> lstSPThanhToan = LayDsSpThanhToan();
            List<Cart> lstGioHang = LayGioHang();
            
            //Session["Cart"] = null;
            if (selectedValues != null)
            {
                foreach (var id in selectedValues)
                {
                    Cart sp = lstGioHang.Find(n => n.iMasp == id);
                    lstSPThanhToan.Add(sp);
                }
            }
            
            //List<Cart> lstGioHang = LayGioHang();
            return View("DatHang", lstSPThanhToan);
        }

        [HttpGet]
        public ActionResult DatHang()
        {
            List<Cart> lstSPThanhToan = LayDsSpThanhToan();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTienDSTT();
            ViewBag.TongTiencoship = TongTienDSTT() + 32000;
            return View(lstSPThanhToan);
        }
        [HttpPost]
        public ActionResult Dat(FormCollection f)
        {
            DonHang dh = new DonHang();
            Customer kh = (Customer)Session["TaiKhoan"];
            List<Cart> lstGioHang = LayGioHang();
            List<Cart> lstSPThanhToan = LayDsSpThanhToan();
            dh.Customer_Id = kh.Customer_Id;
            dh.NgayDat = DateTime.Now;
            dh.NgayGiao = DateTime.Now;
            dh.TinhTrangGiaoHang = true;
            dh.TinhTrangThanhToan = false;
            db.DonHangs.Add(dh);
            db.SaveChanges();
            //foreach (var item in lstGioHang)
            //{
            //    CTDonHang ctdh = new CTDonHang();
            //    ctdh.MaDonHang = dh.MaDonHang;
            //    ctdh.Product_Id = item.iMasp;
            //    ctdh.SoLuong = item.iSoLuong;
            //    ctdh.DonGia = (decimal)item.dDonGia;
            //    db.CTDonHangs.Add(ctdh);
            //}
            //db.SaveChanges();
            Session["Cart"] = null;
            ViewBag.mess = "Đặt hàng thành công";
            return RedirectToAction("Index", "Mercuryshop");
        }
        public ActionResult XacNhanDonHang()
        {
            return View();
        }
        // GET: ShoppingCart
        public ActionResult Index()
        {
            return View();
        }
    }
}