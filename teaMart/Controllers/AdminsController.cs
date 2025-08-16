using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using teaMart.CommonUtil;
using teaMart.Models;

namespace teaMart.Controllers
{
    public class AdminsController : Controller
    {
        private readonly dbContext _context;

        public AdminsController(dbContext context)
        {
            _context = context;
        }

        // GET: User 管理员列表
        public async Task<IActionResult> Index(string phone="", string nickname="", string sex="", int page=1)
        {
            IEnumerable<User> userList = _context.Users.Where(u => u.Role == 1); // 只查询普通管理员

            // 
            if (!string.IsNullOrEmpty(phone))
            {
                userList = userList.Where(u => u.Phone.Contains(phone));
            }
            if (!string.IsNullOrEmpty(nickname))
            {
                userList = userList.Where(u => u.Nickname.Contains(nickname));
            }
            if (!string.IsNullOrEmpty(sex))
            {
                userList = userList.Where(u => u.Sex!=null && u.Sex.Contains(sex));
            }
            ViewBag.Phone = phone;
            ViewBag.Nickname = nickname;
            ViewBag.Sex = sex;

            // 分页处理
            //限制分页的条数
            int pageSize = 10;
            //总条数有多少
            var total = userList.Count();
            //一页10条的话，总的可以分多少页  total/10
            // 21条数据 每页10条 问：可以分多少页？  21/10 = 2.1 向上取整 得到3 实际可以分3页
            ViewBag.pageNum = Math.Ceiling(Convert.ToDecimal(total) / Convert.ToDecimal(pageSize));
            // 分页算法原理  显示第一页：（1-1）*10 = 0，10 得到的是 0-10条 
            // 显示第二页：（2-1）*10 = 10，10 得到的是 10-20条 
            userList = userList.OrderByDescending(p => p.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();


            return View(userList);
        }




        // GET: User/Details/5  管理员详情
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Create 管理员创建
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create  管理员保存

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Phone,Pwd,Nickname,Sex,Introduce,Age,Img,Mibao,Role")] User user)
        {
            // 新西兰手机号正则：以02开头，后面7~9位数字
            var nzPhonePattern = @"^02\d{7,9}$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(user.Phone ?? "", nzPhonePattern))
            {
                ModelState.AddModelError("Phone", "请输入有效的新西兰手机号（如02xxxxxxxx）");
                return View(user);
            }

            // 判断管理员是否存在
            var phoneExists = _context.Users.Any(u => u.Phone == user.Phone);
            if (phoneExists)
            {
                ModelState.AddModelError("Phone", "手机号已存在，请使用其他手机号。");
                return View(user);
            }

            //对管理员当前的密码进行加密处理
            user.Pwd = PasswordHelper.HashPasswordWithMD5(user.Pwd, PasswordHelper.GenerateSalt());
            // 设置默认头像
            if (string.IsNullOrEmpty(user.Img))
            {
                user.Img = "/assets/head.png"; // 默认头像路径
            }
            // 设置默认角色为普通管理员
            user.Role = 1; // 0表示普通管理员


            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: User/Edit/5 管理员编辑
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5 保存管理员编辑
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Phone,Pwd,Nickname,Sex,Introduce,Age,Img,Mibao,Role")] User user,string oldPwd)
        {

            if (id != user.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                try
                {

                   
                    
                    //如果输入了新的密码，则加密更新密码
                    if (oldPwd!=user.Pwd)
                    {
                        user.Pwd = PasswordHelper.HashPasswordWithMD5(user.Pwd, PasswordHelper.GenerateSalt());
                    }
                    
                    

                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }


        // POST: User/Delete/5 删除管理员
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
