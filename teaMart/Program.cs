using Microsoft.EntityFrameworkCore;
using teaMart.Models;
using UEditorNetCore;


namespace teaMart
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //==== 添加数据库上下文对象  ======
            builder.Services.AddDbContext<dbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("dblink")));

            // 添加Session服务
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSession();

            //添加ueditor服务
            builder.Services.AddUEditorService();
            builder.Services.AddMvc();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            // 使用Session中间件
            app.UseSession();

            app.MapControllerRoute(
                name: "default",
            //pattern: "{controller=Home}/{action=Index}/{id?}");
            pattern: "{controller=Admin}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
