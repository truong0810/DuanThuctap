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
    public class BanhangController : Controller
    {
        BANDONGHOEntities db = new BANDONGHOEntities();
        // GET: Banhang
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Donhang(int? page)
        {
            var display = db.DONHANGs.Where(o => o.TRANGTHAI == null).ToList();
            if(page == null) page = 1;
            int pagesize = 5;
            int pageNumber = (page ?? 1);
            return View(display.ToPagedList(pageNumber, pagesize));
        }
        public ActionResult Deleteorder(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DONHANG display = db.DONHANGs.Find(id);

            if (display == null)
            {
                return HttpNotFound();
            }
            return View(display);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deleteorder(int id)
        {
            DONHANG donhang = db.DONHANGs.Find(id);

            if (donhang == null)
            {
                return HttpNotFound();
            }
            // Remove associated CHITIETDONHANG entries first
           
            // Now remove the order itself
            db.DONHANGs.Remove(donhang);

            db.SaveChanges();

            return RedirectToAction("Donhang");
        }
        public ActionResult ApproveOrder(int? id)
        {
            // Lấy đơn hàng từ cơ sở dữ liệu dựa trên ID
            var order = db.DONHANGs.FirstOrDefault(o => o.MADH == id);
            if (order != null)
            {
                // Thực hiện các thay đổi trong đơn hàng đã duyệt
                order.TRANGTHAI = "Đang chuẩn bị hàng";
                DateTime date = DateTime.Now;
                order.NGAYDAT = DateTime.Parse(date.ToString("dd-MM-yyyy"));
                order.NGAYGIAO = order.NGAYDAT?.AddDays(3);
                // Lấy đối tượng Hoadon tương ứng từ cơ sở dữ liệu
                var hoadon = db.Hoadons.FirstOrDefault(h => h.MADH == id);
                if (hoadon == null)
                {
                    // Tạo mới một hóa đơn nếu không tìm thấy
                    hoadon = new Hoadon
                    {
                        MADH = order.MADH,
                        MAKH = (int)order.MAKH,
                        MASP = (int)order.MASP,
                        // Gán các thuộc tính khác nếu có
                    };
                    // Thêm hóa đơn mới vào DbSet
                    db.Hoadons.Add(hoadon);
                }
                else
                {
                    // Cập nhật thông tin từ DONHANG vào Hoadon
                    hoadon.MAKH = (int)order.MAKH;
                    hoadon.MASP = (int)order.MASP;
                    // Cập nhật các thuộc tính khác nếu có
                }
                // Lưu thay đổi vào cơ sở dữ liệu
                db.SaveChanges();
            }
            return RedirectToAction("Donhangdaduyet");
        }

        public ActionResult Donhangdaduyet(int? page)
        {
            var display = db.Hoadons.Include("SANPHAM").Include("KHACHHANG").Include("DONHANG").ToList();
            if (page == null) page = 1;
            int pagesize = 5;
            int pageNumber = (page ?? 1);
            return View(display.ToPagedList(pageNumber, pagesize));
        }
        
        public ActionResult Editorderapproved(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hoadon loaitk = db.Hoadons.Find(id);
            if (loaitk == null)
            {
                return HttpNotFound();
            }
            return View(loaitk);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editorderapproved([Bind(Include = "MADH,MASP,MAKH,MADH")] Hoadon hoadon)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hoadon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Donhangdaduyet");
            }
            else
            {
                ViewBag.erorr = "Lỗi dữ liệu";
            }
            return View(hoadon);
        }
        public ActionResult Danhgia_Nhanxet(int? page)
        {
            var display = db.DONHANGs.Include("KHACHHANG").ToList();
            if (page == null) page = 1;
            int pagesize = 15;
            int pageNumber = (page ?? 1);
            return View(display.ToPagedList(pageNumber, pagesize));
        }
    }
}