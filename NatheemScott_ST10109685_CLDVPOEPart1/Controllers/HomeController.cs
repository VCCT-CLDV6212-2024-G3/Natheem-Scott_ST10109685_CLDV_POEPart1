using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using NatheemScott_ST10109685_CLDVPOEPart1.Services;
using NatheemScott_ST10109685_CLDVPOEPart1.Models;
using System.Threading.Tasks;

namespace NatheemScott_ST10109685_CLDVPOEPart1.Controllers
{
    public class HomeController : Controller
    {
        private readonly BlobService _blobService;
        private readonly TableService _tableService;
        private readonly QueueService _queueService;
        private readonly FileService _fileService;

        public HomeController(BlobService blobService, TableService tableService, QueueService queueService, FileService fileService)
        {
            _blobService = blobService;
            _tableService = tableService;
            _queueService = queueService;
            _fileService = fileService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpldImage(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                using (var stream = file.OpenReadStream())
                {
                    await _blobService.UploadBlobAsync("images", fileName, stream);
                }
                ViewBag.Message = "Image uploaded successfully";
            }
            else
            {
                ViewBag.Message = "Please select a file to upload";
            }
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddCustProfile(CustomerProf profile)
        {
            if (ModelState.IsValid)
            {
                await _tableService.AddEntityAsync(profile);
                ViewBag.Message = "Customer profile added successfully";
            }
            else
            {
                ViewBag.Message = "Error adding customer profile";
            }
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ProcOrder(string orderId)
        {
            if (!string.IsNullOrEmpty(orderId))
            {
                await _queueService.SendMessageAsync("orders", orderId);
                ViewBag.Message = "Order queued for processing";
            }
            else
            {
                ViewBag.Message = "Please enter an order ID";
            }
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpldContract(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                using (var stream = file.OpenReadStream())
                {
                    await _fileService.UploadFileAsync("contracts", fileName, stream);
                }
                ViewBag.Message = "Contract uploaded successfully";
            }
            else
            {
                ViewBag.Message = "Please select a file to upload";
            }
            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}