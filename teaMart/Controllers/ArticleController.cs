using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using teaMart.Models;

namespace teaMart.Controllers
{
    public class ArticleController : Controller
    {
        private readonly dbContext _context;

        public ArticleController(dbContext context)
        {
            _context = context;
        }

        // 文章列表
        // GET: Article
        public async Task<IActionResult> Index(string keyword = "", int page = 1)
        {
            IEnumerable<Article> List = _context.Articles;

            // 
            if (!string.IsNullOrEmpty(keyword))
            {
                List = List.Where(p => p.Title.Contains(keyword));
            }
            ViewBag.keyword = keyword;
          

            // 分页处理
            //限制分页的条数
            int pageSize = 10;
            //总条数有多少
            var total = List.Count();
            //一页10条的话，总的可以分多少页  total/10
            // 21条数据 每页10条 问：可以分多少页？  21/10 = 2.1 向上取整 得到3 实际可以分3页
            ViewBag.pageNum = Math.Ceiling(Convert.ToDecimal(total) / Convert.ToDecimal(pageSize));
            // 分页算法原理  显示第一页：（1-1）*10 = 0，10 得到的是 0-10条 
            // 显示第二页：（2-1）*10 = 10，10 得到的是 10-20条 
            List = List.OrderByDescending(p => p.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();


            return View(List);
        }

        // 文章详情: 在前端展示
        // GET: Article/Details/5 
       

        // 添加文章
        // GET: Article/Create
        public IActionResult Create()
        {
            return View();
        }

        // 保存添加文章
        // POST: Article/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Detail,Createtime,Sight")] Article article)
        {
            if (ModelState.IsValid)
            {
                _context.Add(article);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(article);
        }

        // 编辑文章
        // GET: Article/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }
        // 保存编辑文章
        // POST: Article/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Detail,Createtime,Sight")] Article article)
        {
            if (id != article.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(article);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.Id))
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
            return View(article);
        }


        // 删除文章
        // POST: Article/Delete/5

        public async Task<IActionResult> Delete(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article != null)
            {
                _context.Articles.Remove(article);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }
    }
}
