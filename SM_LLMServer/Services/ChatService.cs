using System;
using System.IO;
using System.Threading.Tasks;
using SM_LLMServer.Services;

namespace SM_LLMServer.Services
{
    public class ChatResponse
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public string Provider { get; set; }
    }

    public class ChatService
    {
        private readonly ConversationRepository _conversationRepository;
        private readonly LlmClient _llmClient;

        public ChatService(ConversationRepository conversationRepository, LlmClient llmClient)
        {
            _conversationRepository = conversationRepository;
            _llmClient = llmClient;
        }

        public async Task<ChatResponse> SendMessageAsync(string prompt, string conversationId, AiProvider provider)
        {
            try
            {
                // Parse conversationId to Guid
                if (!Guid.TryParse(conversationId, out Guid guidConversationId))
                {
                    guidConversationId = Guid.NewGuid();
                }

                // Retrieve last response id if any
                var previousResponseId = _conversationRepository.GetLastResponseId(guidConversationId);

                // Get instructions (lazy loading)
                var instructions = GetInstructions();

                // Create LLM request with selected provider
                var llmRequest = new LlmRequest
                {
                    Model = GetModelForProvider(provider),
                    Instructions = instructions,
                    Prompt = prompt,
                    Temperature = 0.2,
                    MaxTokens = 200,
                    PreviousResponseId = previousResponseId,
                    Provider = provider
                };

                // Get response from selected AI provider
                var response = await _llmClient.GenerateTextAsync(llmRequest);

                // Save last response id in conversation repository
                _conversationRepository.SetLastResponseId(guidConversationId, response.Id);

                return new ChatResponse
                {
                    Id = response.Id,
                    Message = response.Text,
                    Provider = response.Provider
                };
            }
            catch (Exception ex)
            {
                // Return a friendly error message instead of crashing
                return new ChatResponse
                {
                    Id = Guid.NewGuid().ToString(),
                    Message = $"I'm having trouble processing your request right now. Please try again later. (Error: {ex.Message})",
                    Provider = provider.ToString()
                };
            }
        }

        public async Task<ChatResponse> SendRegularMessageAsync(string prompt, string conversationId, AiProvider provider)
        {
            try
            {
                // Parse conversationId to Guid
                if (!Guid.TryParse(conversationId, out Guid guidConversationId))
                {
                    guidConversationId = Guid.NewGuid();
                }

                // Retrieve last response id if any
                var previousResponseId = _conversationRepository.GetLastResponseId(guidConversationId);

                // Use general AI instructions instead of WonderWorld-specific ones
                var instructions = "You are a helpful AI assistant. Please provide a helpful and informative response to the user's question. Be conversational and engaging.";

                // Create LLM request with selected provider
                var llmRequest = new LlmRequest
                {
                    Model = GetModelForProvider(provider),
                    Instructions = instructions,
                    Prompt = prompt,
                    Temperature = 0.7,
                    MaxTokens = 500,
                    PreviousResponseId = previousResponseId,
                    Provider = provider
                };

                // Get response from selected AI provider
                var response = await _llmClient.GenerateTextAsync(llmRequest);

                // Save last response id in conversation repository
                _conversationRepository.SetLastResponseId(guidConversationId, response.Id);

                return new ChatResponse
                {
                    Id = response.Id,
                    Message = response.Text,
                    Provider = response.Provider
                };
            }
            catch (Exception ex)
            {
                // Return a friendly error message instead of crashing
                return new ChatResponse
                {
                    Id = Guid.NewGuid().ToString(),
                    Message = $"I'm having trouble processing your request right now. Please try again later. (Error: {ex.Message})",
                    Provider = provider.ToString()
                };
            }
        }

        private string GetInstructions()
        {
            try
            {
                // Try to load prompt template and park info
                var templatePath = Path.Combine("llm", "prompts", "chatbot.txt");
                var parkInfoPath = Path.Combine("llm", "prompts", "WonderWorld.md");
                
                string template = "";
                string parkInfo = "";
                
                // Only try to read files if they exist
                if (File.Exists(templatePath))
                {
                    template = File.ReadAllText(templatePath);
                }
                else
                {
                    template = "You are a helpful AI assistant. Please provide a helpful and informative response to the user's question.";
                }
                
                if (File.Exists(parkInfoPath))
                {
                    parkInfo = File.ReadAllText(parkInfoPath);
                }
                else
                {
                    parkInfo = "WonderWorld theme park information not available.";
                }
                
                return template.Replace("{{parkInfo}}", parkInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Could not load prompt files: {ex.Message}");
                // Generic fallback for any other file reading errors
                return "You are a helpful AI assistant. Please provide a helpful and informative response to the user's question.";
            }
        }

        private string GetModelForProvider(AiProvider provider)
        {
            return provider switch
            {
                AiProvider.OpenAI => "gpt-4o-mini",
                AiProvider.Ollama => "llama3.2",
                AiProvider.CustomKnowledge => "gpt-4o-mini", // Uses OpenAI as backend
                AiProvider.HuggingFace => "microsoft/DialoGPT-medium",
                _ => "gpt-4o-mini"
            };
        }
    }
}
