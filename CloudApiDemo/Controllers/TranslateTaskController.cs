using Domain;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CloudApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranslateTaskController : ControllerBase
    {
        private ICloudScaleService _cloudService;

        public TranslateTaskController(ICloudScaleService cloudService)
        {
            _cloudService = cloudService;
        }

        // POST api/<CloudTaskController>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> CreateAsync(TranslateRequest tasks)
        {
            await Task.Run(() => _cloudService.SendTranslateTasks(tasks));
            
            return Ok();
        }
    }
}
