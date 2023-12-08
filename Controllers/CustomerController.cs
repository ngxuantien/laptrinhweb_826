using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mercuryshop.Models;
using System.Data.Entity;
using System.Net.Mail;

namespace Mercuryshop.Controllers
{
    public class CustomerController : Controller
    {
        MecuryshopEntities db = new MecuryshopEntities();
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(FormCollection collection, Customer kh)
        {
            var sHoTen = collection["Name"];
            var sMatKhau = collection["Pass"];
            var sNhapLaiMK = collection["rePass"];
            var sDiaChi = collection["Address"];
            var sEmail = collection["Email"];
            var sDienThoai = collection["Numberphone"];
            if (String.IsNullOrEmpty(sHoTen))
            {
                ViewData["err1"] = "Họ tên không được rỗng";
            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["err2"] = "Phải nhập mật khẩu";
            }
            else if (String.IsNullOrEmpty(sNhapLaiMK))
            {
                ViewData["err2"] = "Phải nhập lại mật khẩu";
            }
            else if (sMatKhau != sNhapLaiMK)
            {
                ViewData[""] = "Mật khẩu nhập lại không khớp";
            }
            else if (String.IsNullOrEmpty(sEmail))
            {
                ViewData["err2"] = "Email không được rỗng";
            }
            else if (String.IsNullOrEmpty(sDienThoai))
            {
                ViewData["err2"] = "Số điện thoại không được rỗng";
            }
            else if (db.Customers.SingleOrDefault(n => n.Customer_Email == sEmail) != null)
            {
                ViewBag.ThongBao = "Email đã được sử dụng";
            }
            else
            {
                kh.Customer_Id = 0;
                kh.Customer_Name = sHoTen;
                kh.Customer_Password = sMatKhau;
                kh.Customer_Email = sEmail;
                kh.Customer_Address = sDiaChi;
                kh.Customer_Phone = sDienThoai;
                db.Customers.Add(kh);
                db.SaveChanges();
                return RedirectToAction("DangNhap");
            }
            return this.DangKy();
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var sTenDN = collection["Email"];
            var sMatKhau = collection["Pass"];
            if (String.IsNullOrEmpty(sTenDN))
            {
                ViewData["Err1"] = "Bạn chưa nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["Err2"] = "Phải nhập mật khẩu";
            }
            else
            {
                Customer kh = db.Customers.SingleOrDefault(n => n.Customer_Email == sTenDN && n.Customer_Password == sMatKhau);
                if (kh != null)
                {
                    ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                    Session["TaiKhoan"] = kh;
                    return RedirectToAction("Index", "Mercuryshop");
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }
        public ActionResult logout()
        {
            if(Session["Customers"] != null)
            {
                //Session["NameCustomers"] = null;
                Session["Customers"] = null;
            }
            return RedirectToAction("Index", "Mercuryshop");
        }

        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        // Dat lai mk
        [HttpGet]
        public ActionResult ResetPW()
        {
            return View();
        }

        [HttpPost]
        public ActionResult resetPW()
        {
            return View();
        }


        [HttpGet]
        public ActionResult RequestResetPX()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RequestResetPX(FormCollection collection)
        {
            var sEmail = collection["Email"];
            Customer TK = db.Customers.SingleOrDefault(n => n.Customer_Email == sEmail);
            if (TK != null)
            {
                ViewBag.ThongBao = "1";
                MailMessage mail = new MailMessage();
                mail.To.Add("testwebpbshop@gmail.com");
                mail.From = new MailAddress("testwebpbshop@gmail.com");
                mail.Subject = "Đặt lại mật khẩu";
                string Body = "Bấm vào đây để <a href='https://localhost:44364/User/ResetPW'>Đặt lại mật Khẩu</a>";
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("testwebpbshop@gmail.com", "zldo aupo wkff mqvi"); // Enter seders User name and password       
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
            else
            {
                ViewBag.ThongBao = " ";
            }

            return View();
        }
    }
}