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
    public class ImageChartController : Controller
    {
        private readonly dbContext _context;

        public ImageChartController(dbContext context)
        {
            _context = context;
        }

        // GET: ImageChart 轮播图列表
        public async Task<IActionResult> Index(string url="", int state=-1,int page = 1)
        {
            IEnumerable<ImageChart> imageChartList = _context.ImageCharts;

            // 
            if (!string.IsNullOrEmpty(url))
            {
                imageChartList = imageChartList.Where(image => image.Url.Contains(url));
            }
            if (state!=-1)
            {
                imageChartList = imageChartList.Where(image => image.State == state);
            }
            
            ViewBag.url = url;
            ViewBag.state = state;
          
            // 分页处理
            //限制分页的条数
            int pageSize = 10;
            //总条数有多少
            var total = imageChartList.Count();
            //一页10条的话，总的可以分多少页  total/10
            // 21条数据 每页10条 问：可以分多少页？  21/10 = 2.1 向上取整 得到3 实际可以分3页
            ViewBag.pageNum = Math.Ceiling(Convert.ToDecimal(total) / Convert.ToDecimal(pageSize));
            // 分页算法原理  显示第一页：（1-1）*10 = 0，10 得到的是 0-10条 
            // 显示第二页：（2-1）*10 = 10，10 得到的是 10-20条 
            imageChartList = imageChartList.OrderByDescending(p => p.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();


            return View(imageChartList);
        }



        // GET: ImageChart/Create 轮播图添加
        public IActionResult Create()
        {
            return View();
        }


        // 轮播图添加保存
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Url,ImageUrl,Createtime,State")] ImageChart imageChart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(imageChart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(imageChart);
        }

        // GET: ImageChart/Edit/5 轮播图编辑
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imageChart = await _context.ImageCharts.FindAsync(id);
            if (imageChart == null)
            {
                return NotFound();
            }
            return View(imageChart);
        }

        // 轮播图编辑保存
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Url,ImageUrl,Createtime,State")] ImageChart imageChart)
        {
            if (id != imageChart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(imageChart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageChartExists(imageChart.Id))
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
            return View(imageChart);
        }



        // POST: ImageChart/Delete/5   轮播图删除
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var imageChart = await _context.ImageCharts.FindAsync(id);
            if (imageChart != null)
            {
                _context.ImageCharts.Remove(imageChart);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImageChartExists(int id)
        {
            return _context.ImageCharts.Any(e => e.Id == id);
        }
    }
}
