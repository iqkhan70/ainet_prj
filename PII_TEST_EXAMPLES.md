# PII Detection Test Examples

Test the PII Chat functionality with these examples. Copy and paste any of these into the PII Chat input field to see the detection in action.

## 🎯 **Chat Types Available:**

- **WonderWorld Chat**: Specialized chat with theme park knowledge and custom prompts
- **Regular Chat**: Standard ChatGPT-style interface for general questions
- **PII Chat**: Secure chat with PII detection and blocking
- **Reviews**: Product reviews and AI-generated summaries

## 🚫 **PII Examples (Will Be Blocked):**

### **Social Security Number (SSN):**

- `123-45-6789`
- `123456789`

### **Credit Card Numbers:**

- `1234 5678 9012 3456`
- `1234-5678-9012-3456`
- `1234567890123456`

### **Phone Numbers:**

- `(555) 123-4567`
- `555-123-4567`
- `5551234567`
- `+1 555 123 4567`

### **Email Addresses:**

- `john.doe@example.com`
- `user123@gmail.com`
- `test@company.org`

### **Physical Addresses:**

- `123 Main Street`
- `456 Oak Avenue`
- `789 Pine Road`
- `321 Elm Boulevard`

### **IP Addresses:**

- `192.168.1.1`
- `10.0.0.1`
- `172.16.0.1`

### **Dates of Birth:**

- `12/25/1990`
- `01-15-1985`
- `06/30/1975`

### **Passport Numbers:**

- `AB1234567`
- `CD9876543`

### **Driver License Numbers:**

- `CA1234567`
- `NY9876543`

## ✅ **Safe Examples (Will Be Sent):**

### **General Questions:**

- `What is the weather like today?`
- `How do I cook pasta?`
- `Tell me a joke`
- `What are the benefits of exercise?`

### **Business Questions:**

- `How do I start a business?`
- `What is marketing strategy?`
- `How to write a business plan?`

### **Technical Questions:**

- `How do I learn programming?`
- `What is machine learning?`
- `How to deploy a web application?`

## 🔍 **How It Works:**

1. **Type your message** in the PII Chat input field
2. **PII detection runs automatically** before sending
3. **If PII is detected:**
   - Message is blocked from sending
   - Warning appears below the input
   - Specific PII types are listed
4. **If no PII is detected:**
   - Message sends normally
   - AI responds as usual

## 🎭 **Demonstrating the Differences:**

### **WonderWorld Chat:**
- Specialized for theme park questions
- Custom knowledge base integration
- Placeholder: "Ask me about WonderWorld theme park..."
- **API Endpoint**: `/api/Chat` (with WonderWorld prompts)

### **Regular Chat:**
- General AI assistance
- ChatGPT-style interface
- Welcome message with robot icon
- Placeholder: "Message..."
- **API Endpoint**: `/api/Chat/regular` (general AI prompts)
- **No WonderWorld knowledge** - answers general questions normally

### **PII Chat:**
- Same as Regular Chat but with PII protection
- Blocks sensitive information
- Shows warning when PII detected
- **API Endpoint**: `/api/Chat/regular` (same as Regular Chat)

## 💡 **Tips:**

- The detection is real-time and happens as you type
- Multiple PII types can be detected in a single message
- The system is designed to be conservative (better safe than sorry)
- Each chat type serves a different purpose
- Navigation is consistent across all pages
