using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using BusinessLogic.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Services.Controllers
{
    [ApiController]
    [Route("api/securityquestions")]
    public class SecurityQuestionController : ControllerBase
    {
        private readonly ISecurityQuestionService _securityQuestionService;

        public SecurityQuestionController(ISecurityQuestionService securityQuestionService)
        {
            _securityQuestionService = securityQuestionService;
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<List<PreguntaSeguridadDto>>> Get(string username)
        {
            var questions = await _securityQuestionService.GetUserSecurityQuestionsAsync(username);
            return Ok(questions);
        }

        [HttpGet]
        public ActionResult<List<PreguntaSeguridadDto>> Get()
        {
            var questions = _securityQuestionService.GetSecurityQuestions();
            return Ok(questions);
        }

        [HttpPost("{username}/answers")]
        public async Task<IActionResult> Post(string username, [FromBody] Dictionary<int, string> answers)
        {
            await _securityQuestionService.SaveSecurityAnswersAsync(username, answers);
            return NoContent();
        }
    }
}
