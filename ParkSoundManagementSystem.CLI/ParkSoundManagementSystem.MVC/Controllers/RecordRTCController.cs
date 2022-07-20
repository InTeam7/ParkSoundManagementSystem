using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ParkSoundManagementSystem.MVC.Controllers
{
    public class RecordRTCController : Controller
    {
        // GET: RecordRTCController
        private readonly IHubContext<NotifyHub> _hubContext;
        public RecordRTCController(IHubContext<NotifyHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public ActionResult Index()
        {
            return View();
        }

        // ---/RecordRTC/PostRecordedAudioVideo
        [HttpPost]
        public async Task<ActionResult> PostRecordedAudioVideo()
        {
            if (Request.Form.Files.Any())
            {
                var file = Request.Form.Files["audio-blob"];
                string UploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles");
                var key = Request.Form.Keys;
                var value = key.FirstOrDefault(x => x != "audio-filename");
                string UniqueFileName = value;
                string UploadPath = Path.Combine(UploadFolder, UniqueFileName);
                var fs = new FileStream(UploadPath, FileMode.Create);
                await file.CopyToAsync(fs);
                fs.Close();
                await _hubContext.Clients.All.SendAsync("DownLoad", UniqueFileName);
            }
            return Json(HttpStatusCode.OK);
        }

        // ---/RecordRTC/DeleteFile
        [HttpPost]
        public ActionResult DeleteFile()
        {
            string UploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles");
            var fileUrl = UploadFolder + Request.Form["delete-file"];
            new FileInfo(fileUrl + ".wav").Delete();
            new FileInfo(fileUrl + ".webm").Delete();
            return Json(true);
        }


    }
}
