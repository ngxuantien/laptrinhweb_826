using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
namespace Mercuryshop.Models
{
    public class Cart
    {
        MecuryshopEntities db = new MecuryshopEntities();
        public int iMasp { get; set; }
        public string sTensp { get; set; }
        public string sAnhBia { get; set; }
        public double dDonGia { get; set; }
        public int iSoLuong { get; set; }
        public double dThanhTien
        {
            get { return iSoLuong * dDonGia; }
        }
        public Cart(int ms)
        {
            iMasp = ms;
            Product s = db.Products.Single(n => n.Product_Id == iMasp);
            sTensp = s.Product_Name;
            sAnhBia = s.Product_Img;
            dDonGia = double.Parse(s.Product_Price.ToString());
            iSoLuong = 1;
        }
    }
}