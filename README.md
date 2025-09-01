# SM LLM Full Stack Application

A full-stack Blazor WebAssembly application with multiple AI providers integration.

## Setup

### Configuration

1. Copy `SM_LLMServer/appsettings.template.json` to `SM_LLMServer/appsettings.json`
2. Fill in your API keys:
   - **OpenAI**: Get your API key from [OpenAI Platform](https://platform.openai.com/api-keys)
   - **Hugging Face**: Get your token from [Hugging Face Settings](https://huggingface.co/settings/tokens)
   - **Ollama**: Default URL is `http://localhost:11434` (if running locally)
   - **Custom Knowledge**: Path to your markdown knowledge base file

### Running the Application

1. **Start the Server:**
   ```bash
   cd SM_LLMServer
   dotnet run
   ```
   Server will be available at `http://localhost:7000`

2. **Start the Client:**
   ```bash
   cd SM_LLMClient
   dotnet run
   ```
   Client will be available at `http://localhost:7010`

## Features

- **Multiple AI Providers**: OpenAI, Ollama, Hugging Face, Custom Knowledge Base
- **Chat Interface**: ChatGPT-like UI with message history
- **Real-time Communication**: WebSocket-like experience with HTTP
- **Responsive Design**: Modern, mobile-friendly interface

## API Endpoints

- `GET /api/Chat` - Get available providers
- `POST /api/Chat` - Send a message
  - Query parameter `provider`: `OpenAI`, `Ollama`, `CustomKnowledge`, or `HuggingFace`

## Security Note

The `appsettings.json` file is gitignored to prevent accidentally committing API keys. Always use the template file and keep your actual configuration local.
