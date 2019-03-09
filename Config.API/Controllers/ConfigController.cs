using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Config.Lib.Models;
using Microsoft.AspNetCore.Mvc;

namespace Config.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : Controller
    {
        private readonly IConfigService configService;
        
        public ConfigController(IConfigService configService)
        {
            this.configService = configService;
        }
        // GET api/values
        [HttpGet("{appName}")]
        public async Task<ActionResult<List<ConfigModel>>> Get(string appName)
        {
            var allConfig = await configService.ConfigRepository.ReadAllConfig(appName);
            return Json(allConfig);
        }

        [HttpGet]
        [HttpGet("{appName}/{key}")]
        public async Task<ActionResult<List<ConfigModel>>> Get(string appName, string key)
        {
            var config = await configService.ConfigRepository.ReadConfig(appName, key);
            var configFromCache = await configService.GetValue<bool>(key);
            return Json(config);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
