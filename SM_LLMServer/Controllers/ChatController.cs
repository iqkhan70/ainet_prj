
using Microsoft.AspNetCore.Mvc;
using SM_LLMServer.Models;
using SM_LLMServer.Services;

namespace SM_LLMServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly ChatService _chatService;

    public ChatController(ChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost]
    public async Task<IActionResult> Post(ChatRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Prompt))
            return BadRequest(new { error = "Prompt is required" });

        var reply = await _chatService.SendMessageAsync(request.Prompt, request.ConversationId);
        return Ok(new { message = reply });
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { message = "Chat API is alive. Use POST to send prompts." });
    }
}
