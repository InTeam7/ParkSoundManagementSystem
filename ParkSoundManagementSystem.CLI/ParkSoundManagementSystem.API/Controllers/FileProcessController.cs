using Microsoft.AspNetCore.Mvc;
using ParkSoundManagementSystem.Core.Services;
using System.Threading.Tasks;

namespace ParkSoundManagementSystem.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileProcessController : ControllerBase
    {
        private readonly ISystemProcessService _processService;
        public FileProcessController(ISystemProcessService processService)
        {
            _processService = processService;
        }
        [HttpGet]
        [Route("create")]
        public async Task<IActionResult> Create()
        {
            var name = await _processService.SetProcessAutomatically();
            return Ok(name);
        }
        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> GetName()
        {
            var name = await _processService.GetProcessName();
            return Ok(name);
        }
        [HttpPost]
        [Route("set")]
        public async Task<IActionResult> SetName(string name)
        {
            var text = await _processService.SetProcess(name);
            return Ok(text);
        }
    }
}
