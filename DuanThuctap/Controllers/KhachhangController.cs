using DuanThuctap.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DuanThuctap.Controllers
{
    public class KhachhangController : Controller
    {
        BANDONGHOEntities db = new BANDONGHOEntities();
        // GET: Khachhang
        public ActionResult Loaitaikhoan(int? page)
        {
            var display = db.LOAITKs.ToList();
            if (page == null) page = 1;
            int pagesize = 3;
            int pageNumber = (page ?? 1);
            return View(display.ToPagedList(pageNumber, pagesize));
        }

        public ActionResult Createloaitk()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Createloaitk([Bind(Include = "MALOAITK,TENLOAITK")] LOAITK loaitk)
        {
            if (ModelState.IsValid)
            {
                if (db.LOAITKs.Any(m => m.MALOAITK == loaitk.MALOAITK))
                {
                    loaitk.MALOAITK.Trim();
                    TempData["erorr"] = "Mã này đã tồn tại";
                    return View(loaitk);
                }
                db.LOAITKs.Add(loaitk);
                db.SaveChanges();
                return RedirectToAction("Loaitaikhoan");
            }
            return View(loaitk);
        }

        public ActionResult Editloaitk(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOAITK loaitk = db.LOAITKs.Find(id);
            if (loaitk == null)
            {
                return HttpNotFound();
            }
            return View(loaitk);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editloaitk([Bind(Include = "MALOAITK,TENLOAITK")] LOAITK loaitk)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loaitk).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Loaitaikhoan");
            }
            return View(loaitk);
        }

        // GET: TAIKHOANs/Delete/5
        public ActionResult Deleteloaitk(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOAITK loaitk = db.LOAITKs.Find(id);

            if (loaitk == null)
            {
                return HttpNotFound();
            }
            return View(loaitk);
        }

        // POST: TAIKHOANs/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deleteloaitk(string id, int? page)
        {
            LOAITK loaitk = db.LOAITKs.Find(id);
            db.LOAITKs.Remove(loaitk);
            db.SaveChanges();
            return RedirectToAction("Loaitaikhoan");
        }
        public ActionResult Searchloaitk(string searchloaitk)
        {
            var display = db.LOAITKs.ToList();
            if (!string.IsNullOrEmpty(searchloaitk))
            {
                var displaymasp = display.Where(m => m.MALOAITK.Contains(searchloaitk));
                var displaytensp = display.Where(m => m.TENLOAITK.Contains(searchloaitk));
                display = displaymasp.Concat(displaytensp).ToList();
            }

            return View(display);
        }

        public ActionResult Taikhoan(int? page)
        {
            var display = db.TAIKHOANs.ToList();
            if (page == null) page = 1;
            int pagesize = 3;
            int pageNumber = (page ?? 1);
            ViewBag.MALOAITK = new SelectList(db.LOAITKs, "MALOAITK", "TENLOAITK");
            return View(display.ToPagedList(pageNumber, pagesize));
        }
        public ActionResult Createaccount()
        {
            ViewBag.MALOAITK = new SelectList(db.LOAITKs, "MALOAITK", "TENLOAITK");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Createaccount([Bind(Include = "MATK,TENDN,MATKHAU,NGAYDANGKY,TRANGTHAI,MALOAITK,EMAIL")] TAIKHOAN tk)
        {
            ViewBag.MALOAITK = new SelectList(db.LOAITKs, "MALOAITK", "TENLOAITK", tk.MALOAITK);
            if (ModelState.IsValid)
            {
                if (db.TAIKHOANs.Any(m => m.MATK == tk.MATK))//chuyển thành tên người dùng-nếu có thời gian
                {
                    TempData["erorr"] = "Mã này đã tồn tại";
                    return View(tk);
                }
                tk.TRANGTHAI = true;
                tk.NGAYDANGKY = DateTime.Now;
                db.TAIKHOANs.Add(tk);
                db.SaveChanges();
                return RedirectToAction("Taikhoan");
            }
            else
            {
                TempData["erorr1"] = "Lỗi dữ liệu";
            }
            return View(tk);
        }

        public ActionResult Editaccount(int? id)
        {
            ViewBag.MALOAITK = new SelectList(db.LOAITKs, "MALOAITK", "TENLOAITK");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAIKHOAN tk = db.TAIKHOANs.Find(id);
            if (tk == null)
            {
                return HttpNotFound();
            }
            return View(tk);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editaccount([Bind(Include = "MATK,TENDN,MATKHAU,NGAYDANGKY,TRANGTHAI,MALOAITK,EMAIL")] TAIKHOAN tk)
        {
            ViewBag.MALOAITK = new SelectList(db.LOAITKs, "MALOAITK", "TENLOAITK", tk.MALOAITK);
            if (ModelState.IsValid)
            {

                db.Entry(tk).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Taikhoan");
            }
            return View(tk);
        }
        public ActionResult Deleteaccount(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAIKHOAN tk = db.TAIKHOANs.Find(id);

            if (tk == null)
            {
                return HttpNotFound();
            }
            return View(tk);
        }

        // POST: TAIKHOANs/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deleteaccount(int id, int? page)
        {
            try
            {
                // Tìm bản ghi TAIKHOAN
                TAIKHOAN tk = db.TAIKHOANs.Find(id);

                // Tìm các bản ghi KHACHHANG liên quan
                var taikhoan = db.KHACHHANGs.Where(c => c.MATK == id).ToList();
                foreach (var chitiet in taikhoan)
                {
                    // Tìm các bản ghi DONHANG liên quan cho mỗi KHACHHANG
                    var donhangs = db.DONHANGs.Where(d => d.MAKH == chitiet.MAKH).ToList();
                    foreach (var donhang in donhangs)
                    {
                        db.DONHANGs.Remove(donhang);
                    }

                    // Xóa bản ghi KHACHHANG
                    db.KHACHHANGs.Remove(chitiet);
                }

                // Xóa bản ghi TAIKHOAN
                db.TAIKHOANs.Remove(tk);

                // Lưu các thay đổi vào cơ sở dữ liệu
                db.SaveChanges();

                // Chuyển hướng đến hành động mong muốn
                return RedirectToAction("Taikhoan");
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ một cách thích hợp (ghi log, hiển thị thông báo lỗi, v.v.)
                TempData["error"] = "Lỗi khi xóa tài khoản: " + ex.Message;
                return RedirectToAction("Taikhoan");
            }
        }

        public ActionResult Searchaccount(string searchaccount)
        {
            var display = db.TAIKHOANs.ToList();
            if (!string.IsNullOrEmpty(searchaccount))
            {
                var displaymasp = display.Where(m => m.MATK.ToString().Contains(searchaccount));
                var displaytensp = display.Where(m => m.TENDN.Contains(searchaccount));
                display = displaymasp.Concat(displaytensp).ToList();
            }

            return View(display);
        }
        public ActionResult Thongketaikhoan()
        {
            var statisticalByCategory = db.TAIKHOANs
                .GroupBy(c => c.LOAITK.TENLOAITK)
                .Select(group => new StatisticalAccount
                {
                    Categoryaccount = group.Key,
                    AccountCount = group.Count()
                })
                .OrderBy(c => c.AccountCount)
                .ToList();

            var statisticalByMonth = db.TAIKHOANs
                .Where(t => t.NGAYDANGKY != null)
                .GroupBy(t => new { t.NGAYDANGKY.Value.Month, t.NGAYDANGKY.Value.Year })
                .Select(group => new StatisticalAccount
                {
                    Month = group.Key.Month,
                    Year=group.Key.Year,
                    AccountCount = group.Count()
                })
                .OrderBy(group => group.Year)
                .ToList();

            var combinedStatistical = new CombinedStatistical
            {
                StatisticalByCategory = statisticalByCategory,
                StatisticalByMonth = statisticalByMonth
            };

            return View(combinedStatistical);
        }

    }

}