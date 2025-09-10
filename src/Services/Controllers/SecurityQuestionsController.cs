using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using BusinessLogic.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Services.Models;
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
        public async Task<IActionResult> GetUserQuestions(string username)
        {
            var questions = await _securityQuestionService.GetUserSecurityQuestionsAsync(username);
            return Ok(ApiResponse<List<PreguntaSeguridadDto>>.CreateSuccess(questions));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAllQuestions()
        {
            var questions = _securityQuestionService.GetSecurityQuestions();
            return Ok(ApiResponse<List<PreguntaSeguridadDto>>.CreateSuccess(questions));
        }

        [HttpPost("{username}/answers")]
        [Authorize]
        public async Task<IActionResult> SaveAnswers(string username, [FromBody] Dictionary<int, string> answers)
        {
            await _securityQuestionService.SaveSecurityAnswersAsync(username, answers);
            return Ok(ApiResponse<object>.CreateSuccess(new { message = "Security answers saved successfully." }));
        }
    }
}
