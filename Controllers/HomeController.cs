using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace UploadFile.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

       

        public IActionResult Index()
        {
            ViewBag.ServerName = Environment.MachineName.ToString();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
           
            string webRootPath = _webHostEnvironment.WebRootPath;

            if (file != null && file.Length > 0)
            {
                try
                {
                    // Define the directory to save the file
                    string uploadPath = Path.Combine(webRootPath, "UploadedFiles");

                    // Ensure the directory exists
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    // Save the file
                    string filePath = Path.Combine(uploadPath, file.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    ViewBag.Message = "File uploaded successfully!";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR: " + ex.Message;
                }
            }
            else
            {
                ViewBag.Message = "No file selected!";
            }

            return View("Index");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
