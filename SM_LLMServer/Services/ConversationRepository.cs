using System;
using System.Collections.Concurrent;

namespace SM_LLMServer.Services
{
    public class ConversationRepository
    {
        private readonly ConcurrentDictionary<Guid, string> _lastResponseIds = new();

        public string GetLastResponseId(Guid conversationId)
        {
            return _lastResponseIds.TryGetValue(conversationId, out var id) ? id : null;
        }

        public void SetLastResponseId(Guid conversationId, string responseId)
        {
            _lastResponseIds[conversationId] = responseId;
        }
    }
}
