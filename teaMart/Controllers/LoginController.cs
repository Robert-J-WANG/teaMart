using Microsoft.AspNetCore.Mvc;
using teaMart.CommonUtil;
using teaMart.Models;

namespace teaMart.Controllers
{
    // 登录控制器，处理与用户登录相关的请求
    public class LoginController : Controller
    {
        // 数据库上下文对象，用于访问数据库
        private readonly dbContext _dbContext;
        // 依赖注入HttpContextAccessor的接口
        private readonly IHttpContextAccessor _httpContextAccessor;


        // 通过依赖注入方式获取数据库上下文
        public LoginController(dbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: /Login/Index
        // 返回登录页面视图
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Login/Index
        // 处理用户登录请求
        [HttpPost]
        public IActionResult Index(string phone, string pwd)
        {
            // 对用户输入的密码进行MD5加密（加盐）
            string newPwd = PasswordHelper.HashPasswordWithMD5(pwd, PasswordHelper.GenerateSalt());

            // 查询数据库，查找手机号和加密密码都匹配且角色大于等于1的用户
            User user = _dbContext.Users.Where(u => u.Phone == phone && u.Pwd == newPwd && u.Role >= 1).FirstOrDefault();

            // 判断用户是否存在，登录是否成功
            if (user != null)
            {
                // 登录成功，将用户信息存入Session
                _httpContextAccessor.HttpContext.Session.SetInt32("id", user.Id);
                _httpContextAccessor.HttpContext.Session.SetString("nickname", user.Nickname);
                _httpContextAccessor.HttpContext.Session.SetInt32("role", (int)user.Role);
                _httpContextAccessor.HttpContext.Session.SetString("img2", user.Img);
                // 登录成功，返回成功信息和用户数据 
                return Ok(new
                {
                    code = 200,
                    msg = "登录成功",
                });
            }

            // 登录失败，返回错误信息
            return Ok(new
            {
                code = 201,
                msg = "手机号或密码错误"
            });

            // return View(); // 可选：返回视图
        }

        //管理员退出系统
        public IActionResult Logout()
        {
            // 清除Session中的用户信息
            _httpContextAccessor.HttpContext.Session.Remove("id");
            _httpContextAccessor.HttpContext.Session.Remove("nickname");
            _httpContextAccessor.HttpContext.Session.Remove("role");
            _httpContextAccessor.HttpContext.Session.Remove("img2");

            // 重定向到登录页面
            return Redirect("/Login/Index");
        }

    }
}