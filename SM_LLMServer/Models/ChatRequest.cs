
using System;

namespace SM_LLMServer.Models
{
    public class ChatRequest
    {
        public string Prompt { get; set; }
        public Guid ConversationId { get; set; }
    }
}
