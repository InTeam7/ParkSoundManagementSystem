using Microsoft.AspNetCore.Mvc;
using ParkSoundManagementSystem.Core.Services;
using System.Threading.Tasks;

namespace ParkSoundManagementSystem.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileProcessController: ControllerBase
    {
        private readonly ISystemProcessService _processService;
        public FileProcessController(ISystemProcessService processService)
        {
            _processService=processService;
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var name = await _processService.SetProcessAutomatically();
            return Ok(name);
        }
    }
}
