using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using BusinessLogic.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Services.Controllers
{
    public class SecurityQuestionsController : BaseApiController
    {
        private readonly ISecurityQuestionService _securityQuestionService;

        public SecurityQuestionsController(ISecurityQuestionService securityQuestionService)
        {
            _securityQuestionService = securityQuestionService;
        }

        [HttpGet("{username}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PreguntaSeguridadDto>>> GetUserQuestions(string username)
        {
            var questions = await _securityQuestionService.GetUserSecurityQuestionsAsync(username);
            return Ok(questions);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<PreguntaSeguridadDto>> GetAllQuestions()
        {
            var questions = _securityQuestionService.GetSecurityQuestions();
            return Ok(questions);
        }

        [HttpPost("{username}/answers")]
        [Authorize]
        public async Task<IActionResult> SaveAnswers(string username, [FromBody] Dictionary<int, string> answers)
        {
            await _securityQuestionService.SaveSecurityAnswersAsync(username, answers);
            return NoContent();
        }
    }
}
