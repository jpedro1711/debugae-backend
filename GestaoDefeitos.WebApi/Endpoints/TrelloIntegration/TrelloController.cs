using GestaoDefeitos.Application.TrelloIntegration;
using GestaoDefeitos.Application.TrelloIntegration.Responses;
using GestaoDefeitos.Application.Utils;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDefeitos.WebApi.Endpoints.TrelloIntegration;
[ApiController]
[Route("api/trello")]
public class TrelloController(AuthenticationContextAcessor _authenticationContextAcessor, ITrelloIntegrationService _trelloIntegrationService) : ControllerBase
{
    [HttpGet("login")]
    public async Task<IActionResult> Login()
    {
        var redirectUrl = await _trelloIntegrationService.GetLoginRedirectUrlAsync();
        return Redirect(redirectUrl);
    }

    [HttpGet("callback")]
    public async Task<IActionResult> Callback([FromQuery] string oauth_token, [FromQuery] string oauth_verifier)
    {
        var userId = _authenticationContextAcessor.GetCurrentLoggedUserId().ToString();
        var success = await _trelloIntegrationService.HandleCallbackAsync(oauth_token, oauth_verifier, userId);

        if (!success)
            return BadRequest("Token desconhecido ou erro na autenticação.");

        return Ok("Autenticado com sucesso.");
    }

    [HttpGet("workspaces")]
    public async Task<List<TrelloWorkspaceViewModel>> GetWorkspaces()
    {
        return await _trelloIntegrationService.GetWorkspacesAsync(_authenticationContextAcessor.GetCurrentLoggedUserId().ToString());
    }

    [HttpGet("boards/{workspaceId}")]
    public async Task<List<TrelloBoardViewModel>> GetBoards(string workspaceId)
    {
        return await _trelloIntegrationService.GetBoardsAsync(_authenticationContextAcessor.GetCurrentLoggedUserId().ToString(), workspaceId);
    }

    [HttpGet("cards/{boardId}")]
    public async Task<List<TrelloCardViewModel>> GetCards(string boardId)
    {
        return await _trelloIntegrationService.GetCardsAsync(_authenticationContextAcessor.GetCurrentLoggedUserId().ToString(), boardId);
    }

    [HttpPost("cards/{cardId}/defects/{defectId}/comments/{comment}")]
    public async Task<string> AddComment(string cardId, Guid defectId, string comment)
    {
        return await _trelloIntegrationService.AddCommentAsync(_authenticationContextAcessor.GetCurrentLoggedUserId().ToString(), cardId, comment, defectId);
    }
}