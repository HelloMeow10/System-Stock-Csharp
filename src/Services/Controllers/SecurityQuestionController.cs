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
        public async Task<ActionResult<List<PreguntaSeguridadDto>>> GetUserSecurityQuestions(string username)
        {
            var questions = await _securityQuestionService.GetPreguntasDeUsuarioAsync(username);
            return Ok(questions);
        }

        [HttpGet]
        public ActionResult<List<PreguntaSeguridadDto>> GetSecurityQuestions()
        {
            var questions = _securityQuestionService.GetPreguntasSeguridad();
            return Ok(questions);
        }

        [HttpPost("{username}/answers")]
        public async Task<IActionResult> SaveUserSecurityAnswers(string username, [FromBody] Dictionary<int, string> answers)
        {
            await _securityQuestionService.GuardarRespuestasSeguridadAsync(username, answers);
            return Ok();
        }
    }
}
