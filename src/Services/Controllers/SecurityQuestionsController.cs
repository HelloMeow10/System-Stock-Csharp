using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using Contracts;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;
using System.Linq;

namespace Services.Controllers
{
    [ApiVersion("1.0")]
    public class SecurityQuestionsController : BaseApiController
    {
        private readonly ISecurityQuestionService _securityQuestionService;

        public SecurityQuestionsController(ISecurityQuestionService securityQuestionService)
        {
            _securityQuestionService = securityQuestionService;
        }

        [HttpGet("{username}")]
        [ProducesResponseType(typeof(IEnumerable<PreguntaSeguridadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<PreguntaSeguridadDto>>> GetUserQuestions(string username)
        {
            var questions = await _securityQuestionService.GetUserSecurityQuestionsAsync(username);
            return questions.ToList();
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<PreguntaSeguridadDto>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<PreguntaSeguridadDto>> GetAllQuestions()
        {
            var questions = _securityQuestionService.GetSecurityQuestions();
            return Ok(questions.ToList());
        }

        [HttpPost("{username}/answers")]
        [Authorize]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SaveAnswers(string username, [FromBody] Dictionary<int, string> answers)
        {
            await _securityQuestionService.SaveSecurityAnswersAsync(username, answers);
            return NoContent();
        }
    }
}
