using Microsoft.AspNetCore.Mvc;
using teaMart.Models;

namespace teaMart.Controllers
{
    public class AdminController : Controller
    {
        // 数据库上下文对象，用于访问数据库
        private readonly dbContext _dbContext;
        // 依赖注入HttpContextAccessor的接口
        private readonly IHttpContextAccessor _httpContextAccessor;





        public IActionResult Index()
        {
            
            return View();


            
                
            
        }
    }
}
