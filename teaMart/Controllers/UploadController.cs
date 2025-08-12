using Microsoft.AspNetCore.Mvc;

namespace teaMart.Controllers
{
    public class UploadController : Controller
    {
        // 注入 IWebHostEnvironment，用于获取应用根目录，便于文件保存
        private readonly IWebHostEnvironment _environment;

        public UploadController(IWebHostEnvironment environment)
        {
            this._environment = environment;
        }

        // 上传方法：用于异步上传图片文件
        [HttpPost]
        public async Task<IActionResult> file(IFormFile pic)
        {
            try
            {
                // 1. 检查是否有文件上传
                if (pic != null)
                {
                    // 2. 检查文件是否为空
                    if (pic.Length == 0)
                    {
                        // 返回 209，前端可根据此状态提示“请选择图片”
                        return Content("209");
                    }
                    else
                    {
                        // 3. 检查文件扩展名是否合法（只允许 gif/png/jpg/jpeg）
                        string backFix = Path.GetExtension(pic.FileName);
                        if (backFix != ".gif" && backFix != ".png" && backFix != ".jpg" && backFix != ".jpeg")
                        {
                            // 返回 210，前端可提示“格式不对”
                            return Content("210");
                        }

                        // 4. 生成文件名，避免重名（用时间戳+后缀）
                        string fileName = DateTime.Now.ToString("MMddHHmmss") + backFix;

                        // 5. 组合保存目录路径（wwwroot/pic），如不存在则自动创建
                        string dir = Path.Combine(_environment.ContentRootPath, "wwwroot", "pic");
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }
                        string filePath = Path.Combine(dir, fileName);

                        // 6. 保存文件到服务器指定目录
                        using (var fs = System.IO.File.Create(filePath))
                        {
                            await pic.CopyToAsync(fs);
                        }

                        // 7. 返回图片的相对路径，前端可直接用于图片显示
                        return Content("/pic/" + fileName);
                    }
                }
                else
                {
                    // 没有选择文件，返回 300
                    return Content("300");
                }
            }
            catch (Exception ex)
            {
                // 捕获异常，返回 400，前端可提示“上传失败”
                
                // 这里简单返回错误信息，实际应用中应记录日志
                Console.WriteLine(ex.Message);

                return Content("400");
            }
        }
    }
}


//需要注意的事项
//1.	文件类型校验
//只允许 .gif、.png、.jpg、.jpeg，但不能防止伪造扩展名，建议后端进一步校验文件内容（如 MIME 类型）。
//2.	文件名唯一性
//仅用时间戳可能会有重复风险，建议加上 Guid 或用户ID等更保险。
//3.	目录权限
//wwwroot/pic 目录必须有写入权限，否则会抛异常。
//4.	异常处理
//生产环境应记录详细异常日志，便于排查问题。
//5.	安全性
//•	不要允许用户上传可执行文件或脚本，防止安全漏洞。
//•	返回的路径仅供前端显示图片，不应暴露服务器绝对路径。
//6.	大文件限制
//可在 appsettings.json 或 Startup 配置最大上传文件大小，防止恶意大文件攻击。
//7.	前端配合
//前端需根据返回的状态码（209/210/300/400）做出相应提示，提升用户体验。