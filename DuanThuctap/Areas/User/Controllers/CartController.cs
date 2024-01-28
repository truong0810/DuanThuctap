using DuanThuctap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DuanThuctap.Areas.User.Models
{
    public class CartController : Controller
    {
        // GET: Models/Cart
        public ActionResult Index(List<CHITIETDONHANG> sanPhams)
        {
            return View(sanPhams);
        }
        [HttpPost]
        public ActionResult LoadSanPham(List<CHITIETDONHANG> sanPhams)
        {
            return PartialView("_LoadSanPhamRow", sanPhams);
        }

       
    }
}