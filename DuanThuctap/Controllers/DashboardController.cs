using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DuanThuctap.Models;
namespace DuanThuctap.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        BANDONGHOEntities db = new BANDONGHOEntities();
        public ActionResult Index()
        {
            // Lấy dữ liệu đơn hàng từ cơ sở dữ liệu
            var donHangs = db.DONHANGs.ToList();

            // Lọc và nhóm dữ liệu đơn hàng theo tháng, loại bỏ đơn hàng có trạng thái null
            var thongKeTheoThang = donHangs
                .Where(d => d.NGAYDAT.HasValue && d.TRANGTHAI != null) // Loại bỏ đơn hàng không có ngày đặt hoặc có trạng thái null
                .GroupBy(d => new { Thang = d.NGAYDAT.Value.Month, Nam = d.NGAYDAT.Value.Year })
                .Select(g => new
                {
                    Thang = g.Key.Thang,
                    Nam = g.Key.Nam,
                    DoanhThu = g.Sum(d => d.TONGTIEN ?? 0) // Tổng doanh thu của từng tháng
        })
                .OrderBy(g => g.Nam)
                .ThenBy(g => g.Thang)
                .ToList();

            // Chuẩn bị dữ liệu cho biểu đồ Highcharts
            var categories = thongKeTheoThang.Select(g => $"{g.Thang}/{g.Nam}").ToArray();
            var doanhThuData = thongKeTheoThang.Select(g => g.DoanhThu).ToArray();

            // Truyền dữ liệu vào biểu đồ Highcharts
            ViewBag.Categories = categories;
            ViewBag.DoanhThuData = doanhThuData;

            return View();
        }


    }


}