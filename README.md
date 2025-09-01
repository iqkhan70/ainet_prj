# SM LLM Full Stack Application

A full-stack .NET application with Blazor WebAssembly client and ASP.NET Core Web API server, featuring LLM integration and a reviews system.

## Features

- **Chat Interface**: AI-powered chat with multiple LLM providers (OpenAI, Ollama, Custom Knowledge, Hugging Face)
- **Reviews System**: Product reviews with AI-generated summaries stored in MySQL database
- **Modern UI**: ChatGPT-like interface with animations and sound effects
- **Multiple AI Providers**: Support for OpenAI, Ollama, Custom Knowledge, and Hugging Face

## Prerequisites

- .NET 8.0 SDK
- MySQL Server (8.0 or later)
- Node.js (for development tools)

## Setup Instructions

### 1. Database Setup

1. **Install MySQL Server** (if not already installed)
   - macOS: `brew install mysql`
   - Windows: Download from [MySQL website](https://dev.mysql.com/downloads/mysql/)
   - Linux: `sudo apt install mysql-server`

2. **Start MySQL Service**
   ```bash
   # macOS
   brew services start mysql
   
   # Windows
   net start mysql
   
   # Linux
   sudo systemctl start mysql
   ```

3. **Create Database and User**
   ```sql
   CREATE DATABASE sm_llm_reviews;
   CREATE USER 'sm_llm_user'@'localhost' IDENTIFIED BY 'your_secure_password';
   GRANT ALL PRIVILEGES ON sm_llm_reviews.* TO 'sm_llm_user'@'localhost';
   FLUSH PRIVILEGES;
   ```

### 2. Configuration

1. **Copy the example configuration**
   ```bash
   cp SM_LLMServer/appsettings.example.json SM_LLMServer/appsettings.json
   ```

2. **Update `appsettings.json` with your credentials**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=sm_llm_reviews;User=sm_llm_user;Password=your_secure_password;Port=3306;"
     },
     "OpenAI": {
       "ApiKey": "your_openai_api_key_here"
     },
     "HuggingFace": {
       "Token": "your_huggingface_token_here"
     }
   }
   ```

### 3. Build and Run

1. **Build the server**
   ```bash
   cd SM_LLMServer
   dotnet restore
   dotnet build
   ```

2. **Run the server**
   ```bash
   dotnet run
   ```
   The server will start at `https://localhost:7000`

3. **Build and run the client** (in a new terminal)
   ```bash
   cd SM_LLMClient
   dotnet restore
   dotnet build
   dotnet run
   ```
   The client will start at `https://localhost:7010`

### 4. Database Migration

The application will automatically create the database schema and seed it with sample data on first run. If you need to manually run migrations:

```bash
cd SM_LLMServer
dotnet ef database update
```

## API Endpoints

### Chat API
- `POST /api/Chat` - Send a chat message
- `GET /api/Chat` - Get available providers

### Reviews API
- `GET /api/Reviews` - Get all reviews
- `GET /api/Reviews/{productId}` - Get reviews for a specific product
- `POST /api/Reviews` - Create a new review
- `POST /api/Reviews/{productId}/summarize` - Generate AI summary for product reviews
- `GET /api/Reviews/providers` - Get available AI providers

## Project Structure

```
SM_LLMFullStack_LLM/
├── SM_LLMServer/           # ASP.NET Core Web API
│   ├── Controllers/        # API controllers
│   ├── Services/           # Business logic services
│   ├── Models/             # Data models
│   ├── Data/               # Entity Framework context and migrations
│   └── appsettings.json    # Configuration
├── SM_LLMClient/           # Blazor WebAssembly client
│   ├── Pages/              # Razor pages
│   ├── wwwroot/            # Static files (CSS, JS)
│   └── Program.cs          # Client configuration
└── README.md               # This file
```

## Features

### Chat System
- Multiple AI providers (OpenAI, Ollama, Custom Knowledge, Hugging Face)
- Conversation history tracking
- Real-time typing indicators
- Sound effects for user interactions

### Reviews System
- Product review management
- AI-powered review summarization
- MySQL database storage
- Automatic summary expiration (7 days)
- Sample data seeding

### UI Features
- Modern ChatGPT-like interface
- Responsive design
- Navigation between Chat and Reviews
- Star ratings for reviews
- Smooth animations and transitions

## Troubleshooting

### Common Issues

1. **Database Connection Error**
   - Verify MySQL is running
   - Check connection string in `appsettings.json`
   - Ensure database and user exist

2. **Build Errors**
   - Run `dotnet restore` before building
   - Check .NET version compatibility
   - Verify all NuGet packages are installed

3. **API Errors**
   - Check server is running on correct port
   - Verify CORS configuration
   - Check API keys in configuration

### Development Tips

- Use Swagger UI at `https://localhost:7000/swagger` to test API endpoints
- Check console logs for detailed error messages
- Use browser developer tools to debug client-side issues

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## License

This project is licensed under the MIT License.
