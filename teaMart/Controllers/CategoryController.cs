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
    public class CategoryController : Controller
    {
        private readonly dbContext _context;

        public CategoryController(dbContext context)
        {
            _context = context;
        }

        // GET: Category
        public async Task<IActionResult> Index(string catename = "", int page = 1)
        {
            IEnumerable<Category> categoryList = _context.Categories;

            // 
            if (!string.IsNullOrEmpty(catename))
            {
                categoryList = categoryList.Where(cate => !string.IsNullOrEmpty(cate.Catename) && cate.Catename.Contains(catename));
            }
            

            ViewBag.catename = catename;

            // 分页处理
            //限制分页的条数
            int pageSize = 10;
            //总条数有多少
            var total = categoryList.Count();
            //一页10条的话，总的可以分多少页  total/10
            // 21条数据 每页10条 问：可以分多少页？  21/10 = 2.1 向上取整 得到3 实际可以分3页
            ViewBag.pageNum = Math.Ceiling(Convert.ToDecimal(total) / Convert.ToDecimal(pageSize));
            // 分页算法原理  显示第一页：（1-1）*10 = 0，10 得到的是 0-10条 
            // 显示第二页：（2-1）*10 = 10，10 得到的是 10-20条 
            categoryList = categoryList.OrderByDescending(p => p.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();


            return View(categoryList);
        }



        //添加种类
        // GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }


        // 保存添加
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Catename")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // 编辑种类
        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // 保存编辑
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Catename")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }



        // 删除种类
        // POST: Category/Delete/5
      
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
