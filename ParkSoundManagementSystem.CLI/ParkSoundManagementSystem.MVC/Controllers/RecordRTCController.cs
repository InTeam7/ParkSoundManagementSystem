using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Net;

namespace ParkSoundManagementSystem.MVC.Controllers
{
    public class RecordRTCController : Controller
    {
        // GET: RecordRTCController
        public ActionResult Index()
        {
            return View();
        }

        // ---/RecordRTC/PostRecordedAudioVideo
        [HttpPost]
        public ActionResult PostRecordedAudioVideo()
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
                file.CopyTo(fs);
                fs.Close();
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
