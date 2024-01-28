using System;
using DuanThuctap.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.IO;

namespace DuanThuctap.Controllers
{
    public class SanphamController : Controller
    {
        BANDONGHOEntities db = new BANDONGHOEntities();
        // GET: Sanpham

        public ActionResult Loaisanpham(int? page)
        {
            if (page == null) page = 1;

            var display = db.LOAISANPHAMs.Include(t => t.SANPHAMs).OrderBy(t => t.MALOAISP);
            int pagesize = 3;
            int pageNumber = (page ?? 1);

            return View(display.ToPagedList(pageNumber, pagesize));

        }
        public ActionResult Createloaisanpham()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Createloaisanpham([Bind(Include = "MALOAISP,TENLOAISP")] LOAISANPHAM loaisp)
        {
            if (ModelState.IsValid)
            {
                if (db.LOAISANPHAMs.Any(m => m.MALOAISP == loaisp.MALOAISP))
                {
                    loaisp.MALOAISP.Trim();
                    TempData["erorr"] = "Mã này đã tồn tại";
                    return View(loaisp);
                }
                db.LOAISANPHAMs.Add(loaisp);
                db.SaveChanges();
                return RedirectToAction("Loaisanpham");
            }
            return View();
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOAISANPHAM loaisp = db.LOAISANPHAMs.Find(id);
            if (loaisp == null)
            {
                return HttpNotFound();
            }
            return View(loaisp);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Deleteloaisp(string id)
        {
            LOAISANPHAM loaiSanPham = db.LOAISANPHAMs.Find(id);
            var hoadonsToRemove = db.Hoadons.Where(c => c.SANPHAM.MALOAISP == id).ToList();
            foreach (var hoadon in hoadonsToRemove)
            {
                foreach (var chitiet in hoadon.DONHANG.CHITIETDONHANGs.ToList())
                {
                    db.CHITIETDONHANGs.Remove(chitiet);
                }
                db.Hoadons.Remove(hoadon);
            }
            var sanpham = db.SANPHAMs.Where(c => c.MALOAISP == id);
            foreach (var sp in sanpham)
            {
                db.SANPHAMs.Remove(sp);
            }
            db.LOAISANPHAMs.Remove(loaiSanPham);
            db.SaveChanges();

            return RedirectToAction("Loaisanpham");
        }


        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOAISANPHAM loaisp = db.LOAISANPHAMs.Find(id);
            if (loaisp == null)
            {
                return HttpNotFound();
            }
            return View(loaisp);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MALOAISP,TENLOAISP")] LOAISANPHAM loaisp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loaisp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Loaisanpham");
            }
            else
            {
                ViewData["erorr1"] = "Chưa nhập dữ liệu đầy đủ";
            }
            ViewBag.MALOAISP = new SelectList(db.LOAISANPHAMs, "MALOAISP", "TENLOAISP", loaisp.MALOAISP);
            return View(loaisp);
        }
        public ActionResult SearchLoaisp(string search)
        {
            var display = db.LOAISANPHAMs.Include(m => m.SANPHAMs);
            if (!string.IsNullOrEmpty(search))
            {
                var displaymasp = display.Where(m => m.MALOAISP.Contains(search));
                var displaytensp = display.Where(m => m.TENLOAISP.Contains(search));
                display = displaymasp.Concat(displaytensp);
            }

            return View(display.ToList());
        }
        //Phần Thông tin sản phẩm
        public ActionResult Thongtinssanpham(string currentFilter, string searchString, int? page)
        {
            ViewBag.Tenloaisp = new SelectList(db.LOAISANPHAMs, "MALOAISP", "TENLOAISP");
            if (page == null) page = 1;

            var display = db.SANPHAMs.Include(t => t.THUONGHIEU).Include(t => t.LOAISANPHAM).Include(t => t.KHUYENMAI).OrderBy(t => t.MASP);
            int pagesize = 3;
            int pageNumber = (page ?? 1);
            ViewBag.currentSize = page;
            return View(display.ToPagedList(pageNumber, pagesize));
        }
        public ActionResult Createsanpham()
        {
            // Trong Controller
            ViewBag.MALOAISP = new SelectList(db.LOAISANPHAMs, "MALOAISP", "TENLOAISP");
            ViewBag.MATH = new SelectList(db.THUONGHIEUx, "MATH", "TENTH");
            ViewBag.MAKM = new SelectList(db.KHUYENMAIs, "MAKM", "TENKM");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Createsanpham([Bind(Include = "MASP,TENSP,HINHNHO,MOTA,MATH,DANHGIA,HINHLON,SOLUONG,MALOAISP,DONGIA,MAKM")] SANPHAM sanpham, HttpPostedFileBase HINHNHO, HttpPostedFileBase HINHLON)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (HINHNHO.ContentLength > 0)
                    {
                        string _FileName = Path.GetFileName(HINHNHO.FileName);
                        string _path = Path.Combine(Server.MapPath("~/Hinhsanpham/anhnho/"), _FileName);
                        HINHNHO.SaveAs(_path);
                        sanpham.HINHNHO = _FileName;
                    }
                    else
                    {
                        ViewBag.erorr = "Bạn chưa chọn ảnh nhỏ";
                    }
                    if (HINHLON.ContentLength > 0)
                    {
                        string _FileName1 = Path.GetFileName(HINHLON.FileName);
                        string _path1 = Path.Combine(Server.MapPath("~/Hinhsanpham/anhlon/"), _FileName1);
                        HINHLON.SaveAs(_path1);
                        sanpham.HINHLON = _FileName1;
                    }
                    else
                    {
                        ViewBag.erorr1 = "Bạn chưa chọn ảnh Lớn";
                    }
                    sanpham.MOTA.Trim();
                    db.SANPHAMs.Add(sanpham);
                    db.SaveChanges();
                    return RedirectToAction("Thongtinssanpham");
                }
                catch
                {
                    ViewBag.Message = "không thành công!!";
                }
            }
            ViewBag.MALOAISP = new SelectList(db.LOAISANPHAMs, "MALOAISP", "TENLOAISP");
            ViewBag.MATH = new SelectList(db.THUONGHIEUx, "MATH", "TENTH");
            ViewBag.MAKM = new SelectList(db.KHUYENMAIs, "MAKM", "TENKM");
            return View(sanpham);
        }

        public ActionResult Deletesp(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM loaisp = db.SANPHAMs.Find(id);
            if (loaisp == null)
            {
                return HttpNotFound();
            }
            return View(loaisp);
        }

        [HttpPost, ActionName("Deletesp")]
        [ValidateAntiForgeryToken]
        public ActionResult Deletesanpham(int id)
        {
            SANPHAM masp = db.SANPHAMs.Find(id);
            var chitietdh = db.CHITIETDONHANGs.Where(c => c.MASP == id);
            foreach (var chitiet in chitietdh)
            {
                db.CHITIETDONHANGs.Remove(chitiet);
            }
            var display = db.DONHANGs.Where(c => c.MADH == id);
            foreach (var donhang in display)
            {
                db.DONHANGs.Remove(donhang);
            }
            db.SANPHAMs.Remove(masp);
            db.SaveChanges();
            return RedirectToAction("Thongtinssanpham");
        }
        public ActionResult Editsp(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM sanpham = db.SANPHAMs.Find(id);
            if (sanpham == null)
            {
                return HttpNotFound();
            }
            ViewBag.MALOAISP = new SelectList(db.LOAISANPHAMs, "MALOAISP", "TENLOAISP", sanpham.MALOAISP);
            ViewBag.MATH = new SelectList(db.THUONGHIEUx, "MATH", "TENTH", sanpham.MATH);
            ViewBag.MAKM = new SelectList(db.KHUYENMAIs, "MAKM", "TENKM", sanpham.MAKM);
            return View(sanpham);

        }

        [HttpPost, ActionName("Editsp")]//lỗi
        [ValidateAntiForgeryToken]
        public ActionResult Editsanpham([Bind(Include = "MASP,TENSP,HINHNHO,MOTA,MATH,HINHLON,SOLUONG,MALOAISP,DONGIA,MAKM")] SANPHAM sanpham, HttpPostedFileBase HINHNHO, HttpPostedFileBase HINHLON)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (HINHNHO != null && HINHNHO.ContentLength > 0)
                    {
                        string _FileName = Path.GetFileName(HINHNHO.FileName);
                        string _path = Path.Combine(Server.MapPath("~/Hinhsanpham/anhnho/"), _FileName);
                        HINHNHO.SaveAs(_path);
                        sanpham.HINHNHO = _FileName;
                    }
                    if (HINHLON != null && HINHLON.ContentLength > 0)
                    {
                        string _FileName1 = Path.GetFileName(HINHLON.FileName);
                        string _path1 = Path.Combine(Server.MapPath("~/Hinhsanpham/anhlon/"), _FileName1);
                        HINHLON.SaveAs(_path1);
                        sanpham.HINHLON = _FileName1;
                    }
                    db.Entry(sanpham).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Thongtinssanpham");
                }
                catch
                {
                    ViewBag.Message = "Không thành công!!";
                }
            }
            else
            {
                ViewBag.Message1 = "Nhập đầy đủ dữ liệu";
            }

            ViewBag.MALOAISP = new SelectList(db.LOAISANPHAMs, "MALOAISP", "TENLOAISP", sanpham.MALOAISP);
            ViewBag.MATH = new SelectList(db.THUONGHIEUx, "MATH", "TENTH", sanpham.MATH);
            ViewBag.MAKM = new SelectList(db.KHUYENMAIs, "MAKM", "TENKM", sanpham.MAKM);
            return View(sanpham);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM sANPHAM = db.SANPHAMs.Find(id);
            if (sANPHAM == null)
            {
                return HttpNotFound();
            }
            return View(sANPHAM);
        }
        public ActionResult SearchProduct(string searchproduct, int? page)
        {
            var display = db.SANPHAMs.Include(m => m.LOAISANPHAM).Include(m => m.THUONGHIEU).Include(m => m.KHUYENMAI);
            if (!string.IsNullOrEmpty(searchproduct))
            {
                var displaymasp = display.Where(m => m.MASP.ToString().Contains(searchproduct));
                var displaytensp = display.Where(m => m.TENSP.Contains(searchproduct));
                display = displaymasp.Concat(displaytensp);
                if (page == null) page = 1;
                int pagesize = 3;
                int pageNumber = (page ?? 1);
                ViewBag.currentSize = page;
                display = display.OrderBy(m => m.MASP);
                return View(display.ToPagedList(pageNumber, pagesize));
            }
            return View();

        }
        //Phần thống kê sản phẩm
        public ActionResult Thongkesanpham()
        {
            var productCountByCategory = db.SANPHAMs
            .GroupBy(sp => sp.LOAISANPHAM.TENLOAISP)
            .Select(g => new ProductCountViewModel
            {
                CategoryName = g.Key,
                ProductCount = g.Count()
            })
            .ToList();

            return View(productCountByCategory);
        }
        public ActionResult Thuonghieu(int? page)
        {
            var display = db.THUONGHIEUx.ToList();
            if (page == null) page = 1;
            int pagesize = 3;
            int pageNumber = (page ?? 1);
            ViewBag.currentSize = page;
            return View(display.ToPagedList(pageNumber, pagesize));
        }


        public ActionResult Deletetrademark(int? id)
        {
            THUONGHIEU thuonghieu = db.THUONGHIEUx.Find(id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (thuonghieu == null)
            {
                return HttpNotFound();
            }
            return View(thuonghieu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deletetrademark(int id)
        {
            THUONGHIEU thuonghieu = db.THUONGHIEUx.Find(id);
            db.THUONGHIEUx.Remove(thuonghieu);
            db.SaveChanges();
            return RedirectToAction("Thuonghieu");
        }
        public ActionResult Createtrademark()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Createtrademark([Bind(Include = "MATH,TENTH,HINHTH")] THUONGHIEU thuonghieu, HttpPostedFileBase HINHTH)
        {
            if (ModelState.IsValid)
            {
                if (db.THUONGHIEUx.Any(m => m.MATH == thuonghieu.MATH))
                {
                    thuonghieu.MATH.ToString();
                    TempData["erorr"] = "Mã này đã tồn tại";
                    return View(thuonghieu);
                }
                if (HINHTH != null && HINHTH.ContentLength > 0)
                {
                    string filename = Path.GetFileName(HINHTH.FileName);
                    string path = Path.Combine(Server.MapPath("~/Hinhsanpham/thuonghieu/"), filename);
                    HINHTH.SaveAs(path);
                    thuonghieu.HINHTH = filename;
                }
                else
                {
                    TempData["erorr1"] = "Bạn chưa chọn ảnh";
                }
                db.THUONGHIEUx.Add(thuonghieu);
                db.SaveChanges();
                return RedirectToAction("Thuonghieu");
            }
            return View(thuonghieu);
        }


        public ActionResult Edittrademark(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            THUONGHIEU loaisp = db.THUONGHIEUx.Find(id);
            if (loaisp == null)
            {
                return HttpNotFound();
            }
            return View(loaisp);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edittrademark([Bind(Include = "MATH,TENTH,HINHTH")] THUONGHIEU thuonghieu, HttpPostedFileBase HINHTH, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (HINHTH != null)
                    {
                        string _FileName = Path.GetFileName(HINHTH.FileName);
                        string _path = Path.Combine(Server.MapPath("~/Hinhsanpham/thuonghieu/"), _FileName);
                        HINHTH.SaveAs(_path);
                        thuonghieu.HINHTH = _FileName;
                        // get Path of old image for deleting it
                        _path = Path.Combine(Server.MapPath("~/Hinhsanpham/thuonghieu/"), form["oldimage"]);
                        if (System.IO.File.Exists(_path))
                            System.IO.File.Delete(_path);

                    }
                    else
                        thuonghieu.HINHTH = form["oldimage"];
                    db.Entry(thuonghieu).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Thuonghieu");
                }
                catch
                {
                    ViewBag.Message = "không thành công!!";
                }

                return RedirectToAction("Thuonghieu");
            }
            return View(thuonghieu);
        }

        public ActionResult Searchtrademark(string searchtrademark)
        {
            var display = db.THUONGHIEUx.ToList();
            if (!string.IsNullOrEmpty(searchtrademark))
            {
                var displaymasp = display.Where(m => m.MATH.ToString().Contains(searchtrademark));
                var displaytensp = display.Where(m => m.TENTH.Contains(searchtrademark));
                display = displaymasp.Concat(displaytensp).ToList();
            }
            return View(display);
        }
        public ActionResult Khuyenmai(int? page)
        {
            var display = db.KHUYENMAIs.ToList();
            if (page == null) page = 1;
            int pagesize = 3;
            int pageNumber = (page ?? 1);
            ViewBag.currentSize = page;
            return View(display.ToPagedList(pageNumber, pagesize));
        }
        public ActionResult Deletepromotion(int? id)
        {
            KHUYENMAI khuyenmai = db.KHUYENMAIs.Find(id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (khuyenmai == null)
            {
                return HttpNotFound();
            }
            return View(khuyenmai);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deletepromotion(int id)
        {
            KHUYENMAI khuyenmai = db.KHUYENMAIs.Find(id);
            db.KHUYENMAIs.Remove(khuyenmai);
            db.SaveChanges();
            return RedirectToAction("Khuyenmai");
        }
        public ActionResult Createpromotion()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Createpromotion([Bind(Include = "MAKM,TENKM,PHANTRAM")] KHUYENMAI khuyenmai)
        {
            if (ModelState.IsValid)
            {
                db.KHUYENMAIs.Add(khuyenmai);
                db.SaveChanges();
                return RedirectToAction("Khuyenmai");
            }
            return View(khuyenmai);
        }


        public ActionResult Editpromotion(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHUYENMAI khuyenmai = db.KHUYENMAIs.Find(id);
            if (khuyenmai == null)
            {
                return HttpNotFound();
            }
            return View(khuyenmai);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editpromotion([Bind(Include = "MAKM,TENKM,PHANTRAM")] KHUYENMAI khuyenmai)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khuyenmai).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Khuyenmai");
            }
            return View(khuyenmai);
        }

        public ActionResult Searchpromotion(string searchpromotion)
        {
            var display = db.KHUYENMAIs.ToList();
            if (!string.IsNullOrEmpty(searchpromotion))
            {
                var displaymasp = display.Where(m => m.MAKM.ToString().Contains(searchpromotion));
                var displaytensp = display.Where(m => m.TENKM.Contains(searchpromotion));
                display = displaymasp.Concat(displaytensp).ToList();
            }
            return View(display);
        }
    }
}