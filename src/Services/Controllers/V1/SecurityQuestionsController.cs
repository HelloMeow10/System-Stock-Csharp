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

        [HttpPost(Name = "CreateSecurityQuestionV1")]
        [Authorize]
        [ProducesResponseType(typeof(PreguntaSeguridadDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] PreguntaSeguridadRequest request)
        {
            var created = await _securityQuestionService.CreateSecurityQuestionAsync(request.Pregunta);
            return CreatedAtRoute("GetAllSecurityQuestionsV1", created);
        }

        [HttpPut("{idPregunta}", Name = "UpdateSecurityQuestionV1")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(int idPregunta, [FromBody] PreguntaSeguridadRequest request)
        {
            await _securityQuestionService.UpdateSecurityQuestionAsync(idPregunta, request.Pregunta);
            return NoContent();
        }

        [HttpDelete("{idPregunta}", Name = "DeleteSecurityQuestionV1")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int idPregunta)
        {
            await _securityQuestionService.DeleteSecurityQuestionAsync(idPregunta);
            return NoContent();
        }
    }
}