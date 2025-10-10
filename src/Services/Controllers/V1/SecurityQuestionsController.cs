using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;
using Contracts;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;

namespace Services.Controllers.V1
{
    [ApiVersion("1.0")]
    public class SecurityQuestionsController : BaseApiController
    {
        private readonly ISecurityQuestionService _securityQuestionService;

        public SecurityQuestionsController(ISecurityQuestionService securityQuestionService)
        {
            _securityQuestionService = securityQuestionService;
        }

        [HttpGet("{username}", Name = "GetUserSecurityQuestionsV1")]
        [ProducesResponseType(typeof(IEnumerable<PreguntaSeguridadDto>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<PreguntaSeguridadDto>> GetUserQuestions(string username)
        {
            return await _securityQuestionService.GetUserSecurityQuestionsAsync(username);
        }

        [HttpGet(Name = "GetAllSecurityQuestionsV1")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<PreguntaSeguridadDto>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<PreguntaSeguridadDto>> GetAllQuestions()
        {
            return await _securityQuestionService.GetSecurityQuestionsAsync();
        }

        [HttpPost("{username}/answers", Name = "SaveUserSecurityAnswersV1")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> SaveAnswers(string username, [FromBody] Dictionary<int, string> answers)
        {
            await _securityQuestionService.SaveSecurityAnswersAsync(username, answers);
            return NoContent();
        }
    }
}