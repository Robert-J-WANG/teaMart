using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using teaMart.Models;

namespace teaMart.Controllers
{
    public class AdminController : Controller
    {
        // 数据库上下文对象，用于访问数据库
        private readonly dbContext _dbContext;
        // 依赖注入HttpContextAccessor的接口
        private readonly IHttpContextAccessor _httpContextAccessor;

        // 构造函数注入依赖
        public AdminController(dbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        // 后台管理首页的信息展示方法
        public IActionResult Index()
        {
            //1， 今天平台的收入
            var startOfToday = DateTime.Now.Date; // 获取今天的开始时间
            //var startOfToday = DateTime.Parse("2024-09-26"); // 获取2024-9-26的0点
            var endOfToday = startOfToday.AddDays(1); // 获取明天的开始时间
            ViewBag.todayMoney = _dbContext.Orders
                .Where(o => o.IsPay == 1 && o.Createtime >= startOfToday && o.Createtime < endOfToday)
                .Sum(o => (Decimal?)o.SumPrice) ?? 0; // 计算今天的总收入

            //2， 本周的收入
            DateTime dateTime = DateTime.Now;
            DateTime startOfWeek = dateTime.AddDays(-(int)dateTime.DayOfWeek+1).Date; // 周一 0.0.0
            DateTime endOfWeek = startOfWeek.AddDays(7).AddMicroseconds(-1); // 下周一 23.59.59
            ViewBag.startOfWeek = startOfWeek; // 本周开始时间
            ViewBag.endOfWeek = endOfWeek; // 本周结束时间（周日）

            ViewBag.weekMoney = _dbContext.Orders
                .Where(o => o.IsPay == 1 && o.Createtime >= startOfWeek && o.Createtime < endOfWeek)
                .Sum(o => (Decimal?)o.SumPrice) ?? 0; // 计算本周的总收入

            //3， 今年的收入
            DateTime startOfYear = new DateTime(DateTime.Now.Year, 1, 1, 0, 0, 0); // 获取今年的开始时间
            DateTime endOfYear = new DateTime(DateTime.Now.Year + 1, 1, 1, 0, 0, 0); // 获取明年开始时间
            ViewBag.yearMoney = _dbContext.Orders
                .Where(o => o.IsPay == 1 && o.Createtime >= startOfYear && o.Createtime < endOfYear)
                .Sum(o => (Decimal?)o.SumPrice) ?? 0; // 计算今年的总收入

            //4， 今天的订单详情（取前10条）
            ViewBag.orderList = _dbContext.Orders.Include(o => o.UidNavigation)
                .Where(o => o.Createtime >= startOfToday && o.Createtime < endOfToday)
                .OrderByDescending(o => o.Id)
                .Take(10)
                .ToList(); // 获取今天的订单详情

            return View();
        }
    }
}
