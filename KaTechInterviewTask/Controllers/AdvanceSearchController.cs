using KaTechInterviewTask.Configs;
using KaTechInterviewTask.Logics;
using Microsoft.AspNetCore.Mvc;

namespace KaTechInterviewTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvanceSearchController : ControllerBase
    {
        public AdvanceSearchController(URLConfig URLconfig,CredentialConfig credentialConfig)
        {
            _URLconfig = URLconfig;
            _credentialConfig = credentialConfig;
        }

        public URLConfig _URLconfig { get; }
        public CredentialConfig _credentialConfig { get; }

        [HttpGet("{word}")]
        public async Task<IActionResult> Get(string word = "abcdef")
        {
            var result = await new AdvanceSearchLogic(_URLconfig, _credentialConfig).GetAdvanceSearchResult(word);
            return StatusCode(result.StatusCode,result);
        }
    }
}
