
using System;
using System.Threading.Tasks;

namespace SM_LLMServer.Services
{
    public class ChatService
    {
        public async Task<string> SendMessageAsync(string prompt, Guid conversationId)
        {
            // Simulated LLM logic (replace with OpenAI call if needed)
            await Task.Delay(200);
            return $"Echo: {prompt} (conversation {conversationId})";
        }
    }
}
