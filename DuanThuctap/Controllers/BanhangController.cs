using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DuanThuctap.Controllers
{
    public class BanhangController : Controller
    {
        // GET: Banhang
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Donhang()
        {
            return View();
        }
        public ActionResult Danhgia_Nhanxet()
        {
            return View();
        }
    }
}