using Example.WebApps.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Web;
using AI.Facial.AgeAndGender;

namespace Example.WebApps.Controllers
{
    public class AgeAndGenderController : Controller
    {
        private readonly ILogger<AgeAndGenderController> _logger;
        private readonly AgeAnalyzer _ageAnalyzer;
        private readonly GenderAnalyzer _genderAnalyzer;
        public AgeAndGenderController(ILogger<AgeAndGenderController> logger)
        {
            _logger = logger;
            _ageAnalyzer = new AgeAnalyzer();
            _genderAnalyzer = new GenderAnalyzer();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        public async Task<JsonResult> UploadImageAsync([FromBody] ImageUploadModel model)
        {
            try
            {
                if (model?.ImageData != null)
                {
                    string base64Data = model.ImageData.Split(',')[1];
                    var ageResult = _ageAnalyzer.AnalyzeAgeFromBase64Async(base64Data).Result;
                    var genderResult = _genderAnalyzer.AnalyzeGenderFromBase64Async(base64Data).Result;

                    return Json(new { success = true, message = new { age = ageResult, gender = genderResult } });
                }
                return Json(new { success = false, message = "Invalid data!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }
    }
}
