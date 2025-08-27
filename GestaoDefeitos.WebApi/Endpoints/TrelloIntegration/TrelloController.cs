using GestaoDefeitos.Application.TrelloIntegration;
using GestaoDefeitos.Application.TrelloIntegration.Responses;
using GestaoDefeitos.Application.Utils;
using GestaoDefeitos.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDefeitos.WebApi.Endpoints.TrelloIntegration;
[ApiController]
[Route("/trello")]
public class TrelloController(AuthenticationContextAcessor _authenticationContextAcessor, ITrelloIntegrationService _trelloIntegrationService) : ControllerBase
{
    [HttpGet("login")]
    public async Task<IActionResult> Login([FromQuery] string returnUrl = "/")
    {
        var redirectUrl = await _trelloIntegrationService.GetLoginRedirectUrlAsync(returnUrl);
        return Redirect(redirectUrl);
    }

    [HttpGet("callback")]
    public async Task<IActionResult> Callback([FromQuery] string oauth_token, [FromQuery] string oauth_verifier, [FromQuery] string returnUrl)
    {
        var userId = _authenticationContextAcessor.GetCurrentLoggedUserId().ToString();
        var success = await _trelloIntegrationService.HandleCallbackAsync(oauth_token, oauth_verifier, userId);

        if (!success)
            return Redirect(returnUrl + "?isAuthenticated=false");

        return Redirect(returnUrl + "?isAuthenticated=true");
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
    public async Task<TrelloUserStory> AddComment(string cardId, Guid defectId, string comment)
    {
        return await _trelloIntegrationService.AddCommentAsync(_authenticationContextAcessor.GetCurrentLoggedUserId().ToString(), cardId, comment, defectId);
    }
}