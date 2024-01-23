using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DuanThuctap.Models;
using System.Data.Entity;
using PagedList;
using System.Net;

namespace DuanThuctap.Controllers
{
    public class ChamsockhachhangController : Controller
    {
        BANDONGHOEntities db = new BANDONGHOEntities();
        // GET: Chamsockhachhang
       
        public ActionResult Khachhang(int? page)
        {
            var display = db.KHACHHANGs.ToList();
            if (page == null) page = 1;
            int pagesize = 3;
            int pageNumber = (page ?? 1);
            return View(display.ToPagedList(pageNumber, pagesize));
        }
        public ActionResult Createcustomer()
        {
            ViewBag.MATK = new SelectList(db.TAIKHOANs,"MATK","MATK");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Createcustomer([Bind(Include = "MAKH,TENKH,MATK,SDT,EMAIL,GIOITINH,DIACHI")] KHACHHANG khachhang)
        {
            ViewBag.MATK = new SelectList(db.TAIKHOANs, "MATK", "MATK");
            if (ModelState.IsValid)
            {
                if (db.KHACHHANGs.Any(m => m.MATK == khachhang.MATK))
                {
                    TempData["erorr"] = "Mã tài khoản có người sử dụng. Vui lòng nhập mã khác !";
                    return View(khachhang);
                }
                db.KHACHHANGs.Add(khachhang);
                db.SaveChanges();
                return RedirectToAction("Khachhang");
            }
            else
            {
                TempData["erorr1"] = "Lỗi dữ liệu";
            }
            return View(khachhang);
        }

        public ActionResult Deletecustomer(int? id)
        {
            KHACHHANG khachhang = db.KHACHHANGs.Find(id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (khachhang == null)
            {
                return HttpNotFound();
            }
            return View(khachhang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deletecustomer(int id)
        {
            KHACHHANG khachhang = db.KHACHHANGs.Find(id);
            var Donhang = db.DONHANGs.Where(c => c.MADH == id);
            foreach (var chitiet in Donhang)//xóa 
            {
                db.DONHANGs.Remove(chitiet);
            }
            db.KHACHHANGs.Remove(khachhang);
            db.SaveChanges();
            return RedirectToAction("Khachhang");
        }

        public ActionResult Editcustomer(int? id)
        {
            ViewBag.MATK = new SelectList(db.TAIKHOANs,"MATK","MATK");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHACHHANG khachhang = db.KHACHHANGs.Find(id);
            if (khachhang == null)
            {
                return HttpNotFound();
            }
            return View(khachhang);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editcustomer([Bind(Include = "MAKH,TENKH,MATK,SDT,EMAIL,GIOITINH,DIACHI")] KHACHHANG khachhang)
        {
            ViewBag.MATK = new SelectList(db.TAIKHOANs, "MATK", "MATK");

            if (ModelState.IsValid)
            {
                if (db.KHACHHANGs.Any(m => m.MATK == khachhang.MATK))
                {
                    TempData["erorr"] = "Mã tài khoản có người sử dụng. Vui lòng nhập mã khác !";
                    return View(khachhang);
                }
                db.Entry(khachhang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Khachhang");
            }
            return View(khachhang);
        }

        public ActionResult Searchcustomer(string searchcustomer)
        {
            var display = db.KHACHHANGs.ToList();
            if (!string.IsNullOrEmpty(searchcustomer))
            {
                var displaymasp = display.Where(m => m.MAKH.ToString().Contains(searchcustomer));
                var displaytensp = display.Where(m => m.TENKH.Contains(searchcustomer));
                display = displaymasp.Concat(displaytensp).ToList();
            }
            return View(display);
        }

        public ActionResult Giaidap()
        {
            return View();
        }
    }
}