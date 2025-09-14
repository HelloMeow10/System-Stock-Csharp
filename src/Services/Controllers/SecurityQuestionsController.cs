using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using Contracts;
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
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PreguntaSeguridadDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PreguntaSeguridadDto>>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetUserQuestions(string username)
        {
            var questions = await _securityQuestionService.GetUserSecurityQuestionsAsync(username);
            return Ok(ApiResponse<IEnumerable<PreguntaSeguridadDto>>.Success(questions));
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PreguntaSeguridadDto>>), StatusCodes.Status200OK)]
        public IActionResult GetAllQuestions()
        {
            var questions = _securityQuestionService.GetSecurityQuestions();
            return Ok(ApiResponse<IEnumerable<PreguntaSeguridadDto>>.Success(questions));
        }

        [HttpPost("{username}/answers")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SaveAnswers(string username, [FromBody] Dictionary<int, string> answers)
        {
            await _securityQuestionService.SaveSecurityAnswersAsync(username, answers);
            return Ok(ApiResponse<object>.Success(new { message = "Answers saved successfully." }));
        }
    }
}
