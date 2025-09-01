
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
    public async Task<IActionResult> Post(ChatRequest request, [FromQuery] string provider = "OpenAI")
    {
        if (string.IsNullOrWhiteSpace(request.Prompt))
            return BadRequest(new { error = "Prompt is required" });

        // Parse provider from query parameter
        var aiProvider = ParseProvider(provider);

        var reply = await _chatService.SendMessageAsync(request.Prompt, request.ConversationId, aiProvider);
        return Ok(new { message = reply.Message, provider = reply.Provider });
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { message = "Chat API is alive. Use POST to send prompts. Available providers: OpenAI, Ollama, CustomKnowledge" });
    }

    private AiProvider ParseProvider(string provider)
    {
        return provider.ToLower() switch
        {
            "ollama" => AiProvider.Ollama,
            "customknowledge" => AiProvider.CustomKnowledge,
            "huggingface" => AiProvider.HuggingFace,
            _ => AiProvider.OpenAI
        };
    }
}
